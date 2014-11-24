Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS

Namespace Db4oDoc.Code.ClientServer.ReferenceCache
    Public Class ReferenceCacheExamples
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            Using server As IObjectServer = Db4oClientServer.OpenServer(DatabaseFile, 1337)
                server.GrantAccess("sa", "sa")
                StoreData(server)

                ReferenceCacheExample()
                UnitOfWork()
            End Using
        End Sub

        Private Shared Sub ReferenceCacheExample()
            Using client1 As IObjectContainer = Db4oClientServer.OpenClient("localhost", 1337, "sa", "sa"), client2 As IObjectContainer = Db4oClientServer.OpenClient("localhost", 1337, "sa", "sa")
                ' #example: Reference cache in client server
                Dim personOnClient1 As Person = QueryForPerson(client1)
                Dim personOnClient2 As Person = QueryForPerson(client2)
                Console.Write(QueryForPerson(client2).Name)

                personOnClient1.Name = ("New Name")
                client1.Store(personOnClient1)
                client1.Commit()

                ' The other client still has the old data in the cache
                Console.Write(QueryForPerson(client2).Name)

                client2.Ext().Refresh(personOnClient2, Integer.MaxValue)

                ' After refreshing the date is visible
                Console.Write(QueryForPerson(client2).Name)
                ' #end example
            End Using
        End Sub

        Private Shared Sub UnitOfWork()
            Using client As IObjectContainer = Db4oClientServer.OpenClient("localhost", 1337, "sa", "sa")
                ' #example: Clean cache for each unit of work
                Using container As IObjectContainer = client.Ext().OpenSession()
                    ' do work
                End Using
                ' Start with a fresh cache:
                Using container As IObjectContainer = client.Ext().OpenSession()
                    ' do work
                End Using
                ' #end example
            End Using
        End Sub

        Private Shared Function QueryForPerson(ByVal container As IObjectContainer) As Person
            Return container.Query(Of Person)()(0)
        End Function

        Private Shared Sub StoreData(ByVal server As IObjectServer)
            Using container As IObjectContainer = server.OpenClient()
                container.Store(New Person("Joe"))
            End Using
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub


        Private Class Person
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
    End Class
End Namespace
