Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Events
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Callbacks.Examples
    Public Class CallbackExamples
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()


            StoreTestObjects()
            ReferentialIntegrity()
        End Sub

        Private Shared Sub ReferentialIntegrity()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Register handler
                Dim events As IEventRegistry = EventRegistryFactory.ForObjectContainer(container)
                AddHandler events.Deleting, AddressOf ReferentialIntegrityCheck
                ' #end example

                Dim pilot As Pilot = container.Query(Of Pilot)()(0)
                container.Delete(pilot)
            End Using
        End Sub

        ' #example: Referential integrity
        Private Shared Sub ReferentialIntegrityCheck(ByVal sender As Object, ByVal eventArguments As CancellableObjectEventArgs)
            Dim toDelete As Object = eventArguments.Object
            If TypeOf toDelete Is Pilot Then
                Dim container As IObjectContainer = eventArguments.ObjectContainer()
                Dim cars As IEnumerable(Of Car) = From c As Car In container _
                                                  Where c.Pilot Is toDelete
                If cars.Count() > 0 Then
                    eventArguments.Cancel()
                End If
            End If
        End Sub
        ' #end example

        Private Shared Sub StoreTestObjects()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim pilot As New Pilot("John")
                container.Store(pilot)
                container.Store(New Car(pilot))
            End Using
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub
    End Class

    Friend Class Pilot
        Private m_name As String

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return m_name
            End Get
        End Property
    End Class

    Friend Class Car
        Private m_pilot As Pilot

        Public Sub New(ByVal pilot As Pilot)
            Me.m_pilot = pilot
        End Sub

        Public ReadOnly Property Pilot() As Pilot
            Get
                Return m_pilot
            End Get
        End Property
    End Class
End Namespace
