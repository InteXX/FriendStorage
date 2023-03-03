Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Namespace FriendStorage.UI.Tests.Extensions
  Public Module NotifyPropertyChangedExtensions
    <Extension>
    Public Function IsPropertyChangedFired(Instance As INotifyPropertyChanged, PropertyName As String, Action As Action) As Boolean
      Dim oHandler As PropertyChangedEventHandler
      Dim lFired As Boolean

      oHandler = Sub(Sender, e) lFired = e.PropertyName = PropertyName

      AddHandler Instance.PropertyChanged, oHandler
      Action()
      RemoveHandler Instance.PropertyChanged, oHandler

      Return lFired
    End Function
  End Module
End Namespace
