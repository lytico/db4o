Imports System
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Activation
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.TA

Namespace Db4oDoc.Code.TA.Collections
    Public Class CollectionsExample
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            Using container As IObjectContainer = openDatabaseWithTA()
                Dim team As New Team()
                team.Add(New Pilot("John"))
                team.Add(New Pilot("Max"))
                container.Store(team)
            End Using
            Using container As IObjectContainer = openDatabaseWithTA()
                Dim team As Team = container.Query(Of Team)()(0)
                For Each pilot As Pilot In team.Pilots
                    Console.WriteLine(pilot)
                Next
            End Using
            CleanUp()
        End Sub

        Private Shared Function openDatabaseWithTA() As IObjectContainer
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentActivationSupport())
            Return Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
        End Function

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub
    End Class


    Public MustInherit Class ActivatableBase
        Implements IActivatable
        <Transient()> _
        Private activator As IActivator

        Public Sub Bind(ByVal activator As IActivator) _
                Implements IActivatable.Bind
            If Me.activator Is activator Then
                Exit Sub
            End If
            If activator IsNot Nothing AndAlso Me.activator IsNot Nothing Then
                Throw New InvalidOperationException("Object can only be bound to one activator")
            End If
            Me.activator = activator
        End Sub

        Public Sub Activate(ByVal activationPurpose As ActivationPurpose) _
                Implements IActivatable.Activate
            If activator IsNot Nothing Then
                activator.Activate(activationPurpose)
            End If
        End Sub
    End Class

    Public Class Pilot
        Inherits ActivatableBase
        Private m_name As String

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Property Name() As String
            Get
                Activate(ActivationPurpose.Read)
                Return m_name
            End Get
            Set(ByVal value As String)
                Activate(ActivationPurpose.Write)
                m_name = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Class
End Namespace
