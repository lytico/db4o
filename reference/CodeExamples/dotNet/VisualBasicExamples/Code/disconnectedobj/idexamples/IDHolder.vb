Imports System

Namespace Db4oDoc.Code.DisconnectedObj.IdExamples

    ''' Id holder. For the example code it supports both, uuid and an int-id.
    ''' For a project you normally choose one or the other.
    Public MustInherit Class IDHolder
        ' #example: generate the id
        Private ReadOnly guid As Guid = Guid.NewGuid()

        Public ReadOnly Property ObjectId() As Guid
            Get
                Return guid
            End Get
        End Property

        ' #end example

        ' #example: id holder
        Private m_id As Integer

        Public Property Id() As Integer
            Get
                Return m_id
            End Get
            Set(ByVal value As Integer)
                m_id = value
            End Set
        End Property

        ' #end example
    End Class
End Namespace
