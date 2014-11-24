Namespace Db4oDoc.Code.Practises.Relations
    Class Country
        Private ReadOnly m_name As String

        Public Sub New(name As String)
            Me.m_name = name
        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return m_name
            End Get
        End Property
    End Class
End Namespace
