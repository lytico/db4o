package com.db4o.cs.internal.objectexchange;

import com.db4o.internal.*;
import com.db4o.internal.slots.*;

public class StandardSlotAccessor implements SlotAccessor {

	private LocalTransaction _transaction;
	
	public StandardSlotAccessor(LocalTransaction transaction) {
		_transaction = transaction;
	}

	public Slot currentSlotOfID(int id) {
		return _transaction.idSystem().currentSlot(id);
    }

}
