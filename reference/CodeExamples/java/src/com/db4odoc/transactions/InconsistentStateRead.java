package com.db4odoc.transactions;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;

import java.io.File;
import java.util.List;

public class InconsistentStateRead {
    private static final int INITIAL_MONEY_ON_ONE_ACCOUNT = 1000;
    private static final String DATABASE_FILE = "database.db4o";
    private final ObjectContainer container;

    public static void main(String[] args) {
        new File(DATABASE_FILE).delete();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            new InconsistentStateRead(container).main();
        } finally {
            container.close();
        }
    }

    public InconsistentStateRead(ObjectContainer container) {
        this.container = container;
    }

    private void main() {
        storeInitialBankAccounts();

        showIsolationIssue();
    }

    private void showIsolationIssue() {
        inTransaction(new DatabaseOperation() {
            @Override
            public void invoke(ObjectContainer container) {
                // #example: We list the bank accounts and sum up the money
                long moneyInOurAccounts = 0;
                List<BankAccount> bankAccounts = container.query(BankAccount.class);
                for (BankAccount account : bankAccounts) {
                    System.out.println("This account has "+account.money());
                    moneyInOurAccounts +=account.money();
                    moveMoneyTransactionFinishes();
                }
                // We get the wrong answer here
                System.out.println("The money total is "+moneyInOurAccounts
                        +". Expected is "+INITIAL_MONEY_ON_ONE_ACCOUNT*bankAccounts.size());
                // #end example
            }
        });
    }

    private void moveMoneyTransactionFinishes() {
        inTransaction(new DatabaseOperation() {
            @Override
            public void invoke(ObjectContainer container) {
                // #example: Meanwhile we transfer money.
                List<BankAccount> bankAccounts = container.query(BankAccount.class);
                final BankAccount debitAccount = bankAccounts.get(0);
                final BankAccount creditAccount = bankAccounts.get(1);

                int moneyToTransfer = 200;
                creditAccount.withdraw(moneyToTransfer);
                debitAccount.deposit(moneyToTransfer);

                container.store(debitAccount);
                container.store(creditAccount);
                container.commit();
                // #end example
            }
        });
    }

    private void storeInitialBankAccounts() {
        inTransaction(new DatabaseOperation() {
            @Override
            public void invoke(ObjectContainer container) {
                container.store(new BankAccount("Account A",INITIAL_MONEY_ON_ONE_ACCOUNT));
                container.store(new BankAccount("Account B",INITIAL_MONEY_ON_ONE_ACCOUNT));
            }
        });
    }

    private void inTransaction(DatabaseOperation operation) {
        ObjectContainer container = this.container.ext().openSession();
        try {
            operation.invoke(container);
        } finally {
            container.close();
        }
    }
}
