Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS

Namespace Db4oDoc.Code.Semaphores
    Public Class SemaphoreExamples
        Private Const Port As Integer = 1337
        Private Const UserAndPassword As String = "sa"

        Public Shared Sub Main(ByVal args As String())
            Using server As IObjectServer = Db4oClientServer.OpenServer("database.db4o", Port)
                server.GrantAccess(UserAndPassword, UserAndPassword)
                GrabSemaphore()
                TryGrabSemaphore()
            End Using
        End Sub

        Private Shared Sub TryGrabSemaphore()
            Using container As IObjectContainer = OpenClient()
                Dim hasLock As Boolean = container.Ext().SetSemaphore("LockName", 1000)
                If hasLock Then
                    Console.WriteLine("Could get lock")
                Else
                    Console.WriteLine("Couldn't get lock")
                End If
                GrabSemaphore()
            End Using
        End Sub

        Private Shared Sub GrabSemaphore()
            Dim container As IObjectContainer = OpenClient()

            ' #example: Grab a semaphore
            ' Grab the lock. Specify the name and a timeout in milliseconds
            Dim hasLock As Boolean = container.Ext().SetSemaphore("LockName", 1000)
            Try
                ' you need to check the lock
                If hasLock Then
                    Console.WriteLine("Could get lock")
                Else
                    Console.WriteLine("Couldn't get lock")
                End If
            Finally
                ' release the lock
                container.Ext().ReleaseSemaphore("LockName")
            End Try
            ' #end example
        End Sub

        Private Shared Function OpenClient() As IObjectContainer
            Return Db4oClientServer.OpenClient("localhost", Port, UserAndPassword, UserAndPassword)
        End Function
    End Class
End Namespace
