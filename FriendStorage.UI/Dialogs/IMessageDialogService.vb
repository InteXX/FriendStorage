Namespace FriendStorage.UI.Dialogs
  Public Interface IMessageDialogService
    Function ShowYesNoDialog(Message As String, Title As String) As MessageDialogResult
  End Interface



  Public Enum MessageDialogResult
    Yes
    No
  End Enum
End Namespace
