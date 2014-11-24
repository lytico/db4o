Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.Configuration.Alias
    Public Class AliasExamples
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()

            AliasesExample()
        End Sub

        Private Shared Sub AliasesExample()
            StoreTypes()

            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Adding aliases
            ' add an alias for a specific type
            configuration.Common.AddAlias(New TypeAlias("Db4oDoc.Code.Configuration.Alias.OldTypeInDatabase, Db4oDoc", _
                                                        "Db4oDoc.Code.Configuration.Alias.NewType, Db4oDoc"))
            ' or add an alias for a whole namespace
            configuration.Common.AddAlias(New WildcardAlias("Db4oDoc.Code.Configuration.Alias.Old.Namespace.*, Db4oDoc", _
                                                            "Db4oDoc.Code.Configuration.Alias.Current.Namespace.*, Db4oDoc"))
            ' #end example

            Using container As IEmbeddedObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
                Dim countRenamed As Integer = container.Query(Of NewType)().Count
                AssertFoundEntries(countRenamed)
                Dim countInOtherPackage As Integer = container.Query(Of Current.Namespace.Car)().Count
                AssertFoundEntries(countInOtherPackage)
            End Using
        End Sub

        Private Shared Sub AssertFoundEntries(ByVal countRenamed As Integer)
            If 1 > countRenamed Then
                Throw New Exception("Expected a least on entry")
            End If
        End Sub

        Private Shared Sub StoreTypes()
            Using container As IObjectContainer = OpenDatabase()
                container.Store(New OldTypeInDatabase())
                container.Store(New Old.Namespace.Car())
            End Using
        End Sub

        Private Shared Sub CleanUp()
            System.IO.File.Delete(DatabaseFileName)
        End Sub

        Private Shared Function OpenDatabase() As IObjectContainer
            Return Db4oEmbedded.OpenFile(DatabaseFileName)
        End Function
    End Class

    Friend Class OldTypeInDatabase
        Private m_name As String = "default"

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property
    End Class

    Friend Class NewType
        Private m_name As String = "default"

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property
    End Class

    Namespace Old.Namespace

        Public Class Pilot
            Private m_name As String = "Joe"

            Public Property Name() As String
                Get
                    Return m_name
                End Get
                Set(ByVal value As String)
                    m_name = value
                End Set
            End Property
        End Class
        Public Class Car
            Private m_pilot As New Pilot()

            Public Property Pilot() As Pilot
                Get
                    Return m_pilot
                End Get
                Set(ByVal value As Pilot)
                    m_pilot = value
                End Set
            End Property
        End Class
    End Namespace
    Namespace Current.Namespace

        Public Class Pilot
            Private m_name As String = "Joe"

            Public Property Name() As String
                Get
                    Return m_name
                End Get
                Set(ByVal value As String)
                    m_name = value
                End Set
            End Property
        End Class
        Public Class Car
            Private m_pilot As New Pilot()

            Public Property Pilot() As Pilot
                Get
                    Return m_pilot
                End Get
                Set(ByVal value As Pilot)
                    m_pilot = value
                End Set
            End Property
        End Class
    End Namespace
End Namespace