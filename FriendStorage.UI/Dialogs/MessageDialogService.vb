Imports System.Windows

Namespace FriendStorage.UI.Dialogs
  Public Class MessageDialogService
    Implements IMessageDialogService

    Public Function ShowYesNoDialog(Message As String, Title As String) As MessageDialogResult Implements IMessageDialogService.ShowYesNoDialog
      Dim oYesNoDialog As YesNoDialog
      Dim lResponse As Boolean

      oYesNoDialog = New YesNoDialog(Message, Title) With {
        .WindowStartupLocation = WindowStartupLocation.CenterOwner,
        .Owner = App.Current.MainWindow
      }

      lResponse = oYesNoDialog.ShowDialog.GetValueOrDefault

      Return If(lResponse, MessageDialogResult.Yes, MessageDialogResult.No)
    End Function
  End Class
End Namespace
