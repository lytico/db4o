using System;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;

namespace Db4oTutorialCode.Code.Activation
{
    public class ActivationConcept
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            StoreExampleObjects();
            RunningIntoActivationLimit();
            DealWithActivation();
            IncreaseActivationDepth();
        }

        private static void StoreExampleObjects()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Store a deep object hierarchy
                var eva = new Person("Eva", null);
                var julia = new Person("Julia", eva);
                var jennifer = new Person("Jennifer", julia);
                var jamie = new Person("Jamie", jennifer);
                var jill = new Person("Jill", jamie);
                var joanna = new Person("Joanna", jill);

                var joelle = new Person("Joelle", joanna);
                container.Store(joelle);
                // #end example
            }
        }

        private static void RunningIntoActivationLimit()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Activation depth in action
                Person joelle = QueryForJoelle(container);
                Person jennifer = joelle.Mother.Mother.Mother.Mother;
                Console.WriteLine("Is activated: {0}", jennifer);
                // Now we step across the activation boundary
                // therefore the next person isn't activate anymore.
                // That means all fields are set to null or default-value
                Person julia = jennifer.Mother;
                Console.WriteLine("Isn't activated anymore {0}", julia);
                // #end example

                try
                {
                    // #example: NullPointer exception due to not activated objects
                    string nameOfMother = julia.Mother.Name;
                    // #end example
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("Exception due to not activated object {0}", ex);
                }
            }
        }

        private static void DealWithActivation()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Person joelle = QueryForJoelle(container);
                Person julia = joelle.Mother.Mother.Mother.Mother.Mother;

                // #example: Check if an instance is activated
                bool isActivated = container.Ext().IsActive(julia);
                // #end example
                Console.WriteLine("Is activated? {0}", isActivated);
                // #example: Activate instance to a depth of five
                container.Activate(julia, 5);
                // #end example
                Console.WriteLine("Is activated? {0}", container.Ext().IsActive(julia));
            }
        }

        private static void IncreaseActivationDepth()
        {
            // #example: Increase the activation depth to 10
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ActivationDepth = 10;
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile))
                // #end example
            {
                Person joelle = QueryForJoelle(container);
                Person julia = joelle.Mother.Mother.Mother.Mother.Mother;

                bool isActivated = container.Ext().IsActive(julia);
                Console.WriteLine("Is activated? {0}", isActivated);
            }
        }

        private static void MoreActivationOptions()
        {
            // #example: More activation options
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // At least activate persons to a depth of 10
            configuration.Common.ObjectClass(typeof (Person)).MinimumActivationDepth(10);
            // Or maybe we just want to activate all referenced objects
            configuration.Common.ObjectClass(typeof (Person)).CascadeOnActivate(true);
            // #end example
        }

        private static Person QueryForJoelle(IObjectContainer container)
        {
            return (from Person p in container
                    where p.Name == "Joelle"
                    select p).Single();
        }
    }
}