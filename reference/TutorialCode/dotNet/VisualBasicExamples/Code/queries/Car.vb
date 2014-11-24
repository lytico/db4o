Namespace Db4oTutorialCode.Code.Queries
    ' #example: Domain model for cars
    Public Class Car
        Private ReadOnly m_carName As String
        Private ReadOnly m_horsePower As Integer

        Public Sub New(carName As String, horsePower As Integer)
            Me.m_carName = carName
            Me.m_horsePower = horsePower
        End Sub

        Public ReadOnly Property CarName() As String
            Get
                Return m_carName
            End Get
        End Property

        Public ReadOnly Property HorsePower() As Integer
            Get
                Return m_horsePower
            End Get
        End Property
    End Class
    ' #end example
End Namespace
