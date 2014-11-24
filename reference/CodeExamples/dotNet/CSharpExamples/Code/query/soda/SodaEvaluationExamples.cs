using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;

namespace Db4oDoc.Code.Query.Soda
{
    public class SodaEvaluationExamples
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                StoreData(container);

                SimpleEvaluation(container);
                EvaluationOnField(container);
                RegexEvaluator(container);
            }
        }


        private static void SimpleEvaluation(IObjectContainer container)
        {
            Console.WriteLine("Simple evaluation");
            // #example: Simple evaluation
            IQuery query = container.Query();
            query.Constrain(typeof (Pilot));
            query.Constrain(new OnlyOddAge());

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void EvaluationOnField(IObjectContainer container)
        {
            Console.WriteLine("Evaluation on field");
            // #example: Evaluation on field
            IQuery query = container.Query();
            query.Constrain(typeof (Car));
            query.Descend("pilot").Constrain(new OnlyOddAge());

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void RegexEvaluator(IObjectContainer container)
        {
            Console.WriteLine("Regex-Evaluation");
            // #example: Regex-evaluation on a field
            IQuery query = container.Query();
            query.Constrain(typeof (Pilot));
            query.Descend("name").Constrain(new RegexConstrain("J.*nn.*"));

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
        }


        private static void ListResult(IEnumerable result)
        {
            foreach (object obj in result)
            {
                Console.WriteLine(obj);
            }
        }

        private static void StoreData(IObjectContainer container)
        {
            Pilot john = new Pilot("John", 42);
            Pilot joanna = new Pilot("Joanna", 45);
            Pilot jenny = new Pilot("Jenny", 21);
            Pilot rick = new Pilot("Rick", 33);

            container.Store(new Car(john, "Ferrari"));
            container.Store(new Car(joanna, "Mercedes"));
            container.Store(new Car(jenny, "Volvo"));
            container.Store(new Car(rick, "Fiat"));
        }
    }


    // #example: Simple evaluation which includes only odd aged pilots
    class OnlyOddAge : IEvaluation
    {
        public void Evaluate(ICandidate candidate)
        {
            Pilot pilot = (Pilot)candidate.GetObject();
            candidate.Include(pilot.Age % 2 != 0);
        }
    }

    // #end example

    // #example: Regex Evaluator
    class RegexConstrain : IEvaluation
    {
        private readonly Regex pattern;

        public RegexConstrain(String pattern)
        {
            this.pattern = new Regex(pattern);
        }

        public void Evaluate(ICandidate candidate)
        {
            string stringValue = (string)candidate.GetObject();
            candidate.Include(pattern.IsMatch(stringValue));
        }
    }
    // #end example

}