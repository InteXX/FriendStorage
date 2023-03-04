Imports Autofac
Imports FriendStorage.DataAccess
Imports FriendStorage.UI.DataProviders
Imports FriendStorage.UI.Dialogs
Imports FriendStorage.UI.ViewModels
Imports FriendStorage.UI.Views
Imports Prism.Events

Namespace FriendStorage.UI.Startup
  Public Class BootStrapper
    Public Function GetContainer() As IContainer
      With New ContainerBuilder
        .RegisterType(Of NavigationDataProvider).As(Of INavigationDataProvider)()
        .RegisterType(Of MessageDialogService).As(Of IMessageDialogService)()
        .RegisterType(Of FriendEditViewModel).As(Of IFriendEditViewModel)()
        .RegisterType(Of NavigationViewModel).As(Of INavigationViewModel)()
        .RegisterType(Of FriendDataProvider).As(Of IFriendDataProvider)()
        .RegisterType(Of EventAggregator).As(Of IEventAggregator).SingleInstance()
        .RegisterType(Of FileDataService).As(Of IDataService)()
        .RegisterType(Of MainViewModel).AsSelf
        .RegisterType(Of MainView).AsSelf.SingleInstance() ' <= Added SingleInstance to prevent disposal by the LifetimeScope
        '.RegisterType(Of MainView).AsSelf

        Return .Build
      End With
    End Function
  End Class
End Namespace
