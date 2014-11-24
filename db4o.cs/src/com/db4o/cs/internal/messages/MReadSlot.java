/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.internal.*;
import com.db4o.internal.slots.*;

public final class MReadSlot extends MsgD implements MessageWithResponse {
	
	public final ByteArrayBuffer getByteLoad() {
		int address = _payLoad.readInt();
		int length = _payLoad.length() - (Const4.INT_LENGTH);
        Slot slot = new Slot(address, length);
		_payLoad.removeFirstBytes(Const4.INT_LENGTH);
		_payLoad.useSlot(slot);
		return this._payLoad;
	}
	
	public final MsgD getWriter(StatefulBuffer bytes) {
		MsgD message = getWriterForLength(bytes.transaction(), bytes.length() + Const4.INT_LENGTH);
		message._payLoad.writeInt(bytes.getAddress());
		message._payLoad.append(bytes._buffer);
		return message;
	}
	
	public final Msg replyFromServer() {
		int address = readInt();
		int length = readInt();
		synchronized (containerLock()) {
			StatefulBuffer bytes =
				new StatefulBuffer(this.transaction(), address, length);
			try {
				container().readBytes(bytes._buffer, address, length);
				return getWriter(bytes);
			} catch (Exception e) {
				// TODO: not nicely handled on the client side yet
				return Msg.NULL;
			}
		}
	}

}