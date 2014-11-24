/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import java.io.*;

import com.db4o.*;
import com.db4o.query.*;

public class EncryptionWrongPassword {
	
	public String name;
	
	public void storeOne() {
		name = "hi";
	}
	
	public void testOne() {
		Db4o.configure().password("wrong");
		Db4o.configure().encrypt(true);
		PrintStream nulout=new PrintStream(new ByteArrayOutputStream());
		Db4o.configure().setOut(nulout);
		try {
			Test.reOpenServer();
            
            // Encryption is turned off, we no longer get the
            // exception above, that's correct behaviour.
            
			// Test.error("expected failure on wrong password");
		}
		catch(Exception exc) {
			// OK, expected
		}
		Db4o.configure().encrypt(false);
        Db4o.configure().password(null);
        
		Db4o.configure().setOut(null);
		Test.reOpenServer();

		Query query=Test.query();
		query.constrain(this.getClass());
		Test.ensure(((EncryptionWrongPassword)query.execute().next()).name.equals(name));
	}
}
