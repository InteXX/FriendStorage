Imports FriendStorage.UI.Wrappers

Namespace FriendStorage.UI.ViewModels
  Public Interface IFriendEditViewModel
    Sub Load(FriendId As Integer?)
    ReadOnly Property [Friend] As FriendWrapper
  End Interface
End Namespace
