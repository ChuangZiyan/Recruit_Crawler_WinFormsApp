Imports AngleSharp.Common
Imports Microsoft.Web.WebView2.Core
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Text.RegularExpressions

Public Class Form1

    Public Const WebSite_URL_Prefix As String = "https://www.recruit.com.hk"

    Public currentDirectory As String = My.Application.Info.DirectoryPath
    Public searchingResultDir As String = currentDirectory + "\SearchingResult"

    Public result_filePath As String

    Public NAVIGATION_COMPLETED = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler MainWebView2.NavigationCompleted, AddressOf WebView2_NavigationCompletedAsync

        'Check if Folder exists
        If Not Directory.Exists(searchingResultDir) Then
            Directory.CreateDirectory(searchingResultDir)
        End If

    End Sub

    Private Async Sub Start_Crawling_Button_Click(sender As Object, e As EventArgs) Handles Start_Crawling_Button.Click
        Start_Crawling_Button.Text = "搜尋中..."
        Start_Crawling_Button.Enabled = False
        Start_Time_TextBox.Text = Now.ToString("G")
        End_Time_TextBox.Text = ""

        result_filePath = searchingResultDir + "\SearchingResult_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt"
        File.Create(result_filePath).Dispose()

        Dim jobcat_list = {"11000", "12000", "47000", "13000", "14000", "23000", "17000", "18000", "19000",
            "20000", "53450", "53700", "22000", "30000", "53460", "24000", "25000", "26000", "28000", "21000",
            "29000", "41000", "15000", "31000", "53600", "48000", "53480", "27000", "32000", "33000", "34000",
            "43000", "44000", "54000"}

        'Dim jobcat_list = {"54000"}

        Dim job_cat_counter = jobcat_list.Length

        For Each jobcat In jobcat_list
            'Debug.WriteLine("https://www.recruit.com.hk/jobseeker/JobSearchResult.aspx?searchPath=B&jobcat=" & jobcat)
            Searching_Status_Label.Text = "正在搜尋工作種類: " & jobcat
            job_cat_counter -= 1
            Jobcat_Progress_Count_Label.Text = "剩餘工作類別數量 : " & job_cat_counter

            MainWebView2.CoreWebView2.Navigate("https://www.recruit.com.hk/jobseeker/JobSearchResult.aspx?searchPath=B&jobcat=" & jobcat)


            ' wait 15 secs until page ready
            NAVIGATION_COMPLETED = False
            For wait_sec = 0 To 15
                If NAVIGATION_COMPLETED Then
                    Exit For
                Else
                    Await Delay_msec(1000)
                End If

            Next

            Dim job_page_counter = 0

            While True

                job_page_counter += 1
                Dim job_Url_List = Await Get_Page_Job_Collection()
                Dim page_total_job_count = job_Url_List.Count
                Searching_Status_Label.Text = "正在搜尋工作種類: " & jobcat & " 第 " & job_page_counter & " 頁 ... "

                Dim job_detail_progress_count = 0
                For Each url In job_Url_List
                    job_detail_progress_count += 1
                    JobCollection_Counter_Label.Text = job_detail_progress_count & "/" & page_total_job_count
                    Await Save_Job_Email_And_Phone_To_File(url)
                    Await Delay_msec(1000)
                Next

                Dim next_page_disabled = Await MainWebView2.ExecuteScriptAsync("document.getElementsByClassName('next-page PageNumber')[0].getAttribute('disabled')")

                If next_page_disabled = "null" Then
                    Await MainWebView2.ExecuteScriptAsync("document.getElementsByClassName('next-page PageNumber')[0].click();")

                    ' wait 15 secs until page ready
                    NAVIGATION_COMPLETED = False
                    For wait_sec = 0 To 15
                        If NAVIGATION_COMPLETED Then
                            Exit For
                        Else
                            Await Delay_msec(1000)
                        End If
                    Next

                Else
                    Exit While
                End If

            End While

        Next
        Searching_Status_Label.Text = "搜尋完成"
        End_Time_TextBox.Text = Now.ToString("G")
        Start_Crawling_Button.Text = "任務開始"
        Start_Crawling_Button.Enabled = True
        MsgBox("搜尋任務完成")
    End Sub

    Private Async Function WebView2_NavigationCompletedAsync(sender As Object, e As CoreWebView2NavigationCompletedEventArgs) As Task(Of Boolean)

        If e.IsSuccess Then
            NAVIGATION_COMPLETED = True
            Debug.WriteLine("PAGE READY : " & NAVIGATION_COMPLETED)
        Else
            MsgBox("Load Page Failure")
            NAVIGATION_COMPLETED = False
        End If


        Return False
    End Function



    Private Async Function Get_Page_Job_Collection() As Task(Of List(Of String))


        Dim jsCode As String = "var elements = document.getElementsByClassName('title-company-col'); " +
                        "var result = []; " +
                        "for (var i = 0; i < elements.length; i++) { " +
                        "   var link = elements[i].getElementsByTagName('a')[0]; " +
                        "   if (link) { " +
                        "       var hrefValue = link.getAttribute('href'); " +
                        "       if (hrefValue) { " +
                        "           result.push(hrefValue); " +
                        "       } " +
                        "   } " +
                        "} " +
                        "JSON.stringify(result);"
        Dim result As String = Await MainWebView2.ExecuteScriptAsync(jsCode)

        result = result.Replace("\", "").Trim("""")

        'Debug.WriteLine(result)

        Dim resultList As List(Of String) = JsonConvert.DeserializeObject(Of List(Of String))(result)

        Return resultList

    End Function


    Public Async Function Get_Job_Detail_HTML(job_url) As Task(Of String)
        Try
            Using httpClient As New HttpClient()

                'httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", bearerToken)
                'Dim content As New StringContent(jsonRequestData, Encoding.UTF8, "application/json")

                Dim response As HttpResponseMessage = Await httpClient.GetAsync(job_url)

                If response.IsSuccessStatusCode Then
                    Dim responseBody As String = Await response.Content.ReadAsStringAsync()
                    'Debug.WriteLine("############ responseBody: ############### ")
                    'Debug.WriteLine(responseBody)
                    Return responseBody
                Else
                    Debug.WriteLine("http status code : " & response.StatusCode)
                    MsgBox("Http Status Code : " & response.StatusCode)
                    Return "error"
                End If

            End Using

            Return "error"
        Catch ex As Exception
            Return "exception"
            'EventLog_ListBox.Items.Add("查詢發生錯誤")

        End Try

    End Function




    Public Async Function Save_Job_Email_And_Phone_To_File(job_Url) As Task

        Dim script As String = Await Get_Job_Detail_HTML(WebSite_URL_Prefix + job_Url)
        'Debug.WriteLine(script)

        Dim pattern As String = "<script type=""application/ld\+json"">(.+?)</script>"
        Dim regex As New Regex(pattern, RegexOptions.Singleline)
        Dim match As Match = regex.Match(script)

        If match.Success Then

            Try

                Dim jsonStr As String = match.Groups(1).Value
                'Debug.WriteLine(jsonStr)
                Dim jobPosting As JobPosting = JsonConvert.DeserializeObject(Of JobPosting)(jsonStr)

                'Debug.WriteLine("des: " & jobPosting.description)

                Dim mail_list = FindEmails(jobPosting.description)
                Dim phone_list = FindPhoneNumbers(jobPosting.description)
                Dim max_count As Integer

                If mail_list.Count >= phone_list.Count Then
                    max_count = mail_list.Count
                Else
                    max_count = phone_list.Count
                End If

                For idx As Integer = 0 To max_count - 1

                    Dim data_line = job_Url & ";"

                    If idx >= 0 AndAlso idx < phone_list.Count Then
                        'Debug.WriteLine(phone_list.GetItemByIndex(idx))
                        data_line += phone_list.GetItemByIndex(idx) + ";"
                    Else
                        'Debug.WriteLine("N/A")
                        data_line += "N/A" + ";"
                    End If


                    If idx >= 0 AndAlso idx < mail_list.Count Then
                        'Debug.WriteLine(mail_list.GetItemByIndex(idx))
                        data_line += data_line + mail_list.GetItemByIndex(idx)
                    Else
                        'Debug.WriteLine("N/A")
                        data_line += data_line + "N/A"
                    End If

                    'Debug.WriteLine(data_line)

                    Using writer As New StreamWriter(result_filePath, True)
                        writer.WriteLine(data_line)
                        writer.Close()
                    End Using

                Next

            Catch ex As Exception

                Debug_Msg_ListBox.Items.Add("網址發生例外: " & WebSite_URL_Prefix + job_Url)
                Using writer As New StreamWriter(searchingResultDir + "\DebugMsg_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt", True)
                    writer.WriteLine(script)
                    writer.Close()
                End Using

            End Try




        Else
            Debug.WriteLine("Not Found")
        End If

    End Function



    Public Function FindPhoneNumbers(input As String) As List(Of String)
        'Filter out phone numbers
        Dim phoneNumberPattern As String = "(\d{8}|\d{4}\s\d{4})" ' match whatsapp number
        Dim regex As New Regex(phoneNumberPattern)

        Dim matches As MatchCollection = regex.Matches(input)
        Dim phones As New List(Of String)


        For Each match As Match In matches
            If Not phones.Contains(match.Value) Then
                phones.Add(match.Value.Replace(" ", ""))
            End If
        Next

        Return phones

    End Function


    Public Shared Function FindEmails(input As String) As List(Of String)
        Dim emailPattern As String = "\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b"
        Dim regex As New Regex(emailPattern)
        Dim matches As MatchCollection = regex.Matches(input)
        Dim emails As New List(Of String)

        For Each match As Match In matches
            If Not emails.Contains(match.Value) Then
                emails.Add(match.Value)
            End If
        Next

        Return emails
    End Function


    Public Class JobPosting
        Public Property description As String
        Public Property title As String

    End Class

    Private Sub Reveal_Crawler_Result_Dir_Button_Click(sender As Object, e As EventArgs) Handles Reveal_Crawler_Result_Dir_Button.Click
        Process.Start("explorer.exe", searchingResultDir)
    End Sub


    Public Shared Async Function Delay_msec(msec As Integer) As Task
        Await Task.Delay(msec)
    End Function


End Class