using System;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Transactions
{
    public class InconsistentStateRead
    {
        private const int InitialMoneyOnOneAccount = 1000;
        private const string DatabaseFile = "database.db4o";
        private readonly IObjectContainer rootContainer;

        public static void Main(string[] args)
        {
            File.Delete(DatabaseFile);
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                new InconsistentStateRead(container).Main();
            }
        }

        public InconsistentStateRead(IObjectContainer rootContainer)
        {
            this.rootContainer = rootContainer;
        }

        private void Main()
        {
            StoreInitialBankAccounts();

            ShowIsolationIssue();
        }

        private void ShowIsolationIssue()
        {
            InTransaction(
                container =>
                    {
                        // #example: We list the bank accounts and sum up the money
                        long moneyInOurAccounts = 0;
                        var bankAccounts = from BankAccount b in container select b;
                        foreach (BankAccount account in bankAccounts)
                        {
                            Console.WriteLine("This account has " + account.Money);
                            moneyInOurAccounts += account.Money;
                            MoveMoneyTransactionFinishes();
                        }
                        // We get the wrong answer here
                        Console.WriteLine("The money total is {0}. Expected is {1}",
                                          moneyInOurAccounts, InitialMoneyOnOneAccount*bankAccounts.Count());
                        // #end example
                    });
        }

        private void MoveMoneyTransactionFinishes()
        {
            InTransaction(
                container =>
                    {
                        // #example: Meanwhile we transfer money.
                        var bankAccounts = from BankAccount b in container select b;
                        var debitAccount = bankAccounts.ElementAt(0);
                        var creditAccount = bankAccounts.ElementAt(1);

                        int moneyToTransfer = 200;
                        creditAccount.WithDraw(moneyToTransfer);
                        debitAccount.Deposit(moneyToTransfer);

                        container.Store(debitAccount);
                        container.Store(creditAccount);
                        container.Commit();
                        // #end example
                    });
        }

        private void StoreInitialBankAccounts()
        {
            InTransaction(
                container =>
                    {
                        container.Store(new BankAccount("Account A", InitialMoneyOnOneAccount));
                        container.Store(new BankAccount("Account B", InitialMoneyOnOneAccount));
                    });
        }

        private void InTransaction(Action<IObjectContainer> operation)
        {
            using (IObjectContainer container = this.rootContainer.Ext().OpenSession())
            {
                operation(container);
            }
        }
    }

    internal class BankAccount
    {
        private string accountName;
        private long moneyOnAccountInCents;

        public BankAccount(string accountName, long moneyOnAccountInCents)
        {
            this.accountName = accountName;
            this.moneyOnAccountInCents = moneyOnAccountInCents;
        }

        public void Deposit(long amount)
        {
            moneyOnAccountInCents += amount;
        }

        public void WithDraw(long amount)
        {
            moneyOnAccountInCents -= amount;
        }

        public string AccountName
        {
            get { return accountName; }
        }


        public long Money
        {
            get { return moneyOnAccountInCents; }
        }
    }
}