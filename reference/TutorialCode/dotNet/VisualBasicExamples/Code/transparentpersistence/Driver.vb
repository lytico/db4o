Imports System.Collections.Generic

Namespace Db4oTutorialCode.Code.TransparentPersistence
    ' #example: Domain model for drivers
    <TransparentPersisted()> _
    Public Class Driver
        Private m_name As String
        Private ReadOnly m_ownedCars As IList(Of Car) = New List(Of Car)()
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

        Public Property MostLovedCar() As Car
            Get
                Return m_mostLovedCar
            End Get
            Set(value As Car)
                m_mostLovedCar = value
            End Set
        End Property

        Public ReadOnly Property OwnedCars() As IList(Of Car)
            Get
                Return m_ownedCars
            End Get
        End Property

        Public Sub AddOwnedCar(car As Car)
            m_ownedCars.Add(car)
        End Sub
    End Class
    ' #end example
End Namespace
