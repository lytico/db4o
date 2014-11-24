Namespace Db4oDoc.Code.Concurrency.Locking
    Class Person
        Private m_name As String

        Public Sub New(name As String)
            Me.m_name = name
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = value
            End Set
        End Property
    End Class
End Namespace
