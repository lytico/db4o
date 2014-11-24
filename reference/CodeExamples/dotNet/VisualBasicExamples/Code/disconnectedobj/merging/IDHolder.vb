Imports System

Namespace Db4oDoc.Code.DisconnectedObj.Merging
    Public MustInherit Class IDHolder
        Private ReadOnly guid As Guid = Guid.NewGuid()

        Public ReadOnly Property ObjectId() As Guid
            Get
                Return guid
            End Get
        End Property
    End Class
End Namespace
