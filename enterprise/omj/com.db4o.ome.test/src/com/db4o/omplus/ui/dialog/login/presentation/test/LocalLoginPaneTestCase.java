/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.presentation.test;

import static com.db4o.omplus.test.util.SWTTestUtil.*;
import static junit.framework.Assert.*;

import java.util.List;

import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;
import org.junit.*;

import com.db4o.omplus.connection.*;
import com.db4o.omplus.ui.dialog.login.presentation.*;
import com.db4o.omplus.ui.dialog.login.test.*;
import com.db4o.omplus.ui.dialog.login.test.LoginPresentationModelFixture.ConnectInterceptor;

public class LocalLoginPaneTestCase {

	private Shell shell;
	private LoginPane<FileConnectionParams> loginPane;
	private LoginPresentationModelFixture fixture;
	
	@Before
	public void setUp() {
		fixture = new LoginPresentationModelFixture();
		shell = new Shell(PlatformUI.getWorkbench().getDisplay());
		loginPane = new LoginPane<FileConnectionParams>(shell, shell, "", new LocalLoginPaneSpec(fixture.localModel()));
	}
	
	@After
	public void tearDown() {
		shell.dispose();
	}
	
	@Test
	public void testInitialStateCancel() {
		Combo recentConnectionCombo = recentConnectionCombo();
		assertEquals(presetParams().size(), recentConnectionCombo.getItemCount());
		String[] items = recentConnectionCombo.getItems();
		for (int idx = 0; idx < items.length; idx++) {
			assertEquals(presetParams().get(idx).getPath(), items[idx]);
		}
		assertEquals(-1, recentConnectionCombo.getSelectionIndex());
		assertEquals("", newConnectionText().getText());
		assertFalse(readOnlyButton().getSelection());
		pressButton(cancelButton());
		fixture.assertNotConnected();
	}
	
	@Test
	public void testSetPathOk() {
		final String path = "baz";
		newConnectionText().setText(path);
		pressButton(okButton());
		fixture.assertConnected(new FileConnectionParams(path));
	}

	@Test
	public void testSetPathReadOnlyOk() {
		final String path = "baz";
		newConnectionText().setText(path);
		selectButton(readOnlyButton(), true);
		pressButton(okButton());
		fixture.assertConnected(new FileConnectionParams(path, true));
	}

	@Test
	public void testSetPathCancel() {
		newConnectionText().setText("baz");
		pressButton(cancelButton());
		fixture.assertNotConnected();
	}

	@Test
	public void testSetPathException() {
		fixture.interceptor(new ConnectInterceptor() {
			@Override
			public void connect(ConnectionParams params) throws DBConnectException {
				throw new IllegalStateException();
			}
		});
		newConnectionText().setText("baz");
		pressButton(okButton());
		fixture.assertNotConnected(DBConnectException.class);
	}

	@Test
	public void testSelectPathOk() {
		Combo recentConnectionCombo = recentConnectionCombo();
		selectCombo(recentConnectionCombo, 1);
		assertEquals(presetParams().get(1).getPath(), newConnectionText().getText());
		pressButton(okButton());
		fixture.assertConnected(presetParams().get(1));
	}

	private Button readOnlyButton() {
		return findChild(loginPane, LocalLoginPaneSpec.READ_ONLY_BUTTON_ID);
	}

	private Button okButton() {
		return findChild(loginPane, LoginButtonsPane.OK_BUTTON_ID);
	}

	private Button cancelButton() {
		return findChild(loginPane, LoginButtonsPane.CANCEL_BUTTON_ID);
	}

	private Text newConnectionText() {
		return findChild(loginPane, LocalLoginPaneSpec.NEW_CONNECTION_TEXT_ID);
	}

	private Combo recentConnectionCombo() {
		return findChild(loginPane, LoginDialogUtil.RECENT_CONNECTION_COMBO_ID);
	}
	
	private List<FileConnectionParams> presetParams() {
		return fixture.presetFileParams();
	}
}
