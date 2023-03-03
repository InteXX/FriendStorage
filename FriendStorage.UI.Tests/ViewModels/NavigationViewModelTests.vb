Imports FriendStorage.Model
Imports FriendStorage.UI.DataProviders
Imports FriendStorage.UI.Events
Imports FriendStorage.UI.ViewModels
Imports Moq
Imports Prism.Events
Imports Xunit

Namespace FriendStorage.UI.Tests.ViewModels
  Public Class NavigationViewModelTests
    Public Sub New()
      Dim oEventAggregatorMock As Mock(Of IEventAggregator)

      Me.FriendDeletedEvent = New FriendDeletedEvent
      Me.FriendSavedEvent = New FriendSavedEvent

      oEventAggregatorMock = New Mock(Of IEventAggregator)

      oEventAggregatorMock.
        Setup(Function(E) E.GetEvent(Of FriendDeletedEvent)).
        Returns(Me.FriendDeletedEvent)

      oEventAggregatorMock.
        Setup(Function(E) E.GetEvent(Of FriendSavedEvent)).
        Returns(Me.FriendSavedEvent)

      Me.DataProviderMock = New Mock(Of INavigationDataProvider)
      Me.DataProviderMock.
        Setup(Function(Dp) Dp.GetAllFriends).
        Returns(Me.Friends)

      Me.ViewModel = New NavigationViewModel(Me.DataProviderMock.Object, oEventAggregatorMock.Object)
    End Sub



    <Fact>
    Public Sub ShouldLoadFriends()
      Dim iFriendId As Integer
      Dim iExpected As Integer
      Dim sExpected As String
      Dim oFriend As NavigationItemViewModel
      Dim iActual As Integer
      Dim sActual As String

      Me.ViewModel.Load()

      iExpected = Me.DataProviderMock.Object.GetAllFriends.Count
      iActual = Me.Friends.Count

      Assert.Equal(iExpected, iActual)

      iFriendId = 1
      sExpected = Me.Friends.SingleOrDefault(Function(F) F.Id = iFriendId).DisplayMember
      oFriend = Me.ViewModel.Friends.SingleOrDefault(Function(F) F.Id = iFriendId)
      sActual = oFriend.DisplayMember

      Assert.NotNull(oFriend)
      Assert.Equal(sExpected, sActual)

      iFriendId = 2
      sExpected = Me.Friends.SingleOrDefault(Function(F) F.Id = iFriendId).DisplayMember
      oFriend = Me.ViewModel.Friends.SingleOrDefault(Function(F) F.Id = iFriendId)
      sActual = oFriend.DisplayMember

      Assert.NotNull(oFriend)
      Assert.Equal(sExpected, sActual)
    End Sub



    <Fact>
    Public Sub ShouldLoadFriendsOnlyOnce()
      Dim iExpected As Integer
      Dim iActual As Integer

      Me.ViewModel.Load()
      Me.ViewModel.Load()

      iExpected = Me.DataProviderMock.Object.GetAllFriends.Count
      iActual = Me.Friends.Count

      Assert.Equal(iExpected, iActual)
    End Sub



    <Fact>
    Public Sub ShouldUpdateNavigationItemWhenFriendIsSaved()
      Dim oNavigationItem As NavigationItemViewModel
      Dim sFirstName As String
      Dim sLastName As String
      Dim iFriendId As Integer
      Dim oFriend As [Friend]

      Me.ViewModel.Load()

      oNavigationItem = Me.ViewModel.Friends.First
      sFirstName = "Anna"
      sLastName = "Huber"
      iFriendId = oNavigationItem.Id
      oFriend = New [Friend] With {.Id = iFriendId, .FirstName = sFirstName, .LastName = sLastName}

      Me.FriendSavedEvent.Publish(oFriend)

      Assert.Equal($"{sFirstName} {sLastName}", oNavigationItem.DisplayMember)
    End Sub



    Private ReadOnly Property Friends As List(Of LookupItem)
      Get
        If Me._Friends Is Nothing Then
          Me._Friends = New List(Of LookupItem) From {
            New LookupItem With {.Id = 1, .DisplayMember = "Julia"},
            New LookupItem With {.Id = 2, .DisplayMember = "Thomas"}
          }
        End If

        Return Me._Friends
      End Get
    End Property
    Private _Friends As List(Of LookupItem)



    <Fact>
    Public Sub ShouldAddNavigationItemWhenAddedFriendIsSaved()
      Dim oAddedItem As NavigationItemViewModel
      Dim iFriendId As Integer
      Dim oFriend As [Friend]

      Me.ViewModel.Load()

      iFriendId = 97
      oFriend = New [Friend] With {
        .Id = iFriendId,
        .FirstName = "Anna",
        .LastName = "Huber"
      }

      Me.FriendSavedEvent.Publish(oFriend)

      Assert.Equal(3, Me.ViewModel.Friends.Count)

      oAddedItem = Me.ViewModel.Friends.SingleOrDefault(Function(F) F.Id = iFriendId)

      Assert.NotNull(oAddedItem)
      Assert.Equal($"{oFriend.FirstName} {oFriend.LastName}", oAddedItem.DisplayMember)
    End Sub



    <Fact>
    Public Sub ShouldRemoveNavigationItemWhenFriendIsDeleted()
      Dim iFriendId As Integer

      Me.ViewModel.Load()

      iFriendId = Me.ViewModel.Friends.First.Id

      Me.FriendDeletedEvent.Publish(iFriendId)

      Assert.Single(Me.ViewModel.Friends)
      Assert.NotEqual(iFriendId, Me.ViewModel.Friends.Single.Id)
    End Sub



    Private ReadOnly FriendDeletedEvent As FriendDeletedEvent
    Private ReadOnly FriendSavedEvent As FriendSavedEvent
    Private ReadOnly DataProviderMock As Mock(Of INavigationDataProvider)
    Private ReadOnly ViewModel As NavigationViewModel
  End Class
End Namespace
