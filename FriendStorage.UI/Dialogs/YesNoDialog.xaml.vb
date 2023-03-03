Namespace FriendStorage.UI.Dialogs
  Public Class YesNoDialog
    Public Sub New(Message As String, Title As String)
      Me.InitializeComponent()

      Me.Message.Text = Message
      Me.Title = Title
    End Sub



    Private Sub ButtonYes_Click(Sender As Object, e As System.Windows.RoutedEventArgs)
      Me.DialogResult = True
    End Sub



    Private Sub ButtonNo_Click(Sender As Object, e As System.Windows.RoutedEventArgs)
      Me.DialogResult = False
    End Sub
  End Class
End Namespace
