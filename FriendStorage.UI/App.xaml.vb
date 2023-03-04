Imports System.Windows
Imports Autofac
Imports FriendStorage.UI.Startup
Imports FriendStorage.UI.Views

Namespace FriendStorage.UI
  Partial Public Class App
    Private Sub App_Startup(Sender As App, e As StartupEventArgs) Handles Me.Startup
      Dim oBootStrapper As BootStrapper
      Dim oContainer As IContainer
      Dim oMainView As MainView

      oBootStrapper = New BootStrapper
      oContainer = oBootStrapper.GetContainer
      'oMainView = oContainer.Resolve(Of MainView)
      'oMainView.Show()

      Using oScope As ILifetimeScope = oContainer.BeginLifetimeScope
        oMainView = oScope.Resolve(Of MainView)
        oMainView.Show()
      End Using
    End Sub
  End Class
End Namespace
