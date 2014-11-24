/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.test;

import static com.db4o.omplus.test.util.Db4oTestUtil.*;
import static com.db4o.omplus.test.util.SWTTestUtil.*;
import static org.junit.Assert.*;

import java.io.*;

import org.eclipse.core.commands.*;
import org.eclipse.swt.widgets.*;
import org.eclipse.ui.*;
import org.eclipse.ui.commands.*;
import org.eclipse.ui.handlers.*;
import org.junit.*;

import com.db4o.internal.*;
import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.test.util.*;
import com.db4o.omplus.ui.*;
import com.db4o.omplus.ui.dialog.login.*;
import com.db4o.omplus.ui.dialog.login.presentation.*;

// TODO unit test for command/action layer with login dialog API mock
public class LoginDialogIntegrationTestCase {

	@Before
	public void setUp() throws Exception {
		PlatformUI.getWorkbench().showPerspective(OMPlusPerspective.ID, PlatformUI.getWorkbench().getActiveWorkbenchWindow());
	}

	@Test
	public void testCommandRegistration() {
		ICommandService cmdService = (ICommandService) PlatformUI.getWorkbench().getService(ICommandService.class);
		Category omeCategory = cmdService.getCategory(OMPlusConstants.OME_COMMAND_CATEGORY_ID);
		assertTrue(omeCategory.isDefined());
		Command connectCommand = cmdService.getCommand(OMPlusConstants.OME_CONNECT_COMMAND_ID);
		assertTrue(connectCommand.isDefined());
		assertTrue(connectCommand.isEnabled());
	}
	
	@Test
	public void testCancel() throws Exception {
		executeCommand();
		Shell dialogShell = findDialogShell();
		Button cancelButton = findChild(dialogShell, LoginButtonsPane.CANCEL_BUTTON_ID);
		pressButton(cancelButton);
		assertNull(findDialogShell());
		assertFalse(Activator.getDefault().dbModel().connected());
	}

	@Test
	public void testOpenLocal() throws Exception {
		File dbFile = createEmptyDatabase();
		try {
			executeCommand();
			Shell dialogShell = findDialogShell();
			TabItem localTab = findTabItem(dialogShell, LoginDialog.TAB_FOLDER_ID, LoginDialog.LOCAL_TAB_ID);
			assertSame(localTab, localTab.getParent().getItems()[localTab.getParent().getSelectionIndex()]);
			Text fileText = findChild(dialogShell, LocalLoginPaneSpec.NEW_CONNECTION_TEXT_ID);
			fileText.setText(dbFile.getAbsolutePath());
			Button okButton = findChild(dialogShell, LoginButtonsPane.OK_BUTTON_ID);
			pressButton(okButton);
			assertNull(findDialogShell());
			LocalObjectContainer db = (LocalObjectContainer) Activator.getDefault().dbModel().db().getDB();
			assertEquals(dbFile.getAbsolutePath(), db.fileName());
			Activator.getDefault().dbModel().disconnect();
		}
		finally {
			dbFile.delete();
		}
	}

	@Test
	public void testOpenRemote() throws Exception {
		executeCommand();
		Shell dialogShell = findDialogShell();
		TabItem remoteTab = findTabItem(dialogShell, LoginDialog.TAB_FOLDER_ID, LoginDialog.REMOTE_TAB_ID);
		remoteTab.getParent().setSelection(remoteTab);
		// TODO
	}

	private void executeCommand() throws Exception {
		IHandlerService handlerService = (IHandlerService) PlatformUI.getWorkbench().getService(IHandlerService.class);
		handlerService.executeCommand(OMPlusConstants.OME_CONNECT_COMMAND_ID, null);
	}

	private Shell findDialogShell() throws Exception {
		return SWTTestUtil.findShell(LoginDialog.SHELL_ID);
	}

}
