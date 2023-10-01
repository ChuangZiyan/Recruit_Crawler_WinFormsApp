<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        MainWebView2 = New Microsoft.Web.WebView2.WinForms.WebView2()
        Start_Crawling_Button = New Button()
        Reveal_Crawler_Result_Dir_Button = New Button()
        Searching_Status_Label = New Label()
        Label1 = New Label()
        Start_Time_TextBox = New TextBox()
        End_Time_TextBox = New TextBox()
        Label2 = New Label()
        Label3 = New Label()
        JobCollection_Counter_Label = New Label()
        Jobcat_Progress_Count_Label = New Label()
        CType(MainWebView2, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' MainWebView2
        ' 
        MainWebView2.AllowExternalDrop = True
        MainWebView2.CreationProperties = Nothing
        MainWebView2.DefaultBackgroundColor = Color.White
        MainWebView2.Location = New Point(12, 90)
        MainWebView2.Name = "MainWebView2"
        MainWebView2.Size = New Size(944, 477)
        MainWebView2.Source = New Uri("https://www.google.com/", UriKind.Absolute)
        MainWebView2.TabIndex = 0
        MainWebView2.ZoomFactor = 1R
        ' 
        ' Start_Crawling_Button
        ' 
        Start_Crawling_Button.Location = New Point(12, 12)
        Start_Crawling_Button.Name = "Start_Crawling_Button"
        Start_Crawling_Button.Size = New Size(94, 29)
        Start_Crawling_Button.TabIndex = 1
        Start_Crawling_Button.Text = "開始任務"
        Start_Crawling_Button.UseVisualStyleBackColor = True
        ' 
        ' Reveal_Crawler_Result_Dir_Button
        ' 
        Reveal_Crawler_Result_Dir_Button.Location = New Point(12, 45)
        Reveal_Crawler_Result_Dir_Button.Name = "Reveal_Crawler_Result_Dir_Button"
        Reveal_Crawler_Result_Dir_Button.Size = New Size(94, 29)
        Reveal_Crawler_Result_Dir_Button.TabIndex = 4
        Reveal_Crawler_Result_Dir_Button.Text = "搜尋結果"
        Reveal_Crawler_Result_Dir_Button.UseVisualStyleBackColor = True
        ' 
        ' Searching_Status_Label
        ' 
        Searching_Status_Label.AutoSize = True
        Searching_Status_Label.Location = New Point(628, 17)
        Searching_Status_Label.Name = "Searching_Status_Label"
        Searching_Status_Label.Size = New Size(54, 19)
        Searching_Status_Label.TabIndex = 5
        Searching_Status_Label.Text = "待命中"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(112, 17)
        Label1.Name = "Label1"
        Label1.Size = New Size(80, 19)
        Label1.TabIndex = 6
        Label1.Text = "開始時間 : "
        ' 
        ' Start_Time_TextBox
        ' 
        Start_Time_TextBox.Location = New Point(198, 12)
        Start_Time_TextBox.Name = "Start_Time_TextBox"
        Start_Time_TextBox.Size = New Size(325, 27)
        Start_Time_TextBox.TabIndex = 7
        ' 
        ' End_Time_TextBox
        ' 
        End_Time_TextBox.Location = New Point(198, 45)
        End_Time_TextBox.Name = "End_Time_TextBox"
        End_Time_TextBox.Size = New Size(325, 27)
        End_Time_TextBox.TabIndex = 9
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(112, 50)
        Label2.Name = "Label2"
        Label2.Size = New Size(80, 19)
        Label2.TabIndex = 8
        Label2.Text = "結束時間 : "
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(542, 17)
        Label3.Name = "Label3"
        Label3.Size = New Size(80, 19)
        Label3.TabIndex = 10
        Label3.Text = "任務狀態 : "
        ' 
        ' JobCollection_Counter_Label
        ' 
        JobCollection_Counter_Label.AutoSize = True
        JobCollection_Counter_Label.Location = New Point(913, 17)
        JobCollection_Counter_Label.Name = "JobCollection_Counter_Label"
        JobCollection_Counter_Label.Size = New Size(43, 19)
        JobCollection_Counter_Label.TabIndex = 12
        JobCollection_Counter_Label.Text = "(0/0)"
        ' 
        ' Jobcat_Progress_Count_Label
        ' 
        Jobcat_Progress_Count_Label.AutoSize = True
        Jobcat_Progress_Count_Label.Location = New Point(542, 50)
        Jobcat_Progress_Count_Label.Name = "Jobcat_Progress_Count_Label"
        Jobcat_Progress_Count_Label.Size = New Size(153, 19)
        Jobcat_Progress_Count_Label.TabIndex = 13
        Jobcat_Progress_Count_Label.Text = "剩餘工作類別數量 : 0 "
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(9F, 19F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(968, 586)
        Controls.Add(Jobcat_Progress_Count_Label)
        Controls.Add(JobCollection_Counter_Label)
        Controls.Add(Label3)
        Controls.Add(End_Time_TextBox)
        Controls.Add(Label2)
        Controls.Add(Start_Time_TextBox)
        Controls.Add(Label1)
        Controls.Add(Searching_Status_Label)
        Controls.Add(Reveal_Crawler_Result_Dir_Button)
        Controls.Add(Start_Crawling_Button)
        Controls.Add(MainWebView2)
        Name = "Form1"
        Text = "Form1"
        CType(MainWebView2, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents MainWebView2 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents Start_Crawling_Button As Button
    Friend WithEvents Reveal_Crawler_Result_Dir_Button As Button
    Friend WithEvents Searching_Status_Label As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Start_Time_TextBox As TextBox
    Friend WithEvents End_Time_TextBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents JobCollection_Counter_Label As Label
    Friend WithEvents Jobcat_Progress_Count_Label As Label
End Class
