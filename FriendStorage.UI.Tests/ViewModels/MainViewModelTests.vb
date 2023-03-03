Imports System.Linq.Expressions
Imports System.Reflection
Imports FriendStorage.Model
Imports FriendStorage.UI.Events
Imports FriendStorage.UI.Tests.Extensions
Imports FriendStorage.UI.ViewModels
Imports FriendStorage.UI.Wrappers
Imports Moq
Imports Prism.Events
Imports Xunit

Namespace FriendStorage.UI.Tests.ViewModels
  Public Class MainViewModelTests
    Public Sub New()
      Me.FriendEditViewModelMocks = New List(Of Mock(Of IFriendEditViewModel))
      Me.OpenFriendEditViewEvent = New OpenFriendEditViewEvent
      Me.EventAggregatorMock = New Mock(Of IEventAggregator)
      Me.FriendDeletedEvent = New FriendDeletedEvent

      Me.EventAggregatorMock.
        Setup(Function(Ea) Ea.GetEvent(Of OpenFriendEditViewEvent)).
        Returns(Me.OpenFriendEditViewEvent)

      Me.EventAggregatorMock.
        Setup(Function(Ea) Ea.GetEvent(Of FriendDeletedEvent)).
        Returns(Me.FriendDeletedEvent)

      Me.ViewModelMock = New Mock(Of INavigationViewModel)
      Me.ViewModel = New MainViewModel(AddressOf Me.CreateFriendEditViewModel, Me.ViewModelMock.Object, Me.EventAggregatorMock.Object)
    End Sub



    <Fact>
    Public Sub ShouldCallTheLoadMethodOfTheNavigationViewModel()
      Me.ViewModel.Load()
      Me.ViewModelMock.Verify(Sub(Vm) Vm.Load(), Times.Once)
    End Sub



    <Fact>
    Public Sub ShouldAddFriendEditViewModelAndLoadAndSelectIt()
      Const FRIEND_ID As Integer = 7

      Me.OpenFriendEditViewEvent.Publish(FRIEND_ID)

      Assert.Single(Me.ViewModel.FriendEditViewModels)
      Assert.Equal(Me.ViewModel.FriendEditViewModels.First, Me.ViewModel.SelectedFriendEditViewModel)

      Me.FriendEditViewModelMocks.First.Verify(Sub(Vm) Vm.Load(FRIEND_ID), Times.Once)
    End Sub



    <Fact>
    Public Sub ShouldAddFriendEditViewModelAndLoadItWithIdNullAndSelectIt()
      Me.ViewModel.AddFriendCommand.Execute(Nothing)

      Assert.Single(Me.ViewModel.FriendEditViewModels)
      Assert.Equal(Me.ViewModel.FriendEditViewModels.First, Me.ViewModel.SelectedFriendEditViewModel)

      Me.FriendEditViewModelMocks.First.Verify(Sub(Vm) Vm.Load(Nothing), Times.Once)
    End Sub



    <Fact>
    Public Sub ShouldAddFriendEditViewModelsOnlyOnce()
      Me.OpenFriendEditViewEvent.Publish(5)
      Me.OpenFriendEditViewEvent.Publish(5)
      Me.OpenFriendEditViewEvent.Publish(6)
      Me.OpenFriendEditViewEvent.Publish(7)
      Me.OpenFriendEditViewEvent.Publish(7)

      Assert.Equal(3, Me.ViewModel.FriendEditViewModels.Count)
    End Sub



    Private Function CreateFriendEditViewModelUnwrapped() As IFriendEditViewModel
      Dim aParameterExpressions As ParameterExpression()
      Dim aArgumentExpressions As UnaryExpression()
      Dim oMethodExpression1 As MethodCallExpression
      Dim oMethodExpression2 As MethodCallExpression
      Dim oIsAnyMethodInfo As MethodInfo
      Dim oLoadMethodInfo As MethodInfo
      Dim aExpressions As Expression()
      Dim sIsAnyName As String
      Dim oMoqItType As Type
      Dim oCallback As Action(Of Integer?)
      Dim sLoadName As String
      Dim aIntTypes As Type()
      Dim oWrapper As FriendWrapper
      Dim oIntType As Type
      Dim oSetup1 As Expression(Of Action(Of IFriendEditViewModel))
      Dim oSetup2 As Expression(Of Func(Of IFriendEditViewModel, FriendWrapper))
      Dim oFriend As [Friend]
      Dim sVmName As String
      Dim oVmType As Type
      Dim oMock As Mock(Of IFriendEditViewModel)

      oMock = New Mock(Of IFriendEditViewModel)
      sVmName = NameOf(FriendEditViewModel)
      oVmType = GetType(IFriendEditViewModel)
      oIntType = GetType(Integer?)
      aIntTypes = {oIntType}
      sLoadName = NameOf(IFriendEditViewModel.Load)
      sIsAnyName = NameOf(It.IsAny)
      oMoqItType = GetType(It)
      aExpressions = Array.Empty(Of Expression)
      oLoadMethodInfo = oVmType.GetMethod(sLoadName, aIntTypes)
      oIsAnyMethodInfo = oMoqItType.GetMethod(sIsAnyName).MakeGenericMethod(aIntTypes)
      oMethodExpression1 = Expression.Call(Nothing, oIsAnyMethodInfo, aExpressions)
      aArgumentExpressions = {Expression.Convert(oMethodExpression1, oIntType)}
      aParameterExpressions = {Expression.Parameter(oVmType, sVmName)}
      oMethodExpression2 = Expression.Call(aParameterExpressions.First, oLoadMethodInfo, aArgumentExpressions)
      oSetup1 = Expression.Lambda(Of Action(Of IFriendEditViewModel))(oMethodExpression2, aParameterExpressions)
      oSetup2 = Function(Vm) Vm.Friend
      oCallback = Sub(FriendId)
                    oFriend = New [Friend] With {.Id = FriendId.Value}
                    oWrapper = New FriendWrapper(oFriend)
                    oMock.Setup(oSetup2).Returns(oWrapper)
                  End Sub

      oMock.Setup(oSetup1).Callback(oCallback)
      Me.FriendEditViewModelMocks.Add(oMock)

      Return oMock.Object
    End Function



    Private Function CreateFriendEditViewModel() As IFriendEditViewModel
      Dim oCallback As Action(Of Integer?)
      Dim oWrapper As FriendWrapper
      Dim oFriend As [Friend]
      Dim oSetup1 As Expression(Of Action(Of IFriendEditViewModel))
      Dim oSetup2 As Expression(Of Func(Of IFriendEditViewModel, FriendWrapper))
      Dim oMock As Mock(Of IFriendEditViewModel)

      oMock = New Mock(Of IFriendEditViewModel)
      oSetup1 = Sub(Vm) Vm.Load(It.IsAny(Of Integer?))
      oSetup2 = Function(Vm) Vm.Friend
      oCallback = Sub(FriendId)
                    oFriend = New [Friend] With {.Id = FriendId.GetValueOrDefault}
                    oWrapper = New FriendWrapper(oFriend)
                    oMock.Setup(oSetup2).Returns(oWrapper)
                  End Sub

      oMock.Setup(oSetup1).Callback(oCallback)
      Me.FriendEditViewModelMocks.Add(oMock)

      Return oMock.Object
    End Function



    <Fact>
    Public Sub ShouldRaisePropertyChangedEventForSelectedFriendEditViewModel()
      Dim lFired As Boolean
      Dim sName As String

      sName = NameOf(Me.ViewModel.SelectedFriendEditViewModel)
      lFired = Me.ViewModel.IsPropertyChangedFired(sName, Sub()
                                                            With New Mock(Of IFriendEditViewModel)
                                                              Me.ViewModel.SelectedFriendEditViewModel = .Object
                                                            End With
                                                          End Sub)

      Assert.True(lFired)
    End Sub



    <Fact>
    Public Sub ShouldRemoveFriendEditViewModelOnCloseFriendTabCommand()
      Dim oFriendEditViewModel As IFriendEditViewModel

      Me.OpenFriendEditViewEvent.Publish(7)
      oFriendEditViewModel = Me.ViewModel.SelectedFriendEditViewModel
      Me.ViewModel.CloseFriendTabCommand.Execute(oFriendEditViewModel)

      Assert.Empty(Me.ViewModel.FriendEditViewModels)
    End Sub



    <Fact>
    Public Sub ShouldRemoveFriendEditViewModelWhenFriendIsDeleted()
      Dim iFriendId As Integer

      iFriendId = 7

      Me.OpenFriendEditViewEvent.Publish(iFriendId)
      Me.OpenFriendEditViewEvent.Publish(8)
      Me.OpenFriendEditViewEvent.Publish(9)

      Me.FriendDeletedEvent.Publish(iFriendId)

      Assert.Equal(2, Me.ViewModel.FriendEditViewModels.Count)
      Assert.False(Me.ViewModel.FriendEditViewModels.Any(Function(F) F.Friend.Id = iFriendId))
    End Sub



    Private ReadOnly FriendEditViewModelMocks As List(Of Mock(Of IFriendEditViewModel))
    Private ReadOnly OpenFriendEditViewEvent As OpenFriendEditViewEvent
    Private ReadOnly EventAggregatorMock As Mock(Of IEventAggregator)
    Private ReadOnly FriendDeletedEvent As FriendDeletedEvent
    Private ReadOnly ViewModelMock As Mock(Of INavigationViewModel)
    Private ReadOnly ViewModel As MainViewModel
  End Class
End Namespace
