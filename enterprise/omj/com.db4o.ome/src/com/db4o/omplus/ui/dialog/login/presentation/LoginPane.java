/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.presentation;

import org.eclipse.jface.layout.*;
import org.eclipse.swt.*;
import org.eclipse.swt.widgets.*;

import com.db4o.foundation.*;
import com.db4o.omplus.connection.*;
import com.db4o.omplus.ui.dialog.login.model.*;
import com.db4o.omplus.ui.dialog.login.model.ConnectionPresentationModel.ConnectionPresentationListener;

public class LoginPane<P extends ConnectionParams> extends Composite {

	private ConnectionPresentationModel<P> model;
	
	public LoginPane(Shell dialog, Composite parent, String openText, LoginPaneSpec<P> spec) {
		super(parent, SWT.NONE);
		model = spec.model();
		createContents(dialog, parent, openText, spec);
	}

	private void createContents(Shell dialog, Composite parent, String openText, LoginPaneSpec<P> spec) {
		Composite innerComposite = new Composite(this, SWT.BORDER);
		spec.create(this, innerComposite);
		
		final Label statusLabel = new Label(this, SWT.BORDER);
		statusLabel.setText("no custom config");
		model.addConnectionPresentationListener(new ConnectionPresentationListener() {
			public void connectionPresentationState(boolean editEnabled, int jarPathCount, int configuratorCount) {
				String msg = jarPathCount == 0 ? "no custom config" : jarPathCount + " jars, " + configuratorCount + " configurators";
				statusLabel.setText(msg);
			}
		});
		
		Closure4<Boolean> openAction = new Closure4<Boolean>() {
			public Boolean run() {
				return model.connect();
			}
		};
		Block4 customAction = new Block4() {
			public void run() {
				model.requestCustomConfig();
			}
		};
		Block4 clearAction = new Block4() {
			public void run() {
				model.clear();
			}
		};
		final LoginButtonsPane buttonComposite = new LoginButtonsPane(this, dialog, openText, openAction, customAction, clearAction);

		GridLayoutFactory.swtDefaults().numColumns(1).equalWidth(false).applyTo(this);
		GridDataFactory.swtDefaults().grab(true, false).align(SWT.FILL, SWT.CENTER).applyTo(innerComposite);
		GridDataFactory.swtDefaults().grab(true, false).align(SWT.FILL, SWT.CENTER).applyTo(statusLabel);
		GridDataFactory.swtDefaults().grab(true, false).align(SWT.FILL, SWT.CENTER).applyTo(buttonComposite);

		pack(true);
	}
}
