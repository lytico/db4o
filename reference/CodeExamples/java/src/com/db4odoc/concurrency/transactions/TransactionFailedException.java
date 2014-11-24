package com.db4odoc.concurrency.transactions;

public class TransactionFailedException extends RuntimeException{
    public TransactionFailedException(String message, Throwable cause) {
        super(message, cause);
    }
}
