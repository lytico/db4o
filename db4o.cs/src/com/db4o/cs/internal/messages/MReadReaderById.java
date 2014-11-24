package com.db4o.cs.internal.messages;

import com.db4o.ext.*;
import com.db4o.internal.*;

public class MReadReaderById extends MsgD implements MessageWithResponse {
	
	public final Msg replyFromServer() {
		ByteArrayBuffer bytes = null;
		// readWriterByID may fail in certain cases, for instance if
		// and object was deleted by another client
		try {
			synchronized (containerLock()) {
				bytes = container().readBufferById(transaction(), _payLoad.readInt(), _payLoad.readInt()==1);
			}
			if (bytes == null) {
				bytes = new ByteArrayBuffer(0);
			}
		}
		catch(Db4oRecoverableException exc) {
			throw exc;
		}
		catch(Throwable exc) {
			throw new Db4oRecoverableException(exc);
		}
		return Msg.READ_BYTES.getWriter(transaction(), bytes);
	}
}