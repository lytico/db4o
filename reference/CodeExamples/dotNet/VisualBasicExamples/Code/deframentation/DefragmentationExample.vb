Imports System.IO
Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Defragment
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Deframentation
    Public Class DefragmentationExample
        Public Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            SimplestPossibleDefragment()
            SimpleDefragmentationWithBackupLocation()
            DefragmentWithConfiguration()
            DefragmentationWithIdMissing()
        End Sub

        ' #end example

        Private Shared Sub DefragmentWithConfiguration()
            CreateAndFillDatabase()
            ' #example: Defragment with configuration
            Dim config As New DefragmentConfig("database.db4o")
            Defragment.Defrag(config)
            ' #end example
        End Sub

        Private Shared Sub SimpleDefragmentationWithBackupLocation()
            CreateAndFillDatabase()
            ' #example: Specify backup file explicitly
            Defragment.Defrag("database.db4o", "database.db4o.bak")
            ' #end example
        End Sub


        Private Shared Sub SimplestPossibleDefragment()
            CreateAndFillDatabase()
            ' #example: Simplest possible defragment use case
            Defragment.Defrag("database.db4o")
            ' #end example
        End Sub

        Private Shared Sub DefragmentationWithIdMissing()
            CreateAndFillDatabase()


            ' #example: Use a defragmentation listener
            Dim config As New DefragmentConfig("database.db4o")
            Defragment.Defrag(config, New DefragmentListener())
            ' #end example
        End Sub

        ' #example: Defragmentation listener implementation
        Private Class DefragmentListener
            Implements IDefragmentListener
            Public Sub NotifyDefragmentInfo(ByVal defragmentInfo As DefragmentInfo) _
                Implements IDefragmentListener.NotifyDefragmentInfo

                Console.WriteLine(defragmentInfo)
            End Sub
        End Class
        ' #end example

        Private Shared Sub CreateAndFillDatabase()
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                container.Store(New Person("Joe"))
                container.Store(New Person("Joanna"))
                container.Store(New Person("Jenny"))
                container.Store(New Person("Julia"))
                container.Store(New Person("John"))
                container.Store(New Person("JJ"))
                Dim jimmy As New Person("Jimmy")
                jimmy.BestFriend = New Person("Bunk")
                container.Store(jimmy)
            End Using
            LeaveInvalidId()
        End Sub

        Private Shared Sub LeaveInvalidId()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim bunk As Person = (From p As Person In container Where p.Name = "Bunk").First()
                container.Delete(bunk)
            End Using
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
            File.Delete(DatabaseFile & ".backup")
            File.Delete(DatabaseFile & ".bak")
        End Sub


        Private Class Person
            Private m_name As String
            Private m_bestFriend As Person

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

            Public Property BestFriend() As Person
                Get
                    Return m_bestFriend
                End Get
                Set(ByVal value As Person)
                    m_bestFriend = value
                End Set
            End Property
        End Class
    End Class
End Namespace