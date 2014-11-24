Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS

Namespace Db4oDoc.Code.Container.Sessions
    Public Class Db4oSessions
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            Sessions()
            SessionsIsolation()
            SessionCache()
            EmbeddedClient()
        End Sub


        Private Shared Sub Sessions()
            CleanUp()
            ' #example: Session object container
            Using rootContainer As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                ' open the db4o-session. For example at the beginning for a web-request
                Using session As IObjectContainer = rootContainer.Ext().OpenSession()
                    ' do the operations on the session-container
                    session.Store(New Person("Joe"))
                End Using
            End Using
            ' #end example
        End Sub

        Private Shared Sub SessionsIsolation()
            CleanUp()
            Using rootContainer As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                Using session1 As IObjectContainer = rootContainer.Ext().OpenSession(), session2 As IObjectContainer = rootContainer.Ext().OpenSession()
                    ' #example: Session are isolated from each other
                    session1.Store(New Person("Joe"))
                    session1.Store(New Person("Joanna"))

                    ' the second session won't see the changes until the changes are committed
                    PrintAll(session2.Query(Of Person)())

                    session1.Commit()

                    ' new the changes are visiable for the second session
                    PrintAll(session2.Query(Of Person)())
                    ' #end example
                End Using
            End Using
        End Sub


        Private Shared Sub SessionCache()
            CleanUp()
            Using rootContainer As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                Using session1 As IObjectContainer = rootContainer.Ext().OpenSession(), session2 As IObjectContainer = rootContainer.Ext().OpenSession()
                    StoreAPerson(session1)

                    ' #example: Each session does cache the objects
                    Dim personOnSession1 As Person = session1.Query(Of Person)()(0)
                    Dim personOnSession2 As Person = session2.Query(Of Person)()(0)

                    personOnSession1.Name = "NewName"
                    session1.Store(personOnSession1)
                    session1.Commit()


                    ' the second session still sees the old value, because it was cached
                    Console.WriteLine(personOnSession2.Name)
                    ' you can explicitly refresh it
                    session2.Ext().Refresh(personOnSession2, Integer.MaxValue)
                    Console.WriteLine(personOnSession2.Name)
                    ' #end example

                End Using
            End Using
        End Sub

        Private Shared Sub EmbeddedClient()
            CleanUp()
            ' #example: Embedded client
            Using server As IObjectServer = Db4oClientServer.OpenServer(DatabaseFileName, 0)
                ' open the db4o-embedded client. For example at the beginning for a web-request
                Using container As IObjectContainer = server.OpenClient()
                    ' do the operations on the session-container
                    container.Store(New Person("Joe"))
                End Using
            End Using
            ' #end example
        End Sub


        Private Shared Sub PrintAll(ByVal persons As IEnumerable(Of Person))
            For Each person As Person In persons
                Console.WriteLine(person)
            Next
        End Sub
        Private Shared Sub StoreAPerson(ByVal session1 As IObjectContainer)
            session1.Store(New Person("Joe"))
            session1.Commit()
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
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

            Public Overrides Function ToString() As String
                Return String.Format("Name: {0}", m_name)
            End Function
        End Class
    End Class
End Namespace