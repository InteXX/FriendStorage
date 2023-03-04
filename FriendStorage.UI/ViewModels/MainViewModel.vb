Imports System
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Windows.Input
Imports FriendStorage.UI.Commands
Imports FriendStorage.UI.Events
Imports Prism.Events

Namespace FriendStorage.UI.ViewModels
  Public Class MainViewModel
    Inherits ViewModel

    Public Sub New(
                  FriendEditViewModelCreator As Func(Of IFriendEditViewModel),
                  NavigationViewModel As INavigationViewModel,
                  EventAggregator As IEventAggregator)

      Me.FriendEditViewModelCreator = FriendEditViewModelCreator
      Me.FriendEditViewModels = New ObservableCollection(Of IFriendEditViewModel)
      Me.NavigationViewModel = NavigationViewModel
      Me.CloseFriendTabCommand = New DelegateCommand(AddressOf Me.OnCloseFriendTabExecute)
      Me.AddFriendCommand = New DelegateCommand(AddressOf Me.OnAddFriendExecute)

      EventAggregator.GetEvent(Of OpenFriendEditViewEvent).Subscribe(AddressOf Me.OnOpenFriendEditView)
      EventAggregator.GetEvent(Of FriendDeletedEvent).Subscribe(AddressOf Me.OnFriendDeleted)
    End Sub



    Public Sub Load()
      Me.NavigationViewModel.Load()
    End Sub



    Public Property SelectedFriendEditViewModel() As IFriendEditViewModel
      Get
        Return Me._SelectedFriendEditViewModel
      End Get
      Set(Value As IFriendEditViewModel)
        Me._SelectedFriendEditViewModel = Value
        Me.OnPropertyChanged()
      End Set
    End Property
    Private _SelectedFriendEditViewModel As IFriendEditViewModel



    Private Sub OnOpenFriendEditView(FriendId As Integer)
      Dim oFriendEditViewModel As IFriendEditViewModel

      oFriendEditViewModel = Me.FriendEditViewModels.SingleOrDefault(Function(ViewModel)
                                                                       Return ViewModel.Friend.Id = FriendId
                                                                     End Function)

      If oFriendEditViewModel Is Nothing Then
        oFriendEditViewModel = Me.CreateAndLoadFriendEditViewModel(FriendId)
      End If

      Me.SelectedFriendEditViewModel = oFriendEditViewModel
    End Sub



    Private Sub OnAddFriendExecute(Argument As Object)
      Me.SelectedFriendEditViewModel = Me.CreateAndLoadFriendEditViewModel(Nothing)
    End Sub



    Private Sub OnCloseFriendTabExecute(Argument As Object)
      Dim oFriendEditViewModel As IFriendEditViewModel

      oFriendEditViewModel = Argument

      Me.FriendEditViewModels.Remove(oFriendEditViewModel)
    End Sub



    Private Sub OnFriendDeleted(FriendId As Integer)
      Dim oViewModel As IFriendEditViewModel

      oViewModel = Me.FriendEditViewModels.Single(Function(Vm) Vm.Friend.Id = FriendId)

      Me.FriendEditViewModels.Remove(oViewModel)
    End Sub



    Private Function CreateAndLoadFriendEditViewModel(FriendId As Integer?) As IFriendEditViewModel
      Dim oFriendEditViewModel As IFriendEditViewModel

      oFriendEditViewModel = Me.FriendEditViewModelCreator.Invoke

      Me.FriendEditViewModels.Add(oFriendEditViewModel)
      oFriendEditViewModel.Load(FriendId)

      Return oFriendEditViewModel
    End Function



    Public ReadOnly Property CloseFriendTabCommand As ICommand
    Public ReadOnly Property FriendEditViewModels As ObservableCollection(Of IFriendEditViewModel)
    Public ReadOnly Property NavigationViewModel As INavigationViewModel
    Public ReadOnly Property AddFriendCommand As ICommand

    Private ReadOnly FriendEditViewModelCreator As Func(Of IFriendEditViewModel)
  End Class
End Namespace
