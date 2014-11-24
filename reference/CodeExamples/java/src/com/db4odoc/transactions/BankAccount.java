package com.db4odoc.transactions;


class BankAccount {
    private String accountName;
    private long moneyOnAccountInCents;

    BankAccount(String accountName, long moneyOnAccountInCents) {
        this.accountName = accountName;
        this.moneyOnAccountInCents = moneyOnAccountInCents;
    }

    public void deposit(long amount){
        moneyOnAccountInCents += amount;
    }

    public void withdraw(long amount){
        moneyOnAccountInCents -= amount;
    }

    public long money() {
        return moneyOnAccountInCents;
    }
}
