Imports System
Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.DisconnectedObj.IdExamples
    Public Module IdExamples
        
        Public Sub Main(ByVal args As String())
            RunExamples(CreateRunner(Db4oInternalIdExample.Create()), CreateRunner(Db4oUuidExample.Create()), CreateRunner(UuidOnObject.Create()), CreateRunner(AutoIncrementExample.Create()))
        End Sub

        Private Sub RunExamples(ByVal ParamArray examplesToRun As IRunnable())
            For Each toRun As IRunnable In examplesToRun
                toRun.Run()
            Next
        End Sub

        Private Function CreateRunner(Of TId)(ByVal toRun As IIdExample(Of TId)) As IdExamples(Of TId)
            Return New IdExamples(Of TId)(toRun)
        End Function
    End Module


    Public Interface IRunnable
        Sub Run()
    End Interface

    Public Class IdExamples(Of TId)
        Implements IRunnable
        Private Const DatabaseFileName As String = "database.db4o"
        Private ReadOnly toRun As IIdExample(Of TId)

        Public Sub New(ByVal toRun As IIdExample(Of TId))
            Me.toRun = toRun
        End Sub


        Public Sub Run() Implements IRunnable.Run
            Console.WriteLine("Running: " & toRun.GetType().Name)
            CleanUp()
            StoreJoe()

            Dim id As TId = IDOfJoe()
            Dim incomingChanges As New Pilot("Joe Junior")

            UpdateJoe(id, incomingChanges)

            AssertWasUpdated()
            ListAllPilots()

            CleanUp()
        End Sub

        Private Sub AssertWasUpdated()
            Using container As IObjectContainer = OpenDatabase()
                Dim pilots As IList(Of Pilot) = container.Query(Of Pilot)()
                AssertEquals(1, pilots.Count)
                AssertEquals("Joe Junior", pilots(0).Name)
            End Using
        End Sub

        Private Shared Sub AssertEquals(ByVal expected As Object, ByVal actual As Object)
            If Not expected.Equals(actual) Then
                Throw New InvalidOperationException(("Expected to be " & expected & " but is ") + actual)
            End If
        End Sub

        Private Sub ListAllPilots()
            Using container As IObjectContainer = OpenDatabase()
                Dim pilots As IList(Of Pilot) = container.Query(Of Pilot)()
                For Each pilot As Pilot In pilots
                    Console.WriteLine(pilot)
                Next
            End Using
        End Sub


        Private Sub UpdateJoe(ByVal id As TId, ByVal incomingChanges As Pilot)
            Using container As IObjectContainer = OpenDatabase()
                Dim joe As Pilot = DirectCast(toRun.ObjectForID(id, container), Pilot)
                MergeChanges(joe, incomingChanges)
                container.Store(joe)
            End Using
        End Sub

        Private Shared Sub MergeChanges(ByVal toUpdate As Pilot, ByVal incomingChanges As Pilot)
            toUpdate.Name = incomingChanges.Name
        End Sub

        Private Function IDOfJoe() As TId
            Using container As IObjectContainer = OpenDatabase()
                Dim joe As Pilot = QueryByName(container, "Joe")
                Dim id As TId = toRun.IdForObject(joe, container)
                Return id
            End Using
        End Function


        Private Shared Function QueryByName(ByVal container As IObjectContainer, ByVal name As String) As Pilot
            Return (From p As Pilot In container _
                Where p.Name.Equals(name) _
                Select p).First()
        End Function

        Private Sub StoreJoe()
            Using container As IObjectContainer = OpenDatabase()
                Dim joe As New Pilot("Joe")
                container.Store(joe)
            End Using
        End Sub


        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub


        Private Function OpenDatabase() As IObjectContainer
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            toRun.Configure(configuration)
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
            toRun.RegisterEventOnContainer(container)
            Return container
        End Function
    End Class

    Public Module ObjectIdPair
        Public Function Create(Of TId, TObject)(ByVal id As TId, ByVal instance As TObject) As ObjectIdPair(Of TId, TObject)
            Return New ObjectIdPair(Of TId, TObject)(id, instance)
        End Function
    End Module

    Public Structure ObjectIdPair(Of TId, TObject)
        Private ReadOnly m_id As TId
        Private ReadOnly m_instance As TObject

        Public Sub New(ByVal id As TId, ByVal instance As TObject)
            Me.m_id = id
            Me.m_instance = instance
        End Sub

        Public ReadOnly Property ID() As TId
            Get
                Return m_id
            End Get
        End Property

        Public ReadOnly Property Instance() As TObject
            Get
                Return m_instance
            End Get
        End Property
    End Structure
End Namespace
