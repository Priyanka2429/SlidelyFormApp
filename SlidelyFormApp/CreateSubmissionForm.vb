Imports System.IO
Imports System.Net
Imports System.Text
Imports Newtonsoft.Json

Public Class CreateSubmissionForm
    Private stopwatchRunning As Boolean = False
    Private stopwatchStartTime As DateTime

    Private Sub btnToggleStopwatch_Click(sender As Object, e As EventArgs) Handles btnToggleStopwatch.Click
        If stopwatchRunning Then
            ' Stop the stopwatch
            stopwatchRunning = False
            UpdateStopwatchDisplay()
        Else
            ' Start the stopwatch
            stopwatchRunning = True
            stopwatchStartTime = DateTime.Now
            UpdateStopwatchDisplay()
        End If
    End Sub

    Private Sub UpdateStopwatchDisplay()
        If stopwatchRunning Then
            ' Calculate elapsed time since stopwatch started
            Dim elapsedTime As TimeSpan = DateTime.Now - stopwatchStartTime
            lblStopwatchTime.Text = elapsedTime.ToString("hh\:mm\:ss")
        Else
            ' Display current stopwatch time
            lblStopwatchTime.Text = "00:00:00"
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        ' Prepare submission data
        Dim submissionData As New SubmissionData()
        submissionData.name = txtName.Text
        submissionData.email = txtEmail.Text
        submissionData.phone = txtPhone.Text
        submissionData.github_link = txtGithubLink.Text
        submissionData.stopwatch_time = CalculateStopwatchSeconds()

        ' Convert submissionData to JSON
        Dim jsonData As String = JsonConvert.SerializeObject(submissionData)

        ' Send POST request to backend
        Dim url As String = "http://localhost:3000/submit"
        Dim request As HttpWebRequest = WebRequest.CreateHttp(url)
        request.Method = "POST"
        request.ContentType = "application/json"

        Try
            ' Write JSON data to request stream
            Using writer As New StreamWriter(request.GetRequestStream())
                writer.Write(jsonData)
                writer.Flush()
                writer.Close()
            End Using

            ' Get response
            Dim response As HttpWebResponse = request.GetResponse()
            Dim reader As New StreamReader(response.GetResponseStream())
            Dim jsonResponse As String = reader.ReadToEnd()

            ' Display success message
            MessageBox.Show("Submission successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear form fields
            ClearFormFields()

        Catch ex As Exception
            MessageBox.Show("Error submitting data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function CalculateStopwatchSeconds() As Integer
        If stopwatchRunning Then
            ' Calculate elapsed time since stopwatch started
            Dim elapsedTime As TimeSpan = DateTime.Now - stopwatchStartTime
            Return CInt(elapsedTime.TotalSeconds)
        Else
            Return 0
        End If
    End Function

    Private Sub ClearFormFields()
        txtName.Text = ""
        txtEmail.Text = ""
        txtPhone.Text = ""
        txtGithubLink.Text = ""
        lblStopwatchTime.Text = "00:00:00"
    End Sub

    ' Class to represent submission data structure
    Public Class SubmissionData
        Public Property name As String
        Public Property email As String
        Public Property phone As String
        Public Property github_link As String
        Public Property stopwatch_time As Integer
    End Class
End Class
