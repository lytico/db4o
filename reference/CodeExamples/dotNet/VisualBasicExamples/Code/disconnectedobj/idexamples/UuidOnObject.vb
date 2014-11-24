Imports System
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.DisconnectedObj.IdExamples
    Public Class UuidOnObject
        Implements IIdExample(Of Guid)
        Public Shared Function Create() As IIdExample(Of Guid)
            Return New UuidOnObject()
        End Function

        Public Function IdForObject(ByVal obj As Object, _
                                    ByVal container As IObjectContainer) As Guid _
                                    Implements IIdExample(Of Guid).IdForObject

            ' #example: get the uuid
            Dim uuidHolder As IDHolder = DirectCast(obj, IDHolder)
            Dim uuid As Guid = uuidHolder.ObjectId
            ' #end example
            Return uuid
        End Function

        Public Function ObjectForID(ByVal idForObject As Guid, _
                                    ByVal container As IObjectContainer) As Object _
                                    Implements IIdExample(Of System.Guid).ObjectForID

            ' #example: get an object its UUID
            Dim instance As IDHolder = container.Query(Function(o As IDHolder) o.ObjectId.Equals(idForObject))(0)
            ' #end example
            Return instance
        End Function

        Public Sub Configure(ByVal configuration As IEmbeddedConfiguration) _
                Implements IIdExample(Of Guid).Configure

            ' #example: index the uuid-field
            configuration.Common.ObjectClass(GetType(IDHolder)).ObjectField("guid").Indexed(True)
            ' #end example
        End Sub

        Public Sub RegisterEventOnContainer(ByVal container As IObjectContainer) _
                Implements IIdExample(Of Guid).RegisterEventOnContainer

            ' no events required for internal ids
        End Sub
    End Class
End Namespace
