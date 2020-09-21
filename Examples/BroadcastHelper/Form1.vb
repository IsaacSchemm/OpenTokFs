Imports OpenTokFs
Imports OpenTokFs.RequestTypes
Imports OpenTokFs.ResponseTypes

Public Class Form1
    Implements IOpenTokCredentials

    Public ReadOnly Property ApiKey As Integer Implements IOpenTokCredentials.ApiKey
        Get
            Return Integer.Parse(TxtApiKey.Text)
        End Get
    End Property

    Public ReadOnly Property ApiSecret As String Implements IOpenTokCredentials.ApiSecret
        Get
            Return TxtApiSecret.Text
        End Get
    End Property

    Private Async Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        BtnRefresh.Enabled = False

        Try
            ListBox1.Items.Clear()

            Dim list = Await Api.Broadcast.ListAllAsync(Me, 100, OpenTokSessionId.Any)
            If list.Length >= 100 Then
                MsgBox("There are 100 or more items in the list. Only showing the top 100 items.")
            End If

            For Each x In list
                ListBox1.Items.Add(x)
            Next
        Catch ex As Exception
            Console.Error.WriteLine(ex)
            MsgBox(ex.Message)
        End Try

        BtnRefresh.Enabled = True
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim item As OpenTokBroadcast = ListBox1.SelectedItem
        If item Is Nothing Then
            PropertyGrid1.SelectedObject = Nothing

            BtnStop.Enabled = False
        Else
            PropertyGrid1.SelectedObject = item

            BtnStop.Enabled = item.Status = "started"
        End If
    End Sub

    Private Async Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        BtnStart.Enabled = False

        Try
            Dim req = New OpenTokBroadcastStartRequest(TxtNewBroadcastSessionId.Text) With {
                .Resolution = If(RadioHD.Checked, "1280x720", "640x480"),
                .Hls = True
            }
            Dim newBroadcast = Await Api.Broadcast.StartAsync(Me, req)
            ListBox1.Items.Insert(0, newBroadcast)
        Catch ex As Exception
            Console.Error.WriteLine(ex)
            MsgBox(ex.Message)
        End Try

        BtnStart.Enabled = True
    End Sub

    Private Async Sub BtnStop_Click(sender As Object, e As EventArgs) Handles BtnStop.Click
        BtnStop.Enabled = False

        Try
            Dim item As OpenTokBroadcast = ListBox1.SelectedItem
            Dim updated = Await Api.Broadcast.StopAsync(Me, item.Id)

            Dim index = ListBox1.SelectedIndex
            ListBox1.Items.RemoveAt(index)
            ListBox1.Items.Insert(index, updated)
        Catch ex As Exception
            Console.Error.WriteLine(ex)
            MsgBox(ex.Message)
        End Try

        BtnStop.Enabled = True
    End Sub
End Class
