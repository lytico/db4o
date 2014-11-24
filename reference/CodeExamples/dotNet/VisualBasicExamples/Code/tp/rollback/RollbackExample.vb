Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.TA

Namespace Db4oDoc.Code.Tp.Rollback
    Public Class RollbackExample
        Public Shared Sub Main(ByVal args As String())
            ' #example: Configure rollback strategy
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentPersistenceSupport(New DeactivatingRollbackStrategy()))
            ' #end example
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                StorePilot(container)

                ' #example: Rollback with rollback strategy
                Dim pilot As Pilot = container.Query(Of Pilot)()(0)
                pilot.Name = "NewName"
                ' Rollback
                container.Rollback()
                ' Now the pilot has the old name again
                Console.Out.WriteLine(pilot.Name)
                ' #end example
            End Using
        End Sub

        Private Shared Sub StorePilot(ByVal container As IObjectContainer)
            container.Store(New Pilot("John"))
            container.Commit()
        End Sub
    End Class

    <PersistanceAware()> _
    Class Pilot
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
End Namespace
