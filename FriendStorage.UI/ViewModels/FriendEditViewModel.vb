Imports System.ComponentModel
Imports System.Windows.Input
Imports FriendStorage.Model
Imports FriendStorage.UI.Commands
Imports FriendStorage.UI.DataProviders
Imports FriendStorage.UI.Dialogs
Imports FriendStorage.UI.Events
Imports FriendStorage.UI.Wrappers
Imports Prism.Events

Namespace FriendStorage.UI.ViewModels
  Public Class FriendEditViewModel
    Inherits ViewModel
    Implements IFriendEditViewModel

    Public Sub New(DataProvider As IFriendDataProvider, EventAggregator As IEventAggregator, MessageDialogService As IMessageDialogService)
      Me.MessageDialogService = MessageDialogService
      Me.EventAggregator = EventAggregator
      Me.DataProvider = DataProvider
      Me.DeleteCommand = New DelegateCommand(AddressOf Me.OnDeleteExecute, AddressOf Me.OnDeleteCanExecute)
      Me.SaveCommand = New DelegateCommand(AddressOf Me.OnSaveExecute, AddressOf Me.OnSaveCanExecute)
    End Sub



    Public Sub Load(FriendId As Integer?) Implements IFriendEditViewModel.Load
      Dim oWrapper As FriendWrapper
      Dim oFriend As [Friend]

      oFriend = If(FriendId.HasValue, Me.DataProvider.GetFriendById(FriendId.Value), New [Friend])
      oWrapper = New FriendWrapper(oFriend)

      Me.Friend = oWrapper

      Me.InvalidateCommands()
    End Sub



    Public Property [Friend] As FriendWrapper Implements IFriendEditViewModel.Friend
      Get
        Return Me.FriendWrapper
      End Get
      Private Set(Value As FriendWrapper)
        Me.FriendWrapper = Value
        Me.OnPropertyChanged()
      End Set
    End Property
    Private WithEvents FriendWrapper As FriendWrapper



    Private Sub OnSaveExecute(Argument As Object)
      Me.DataProvider.SaveFriend(Me.Friend.Model)
      Me.Friend.AcceptChanges()
      Me.EventAggregator.GetEvent(Of FriendSavedEvent).Publish(Me.Friend.Model)
    End Sub



    Private Function OnSaveCanExecute(Argument As Object) As Boolean
      Return Me.Friend IsNot Nothing AndAlso Me.Friend.IsChanged
    End Function



    Private Sub OnDeleteExecute(Argument As Object)
      Dim sMessage As String
      Dim sTitle As String
      Dim eResult As MessageDialogResult

      sMessage = $"Do you really want to delete the friend {Me.Friend.FirstName} {Me.Friend.LastName}?"
      sTitle = "Delete Friend"
      eResult = Me.MessageDialogService.ShowYesNoDialog(sMessage, sTitle)

      If eResult = MessageDialogResult.Yes Then
        Me.DataProvider.DeleteFriend(Me.Friend.Id)
        Me.EventAggregator.GetEvent(Of FriendDeletedEvent).Publish(Me.Friend.Id)
      End If
    End Sub



    Private Function OnDeleteCanExecute(Argument As Object) As Boolean
      Return Me.Friend IsNot Nothing AndAlso Me.Friend.Id > 0
    End Function



    Private Sub InvalidateCommands()
      DirectCast(Me.DeleteCommand, DelegateCommand).RaiseCanExecuteChanged()
      DirectCast(Me.SaveCommand, DelegateCommand).RaiseCanExecuteChanged()
    End Sub



    Private Sub Friend_PropertyChanged(Sender As FriendWrapper, e As PropertyChangedEventArgs) Handles FriendWrapper.PropertyChanged
      Me.InvalidateCommands()
    End Sub



    Public ReadOnly Property DeleteCommand As ICommand
    Public ReadOnly Property SaveCommand As ICommand

    Private ReadOnly MessageDialogService As IMessageDialogService
    Private ReadOnly EventAggregator As IEventAggregator
    Private ReadOnly DataProvider As IFriendDataProvider
  End Class
End Namespace
