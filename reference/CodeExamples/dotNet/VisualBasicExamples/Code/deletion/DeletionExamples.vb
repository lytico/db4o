Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.Deletion
    Public Class DeletionExamples
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            SimpleDeletion()
            ReferenceIsSetToNull()
            CascadeDeletion()

            RemoveFromCollection()
            RemoveAndDelete()
        End Sub

        Private Shared Sub SimpleDeletion()
            PrepareDBWithCarAndPilot()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Deleting object is as simple as storing
                Dim car As Car = FindCar(container)
                container.Delete(car)
                ' We've deleted the only care there is
                AssertEquals(0, AllCars(container).Count)
                ' The pilots are still there
                AssertEquals(1, AllPilots(container).Count)
                ' #end example
            End Using
        End Sub

        Private Shared Sub ReferenceIsSetToNull()
            PrepareDBWithCarAndPilot()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Delete the pilot
                Dim pilot As Pilot = FindPilot(container)
                container.Delete(pilot)
                ' #end example
            End Using
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Reference is null after deleting
                ' Now the car's reference to the car is set to null
                Dim car As Car = FindCar(container)
                AssertEquals(Nothing, car.Pilot)
                ' #end example
            End Using
        End Sub

        Private Shared Sub CascadeDeletion()
            PrepareDBWithCarAndPilot()
            ' #example: Mark field for cascading deletion
            Dim config As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            config.Common.ObjectClass(GetType(Car)).ObjectField("pilot").CascadeOnDelete(True)
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(config, DatabaseFile)
                ' #end example
                ' #example: Cascade deletion
                Dim car As Car = FindCar(container)
                container.Delete(car)
                ' Now the pilot is also gone
                AssertEquals(0, AllPilots(container).Count)
                ' #end example
            End Using
        End Sub

        Private Shared Sub RemoveFromCollection()
            PrepareDBWithPilotGroup()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Removing from a collection doesn't delete the collection-members
                Dim group As PilotGroup = FindGroup(container)
                Dim pilot As Pilot = group.Pilots(0)
                group.Pilots.Remove(pilot)
                container.Store(group.Pilots)

                AssertEquals(3, AllPilots(container).Count)
                AssertEquals(2, group.Pilots.Count)
                ' #end example
            End Using
        End Sub

        Private Shared Sub RemoveAndDelete()
            PrepareDBWithPilotGroup()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Remove and delete
                Dim group As PilotGroup = FindGroup(container)
                Dim pilot As Pilot = group.Pilots(0)
                group.Pilots.Remove(pilot)
                container.Store(group.Pilots)
                container.Delete(pilot)

                AssertEquals(2, AllPilots(container).Count)
                AssertEquals(2, group.Pilots.Count)
                ' #end example
            End Using
        End Sub

        Private Shared Sub AssertEquals(ByVal expectedValue As Object, ByVal actualValue As Object)
            If Not Equals(expectedValue, actualValue) Then
                Throw New Exception("Expected " & Convert.ToString(expectedValue) & " but got " & Convert.ToString(actualValue))
            End If
        End Sub

        Private Shared Function FindPilot(ByVal container As IObjectContainer) As Pilot
            Return AllPilots(container)(0)
        End Function

        Private Shared Function AllPilots(ByVal container As IObjectContainer) As IList(Of Pilot)
            Return container.Query(Of Pilot)()
        End Function

        Private Shared Function FindCar(ByVal container As IObjectContainer) As Car
            Return AllCars(container)(0)
        End Function

        Private Shared Function AllCars(ByVal container As IObjectContainer) As IList(Of Car)
            Return container.Query(Of Car)()
        End Function

        Private Shared Function FindGroup(ByVal container As IObjectContainer) As PilotGroup
            Return container.Query(Of PilotGroup)()(0)
        End Function

        Private Shared Sub PrepareDBWithCarAndPilot()
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                container.Store(New Car(New Pilot("John"), "VM Golf"))
            End Using
        End Sub

        Private Shared Sub PrepareDBWithPilotGroup()
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                container.Store(New PilotGroup(New Pilot("John"), New Pilot("Jenny"), New Pilot("Joanna")))
            End Using
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub
    End Class

    Friend Class Pilot
        Private m_name As String

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property
    End Class

    Friend Class Car
        Private m_name As String
        Private m_pilot As Pilot

        Public Sub New(ByVal pilot As Pilot, ByVal name As String)
            Me.m_name = name
            Me.m_pilot = pilot
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Property Pilot() As Pilot
            Get
                Return m_pilot
            End Get
            Set(ByVal value As Pilot)
                m_pilot = value
            End Set
        End Property
    End Class

    Friend Class PilotGroup
        Private ReadOnly m_pilots As IList(Of Pilot) = New List(Of Pilot)()

        Friend Sub New(ByVal ParamArray pilots As Pilot())
            For Each pilot As Pilot In pilots
                Me.m_pilots.Add(pilot)
            Next
        End Sub

        Public ReadOnly Property Pilots() As IList(Of Pilot)
            Get
                Return m_pilots
            End Get
        End Property
    End Class
End Namespace
