Imports FriendStorage.Model
Imports FriendStorage.UI.DataProviders
Imports FriendStorage.UI.Dialogs
Imports FriendStorage.UI.Events
Imports FriendStorage.UI.Tests.Extensions
Imports FriendStorage.UI.ViewModels
Imports FriendStorage.UI.Wrappers
Imports Moq
Imports Prism.Events
Imports Xunit

Namespace FriendStorage.UI.Tests.ViewModels
  Public Class FriendEditViewModelTests
    Public Sub New()
      Me.FriendDeletedEventMock = New Mock(Of FriendDeletedEvent)
      Me.FriendSavedEventMock = New Mock(Of FriendSavedEvent)
      Me.EventAggregatorMock = New Mock(Of IEventAggregator)
      Me.DataProviderMock = New Mock(Of IFriendDataProvider)

      Me.EventAggregatorMock.
        Setup(Function(Ea) Ea.GetEvent(Of FriendDeletedEvent)).
        Returns(Me.FriendDeletedEventMock.Object)

      Me.EventAggregatorMock.
        Setup(Function(Ea) Ea.GetEvent(Of FriendSavedEvent)).
        Returns(Me.FriendSavedEventMock.Object)

      Me.DataProviderMock.
        Setup(Function(Dp) Dp.GetFriendById(FRIEND_ID)).
        Returns(New [Friend] With {.Id = FRIEND_ID, .FirstName = "Thomas"})

      Me.MessageDialogServiceMock = New Mock(Of IMessageDialogService)

      Me.ViewModel = New FriendEditViewModel(Me.DataProviderMock.Object, Me.EventAggregatorMock.Object, Me.MessageDialogServiceMock.Object)
    End Sub



    <Fact>
    Public Sub ShouldLoadFriend()
      Me.ViewModel.Load(FRIEND_ID)

      Assert.NotNull(Me.ViewModel.Friend)
      Assert.Equal(FRIEND_ID, Me.ViewModel.Friend.Id)

      Me.DataProviderMock.Verify(Function(DataProvider) DataProvider.GetFriendById(FRIEND_ID), Times.Once)
    End Sub



    <Fact>
    Public Sub ShouldRaisePropertyChangedEventForFriend()
      Dim lFired As Boolean
      Dim sName As String

      sName = NameOf(Me.ViewModel.Friend)
      lFired = Me.ViewModel.IsPropertyChangedFired(sName, Sub() Me.ViewModel.Load(FRIEND_ID))

      Assert.True(lFired)
    End Sub



    <Fact>
    Public Sub ShouldDisableSaveCommandWhenFriendIsLoaded()
      Me.ViewModel.Load(FRIEND_ID)

      Assert.False(Me.ViewModel.SaveCommand.CanExecute(Nothing))
    End Sub



    <Fact>
    Public Sub ShouldDisableSaveCommandWithoutLoad()
      Assert.False(Me.ViewModel.SaveCommand.CanExecute(Nothing))
    End Sub



    <Fact>
    Public Sub ShouldEnableSaveCommandWhenFriendIsChanged()
      Me.ViewModel.Load(FRIEND_ID)
      Me.ViewModel.Friend.FirstName = "Changed"

      Assert.True(Me.ViewModel.SaveCommand.CanExecute(Nothing))
    End Sub



    <Fact>
    Public Sub ShouldRaiseCanExecuteChangedForSaveCommandWhenFriendIsChanged()
      Dim oHandler As EventHandler
      Dim lFired As Boolean

      oHandler = Sub(Sender, e) lFired = True

      Me.ViewModel.Load(FRIEND_ID)

      AddHandler Me.ViewModel.SaveCommand.CanExecuteChanged, oHandler
      Me.ViewModel.Friend.FirstName = "Changed"
      RemoveHandler Me.ViewModel.SaveCommand.CanExecuteChanged, oHandler

      Assert.True(lFired)
    End Sub



    <Fact>
    Public Sub ShouldRaiseCanExecuteChangedForSaveCommandAfterLoad()
      Dim oHandler As EventHandler
      Dim lFired As Boolean

      oHandler = Sub(Sender, e) lFired = True

      AddHandler Me.ViewModel.SaveCommand.CanExecuteChanged, oHandler
      Me.ViewModel.Load(FRIEND_ID)
      RemoveHandler Me.ViewModel.SaveCommand.CanExecuteChanged, oHandler

      Assert.True(lFired)
    End Sub



    <Fact>
    Public Sub ShouldRaiseCanExecuteChangedForDeleteCommandAfterLoad()
      Dim oHandler As EventHandler
      Dim lFired As Boolean

      oHandler = Sub(Sender, e) lFired = True

      AddHandler Me.ViewModel.DeleteCommand.CanExecuteChanged, oHandler
      Me.ViewModel.Load(FRIEND_ID)
      RemoveHandler Me.ViewModel.DeleteCommand.CanExecuteChanged, oHandler

      Assert.True(lFired)
    End Sub



    <Fact>
    Public Sub ShouldCallSaveMethodOfDataProviderWhenSaveCommandIsExecuted()
      Me.ViewModel.Load(FRIEND_ID)
      Me.ViewModel.Friend.FirstName = "Changed"
      Me.ViewModel.SaveCommand.Execute(Nothing)

      Me.DataProviderMock.Verify(Sub(Dp) Dp.SaveFriend(Me.ViewModel.Friend.Model), Times.Once)
    End Sub



    <Fact>
    Public Sub ShouldAcceptChangesWhenSaveCommandIsExecuted()
      Me.ViewModel.Load(FRIEND_ID)
      Me.ViewModel.Friend.FirstName = "Changed"
      Me.ViewModel.SaveCommand.Execute(Nothing)

      Assert.False(Me.ViewModel.Friend.IsChanged)
    End Sub



    <Fact>
    Public Sub ShouldPublishFriendSavedEventWhenSaveCommandIsExecuted()
      Me.ViewModel.Load(FRIEND_ID)
      Me.ViewModel.Friend.FirstName = "Changed"
      Me.ViewModel.SaveCommand.Execute(Nothing)

      Me.FriendSavedEventMock.Verify(Sub(E) E.Publish(Me.ViewModel.Friend.Model), Times.Once)
    End Sub



    <Fact>
    Public Sub ShouldCreateNewFriendWhenNothingIsPassedToLoadMethod()
      Me.ViewModel.Load(Nothing)

      Assert.NotNull(Me.ViewModel.Friend)
      Assert.Equal(0, Me.ViewModel.Friend.Id)
      Assert.Null(Me.ViewModel.Friend.FirstName)
      Assert.Null(Me.ViewModel.Friend.LastName)
      Assert.Null(Me.ViewModel.Friend.Birthday)
      Assert.False(Me.ViewModel.Friend.IsDeveloper)

      Me.DataProviderMock.Verify(Function(Dp) Dp.GetFriendById(It.IsAny(Of Integer)), Times.Never)
    End Sub



    <Fact>
    Public Sub ShouldEnableDeleteCommandForExistingFriend()
      Me.ViewModel.Load(FRIEND_ID)
      Assert.True(Me.ViewModel.DeleteCommand.CanExecute(Nothing))
    End Sub



    <Fact>
    Public Sub ShouldDisableDeleteCommandForNewFriend()
      Me.ViewModel.Load(Nothing)
      Assert.False(Me.ViewModel.DeleteCommand.CanExecute(Nothing))
    End Sub



    <Fact>
    Public Sub ShouldDisableDeleteCommandWithoutLoad()
      Assert.False(Me.ViewModel.DeleteCommand.CanExecute(Nothing))
    End Sub



    <Fact>
    Public Sub ShouldRaiseCanExecuteChangedForDeleteCommandWhenAcceptingChanges()
      Dim oHandler As EventHandler
      Dim lFired As Boolean

      oHandler = Sub(Sender, e) lFired = True

      Me.ViewModel.Load(FRIEND_ID)

      Me.ViewModel.Friend.FirstName = "Changed"
      AddHandler Me.ViewModel.DeleteCommand.CanExecuteChanged, oHandler
      Me.ViewModel.Friend.AcceptChanges()
      RemoveHandler Me.ViewModel.DeleteCommand.CanExecuteChanged, oHandler

      Assert.True(lFired)
    End Sub



    <Theory>
    <InlineData(MessageDialogResult.Yes, 1)>
    <InlineData(MessageDialogResult.No, 0)>
    Public Sub ShouldCallDeleteCommandWhenDeleteCommandIsExecuted(Result As MessageDialogResult, Invocations As Integer)
      Me.ViewModel.Load(FRIEND_ID)

      Me.MessageDialogServiceMock.
              Setup(Function(Ds) Ds.ShowYesNoDialog(It.IsAny(Of String), It.IsAny(Of String))).
              Returns(Result)

      Me.ViewModel.DeleteCommand.Execute(Nothing)
      Me.DataProviderMock.Verify(Sub(Dp) Dp.DeleteFriend(FRIEND_ID), Times.Exactly(Invocations))

      Me.MessageDialogServiceMock.
              Verify(Function(Ds) Ds.ShowYesNoDialog(It.IsAny(Of String), It.IsAny(Of String)), Times.Once)
    End Sub



    <Theory>
    <InlineData(MessageDialogResult.Yes, 1)>
    <InlineData(MessageDialogResult.No, 0)>
    Public Sub ShouldPublishFriendDeletedEventWhenDeleteCommandIsExecuted(Result As MessageDialogResult, Invocations As Integer)
      Me.ViewModel.Load(FRIEND_ID)

      Me.MessageDialogServiceMock.
              Setup(Function(Ds) Ds.ShowYesNoDialog(It.IsAny(Of String), It.IsAny(Of String))).
              Returns(Result)

      Me.ViewModel.DeleteCommand.Execute(Nothing)
      Me.FriendDeletedEventMock.Verify(Sub(Ev) Ev.Publish(FRIEND_ID), Times.Exactly(Invocations))

      Me.MessageDialogServiceMock.
              Verify(Function(Ds) Ds.ShowYesNoDialog(It.IsAny(Of String), It.IsAny(Of String)), Times.Once)
    End Sub



    <Fact>
    Public Sub ShouldDisplayCorrectMessageInDeleteDialog()
      Dim sMessage As String
      Dim oFriend As FriendWrapper
      Dim sTitle As String

      Me.ViewModel.Load(FRIEND_ID)

      oFriend = Me.ViewModel.Friend
      oFriend.FirstName = "Thomas"
      oFriend.LastName = "Huber"

      Me.ViewModel.DeleteCommand.Execute(Nothing)

      sMessage = $"Do you really want to delete the friend {oFriend.FirstName} {oFriend.LastName}?"
      sTitle = "Delete Friend"

      Me.MessageDialogServiceMock.Verify(Function(Ds) Ds.ShowYesNoDialog(sMessage, sTitle), Times.Once)
    End Sub



    Private ReadOnly MessageDialogServiceMock As Mock(Of IMessageDialogService)
    Private ReadOnly FriendDeletedEventMock As Mock(Of FriendDeletedEvent)
    Private ReadOnly FriendSavedEventMock As Mock(Of FriendSavedEvent)
    Private ReadOnly EventAggregatorMock As Mock(Of IEventAggregator)
    Private ReadOnly DataProviderMock As Mock(Of IFriendDataProvider)
    Private ReadOnly ViewModel As FriendEditViewModel

    Private Const FRIEND_ID As Integer = 55
  End Class
End Namespace
