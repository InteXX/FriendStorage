Imports FriendStorage.UI.Tests.Extensions
Imports FriendStorage.UI.Events
Imports FriendStorage.UI.ViewModels
Imports Moq
Imports Prism.Events
Imports Xunit

Namespace FriendStorage.UI.Tests.ViewModels
  Public Class NavigationItemViewModelTests
    Public Sub New()
      Me.EventAggregatorMock = New Mock(Of IEventAggregator)
      Me.ViewModel = New NavigationItemViewModel(FRIEND_ID, "Thomas", Me.EventAggregatorMock.Object)
    End Sub



    <Fact>
    Public Sub ShouldPublishOpenFriendEditViewEvent()
      Dim oEventMock As Mock(Of OpenFriendEditViewEvent)

      oEventMock = New Mock(Of OpenFriendEditViewEvent)

      Me.EventAggregatorMock.
        Setup(Function(Ea) Ea.GetEvent(Of OpenFriendEditViewEvent)).
        Returns(oEventMock.Object)

      Me.ViewModel.OpenFriendEditViewCommand.Execute(Nothing)

      oEventMock.Verify(Sub(E) E.Publish(FRIEND_ID), Times.Once)
    End Sub



    <Fact>
    Public Sub ShouldRaisePropertyChangedEventForDisplayMember()
      Dim lFired As Boolean
      Dim sName As String

      sName = NameOf(Me.ViewModel.DisplayMember)
      lFired = Me.ViewModel.IsPropertyChangedFired(sName, Sub()
                                                            Me.ViewModel.DisplayMember = "Changed"
                                                          End Sub)

      Assert.True(lFired)
    End Sub



    Private ReadOnly EventAggregatorMock As Mock(Of IEventAggregator)
    Private ReadOnly ViewModel As NavigationItemViewModel

    Private Const FRIEND_ID As Integer = 7
  End Class
End Namespace
