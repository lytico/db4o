Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.DisconnectedObj.IdExamples

    Public Interface IIdExample(Of TId)
        Function IdForObject(ByVal obj As Object, ByVal database As IObjectContainer) As TId
        Function ObjectForID(ByVal idForObject As TId, ByVal database As IObjectContainer) As Object
        Sub Configure(ByVal configuration As IEmbeddedConfiguration)
        Sub RegisterEventOnContainer(ByVal container As IObjectContainer)
    End Interface
End Namespace
