Imports System.Collections.Generic
Imports FriendStorage.Model

Namespace FriendStorage.UI.DataProviders
  Public Interface INavigationDataProvider
    Function GetAllFriends() As IEnumerable(Of LookupItem)
  End Interface
End Namespace
