using System;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.TA;

namespace Db4oTutorialCode.Code.Transactions
{
public class Transactions {
    private const string DatabaseFile = "database.db4o";

    public static void Main(string[] args) {
        StoreExampleObjects();
        CommitTransactions();
        RollbackTransactions();
        ObjectStateAfterRollbackWithoutTp();
        ObjectStateAfterRollbackWithTp();
        MultipleTransactions();
    }

    private static void CommitTransactions() {
        using(IObjectContainer container = OpenDatabase())
        {
            var toyota = new Car("Toyota Corolla");
            var jimmy = new Driver("Jimmy", toyota);
            container.Store(jimmy);
            // #example: Committing changes
            container.Commit();
            // #end example
        }
    }

    private static void RollbackTransactions() {
        using(IObjectContainer container = OpenDatabase())
        {
            var toyota = new Car("Toyota Corolla");
            var jimmy = new Driver("Jimmy", toyota);
            container.Store(jimmy);
            // #example: Rollback changes
            container.Rollback();
            // #end example
        } 
    }

    private static void ObjectStateAfterRollbackWithoutTp() {
        IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile);
        {
            // #example: Without transparent persistence objects in memory aren't rolled back
            Driver driver = QueryForDriver(container);
            driver.Name = "New Name";
            Console.WriteLine("Name before rollback {0}", driver.Name);
            container.Rollback();
            // Without transparent persistence objects keep the state in memory
            Console.WriteLine("Name after rollback {0}", driver.Name);
            // After refreshing the object is has the state like in the database
            container.Ext().Refresh(driver, int.MaxValue);
            Console.WriteLine("Name after rollback {0}", driver.Name);
            // #end example
        }
    }

    private static void ObjectStateAfterRollbackWithTp() {
        using(IObjectContainer container = OpenDatabase())
        {
            // #example: Transparent persistence rolls back objects in memory
            Driver driver = QueryForDriver(container);
            driver.Name = "New Name";
            Console.WriteLine("Name before rollback {0}", driver.Name);
            container.Rollback();
            // Thanks to transparent persistence with the rollback strategy
            // the object state is rolled back
            Console.WriteLine("Name after rollback {0}",driver.Name);
            // #end example
        } 
    }
    private static void MultipleTransactions() {
        using(IObjectContainer rootContainer = OpenDatabase())
        {
            // #example: Opening a new transaction
            using(IObjectContainer container = rootContainer.Ext().OpenSession())
            {
                // We do our operations in this transaction
            } 
            // #end example
        }
    }


    private static void StoreExampleObjects() {
        using(IObjectContainer container = OpenDatabase())
        {
            var vwBeetle = new Car("VW Beetle");
            var audi = new Car("Audi A6");
            var ferrari = new Car("Ferrari");

            var joe = new Driver("Joe", audi);
            var joanna = new Driver("Joanna", vwBeetle);
            var jenny = new Driver("Jenny");
            var john = new Driver("John", ferrari);
            var jim = new Driver("Jim", audi);

            container.Store(joe);
            container.Store(joanna);
            container.Store(jenny);
            container.Store(john);
            container.Store(jim);
        } 
    }

    private static IObjectContainer OpenDatabase() {
        // #example: Rollback strategy for the transaction
        IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
        configuration.Common.Add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
        // #end example
        return Db4oEmbedded.OpenFile(configuration, DatabaseFile);
    }

    private static Driver QueryForDriver(IObjectContainer container)
    {
        return (from Driver d in container
               select d).First();
    }
}
}