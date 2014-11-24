Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.DisconnectedObj.IdExamples
    Public Class Db4oInternalIdExample
        Implements IIdExample(Of Long)
        Public Shared Function Create() As Db4oInternalIdExample
            Return New Db4oInternalIdExample()
        End Function

        Public Function IdForObject(ByVal obj As Object, _
                                    ByVal container As IObjectContainer) As Long _
                                    Implements IIdExample(Of Long).IdForObject

            ' #example: get the db4o internal ids
            Dim interalId As Long = container.Ext().GetID(obj)
            ' #end example
            Return interalId
        End Function

        Public Function ObjectForID(ByVal idForObject As Long, _
                                    ByVal container As IObjectContainer) As Object _
                                    Implements IIdExample(Of Long).ObjectForID

            ' #example: get an object by db4o internal id
            Dim internalId As Long = idForObject
            Dim objForID As Object = container.Ext().GetByID(internalId)
            ' getting by id doesn't activate the object
            ' so you need to do it manually
            container.Ext().Activate(objForID)
            ' #end example
            Return objForID
        End Function

        Public Sub Configure(ByVal configuration As IEmbeddedConfiguration) _
                Implements IIdExample(Of Long).Configure

            ' no configuration required for internal ids  
        End Sub

        Public Sub RegisterEventOnContainer(ByVal container As IObjectContainer) _
                Implements IIdExample(Of Long).RegisterEventOnContainer

            ' no events required for internal ids  
        End Sub
    End Class
End Namespace
