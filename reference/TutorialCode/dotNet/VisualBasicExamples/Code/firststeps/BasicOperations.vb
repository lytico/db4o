Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oTutorialCode.Code.FirstSteps
    Public Class BasicOperations
        Public Shared Sub Main(args As String())
            OpenAndCloseTheContainer()
            StoreObject()
            Query()
            UpdateObject()
            DeleteObject()
        End Sub

        Private Shared Sub OpenAndCloseTheContainer()
            ' #example: Open and close db4o
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                ' use the object container in here
            End Using
            ' #end example
        End Sub

        Private Shared Sub StoreObject()
            ' #example: Store an object
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                Dim driver = New Driver("Joe")
                container.Store(driver)
            End Using
            ' #end example
        End Sub

        Private Shared Sub Query()
            ' #example: Query for objects
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                Dim drivers = From d As Driver In container
                              Where d.Name = "Joe"
                              Select d
                Console.WriteLine("Stored Pilots:")
                For Each driver As Driver In drivers
                    Console.WriteLine(driver.Name)
                Next
            End Using
            ' #end example
        End Sub

        Private Shared Sub UpdateObject()
            ' #example: Update an object
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                Dim drivers = From d As Driver In container
                              Where d.Name = "Joe"
                              Select d
                Dim driver As Driver = drivers.First()
                Console.WriteLine("Old name {0}", driver.Name)
                driver.Name = "John"
                Console.WriteLine("New name {0}", driver.Name)
                ' update the pilot
                container.Store(driver)
            End Using
            ' #end example
        End Sub

        Private Shared Sub DeleteObject()
            ' #example: Delete an object
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("databaseFile.db4o")
                Dim drivers = From d As Driver In container
                              Where d.Name = "Joe"
                              Select d
                Dim driver As Driver = drivers.First()
                Console.WriteLine("Deleting {0}", driver.Name)
                container.Delete(driver)
            End Using
            ' #end example
        End Sub
    End Class
End Namespace
