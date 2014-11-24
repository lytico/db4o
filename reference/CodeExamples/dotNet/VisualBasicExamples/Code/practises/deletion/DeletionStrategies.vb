Imports Db4objects.Db4o
Imports Db4objects.Db4o.Events

Namespace Db4oDoc.Code.Practises.Deletion
    Public Class DeletionStrategies
        Public Shared Sub Main(ByVal args As String())
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                InstallDeletionFlagSupport(container)
            End Using
        End Sub

        Private Shared Sub InstallDeletionFlagSupport(ByVal container As IObjectContainer)
            ' #example: Deletion-Flag
            Dim events As IEventRegistry = EventRegistryFactory.ForObjectContainer(container)
            AddHandler events.Deleting, AddressOf HandleDeleteEvent
            ' #end example
        End Sub

        ' #example: The delete event handler
        Private Shared Sub HandleDeleteEvent(ByVal sender As Object, ByVal args As CancellableObjectEventArgs)
            Dim obj As Object = args.Object
            ' if the object has a deletion-flag:
            ' set the flag instead of deleting the object
            If TypeOf obj Is Deletable Then
                DirectCast(obj, Deletable).Delete()
                args.ObjectContainer().Store(obj)
                args.Cancel()
            End If
        End Sub
        ' #end example
    End Class

    Friend MustInherit Class Deletable
        Private m_deleted As Boolean = False

        Public Sub Delete()
            m_deleted = True
        End Sub

        Public ReadOnly Property Deleted() As Boolean
            Get
                Return m_deleted
            End Get
        End Property
    End Class

    Friend Class Person
        Inherits Deletable
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
