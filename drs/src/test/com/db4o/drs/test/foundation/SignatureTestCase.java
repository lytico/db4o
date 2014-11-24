/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.foundation;

import com.db4o.drs.foundation.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.encoding.*;

import db4ounit.*;

public class SignatureTestCase implements TestCase {
	
	public void test(){
        StatefulBuffer writer = new StatefulBuffer(null, 300);
        String stringRepresentation = SignatureGenerator.generateSignature();
		new LatinStringIO().write(writer, stringRepresentation);
		Signature signature = new Signature(writer.getWrittenBytes());
		Assert.areEqual(stringRepresentation, signature.toString());
	}

}
