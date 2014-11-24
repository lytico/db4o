package com.db4o.cs.internal.messages;

import com.db4o.internal.*;

public class MReadBytes extends MsgD {

	public Msg getWriter(Transaction trans, ByteArrayBuffer bytes) {
		MsgD msg = getWriterForLength(trans, bytes.length());
		msg._payLoad.append(bytes._buffer);
		return msg;
	}
	
	public final ByteArrayBuffer unmarshall() {
		if(_payLoad._buffer.length == 0){
			return null;
		}
		return new ByteArrayBuffer(_payLoad._buffer);
	}

}
