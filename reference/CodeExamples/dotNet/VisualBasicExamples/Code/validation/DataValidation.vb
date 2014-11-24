Imports System.ComponentModel.DataAnnotations
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Events

Namespace Db4oDoc.Code.Validation
    Public Class DataValidation
        Public Shared Sub Main(ByVal args As String())
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4")
                ' #example: Register validation for the create and update event
                Dim events As IEventRegistry = EventRegistryFactory.ForObjectContainer(container)
                AddHandler events.Creating, AddressOf ValidateObject
                AddHandler events.Updating, AddressOf ValidateObject
                ' #end example


                ' #example: Storing a valid pilot
                Dim pilot = New Pilot()
                pilot.Name = "Joe"
                container.Store(pilot)
                ' #end example


                ' #example: Storing a invalid pilot throws exception
                Dim otherPilot = New Pilot()
                otherPilot.Name = ""
                Try
                    container.Store(otherPilot)
                Catch e As EventException
                    Dim cause As ValidationException = DirectCast(e.InnerException, ValidationException)
                    Console.WriteLine(cause.ValidationResult.ErrorMessage)
                End Try
                ' #end example
            End Using
        End Sub

        ' #example: Validation support
        Private Shared Sub ValidateObject(ByVal sender As Object, ByVal eventInfo As CancellableObjectEventArgs)
            Dim context As New ValidationContext(eventInfo.Object, Nothing, Nothing)
            'This throws when the object isn't valid.
            Validator.ValidateObject(eventInfo.Object, context, True)
        End Sub
        ' #end example 
    End Class

    ' #example: Validation attributes
    Class Pilot
        Private m_name As String

        <Required()> _
        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property
    End Class
    ' #end example

    Friend Class Car
        Private m_name As String
        Private m_drivenBy As Pilot

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        <Required()> _
        Public Property DrivenBy() As Pilot
            Get
                Return m_drivenBy
            End Get
            Set(ByVal value As Pilot)
                m_drivenBy = value
            End Set
        End Property

        <Required()> _
        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property
    End Class
End Namespace
