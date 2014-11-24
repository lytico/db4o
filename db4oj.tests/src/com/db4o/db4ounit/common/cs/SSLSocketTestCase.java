/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.cs;

import java.io.*;
import java.security.*;

import javax.net.ssl.*;

import com.db4o.config.*;
import com.db4o.cs.internal.*;
import com.db4o.cs.ssl.*;
import com.db4o.db4ounit.util.*;

import db4ounit.*;
import db4ounit.extensions.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class SSLSocketTestCase extends StandaloneCSTestCaseBase implements OptOutWorkspaceIssue {

	private static final String KEYSTORE_PATH = "keystore/test_keystore";
	private static final String KEYSTORE_PASSWORD = "keystore";

	public static class Item {
	}

	@Override
	protected void configure(Configuration config) {
		try {
			SSLContext context = createContext();
			config.add(new SSLSupport(context));
		} 
		catch (Exception exc) {
			exc.printStackTrace();
			throw new AssertionException(exc.toString());
		}
	}

	private SSLContext createContext() throws Exception {
		KeyStore ks = KeyStore.getInstance(KeyStore.getDefaultType());
		char[] password = KEYSTORE_PASSWORD.toCharArray();
		InputStream in  = new FileInputStream(new File(WorkspaceLocations.getTestFolder(), KEYSTORE_PATH).getAbsoluteFile());
		ks.load(in, password);
		in.close();
		KeyManagerFactory kmf = KeyManagerFactory.getInstance("SunX509", "SunJSSE");
		kmf.init(ks, password);
		TrustManagerFactory tmf = TrustManagerFactory.getInstance("PKIX", "SunJSSE");
		tmf.init(ks);
		SSLContext ctx = SSLContext.getInstance("TLS");
		ctx.init(kmf.getKeyManagers(), tmf.getTrustManagers(), null);
		return ctx;
	}
	
	@Override
	protected void runTest() throws Throwable {
		ClientObjectContainer client = openClient();
		client.store(new Item());
		client.close();
		client = openClient();
		Assert.areEqual(1, client.query(Item.class).size());
		client.close();
	}
}
