Namespace Db4oTutorialCode.Code.Queries
    ' #example: Domain model for drivers
    Public Class Driver
        Private m_name As String
        Private m_age As Integer
        Private m_mostLovedCar As Car

        Public Sub New(name As String, age As Integer)
            Me.m_name = name
            Me.m_age = age
        End Sub

        Public Sub New(name As String, age As Integer, mostLovedCar As Car)
            Me.m_name = name
            Me.m_age = age
            Me.m_mostLovedCar = mostLovedCar
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = value
            End Set
        End Property

        Public ReadOnly Property Age() As Integer
            Get
                Return m_age
            End Get
        End Property

        Public Property MostLovedCar() As Car
            Get
                Return m_mostLovedCar
            End Get
            Set(value As Car)
                m_mostLovedCar = value
            End Set
        End Property
    End Class
    ' #end example
End Namespace
