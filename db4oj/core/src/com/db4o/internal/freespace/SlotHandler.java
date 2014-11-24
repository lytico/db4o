/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.freespace;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.slots.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public abstract class SlotHandler implements Indexable4{
	
	protected Slot _current;
	
	public void defragIndexEntry(DefragmentContextImpl context) {
		throw new NotImplementedException();
	}

	public int linkLength() {
		return Slot.MARSHALLED_LENGTH;
	}

	public Object readIndexEntry(Context context, ByteArrayBuffer reader) {
		return new Slot(reader.readInt(), reader.readInt());
	}

	public void writeIndexEntry(Context context, ByteArrayBuffer writer, Object obj) {
		Slot slot = (Slot) obj;
		writer.writeInt(slot.address());
		writer.writeInt(slot.length());
	}


}
