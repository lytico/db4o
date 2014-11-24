Imports System.Collections
Imports System.IO
Imports System.Text.RegularExpressions
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Query

Namespace Db4oDoc.Code.Query.Soda
    Public Class SodaEvaluationExamples
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                StoreData(container)

                SimpleEvaluation(container)
                EvaluationOnField(container)
                RegexEvaluator(container)
            End Using
        End Sub


        Private Shared Sub SimpleEvaluation(ByVal container As IObjectContainer)
            Console.WriteLine("Simple evaluation")
            ' #example: Simple evaluation
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.Constrain(New OnlyOddAge())

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub EvaluationOnField(ByVal container As IObjectContainer)
            Console.WriteLine("Evaluation on field")
            ' #example: Evaluation on field
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Car))
            query.Descend("pilot").Constrain(New OnlyOddAge())

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub RegexEvaluator(ByVal container As IObjectContainer)
            Console.WriteLine("Regex-Evaluation")
            ' #example: Regex-evaluation on a field
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.Descend("name").Constrain(New RegexConstrain("J.*nn.*"))

            Dim result As IObjectSet = query.Execute()
            ' #end example
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

    ' #example: Simple evaluation which includes only odd aged pilots
    Class OnlyOddAge
        Implements IEvaluation
        Public Sub Evaluate(ByVal candidate As ICandidate) _
            Implements IEvaluation.Evaluate

            Dim pilot As Pilot = DirectCast(candidate.GetObject(), Pilot)
            candidate.Include(pilot.Age Mod 2 <> 0)
        End Sub
    End Class

    ' #end example

    ' #example: Regex Evaluator
    Class RegexConstrain
        Implements IEvaluation
        Private ReadOnly pattern As Regex

        Public Sub New(ByVal pattern As String)
            Me.pattern = New Regex(pattern)
        End Sub

        Public Sub Evaluate(ByVal candidate As ICandidate) _
            Implements IEvaluation.Evaluate
            Dim stringValue As String = DirectCast(candidate.GetObject(), String)
            candidate.Include(pattern.IsMatch(stringValue))
        End Sub
    End Class
    ' #end example
End Namespace