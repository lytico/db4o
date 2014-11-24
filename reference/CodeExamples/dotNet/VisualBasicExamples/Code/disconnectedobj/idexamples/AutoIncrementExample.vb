Imports System.Collections
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Events
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.DisconnectedObj.IdExamples
    Public Class AutoIncrementExample
        Implements IIdExample(Of Integer)
        Public Shared Function Create() As IIdExample(Of Integer)
            Return New AutoIncrementExample()
        End Function

        Public Function IdForObject(ByVal obj As Object, _
                                ByVal container As IObjectContainer) As Integer _
                                Implements IIdExample(Of Integer).IdForObject

            ' #example: get the id
            Dim idHolder As IDHolder = DirectCast(obj, IDHolder)
            Dim id As Integer = idHolder.Id
            ' #end example
            Return id
        End Function


        Public Function ObjectForID(ByVal idForObject As Integer, _
                                    ByVal container As IObjectContainer) As Object _
                                    Implements IIdExample(Of Integer).ObjectForID

            Dim id As Integer = idForObject
            ' #example: get an object by its id
            Dim instance = (From o As IDHolder In container _
                            Where o.Id = id _
                            Select o).Single()
            ' #end example
            Return instance
        End Function

        Public Sub Configure(ByVal configuration As IEmbeddedConfiguration) _
                Implements IIdExample(Of Integer).Configure
            ' #example: index the id-field
            configuration.Common.ObjectClass(GetType(IDHolder)).ObjectField("id").Indexed(True)
            ' #end example
        End Sub



        Public Sub RegisterEventOnContainer(ByVal container As IObjectContainer) _
                Implements IIdExample(Of Integer).RegisterEventOnContainer

            ' #example: use events to assign the ids
            Dim increment As New AutoIncrement(container)
            Dim eventRegistry As IEventRegistry = EventRegistryFactory.ForObjectContainer(container)

            AddHandler eventRegistry.Creating, AddressOf increment.HandleCreating
            AddHandler eventRegistry.Committing, AddressOf increment.HandleCommiting
            ' #end example
        End Sub

    End Class
End Namespace
