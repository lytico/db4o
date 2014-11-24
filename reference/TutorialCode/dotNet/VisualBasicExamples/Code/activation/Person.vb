Namespace Db4oTutorialCode.Code.Activation
    ' #example: Domain model for people
    Public Class Person
        Private ReadOnly m_name As String
        Private ReadOnly m_mother As Person

        Public Sub New(name As String, mother As Person)
            Me.m_name = name
            Me.m_mother = mother
        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return m_name
            End Get
        End Property

        Public ReadOnly Property Mother() As Person
            Get
                Return m_mother
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("Name: {0}", m_name)
        End Function
    End Class

    ' #end example
End Namespace
