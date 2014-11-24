Imports System.Collections.Generic

Namespace Db4oTutorialCode.Code.Updating
    ' #example: Domain model for drivers
    Public Class Driver
        Private m_name As String
        Private m_mostLovedCar As Car
        Private m_ownedCars As IList(Of Car) = New List(Of Car)()

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
        Public Property MostLovedCar() As Car
            Get
                Return m_mostLovedCar
            End Get
            Set(value As Car)
                m_mostLovedCar = value
            End Set
        End Property

        Public Property OwnedCars() As IList(Of Car)
            Get
                Return m_ownedCars
            End Get
            Set(value As IList(Of Car))
                m_ownedCars = value
            End Set
        End Property

        Public Sub AddOwnedCar(item As Car)
            m_ownedCars.Add(item)
        End Sub
    End Class
    ' #end example
End Namespace
