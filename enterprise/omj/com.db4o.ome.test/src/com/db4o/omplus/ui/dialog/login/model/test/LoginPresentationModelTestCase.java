/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.model.test;

import static org.junit.Assert.*;

import org.junit.*;

import com.db4o.foundation.*;
import com.db4o.omplus.connection.*;
import com.db4o.omplus.ui.dialog.login.model.LocalPresentationModel.LocalSelectionListener;
import com.db4o.omplus.ui.dialog.login.test.*;
import com.db4o.omplus.ui.dialog.login.test.LoginPresentationModelFixture.ConnectInterceptor;

public class LoginPresentationModelTestCase {
	
	private LoginPresentationModelFixture fixture;

	@Before
	public void setUp() {
		fixture = new LoginPresentationModelFixture();
	}

	@Test
	public void testLocalSelectionListener() {
		final String[] received = new String[1];
		fixture.localModel().addLocalSelectionListener(new LocalSelectionListener() {
			@Override
			public void localSelection(String path, boolean readOnly) {
				received[0] = path;
			}
		});
		fixture.localModel().select(1);
		assertEquals(fixture.presetFileParams().get(1).getPath(), received[0]);
	}

	@Test
	public void testLocalOpenException() {
		fixture.interceptor(new ConnectInterceptor() {
			@Override
			public void connect(ConnectionParams params) throws DBConnectException {
				throw new DBConnectException(params, "");
			}
		});
		fixture.localModel().path("foo");
		fixture.localModel().readOnly(false);
		fixture.localModel().connect();
		fixture.assertNotConnected(DBConnectException.class);
	}

	@Test
	public void testPlainOpen() {
		assertOpen("foo", false);
	}

	@Test
	public void testReadOnlyOpen() {
		assertOpen("bar", true);
	}
	
	private void assertOpen(final String path, final boolean readOnly) {
		final ByRef<ConnectionParams> received = new ByRef<ConnectionParams>();
		fixture.interceptor(new ConnectInterceptor() {
			@Override
			public void connect(ConnectionParams params) throws DBConnectException {
				received.value = params;
			}
		});
		fixture.localModel().path(path);
		fixture.localModel().readOnly(readOnly);
		fixture.localModel().connect();
		assertEquals(path, received.value.getPath());
		assertEquals(readOnly, ((FileConnectionParams)received.value).readOnly());
	}
}
