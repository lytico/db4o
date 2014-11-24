Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Diagnostic

Namespace Db4oDoc.Code.Tuning.Diagnostics
    Public Class DiagnosticsExamples
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            FilterDiagnosticMessages()
        End Sub

        Private Shared Sub FilterDiagnosticMessages()
            CleanUp()
            Using container As IObjectContainer = OpenDatabase()
                container.Store(New SimpleClass())
                Dim result As IList(Of SimpleClass) = RunQuery(container)
                PrintResult(result)
            End Using
            CleanUp()
        End Sub

        Private Shared Function RunQuery(ByVal container As IObjectContainer) As IList(Of SimpleClass)
            Return container.Query(Function(cwf As SimpleClass) cwf.Number < 100)
        End Function

        Private Shared Sub PrintResult(ByVal result As IEnumerable(Of SimpleClass))
            For Each item As SimpleClass In result
                Console.Out.WriteLine(item)
            Next
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub

        Private Shared Function OpenDatabase() As IObjectContainer
            ' #example: Filter for unindexed fields
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Diagnostic _
                .AddListener(New DiagnosticFilter(New DiagnosticToConsole(), GetType(LoadedFromClassIndex)))
            ' #end example
            Return Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
        End Function

        ' #example: A simple message filter
        Private Class DiagnosticFilter
            Implements IDiagnosticListener
            Private ReadOnly filterFor As ICollection(Of Type)
            Private ReadOnly target As IDiagnosticListener

            Public Sub New(ByVal target As IDiagnosticListener, ByVal ParamArray filterFor As Type())
                Me.target = target
                Me.filterFor = New List(Of Type)(filterFor)
            End Sub

            Public Sub OnDiagnostic(ByVal diagnostic As IDiagnostic) _
                Implements IDiagnosticListener.OnDiagnostic

                Dim type As Type = diagnostic.GetType()
                If filterFor.Contains(type) Then
                    target.OnDiagnostic(diagnostic)
                End If
            End Sub
        End Class
        ' #end example

        Private Class SimpleClass
            Private m_number As Integer = 0

            Public Property Number() As Integer
                Get
                    Return m_number
                End Get
                Set(ByVal value As Integer)
                    m_number = value
                End Set
            End Property
        End Class
    End Class
End Namespace