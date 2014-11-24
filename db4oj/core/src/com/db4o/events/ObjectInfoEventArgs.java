package com.db4o.events;

import com.db4o.ext.*;
import com.db4o.internal.*;

public class ObjectInfoEventArgs extends ObjectEventArgs {

	private final ObjectInfo _info;

	public ObjectInfoEventArgs(Transaction transaction, ObjectInfo info) {
	    super(transaction);
	    _info = info;
    }

	@Override
    public Object object() {
		return _info.getObject();
    }
	
	/**
	 * @sharpen.property
	 */
	public ObjectInfo info() {
		return _info;
	}

}
