Imports System
Imports System.Windows.Input

Namespace FriendStorage.UI.Commands
  Public Class DelegateCommand
    Implements ICommand

    Public Sub New(Execute As Action(Of Object))
      Me.New(Execute, Nothing)
    End Sub



    Public Sub New(Execute As Action(Of Object), CanExecute As Func(Of Object, Boolean))
      If Execute Is Nothing Then
        Throw New ArgumentNullException(NameOf(Execute))
      Else
        Me._Execute = Execute
        Me._CanExecute = CanExecute
      End If
    End Sub



    Public Function CanExecute(Parameter As Object) As Boolean Implements ICommand.CanExecute
      Return Me._CanExecute Is Nothing OrElse Me._CanExecute(Parameter)
    End Function



    Public Sub Execute(Parameter As Object) Implements ICommand.Execute
      Me._Execute(Parameter)
    End Sub



    Public Sub RaiseCanExecuteChanged()
      RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
    End Sub



    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Private ReadOnly _Execute As Action(Of Object)
    Private ReadOnly _CanExecute As Func(Of Object, Boolean)
  End Class
End Namespace
