Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.IO

Namespace Db4oDoc.Code.Backup
    Public Class BackupExample
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanDb()
            SimpleBackup()
            BackupWithStorage()
        End Sub

        Private Shared Sub CleanDb()
            File.Delete(DatabaseFile)
        End Sub

        Private Shared Sub BackupWithStorage()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                StoreObjects(container)
                ' #example: Store a backup with storage
                container.Ext().Backup(New FileStorage(), "advanced-backup.db4o")
                ' #end example
            End Using
        End Sub

        Private Shared Sub SimpleBackup()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                StoreObjects(container)
                ' #example: Store a backup while using the database
                container.Ext().Backup("backup.db4o")
                ' #end example
            End Using
        End Sub

        Private Shared Sub StoreObjects(ByVal container As IObjectContainer)
            container.Store(New Person("John", "Walker"))
            container.Store(New Person("Joanna", "Waterman"))
            container.Commit()
        End Sub

        Private Class Person
            Private m_sirname As String
            Private m_firstname As String

            Public Sub New(ByVal name As String, ByVal firstname As String)
                Me.m_firstname = firstname
                Me.m_sirname = name
            End Sub

            Public ReadOnly Property Sirname() As String
                Get
                    Return m_sirname
                End Get
            End Property

            Public ReadOnly Property Firstname() As String
                Get
                    Return m_firstname
                End Get
            End Property
        End Class
    End Class
End Namespace
