using System;
using Db4objects.Db4o;

namespace Db4oDoc.Code.Concurrency.Transactions
{
    public delegate void TransactionAction(IObjectContainer container);

    public delegate T TransactionFunction<out T>(IObjectContainer container);

    public class DatabaseSupport : IDisposable
    {
        private readonly object transactionLock = new object();
        private readonly IObjectContainer database;

        public DatabaseSupport(IObjectContainer database)
        {
            this.database = database;
        }

        // #example: A transaction method
        public T InTransaction<T>(TransactionFunction<T> transactionClosure)
        {
            lock (transactionLock)
            {
                try
                {
                    return transactionClosure(database);
                }
                catch (Exception)
                {
                    database.Rollback();
                    throw;
                }
                finally
                {
                    database.Commit();
                }
            }
        }

        // #end example

        public void InTransaction(TransactionAction transactionClosure)
        {
            const int voidReturn = 0;
            InTransaction(container =>
                              {
                                  transactionClosure(container);
                                  return voidReturn;
                              });
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}