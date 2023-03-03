Imports System
Imports System.Runtime.CompilerServices
Imports FriendStorage.Model
Imports FriendStorage.UI.ViewModels

Namespace FriendStorage.UI.Wrappers
  Public Class FriendWrapper
    Inherits ViewModel

    Public Sub New([Friend] As [Friend])
      Me.Friend = [Friend]
    End Sub



    Public Sub AcceptChanges()
      Me.IsChanged = False
    End Sub



    Protected Overrides Sub OnPropertyChanged(<CallerMemberName> Optional PropertyName As String = Nothing)
      MyBase.OnPropertyChanged(PropertyName)

      If PropertyName <> NameOf(Me.IsChanged) Then
        Me.IsChanged = True
      End If
    End Sub



    Public ReadOnly Property Model() As [Friend]
      Get
        Return Me.Friend
      End Get
    End Property



    Public ReadOnly Property Id As Integer
      Get
        Return Me.Friend.Id
      End Get
    End Property



    Public Property FirstName() As String
      Get
        Return Me.Friend.FirstName
      End Get
      Set(Value As String)
        Me.Friend.FirstName = Value
        Me.OnPropertyChanged()
      End Set
    End Property




    Public Property LastName() As String
      Get
        Return Me.Friend.LastName
      End Get
      Set(Value As String)
        Me.Friend.LastName = Value
        Me.OnPropertyChanged()
      End Set
    End Property



    Public Property Birthday() As Date?
      Get
        Return Me.Friend.Birthday
      End Get
      Set(Value As Date?)
        Me.Friend.Birthday = Value
        Me.OnPropertyChanged()
      End Set
    End Property



    Public Property IsDeveloper() As Boolean
      Get
        Return Me.Friend.IsDeveloper
      End Get
      Set(ByVal value As Boolean)
        Me.Friend.IsDeveloper = value
        Me.OnPropertyChanged()
      End Set
    End Property



    Public Property IsChanged() As Boolean
      Get
        Return Me._IsChanged
      End Get
      Private Set(Value As Boolean)
        Me._IsChanged = Value
        Me.OnPropertyChanged()
      End Set
    End Property
    Private _IsChanged As Boolean



    Private ReadOnly [Friend] As [Friend]
  End Class
End Namespace
