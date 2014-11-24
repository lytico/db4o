/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.presentation.test;

import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;
import org.junit.*;

import org.easymock.*;

import static org.easymock.EasyMock.*;
import static org.junit.Assert.*;

import com.db4o.omplus.ui.dialog.login.model.*;
import com.db4o.omplus.ui.dialog.login.model.CustomConfigModel.CustomConfigListener;
import com.db4o.omplus.ui.dialog.login.presentation.*;
import com.db4o.omplus.ui.dialog.login.presentation.CustomConfigPane.JarPathSource;

import static com.db4o.omplus.test.util.SWTTestUtil.*;

public class CustomConfigPaneTestCase {

	private Shell shell;
	private CustomConfigModel model;
	private CustomConfigPane configPane;
	private CustomConfigListener listener;
	private JarPathSource jarSource;
	
	@Before
	public void setUp() {
		shell = new Shell(PlatformUI.getWorkbench().getDisplay());
		model = createMock(CustomConfigModel.class);
		jarSource = createMock(JarPathSource.class);
		Capture<CustomConfigListener> listener = new Capture<CustomConfigListener>();
		model.addListener(capture(listener));
		replayMocks();
		configPane = new CustomConfigPane(shell, shell, model, jarSource);
		this.listener = listener.getValue();
		verifyMocks();
		resetMocks();
	}

	@After
	public void tearDown() {
		verifyMocks();
		shell.dispose();
	}

	@Test
	public void testRawInitialState() {
		replayMocks();
		assertEquals(0, jarList().getItemCount());
		assertEquals(0, configList().getItemCount());
		assertFalse(removeButton().isEnabled());
		assertFalse(shell.isDisposed());
		shell.dispose();
	}

	@Test
	public void testCancelInitialState() {
		replayMocks();
		assertEquals(0, jarList().getItemCount());
		assertEquals(0, configList().getItemCount());
		pressButton(cancelButton());
		assertTrue(shell.isDisposed());
	}

	@Test
	public void testCommitInitialState() {
		model.commit();
		replayMocks();
		pressButton(okButton());
		assertTrue(shell.isDisposed());
	}

	@Test
	public void testAddJar() {
		String jarPath = "foo.jar";
		model.addJarPaths(new String[] { jarPath });
		expect(jarSource.jarPath()).andReturn(jarPath);
		replayMocks();
		pressButton(addButton());		
	}

	@Test
	public void testJarAdded() {
		replayMocks();
		final String[] jarPaths = { "foo.jar" };
		final String[] configNames = { "FooConfig" };
		listener.customConfig(jarPaths, configNames, new String[0]);
		assertArrayEquals(jarPaths, jarList().getItems());
		assertArrayEquals(configNames, configList().getItems());
		assertEquals(0, configList().getSelectionCount());
		assertFalse(removeButton().isEnabled());
	}

	@Test
	public void testSelectConfig() {
		final String[] configNames = { "FooConfig" };
		model.selectConfigClassNames(configNames);
		replayMocks();
		listener.customConfig(new String[] { "foo.jar" }, configNames, new String[0]);
		selectList(configList(), configNames);
	}

	@Test
	public void testConfigSelected() {
		replayMocks();
		final String[] configNames = { "FooConfig" };
		listener.customConfig(new String[0], configNames, configNames);
		assertArrayEquals(configNames, configList().getItems());
		assertArrayEquals(configNames, configList().getSelection());
	}

	@Test
	public void testRemoveJars() {
		final String[] jarPaths = { "foo.jar" };
		final String[] configNames = { "FooConfig" };
		listener.customConfig(jarPaths, configNames, new String[0]);
		
		model.removeJarPaths(jarPaths);
		replayMocks();
		assertFalse(removeButton().isEnabled());
		selectList(jarList(), jarPaths);
		assertTrue(removeButton().isEnabled());
		pressButton(removeButton());
	}

	@Test
	public void testJarsRemoved() {
		replayMocks();
		final String[] jarPaths = { "foo.jar" };
		final String[] configNames = { "FooConfig" };
		listener.customConfig(jarPaths, configNames, new String[0]);
		selectList(jarList(), jarPaths);
		listener.customConfig(new String[0], new String[0], new String[0]);
		assertEquals(0, jarList().getItems().length);
		assertEquals(0, configList().getItems().length);
		assertFalse(removeButton().isEnabled());
	}

	private Button okButton() {
		return findChild(configPane, CustomConfigPane.OK_BUTTON_ID);
	}

	private Button cancelButton() {
		return findChild(configPane, CustomConfigPane.CANCEL_BUTTON_ID);
	}

	private List configList() {
		return findChild(configPane, CustomConfigPane.CONFIGURATOR_LIST_ID);
	}

	private List jarList() {
		return findChild(configPane, CustomConfigPane.JAR_LIST_ID);
	}

	private Button addButton() {
		return findChild(configPane, CustomConfigPane.ADD_JAR_BUTTON_ID);
	}

	private Button removeButton() {
		return findChild(configPane, CustomConfigPane.REMOVE_JAR_BUTTON_ID);
	}

	private void resetMocks() {
		reset(model);
		reset(jarSource);
	}

	private void verifyMocks() {
		verify(model);
		verify(jarSource);
	}

	private void replayMocks() {
		replay(model);
		replay(jarSource);
	}
	
}
