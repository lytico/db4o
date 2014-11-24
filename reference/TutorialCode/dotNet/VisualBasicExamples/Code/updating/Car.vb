Namespace Db4oTutorialCode.Code.Updating
    ' #example: Domain model for cars
    Public Class Car
        Private m_carName As String

        Public Sub New(carName As String)
            Me.m_carName = carName
        End Sub

        Public Property CarName() As String
            Get
                Return m_carName
            End Get
            Set(value As String)
                m_carName = value
            End Set
        End Property

    End Class
    ' #end example
End Namespace
