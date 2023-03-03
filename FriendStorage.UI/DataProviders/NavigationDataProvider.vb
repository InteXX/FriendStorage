Imports System
Imports System.Collections.Generic
Imports FriendStorage.DataAccess
Imports FriendStorage.Model

Namespace FriendStorage.UI.DataProviders
  Public Class NavigationDataProvider
    Implements INavigationDataProvider

    Public Sub New(DataServiceCreator As Func(Of IDataService))
      Me.DataServiceCreator = DataServiceCreator
    End Sub



    Public Function GetAllFriends() As IEnumerable(Of LookupItem) Implements INavigationDataProvider.GetAllFriends
      Using oDataService As IDataService = Me.DataServiceCreator()
        Return oDataService.GetAllFriends
      End Using
    End Function



    Private ReadOnly DataServiceCreator As Func(Of IDataService)
  End Class
End Namespace
