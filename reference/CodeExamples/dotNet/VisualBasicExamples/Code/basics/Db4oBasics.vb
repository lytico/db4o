Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Basics
    Public Class Db4oBasics
        Public Shared Sub Main(ByVal args As String())
            OpenAndCloseTheContainer()
            StoreObject()
            Query()
            UpdateDatabase()
            DeleteObject()

            AllOperationsInOnGo()
        End Sub

        Private Shared Sub StoreObject()
            ' #example: Store an object
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                Dim pilot As New Pilot("Joe")
                container.Store(pilot)
            End Using
            ' #end example
        End Sub

        Private Shared Sub Query()
            ' #example: Query for objects
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                Dim pilots = From p As Pilot In container Where p.Name = "Joe"
                For Each pilot As Pilot In pilots
                    Console.Out.WriteLine(pilot.Name)
                Next
            End Using
            ' #end example
        End Sub

        Private Shared Sub UpdateDatabase()
            ' #example: Update a pilot
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                Dim pilot = (From p As Pilot In container Where p.Name = "Joe").First()
                pilot.Name = "New Name"
                ' update the pilot
                container.Store(pilot)
            End Using
            ' #end example
        End Sub

        Private Shared Sub DeleteObject()
            ' #example: Delete a object
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                Dim pilot = (From p As Pilot In container Where p.Name = "Joe").First()
                container.Delete(pilot)
            End Using
            ' #end example
        End Sub

        Private Shared Sub OpenAndCloseTheContainer()
            ' #example: Open the object container to use the database
            ' use the object container
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
            End Using
            ' #end example
        End Sub

        Private Shared Sub AllOperationsInOnGo()
            ' #example: The basic operations
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                ' store a new pilot
                Dim pilot As New Pilot("Joe")
                container.Store(pilot)

                ' query for pilots
                Dim pilots = From p As Pilot In container Where p.Name.StartsWith("Jo")

                ' update pilot
                Dim toUpdate As Pilot = pilots.First()
                toUpdate.Name = "New Name"
                container.Store(toUpdate)

                ' delete pilot
                container.Delete(toUpdate)
            End Using
            ' #end example
        End Sub
    End Class

    Friend Class Pilot
        Private m_name As String

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property
    End Class
End Namespace
