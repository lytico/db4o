using System;
using System.Threading;
using Db4objects.Db4o;

namespace Db4oDoc.Code.Concurrency.Transactions
{
    public class DatabaseSupportWithReadWriteLock : IDisposable
    {
        private readonly ReaderWriterLockSlim transactionLock = new ReaderWriterLockSlim();
        private readonly IObjectContainer database;

        public DatabaseSupportWithReadWriteLock(IObjectContainer database)
        {
            this.database = database;
        }

        // #example: The read transaction method
        public T InWriteTransaction<T>(TransactionFunction<T> transactionClosure)
        {
            transactionLock.EnterWriteLock();
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
                transactionLock.ExitWriteLock();
            }
        }
        // #end example
        // #example: The write transaction method
        public T InReadTransaction<T>(TransactionFunction<T> transactionClosure)
        {
            transactionLock.EnterReadLock();
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
                transactionLock.ExitReadLock();
            }
        }
        // #end example

        public void InWriteTransaction(TransactionAction transactionClosure)
        {
            const int voidReturn = 0;
            InWriteTransaction(container =>
                              {
                                  transactionClosure(container);
                                  return voidReturn;
                              });
        }

        public void InReadTransaction(TransactionAction transactionClosure)
        {
            const int voidReturn = 0;
            InReadTransaction(container =>
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