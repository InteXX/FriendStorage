Imports System
Imports System.Collections.Generic
Imports FriendStorage.Model

Namespace FriendStorage.DataAccess
  Public Interface IDataService
    Inherits IDisposable

    Function GetAllFriends() As IEnumerable(Of LookupItem)
    Function GetFriendById(FriendId As Integer) As [Friend]
    Sub SaveFriend([Friend] As [Friend])
    Sub DeleteFriend(FriendId As Integer)
  End Interface
End Namespace
