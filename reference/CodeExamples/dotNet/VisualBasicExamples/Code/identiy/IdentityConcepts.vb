Imports System.Collections
Imports System.IO
Imports Db4objects.Db4o

Namespace Db4oDoc.Code.Identiy
    Public Class IdentityConcepts
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            storeAObject()
            ReferenceEquals()

            StoreAndLoadWithTheSame()
            StoreOnDifferentContainers()

            RemoveFromReferenceCache()
        End Sub

        Private Overloads Shared Sub ReferenceEquals()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: db4o ensures reference equality
                Dim theCar As Car = container.Query(Of Car)()(0)
                Dim thePilot As Pilot = container.Query(Of Pilot)()(0)
                Dim pilotViaCar As Pilot = theCar.Pilot
                AssertTrue(ReferenceEquals(thePilot, pilotViaCar))
                ' #end example
            End Using
        End Sub

        Private Shared Sub StoreAndLoadWithTheSame()
            Using rootContainer As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile), container1 As IObjectContainer = rootContainer.Ext().OpenSession(), container2 As IObjectContainer = rootContainer.Ext().OpenSession()
                ' #example: Loading with different object container results in different objects
                Dim loadedWithContainer1 As Car = container1.Query(Of Car)()(0)
                Dim loadedWithContainer2 As Car = container2.Query(Of Car)()(0)
                AssertFalse(ReferenceEquals(loadedWithContainer1, loadedWithContainer2))
                ' #end example
            End Using
        End Sub

        Private Shared Sub StoreOnDifferentContainers()
            Using rootContainer As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile), container1 As IObjectContainer = rootContainer.Ext().OpenSession(), container2 As IObjectContainer = rootContainer.Ext().OpenSession()
                ' #example: Don't use different object-container for the same object.
                Dim loadedWithContainer1 As Car = container1.Query(Of Car)()(0)
                container2.Store(loadedWithContainer1)
                ' Now the car is store twice.
                ' Because the container2 cannot recognize objects from other containers
                ' Therefore always use the same container to store and load objects
                ' #end example
                PrintAll(container2.Query(Of Car)())
            End Using
        End Sub

        Private Shared Sub RemoveFromReferenceCache()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: With purge you can remove objects from the reference cache
                Dim theCar As Car = container.Query(Of Car)()(0)
                ' #end example
                container.Ext().Purge(theCar)
            End Using
        End Sub

        Private Shared Sub PrintAll(ByVal objects As IEnumerable)
            For Each obj As Object In objects
                Console.WriteLine(obj)
            Next
        End Sub

        Private Shared Sub AssertTrue(ByVal mustBeTrue As Boolean)
            If Not mustBeTrue Then
                Throw New Exception("expected true")
            End If
        End Sub

        Private Shared Sub AssertFalse(ByVal mustBeTrue As Boolean)
            If mustBeTrue Then
                Throw New Exception("expected false")
            End If
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub

        Private Shared Sub storeAObject()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                container.Store(New Car(New Pilot("John"), "VW Golf"))
            End Using
        End Sub
    End Class
End Namespace
