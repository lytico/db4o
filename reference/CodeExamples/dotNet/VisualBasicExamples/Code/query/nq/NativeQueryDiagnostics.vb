Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Diagnostic

Namespace Db4oDoc.Code.Query.NativeQueries
    Public Class NativeQueryDiagnostics

        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Use diagnostics to find unoptimized queries
            configuration.Common.Diagnostic.AddListener(New NativeQueryListener())
            ' #end example
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
                StoreData(container)

                OptimizedNativeQuery(container)

                NotOptimizedNativeQuery(container)
            End Using
        End Sub


        Private Shared Sub OptimizedNativeQuery(ByVal container As IObjectContainer)
            Dim result As IList(Of Pilot) = container.Query(Function(pilot As Pilot) pilot.Name.StartsWith("J"))

            ListResult(result)
        End Sub

        Private Shared Sub NotOptimizedNativeQuery(ByVal container As IObjectContainer)
            Dim result As IList(Of Pilot) = container.Query(Function(pilot As Pilot) pilot.Name.ToUpper(CultureInfo.CurrentUICulture).StartsWith("J"))

            ListResult(result)
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub


        Private Shared Sub ListResult(ByVal result As IEnumerable)
            For Each obj As Object In result
                Console.WriteLine(obj)
            Next
        End Sub

        Private Shared Sub StoreData(ByVal container As IObjectContainer)
            Dim john As New Pilot("John", 42)
            Dim joanna As New Pilot("Joanna", 45)
            Dim jenny As New Pilot("Jenny", 21)
            Dim rick As New Pilot("Rick", 33)

            container.Store(New Car(john, "Ferrari"))
            container.Store(New Car(joanna, "Mercedes"))
            container.Store(New Car(jenny, "Volvo"))
            container.Store(New Car(rick, "Fiat"))
        End Sub

    End Class

    ' #example: The native query listener
    Class NativeQueryListener
        Implements IDiagnosticListener
        Public Sub OnDiagnostic(ByVal diagnostic As IDiagnostic) Implements IDiagnosticListener.OnDiagnostic

            If TypeOf diagnostic Is NativeQueryNotOptimized Then
                Console.WriteLine("Query not optimized" & Convert.ToString(diagnostic))
            ElseIf TypeOf diagnostic Is NativeQueryOptimizerNotLoaded Then
                Console.WriteLine("Missing native query optimisation assemblies" & Convert.ToString(diagnostic))
            End If
        End Sub
    End Class
    ' #end example
End Namespace