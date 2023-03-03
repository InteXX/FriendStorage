Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Namespace FriendStorage.UI.ViewModels
  Public Class ViewModel
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Overridable Sub OnPropertyChanged(<CallerMemberName> Optional PropertyName As String = Nothing)
      RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
    End Sub
  End Class
End Namespace
