Imports FriendStorage.Model
Imports Prism.Events

Namespace FriendStorage.UI.Events
  Public Class FriendSavedEvent
    Inherits PubSubEvent(Of [Friend])
  End Class
End Namespace
