Imports System.Windows
Imports FriendStorage.UI.ViewModels

Namespace FriendStorage.UI.Views
  Partial Public Class MainView
    Inherits Window

    Public Sub New(ViewModel As MainViewModel)
      Me.InitializeComponent()

      Me.ViewModel = ViewModel
      Me.DataContext = Me.ViewModel
    End Sub



    Private Sub MainView_Loaded(Sender As Object, e As RoutedEventArgs) Handles Me.Loaded
      Me.ViewModel.Load()
    End Sub



    Private ReadOnly ViewModel As MainViewModel
  End Class
End Namespace
