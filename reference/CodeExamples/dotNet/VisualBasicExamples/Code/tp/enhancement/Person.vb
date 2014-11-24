Namespace Db4oDoc.Code.Tp.Enhancement
    ' #example: Mark your domain model with the annotations
    <TransparentPersisted()> _
    Public Class Person
        ' #end example 

        Private m_name As String
        Private m_mother As Person

        Public Sub New(name As String)
            Me.m_name = name
        End Sub

        Public Sub New(name As String, mother As Person)
            Me.m_name = name
            Me.m_mother = mother
        End Sub

        Public Property Mother() As Person
            Get
                Return m_mother
            End Get
            Set(value As Person)
                m_mother = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = value
            End Set
        End Property


        Public Shared Function PersonWithHistory() As Person
            Return CreateFamily(10)
        End Function

        Private Shared Function CreateFamily(generation As Integer) As Person
            If 0 < generation Then
                Dim previousGeneration As Integer = generation - 1
                Return New Person("Joanna the " & generation, CreateFamily(previousGeneration))
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace
