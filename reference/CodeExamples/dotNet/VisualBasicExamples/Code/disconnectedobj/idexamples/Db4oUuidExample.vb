Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Ext

Namespace Db4oDoc.Code.DisconnectedObj.IdExamples
    Public Class Db4oUuidExample
        Implements IIdExample(Of Db4oUUID)

        Public Shared Function Create() As Db4oUuidExample
            Return New Db4oUuidExample()
        End Function

        Public Function IdForObject(ByVal obj As Object, _
                                    ByVal container As IObjectContainer) As Db4oUUID _
                                    Implements IIdExample(Of Db4oUUID).IdForObject

            ' #example: get the db4o-uuid
            Dim uuid As Db4oUUID = container.Ext().GetObjectInfo(obj).GetUUID()
            ' #end example
            Return uuid
        End Function

        Public Function ObjectForID(ByVal idForObject As Db4oUUID, _
                                    ByVal container As IObjectContainer) As Object _
                                    Implements IIdExample(Of Db4oUUID).ObjectForID

            ' #example: get an object by a db4o-uuid
            Dim objForId As Object = container.Ext().GetByUUID(idForObject)
            ' getting by uuid doesn't activate the object
            ' so you need to do it manually
            container.Ext().Activate(objForId)
            ' #end example
            Return objForId
        End Function


        Public Sub Configure(ByVal configuration As IEmbeddedConfiguration) _
                Implements IIdExample(Of Db4oUUID).Configure

            ' #example: db4o-uuids need to be activated
            configuration.File.GenerateUUIDs = ConfigScope.Globally
            ' #end example
        End Sub

        Public Sub RegisterEventOnContainer(ByVal container As IObjectContainer) _
                Implements IIdExample(Of Db4objects.Db4o.Ext.Db4oUUID).RegisterEventOnContainer

            ' no events required for internal ids
        End Sub
        
    End Class
End Namespace
