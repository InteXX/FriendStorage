Imports System.Windows.Input
Imports FriendStorage.UI.Commands
Imports FriendStorage.UI.Events
Imports Prism.Events

Namespace FriendStorage.UI.ViewModels
  Public Class NavigationItemViewModel
    Inherits ViewModel

    Public Sub New(Id As Integer, DisplayMember As String, EventAggregator As IEventAggregator)
      Me.OpenFriendEditViewCommand = New DelegateCommand(AddressOf Me.OnFriendEditViewExecute)
      Me.EventAggregator = EventAggregator
      Me.DisplayMember = DisplayMember
      Me.Id = Id
    End Sub



    Public Property DisplayMember As String
      Get
        Return Me._DisplayMember
      End Get
      Set(Value As String)
        Me._DisplayMember = Value
        Me.OnPropertyChanged()
      End Set
    End Property
    Private _DisplayMember As String


    Private Sub OnFriendEditViewExecute(Argument As Object)
      Me.EventAggregator.GetEvent(Of OpenFriendEditViewEvent).Publish(Me.Id)
    End Sub



    Public ReadOnly Property OpenFriendEditViewCommand As ICommand
    Public ReadOnly Property Id As Integer

    Private ReadOnly EventAggregator As IEventAggregator
  End Class
End Namespace
