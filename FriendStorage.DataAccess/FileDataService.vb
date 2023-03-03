Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports FriendStorage.Model
Imports System.IO
Imports Newtonsoft.Json
Imports System.Collections

Namespace FriendStorage.DataAccess
  Public Class FileDataService
    Implements IDataService

    Public Function GetFriendById(FriendId As Integer) As [Friend] Implements IDataService.GetFriendById
      Return Me.ReadFromFile.Single(Function(F) F.Id = FriendId)
    End Function



    Public Sub SaveFriend([Friend] As [Friend]) Implements IDataService.SaveFriend
      If [Friend].Id = 0 Then
        Me.InsertFriend([Friend])
      Else
        Me.UpdateFriend([Friend])
      End If
    End Sub



    Public Sub DeleteFriend(FriendId As Integer) Implements IDataService.DeleteFriend
      Me.WriteToFile(Me.ReadFromFile.Where(Function(F) F.Id <> FriendId))
    End Sub



    Public Function GetAllFriends() As IEnumerable(Of LookupItem) Implements IDataService.GetAllFriends
      Return Me.ReadFromFile.Select(Function(F) New LookupItem With {.Id = F.Id, .DisplayMember = $"{F.FirstName} {F.LastName}"})
    End Function



    Public Sub Dispose() Implements IDisposable.Dispose
      ' Usually Service-Proxies are disposable. This method is added as demo-purpose
      ' to show how to use an IDisposable in the client with a Func(Of T). =>  Look for example at the FriendDataProvider-class
    End Sub



    Private Sub InsertFriend([Friend] As [Friend])
      Dim oFriends As List(Of [Friend])
      Dim iMaxId As Integer

      oFriends = Me.ReadFromFile()
      iMaxId = If(oFriends.Count = 0, 0, oFriends.Max(Function(F) F.Id))

      [Friend].Id = iMaxId + 1
      oFriends.Add([Friend])

      Me.WriteToFile(oFriends)
    End Sub



    Private Sub UpdateFriend([Friend] As [Friend])
      Dim oExisting As [Friend]
      Dim oFriends As List(Of [Friend])
      Dim iFriend As Integer

      oFriends = Me.ReadFromFile()
      oExisting = oFriends.Single(Function(F) F.Id = [Friend].Id)
      iFriend = oFriends.IndexOf(oExisting)

      oFriends.Insert(iFriend, [Friend])
      oFriends.Remove(oExisting)

      Me.WriteToFile(oFriends)
    End Sub



    Private Function ReadFromFile() As List(Of [Friend])
      Dim sData As String

      If File.Exists(STORAGE_FILE) Then
        sData = File.ReadAllText(STORAGE_FILE)
        ReadFromFile = JsonConvert.DeserializeObject(Of List(Of [Friend]))(sData)
      Else
        ReadFromFile = New List(Of [Friend]) From {
          New [Friend] With {
            .Id = 1,
            .FirstName = "Thomas",
            .LastName = "Huber",
            .Birthday = New DateTime(1980, 10, 28),
            .IsDeveloper = True
          },
          New [Friend] With {
            .Id = 2,
            .FirstName = "Julia",
            .LastName = "Huber",
            .Birthday = New DateTime(1982, 10, 10)
          },
          New [Friend] With {
            .Id = 3,
            .FirstName = "Anna",
            .LastName = "Huber",
            .Birthday = New DateTime(2011, 5, 13)
          },
          New [Friend] With {
            .Id = 4,
            .FirstName = "Sara",
            .LastName = "Huber",
            .Birthday = New DateTime(2013, 2, 25)
          },
          New [Friend] With {
            .Id = 5,
            .FirstName = "Andreas",
            .LastName = "Böhler",
            .Birthday = New DateTime(1981, 1, 10),
            .IsDeveloper = True
          },
          New [Friend] With {
            .Id = 6,
            .FirstName = "Urs",
            .LastName = "Meier",
            .Birthday = New DateTime(1970, 3, 5),
            .IsDeveloper = True
          },
          New [Friend] With {
            .Id = 7,
            .FirstName = "Chrissi",
            .LastName = "Heuberger",
            .Birthday = New DateTime(1987, 7, 16)
          },
          New [Friend] With {
            .Id = 8,
            .FirstName = "Erkan",
            .LastName = "Egin",
            .Birthday = New DateTime(1983, 5, 23)
          }
        }
      End If
    End Function



    Private Sub WriteToFile(Friends As IEnumerable(Of [Friend]))
      Dim sData As String

      sData = JsonConvert.SerializeObject(Friends, Formatting.Indented)

      File.WriteAllText(STORAGE_FILE, sData)
    End Sub



    Private Const STORAGE_FILE As String = "Friends.json"
  End Class
End Namespace
