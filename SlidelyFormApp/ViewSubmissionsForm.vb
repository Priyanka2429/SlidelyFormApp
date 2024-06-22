Public Class ViewSubmissionsForm
    Private apiClient As ApiClient

    Public Sub New()
        InitializeComponent()
        apiClient = New ApiClient()
        LoadSubmissions()
    End Sub

    Private Async Sub LoadSubmissions()
        Try
            Dim response As String = Await apiClient.GetSubmissionsAsync()
            ' Deserialize response and populate form fields
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class
