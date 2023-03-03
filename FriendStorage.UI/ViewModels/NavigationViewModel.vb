Imports System.Collections.ObjectModel
Imports System.Linq
Imports FriendStorage.Model
Imports FriendStorage.UI.DataProviders
Imports FriendStorage.UI.Events
Imports MoreLinq
Imports Prism.Events

Namespace FriendStorage.UI.ViewModels
  Public Class NavigationViewModel
    Inherits ViewModel
    Implements INavigationViewModel

    Public Sub New(DataProvider As INavigationDataProvider, EventAggregator As IEventAggregator)
      Me.EventAggregator = EventAggregator
      Me.DataProvider = DataProvider
      Me.Friends = New ObservableCollection(Of NavigationItemViewModel)

      Me.EventAggregator.GetEvent(Of FriendDeletedEvent).Subscribe(AddressOf Me.OnFriendDeleted)
      Me.EventAggregator.GetEvent(Of FriendSavedEvent).Subscribe(AddressOf Me.OnFriendSaved)
    End Sub



    Public Sub Load() Implements INavigationViewModel.Load
      Dim oViewModel As NavigationItemViewModel

      Me.Friends.Clear()
      Me.DataProvider.GetAllFriends.ForEach(Sub(F)
                                              oViewModel = New NavigationItemViewModel(F.Id, F.DisplayMember, Me.EventAggregator)
                                              Me.Friends.Add(oViewModel)
                                            End Sub)
    End Sub



    Private Sub OnFriendDeleted(FriendId As Integer)
      Dim oNavigationItem As NavigationItemViewModel

      oNavigationItem = Me.Friends.Single(Function(F) F.Id = FriendId)

      Me.Friends.Remove(oNavigationItem)
    End Sub



    Private Sub OnFriendSaved([Friend] As [Friend])
      Dim oNavigationItem As NavigationItemViewModel
      Dim sDisplayMember As String

      oNavigationItem = Me.Friends.SingleOrDefault(Function(F) F.Id = [Friend].Id)
      sDisplayMember = $"{[Friend].FirstName} {[Friend].LastName}"

      If oNavigationItem Is Nothing Then
        oNavigationItem = New NavigationItemViewModel([Friend].Id, sDisplayMember, Me.EventAggregator)
        Me.Friends.Add(oNavigationItem)
      Else
        oNavigationItem.DisplayMember = sDisplayMember
      End If
    End Sub



    Public ReadOnly Property Friends() As ObservableCollection(Of NavigationItemViewModel)

    Private ReadOnly EventAggregator As IEventAggregator
    Private ReadOnly DataProvider As INavigationDataProvider
  End Class
End Namespace
