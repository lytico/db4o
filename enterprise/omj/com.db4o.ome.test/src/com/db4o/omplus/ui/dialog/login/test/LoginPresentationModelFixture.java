/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.test;

import static junit.framework.Assert.*;

import java.util.*;

import com.db4o.omplus.*;
import com.db4o.omplus.connection.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.debug.*;
import com.db4o.omplus.ui.dialog.login.model.*;

public class LoginPresentationModelFixture {

	private final RecentConnectionList recentConnections;
	private final List<FileConnectionParams> presetFileParams;
	private final LoginPresentationModel model;
	private final LocalPresentationModel localModel;
	private final RemotePresentationModel remoteModel;
	private FileConnectionParams paramsReceived;
	private Throwable exceptionReceived;
	private String errorMsgReceived;
	private ConnectInterceptor interceptor;

	public LoginPresentationModelFixture() {
		presetFileParams = Collections.unmodifiableList(Arrays.asList(new FileConnectionParams("foo", false), new FileConnectionParams("bar", true)));
		OMEDataStore dataStore = new InMemoryOMEDataStore();
		recentConnections = new DataStoreRecentConnectionList(dataStore);
		for(int idx = presetFileParams.size() - 1; idx >= 0; idx--) {
			recentConnections.addNewConnection(presetFileParams.get(idx));
		}
		ErrorMessageSink errSink = new ErrorMessageSink() {
			public void showError(String msg) {
				errorMsgReceived = msg;
			}

			public void showExc(String msg, Throwable exc) {
				exceptionReceived = exc;
			}

			public void logWarning(String msg, Throwable exc) {
			}
		};
		Connector connector = new Connector() {
			@Override
			public boolean connect(ConnectionParams params) throws DBConnectException {
				interceptor.connect(params);
				paramsReceived = (FileConnectionParams) params;
				return true;
			}
		};
		model = new LoginPresentationModel(recentConnections, new ErrorMessageHandler(errSink),  connector);
		localModel = new LocalPresentationModel(model, null); // FIXME
		remoteModel = new RemotePresentationModel(model, null); // FIXME
		interceptor = new NullConnectInterceptor();
		paramsReceived = null;
		exceptionReceived = null;
		errorMsgReceived = null;
	}
	
	public LoginPresentationModel model() {
		return model;
	}

	public LocalPresentationModel localModel() {
		return localModel;
	}

	public RemotePresentationModel remoteModel() {
		return remoteModel;
	}

	public List<FileConnectionParams> presetFileParams() {
		return presetFileParams;
	}
	
	public void interceptor(ConnectInterceptor interceptor) {
		this.interceptor = interceptor;
	}
	
	public void assertConnected(FileConnectionParams expected) {
		assertParamsEquals(expected, paramsReceived);
		List<FileConnectionParams> recentFileConnections = recentConnections.getRecentConnections(FileConnectionParams.class);
		assertParamsEquals(expected, recentFileConnections.get(0));
		int cmpIdx = 1;
		for (FileConnectionParams curPreset : presetFileParams) {
			if(equals(curPreset, expected)) {
				continue;
			}
			assertParamsEquals(curPreset, recentFileConnections.get(cmpIdx));
			cmpIdx++;
		}
	}

	public void assertNotConnected() {
		assertNull(paramsReceived);
	}

	public void assertNotConnected(Class<? extends Throwable> excType) {
		assertNotConnected();
		assertExceptionReceived(excType);
	}

	public void assertExceptionReceived(Class<? extends Throwable> excType) {
		assertNotNull(errorMsgReceived);
		if(excType != null) {
			assertTrue(excType.isAssignableFrom(exceptionReceived.getClass()));
		}
	}

	public void assertExceptionReceived() {
		assertNotNull(exceptionReceived);
	}


	public void assertNoError() {
		assertNull(errorMsgReceived);
		assertNull(exceptionReceived);
	}
	
	public static interface ConnectInterceptor {
		void connect(ConnectionParams params) throws DBConnectException;
	}
	
	private void assertParamsEquals(FileConnectionParams a, FileConnectionParams b) {
		assertTrue(equals(a, b));
	}

	private boolean equals(FileConnectionParams a, FileConnectionParams b) {
		return a.equals(b) && a.readOnly() == b.readOnly() && Arrays.equals(a.jarPaths(), b.jarPaths()) && Arrays.equals(a.configuratorClassNames(), b.configuratorClassNames());
	}
	
	private static class NullConnectInterceptor implements ConnectInterceptor {
		@Override
		public void connect(ConnectionParams params) throws DBConnectException {
		}
	}
}
