/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.presentation;

import org.eclipse.swt.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;

import com.db4o.foundation.*;
import com.db4o.omplus.ui.*;

public class LoginButtonsPane extends Composite {
	public static final String OK_BUTTON_ID = LoginButtonsPane.class.getName() + "$okButton";
	public static final String CANCEL_BUTTON_ID = LoginButtonsPane.class.getName() + "$cancelButton";
	public static final String CUSTOM_BUTTON_ID = LoginButtonsPane.class.getName() + "$customButton";
	public static final String CLEAR_BUTTON_ID = LoginButtonsPane.class.getName() + "$clearButton";

	private static final String CANCEL_TEXT = "Cancel";
	private Button cancelBtn;
	private Button openBtn;
	private Button customBtn;
	private Button clearBtn;
	
	public LoginButtonsPane(Composite parent, Composite dialog, String openText, Closure4<Boolean> openAction, Block4 customAction, Block4 clearAction) {
		super(parent, SWT.NONE);
		createContents(parent, dialog, openText, openAction, customAction, clearAction);
	}

	// TODO register model as action listener and invoke closure from model in response
	private void createContents(Composite parent, final Composite dialog, String openText, final Closure4<Boolean> openAction, final Block4 customAction, final Block4 clearAction) {	
		setLayout(new FormLayout());
		
		openBtn = new Button(this, SWT.PUSH);
		OMESWTUtil.assignWidgetId(openBtn, OK_BUTTON_ID);
		openBtn.setText(openText);
		openBtn.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				if(openAction.run()) {
					dialog.dispose();
				}
			}
		});
		
		cancelBtn = new Button(this, SWT.PUSH);
		OMESWTUtil.assignWidgetId(cancelBtn, CANCEL_BUTTON_ID);
		cancelBtn.setText(CANCEL_TEXT);
		cancelBtn.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				dialog.dispose();
			}
		});

		customBtn = new Button(this, SWT.PUSH);
		OMESWTUtil.assignWidgetId(customBtn, CUSTOM_BUTTON_ID);
		customBtn.setText("Custom config...");
		customBtn.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				customAction.run();
			}
		});

		clearBtn = new Button(this, SWT.PUSH);
		OMESWTUtil.assignWidgetId(clearBtn, CLEAR_BUTTON_ID);
		clearBtn.setText("Clear");
		clearBtn.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				clearAction.run();
			}
		});

		FormData data = new FormData();
		data.top = new FormAttachment(0);
		data.left = new FormAttachment(60);
		data.right = new FormAttachment(cancelBtn , -5);
		openBtn.setLayoutData(data);

		data = new FormData();
		data.top = new FormAttachment(0);
		data.left = new FormAttachment(81);
		data.right = new FormAttachment(100);
		cancelBtn.setLayoutData(data);	

		data = new FormData();
		data.top = new FormAttachment(0);
		data.left = new FormAttachment(0);
		data.right = new FormAttachment(19);
		customBtn.setLayoutData(data);	

		data = new FormData();
		data.top = new FormAttachment(0);
		data.left = new FormAttachment(customBtn, 5);
		clearBtn.setLayoutData(data);	
	}
	
	@Override
	public void dispose() {
		super.dispose();
	}
	
	public void customActionEnabled(boolean enabled) {
		customBtn.setEnabled(enabled);
	}
}
