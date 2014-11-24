Namespace Db4oDoc.Code.Pitfalls.Activation
    ' #example: Person with a reference to the mother
    Friend Class Person
        Private m_mother As Person
        Private m_name As String

        Public Sub New(ByVal name As String)
            m_mother = m_mother
            Me.m_name = name
        End Sub

        Public Sub New(ByVal mother As Person, ByVal name As String)
            Me.m_mother = mother
            Me.m_name = name
        End Sub

        Public ReadOnly Property Mother() As Person
            Get
                Return m_mother
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return m_name
            End Get
        End Property
    End Class
    ' #end example
End Namespace
