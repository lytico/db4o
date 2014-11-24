Imports Db4oTutorialCode.Db4oTutorialCode.Code.TransparentPersistence

Namespace Db4oTutorialCode.Code.Transactions
    ' #example: Domain model for drivers
    <TransparentPersisted()> _
    Public Class Driver
        Private m_name As String
        Private m_mostLovedCar As Car

        Public Sub New(name As String)
            Me.m_name = name
        End Sub

        Public Sub New(name As String, mostLovedCar As Car)
            Me.m_name = name
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

        Public ReadOnly Property MostLovedCar() As Car
            Get
                Return m_mostLovedCar
            End Get
        End Property
    End Class
    ' #end example
End Namespace
