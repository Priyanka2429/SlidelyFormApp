Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Public Class ApiClient
    Private ReadOnly client As HttpClient

    Public Sub New()
        client = New HttpClient()
        client.BaseAddress = New Uri("http://localhost:3000/") ' Your backend server URL
    End Sub

    Public Async Function GetSubmissionsAsync() As Task(Of String)
        Dim response As HttpResponseMessage = Await client.GetAsync("submissions")
        response.EnsureSuccessStatusCode()
        Dim responseBody As String = Await response.Content.ReadAsStringAsync()
        Return responseBody
    End Function

    Public Async Function CreateSubmissionAsync(ByVal submission As Submission) As Task(Of String)
        Dim content As StringContent = New StringContent(JsonConvert.SerializeObject(submission), Encoding.UTF8, "application/json")
        Dim response As HttpResponseMessage = Await client.PostAsync("submissions", content)
        response.EnsureSuccessStatusCode()
        Dim responseBody As String = Await response.Content.ReadAsStringAsync()
        Return responseBody
    End Function
End Class

Public Class Submission
    Public Property Name As String
    Public Property Email As String
    Public Property Phone As String
    Public Property GithubLink As String
    Public Property StopwatchTime As String
End Class
