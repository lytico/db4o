using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Constraints;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.Ext;

namespace Db4oDoc.Code.Strategies.Exceptions
{
    public class ImportantExceptionCases
    {
        public static void Main(string[] args)
        {
            AlreadyOpenDatabaseThrows();
            ConnectToNotExistingServer();
            UniqueViolation();
        }

        private static void UniqueViolation()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (UniqueId)).ObjectField("id").Indexed(true);
            configuration.Common.Add(new UniqueFieldValueConstraint(typeof (UniqueId), "id"));
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                // #example: Violation of the unique constraint
                container.Store(new UniqueId(42));
                container.Store(new UniqueId(42));
                try
                {
                    container.Commit();
                }
                catch (UniqueFieldValueConstraintViolationException e)
                {
                    // Violated the unique-constraint!
                    // Retry with a new value or handle this gracefully
                    container.Rollback();
                }
                // #end example
            }
        }

        private static void ConnectToNotExistingServer()
        {
            // #example: Cannot connect to the server
            try
            {
                IObjectContainer container = Db4oClientServer.OpenClient("localhost", 1337, "sa", "sa");
            }
            catch (Db4oIOException e)
            {
                // Couldn't connect to the server.
                // Ask for new connection-settings or handle this case gracefully
            }
            // #end example
        }

        private static void AlreadyOpenDatabaseThrows()
        {
            IObjectContainer allReadyOpen = Db4oEmbedded.OpenFile("database.db4o");
            try
            {
                // #example: If the database is already open
                try
                {
                    IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o");
                }
                catch (DatabaseFileLockedException e)
                {
                    // Database is already open!
                    // Use another database-file or handle this case gracefully
                }
                // #end example
            }
            finally
            {
                allReadyOpen.Close();
            }
        }


        private class UniqueId
        {
            private int id;

            internal UniqueId(int id)
            {
                this.id = id;
            }
        }
    }
}