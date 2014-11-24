/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.model.test;

import static org.easymock.EasyMock.*;

import java.util.*;

import org.easymock.*;
import org.junit.*;
import static org.junit.Assert.*;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.omplus.*;
import com.db4o.omplus.connection.*;
import com.db4o.omplus.ui.dialog.login.model.*;
import com.db4o.omplus.ui.dialog.login.model.ConnectionPresentationModel.ConnectionPresentationListener;

public class ConnectionPresentationModelTestCase {
	
	private Connector connector;
	private ErrorMessageSink errSink;
	private RecentConnectionList recentConnections;
	private CustomConfigSource configSource;
	private ConnectionPresentationListener listener;
	private MockConnectionPresentationModel model;
	
	@Before
	public void setUp() {
		recentConnections = createMock(RecentConnectionList.class);
		errSink = createMock(ErrorMessageSink.class);
		connector = createMock(Connector.class);
		configSource = createMock(CustomConfigSource.class);
		listener = createMock(ConnectionPresentationListener.class);
		LoginPresentationModel loginModel = new LoginPresentationModel(recentConnections, new ErrorMessageHandler(errSink), connector);
		model = new MockConnectionPresentationModel(loginModel, configSource);
	}
	
	@Test
	public void testRecentConnections() {
		final String id = "foo";
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(id)));
		replayMocks();
		assertArrayEquals(new String[] { id }, model.recentConnections());
		verifyMocks();
	}
	
	@Test
	public void testManualEntry() throws Exception {
		final String id = "foo";
		expect(connector.connect(eqParams(id))).andReturn(true);
		recentConnections.addNewConnection(eqParams(id));
		replayMocks();
		model.id(id);
		model.connect();
		verifyMocks();
	}

	@Test
	public void testSelectionEntry() throws Exception {
		final String id = "foo";
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(id)));
		expect(connector.connect(eqParams(id))).andReturn(true);
		recentConnections.addNewConnection(eqParams(id));
		replayMocks();
		model.select(0);
		model.connect();
		verifyMocks();
	}

	@Test
	public void testSelectionThenManualEntry() throws Exception {
		final String selectedId = "foo";
		final String manualId = "bar";
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(selectedId)));
		expect(connector.connect(eqParams(manualId))).andReturn(true);
		recentConnections.addNewConnection(eqParams(manualId));
		replayMocks();
		model.select(0);
		model.id(manualId);
		model.connect();
		verifyMocks();
	}

	@Test
	public void testManualThenSelectionEntry() throws Exception {
		final String selectedId = "foo";
		final String manualId = "bar";
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(selectedId)));
		expect(connector.connect(eqParams(selectedId))).andReturn(true);
		recentConnections.addNewConnection(eqParams(selectedId));
		replayMocks();
		model.id(manualId);
		model.select(0);
		model.connect();
		verifyMocks();
	}

	@Test
	public void testManualEntryWithConfig() throws Exception {
		final String id = "foo";
		final String[] jarFiles = { "foo.jar" };
		final String[] configNames = { "FooConfig" };
		expect(connector.connect(eqParams(id, jarFiles, configNames))).andReturn(true);
		recentConnections.addNewConnection(eqParams(id, jarFiles, configNames));
		replayMocks();
		model.id(id);
		model.customConfig(jarFiles, configNames);
		model.connect();
		verifyMocks();
	}

	@Test
	public void testSelectionEntryWithConfig() throws Exception {
		final String id = "foo";
		final String[] jarFiles = { "foo.jar" };
		final String[] configNames = { "FooConfig" };
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(id, "", jarFiles, configNames)));
		expect(connector.connect(eqParams(id, jarFiles, configNames))).andReturn(true);
		recentConnections.addNewConnection(eqParams(id, jarFiles, configNames));
		replayMocks();
		model.select(0);
		model.connect();
		verifyMocks();
	}

	@Test
	public void testSelectionWithConfigThenManualEntry() throws Exception {
		final String selectedId = "foo";
		final String manualId = "bar";
		final String[] jarFiles = { "foo.jar" };
		final String[] configNames = { "FooConfig" };
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(selectedId, "", jarFiles, configNames)));
		expect(connector.connect(eqParams(manualId, jarFiles, configNames))).andReturn(true);
		recentConnections.addNewConnection(eqParams(manualId, jarFiles, configNames));
		replayMocks();
		model.select(0);
		model.id(manualId);
		model.connect();
		verifyMocks();
	}

	@Test
	public void testManualWithConfigThenSelectionEntry() throws Exception {
		final String selectedId = "foo";
		final String manualId = "bar";
		final String[] jarFiles = { "foo.jar" };
		final String[] configNames = { "FooConfig" };
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(selectedId)));
		expect(connector.connect(eqParams(selectedId))).andReturn(true);
		recentConnections.addNewConnection(eqParams(selectedId));
		replayMocks();
		model.id(manualId);
		model.customConfig(jarFiles, configNames);
		model.select(0);
		model.connect();
		verifyMocks();
	}

	@Test
	public void testSelectionWithConfigThenManualEntryWithConfig() throws Exception {
		final String selectedId = "foo";
		final String manualId = "bar";
		final String[] selectedJarFiles = { "foo.jar" };
		final String[] selectedConfigNames = { "FooConfig" };
		final String[] manualJarFiles = { "bar.jar" };
		final String[] manualConfigNames = { "BarConfig" };
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(selectedId, "", selectedJarFiles, selectedConfigNames)));
		expect(connector.connect(eqParams(manualId, manualJarFiles, manualConfigNames))).andReturn(true);
		recentConnections.addNewConnection(eqParams(manualId, manualJarFiles, manualConfigNames));
		replayMocks();
		model.select(0);
		model.id(manualId);
		model.customConfig(manualJarFiles, manualConfigNames);
		model.connect();
		verifyMocks();
	}

	@Test
	public void testSelectionEdit() throws Exception {
		final String id = "foo";
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(id, "a", new String[0], new String[0])));
		expect(connector.connect(eqParams(id, "b", new String[0], new String[0]))).andReturn(true);
		recentConnections.addNewConnection(eqParams(id, "b", new String[0], new String[0]));
		replayMocks();
		model.select(0);
		model.other("b");
		model.connect();
		verifyMocks();
	}
	
	@Test
	public void testListenerStateChange() {
		listener.connectionPresentationState(true, 0, 0);
		replayMocks();
		model.addConnectionPresentationListener(listener);
		verifyMocks();
		resetMocks();

		final String selectedId = "foo";
		final String[] selectedJarFiles = { "foo.jar" };
		final String[] selectedConfigNames = { "FooConfig" };
		expect(recentConnections.getRecentConnections(MockConnectionParams.class)).andReturn(Arrays.asList(new MockConnectionParams(selectedId, "", selectedJarFiles, selectedConfigNames)));
		listener.connectionPresentationState(false, 1, 1);
		replayMocks();
		model.select(0);
		verifyMocks();
		resetMocks();
		
		listener.connectionPresentationState(true, 1, 1);
		replayMocks();
		model.id("bar");
		verifyMocks();
	}
	
	@Test
	public void testListenerConfigChange() {
		listener.connectionPresentationState(true, 0, 0);
		replayMocks();
		model.addConnectionPresentationListener(listener);
		verifyMocks();
		resetMocks();
		
		final String[] jarFiles = { "bar.jar" };
		final String[] configNames = { "BarConfig" };
		listener.connectionPresentationState(true, 1, 1);
		replayMocks();
		model.customConfig(jarFiles, configNames);
		verifyMocks();
	}
	
	@Test
	public void testClear() throws Exception {
		replayMocks();
		model.id("foo");
		final String[] jarFiles = { "bar.jar" };
		final String[] configNames = { "BarConfig" };
		model.customConfig(jarFiles, configNames);
		verifyMocks();
		resetMocks();
		expect(connector.connect(eqParams(""))).andReturn(true);
		recentConnections.addNewConnection(eqParams(""));
		replayMocks();
		model.clear();
		model.connect();
		verifyMocks();
	}
	
	private void replayMocks() {
		replay(recentConnections);
		replay(errSink);
		replay(connector);
		replay(configSource);
		replay(listener);
	}

	private void verifyMocks() {
		verify(recentConnections);
		verify(errSink);
		verify(connector);
		verify(configSource);
		verify(listener);
	}	

	private void resetMocks() {
		reset(recentConnections);
		reset(errSink);
		reset(connector);
		reset(configSource);
		reset(listener);
	}	

	private static class ConnectionParamMatcher implements IArgumentMatcher {
		
		private String expectedId;
		private String expectedOther;
		private String[] jarPaths;
		private String[] configNames;
		
		public ConnectionParamMatcher(String id, String other, String[] jarPaths, String[] configNames) {
			this.expectedId = id;
			this.expectedOther = other;
			this.jarPaths = jarPaths;
			this.configNames = configNames;
		}
		
		@Override
		public void appendTo(StringBuffer str) {
			str.append("eqParams(" + expectedId + "," + expectedOther + "," + Arrays.toString(jarPaths) + "," + Arrays.toString(configNames) + ")");
		}

		@Override
		public boolean matches(Object other) {
			if(other.getClass() != MockConnectionParams.class) {
				return false;
			}
			final MockConnectionParams params = (MockConnectionParams)other;
			return expectedId.equals(params.id) && expectedOther.equals(params.other) && Arrays.equals(jarPaths, params.jarPaths()) && Arrays.equals(configNames, params.configNames());
		}
	}
	
	public static ConnectionParams eqParams(String id) {
		return eqParams(id, new String[0], new String[0]);
	}

	public static ConnectionParams eqParams(String id, String[] jarPaths, String[] configNames) {
		return eqParams(id, "", jarPaths, configNames);
	}

	public static ConnectionParams eqParams(String id, String other, String[] jarPaths, String[] configNames) {
	    EasyMock.reportMatcher(new ConnectionParamMatcher(id, other, jarPaths, configNames));
	    return null;
	}

	
	private static class MockConnectionParams extends ConnectionParams {
		
		private String id;
		private String other;

		public MockConnectionParams(String id) {
			this(id, "");
		}

		public MockConnectionParams(String id, String other) {
			this(id, other, new String[0], new String[0]);
		}

		public MockConnectionParams(String id, String other, String[] jarPaths, String[] configNames) {
			super(jarPaths, configNames);
			this.id = id;
			this.other = other;
		}

		@Override
		public String getPath() {
			return id;
		}

		@Override
		public ObjectContainer connect(Function4<String, Boolean> userCallback) throws DBConnectException {
			return null;
		}
		
		public String[] jarPaths() {
			return jarPaths;
		}
		
		public String[] configNames() {
			return configNames;
		}
	}
	
	private static class MockConnectionPresentationModel extends ConnectionPresentationModel<MockConnectionParams> {
		
		private String id = "";
		private String other = "";
		
		public MockConnectionPresentationModel(LoginPresentationModel model, CustomConfigSource configSource) {
			super(model, configSource);
		}

		@Override
		protected MockConnectionParams fromState(String[] jarPaths, String[] configNames) throws DBConnectException {
			return new MockConnectionParams(id, other, jarPaths, configNames);
		}

		@Override
		protected void fromState(MockConnectionParams params) {
			params.id = id;
			params.other = other;
		}

		@Override
		protected void selected(MockConnectionParams selected) {
			id = selected.id;
			other = selected.other;
		}

		@Override
		protected List<MockConnectionParams> connections(RecentConnectionList recentConnections) {
			return recentConnections.getRecentConnections(MockConnectionParams.class);
		}	

		public void id(String id) {
			if(this.id != null && this.id.equals(id)) {
				return;
			}
			this.id = id;
			newState();
		}
		
		public void other(String other) {
			this.other = other;
		}

		@Override
		protected void clearSpecificState() {
			id = "";
			other = "";
		}
	}
	
}
