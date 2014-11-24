Imports System
Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.DisconnectedObj.ObjectIdentity
    Public Class ObjectIdentityExamples
        Private Const DatabaseFileName As String = "database.db4o"


        Public Shared Sub Main(ByVal args As String())
            UpdateWorksOnSameContainer()
            NewObjectIsStoredIfDifferentContainer()
        End Sub

        Private Shared Sub NewObjectIsStoredIfDifferentContainer()
            CleanUp()
            StoreJoe()

            ' #example: Update doesn't works when using the different object containers
            Dim joe As Pilot
            Using container As IObjectContainer = OpenDatabase()
                joe = QueryByName(container, "Joe")
            End Using
            ' The update on another object 
            joe.Name = "Joe New"
            Using otherContainer As IObjectContainer = OpenDatabase()
                otherContainer.Store(joe)
            End Using
            Using container As IObjectContainer = OpenDatabase()
                ' instead of updating the existing pilot,
                ' a new instance was stored.
                Dim pilots As IList(Of Pilot) = container.Query(Of Pilot)()
                Console.WriteLine("Amount of pilots: " & pilots.Count)
                For Each pilot As Pilot In pilots
                    Console.WriteLine(pilot)
                Next
            End Using
            ' #end example

            CleanUp()
        End Sub

        Private Shared Sub UpdateWorksOnSameContainer()
            CleanUp()
            StoreJoe()

            ' #example: Update works when using the same object container
            Using container As IObjectContainer = OpenDatabase()
                Dim joe As Pilot = QueryByName(container, "Joe")
                joe.Name = "Joe New"
                container.Store(joe)
            End Using
            Using container As IObjectContainer = OpenDatabase()
                Dim pilots As IList(Of Pilot) = container.Query(Of Pilot)()
                Console.WriteLine("Amount of pilots: " & pilots.Count)
                For Each pilot As Pilot In pilots
                    Console.WriteLine(pilot)
                Next
            End Using
            ' #end example

            CleanUp()
        End Sub

        Private Shared Function QueryByName(ByVal container As IObjectContainer, ByVal name As String) As Pilot
            Return (From p As Pilot In container _
                    Where p.Name.Equals(name) _
                    Select p).First()
        End Function

        Private Shared Sub StoreJoe()
            Using container As IObjectContainer = OpenDatabase()
                container.Store(New Pilot("Joe"))
            End Using
        End Sub


        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub


        Private Shared Function OpenDatabase() As IObjectContainer
            Return Db4oEmbedded.OpenFile(DatabaseFileName)
        End Function
    End Class
End Namespace
