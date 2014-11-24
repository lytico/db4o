/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import java.io.*;

import com.db4o.*;
import com.db4o.internal.handlers.*;


final class MessageOutput
{
	final PrintStream stream;

	MessageOutput(ObjectContainerBase a_stream, String msg){
		stream = a_stream.configImpl().outStream();
		print(msg, true);
	}

	MessageOutput(String a_StringParam, int a_intParam, PrintStream a_stream, boolean header){
		stream = a_stream;
		print(Messages.get(a_intParam,a_StringParam), header );
	}

	MessageOutput(String a_StringParam, int a_intParam, PrintStream a_stream){
		this(a_StringParam, a_intParam , a_stream, true);
	}


	private void print(String msg, boolean header){
		if(stream != null){
			if(header){
				stream.println("[" + Db4o.version() + "   " + DateHandlerBase.now() + "] ");
			}
			stream.println(" " + msg);
		}
	}
}
