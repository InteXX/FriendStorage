Imports FriendStorage.Model

Namespace FriendStorage.UI.DataProviders
  Public Interface IFriendDataProvider
    Function GetFriendById(Id As Integer) As [Friend]
    Sub SaveFriend([Friend] As [Friend])
    Sub DeleteFriend(Id As Integer)
  End Interface
End Namespace
