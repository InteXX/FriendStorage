Imports System
Imports FriendStorage.DataAccess
Imports FriendStorage.Model

Namespace FriendStorage.UI.DataProviders
  Public Class FriendDataProvider
    Implements IFriendDataProvider

    Public Sub New(DataServiceCreator As Func(Of IDataService))
      Me.DataServiceCreator = DataServiceCreator
    End Sub



    Public Function GetFriendById(Id As Integer) As [Friend] Implements IFriendDataProvider.GetFriendById
      Using oDataService As IDataService = Me.DataServiceCreator.Invoke
        Return oDataService.GetFriendById(Id)
      End Using
    End Function



    Public Sub SaveFriend([Friend] As [Friend]) Implements IFriendDataProvider.SaveFriend
      Using oDataService As IDataService = Me.DataServiceCreator.Invoke
        oDataService.SaveFriend([Friend])
      End Using
    End Sub



    Public Sub DeleteFriend(Id As Integer) Implements IFriendDataProvider.DeleteFriend
      Using oDataService As IDataService = Me.DataServiceCreator.Invoke
        oDataService.DeleteFriend(Id)
      End Using
    End Sub



    Private ReadOnly DataServiceCreator As Func(Of IDataService)
  End Class
End Namespace
