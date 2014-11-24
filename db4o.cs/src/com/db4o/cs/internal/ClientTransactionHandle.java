/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.internal.*;

public class ClientTransactionHandle {
	
    private final ClientTransactionPool _transactionPool;
    private Transaction _mainTransaction;
    private Transaction _transaction;
    private boolean _rollbackOnClose;
    
    public ClientTransactionHandle(ClientTransactionPool transactionPool) {
		_transactionPool = transactionPool;
        _mainTransaction = _transactionPool.acquireMain();
		_rollbackOnClose = true;
	}

    public void acquireTransactionForFile(String fileName) {
        _transaction = _transactionPool.acquire(fileName);
	}
	
    public void releaseTransaction(ShutdownMode mode) {
		if (_transaction != null) {
			_transactionPool.release(mode, _transaction, _rollbackOnClose);
			_transaction = null;
		}
	}
	
    public boolean isClosed() {
		return _transactionPool.isClosed();
	}
    
    public void close(ShutdownMode mode) {
		if ((!_transactionPool.isClosed()) && (_mainTransaction != null)) {
			_transactionPool.release(mode, _mainTransaction, _rollbackOnClose);
        }
	}
	
    public Transaction transaction() {
        if (_transaction != null) {
            return _transaction;
        }
        return _mainTransaction;
    }

    public void transaction(Transaction transaction) {
		if (_transaction != null) {
			_transaction = transaction;
		} else {
			_mainTransaction = transaction;
		}
		_rollbackOnClose = false;
    }

}
