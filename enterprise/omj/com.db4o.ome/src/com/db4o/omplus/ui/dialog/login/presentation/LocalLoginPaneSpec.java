/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.presentation;

import org.eclipse.jface.layout.*;
import org.eclipse.swt.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.widgets.*;

import com.db4o.omplus.connection.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.ui.*;
import com.db4o.omplus.ui.dialog.login.model.*;
import com.db4o.omplus.ui.dialog.login.model.LocalPresentationModel.LocalSelectionListener;

public class LocalLoginPaneSpec implements LoginPaneSpec<FileConnectionParams> {

	public static final String NEW_CONNECTION_TEXT_ID = LocalLoginPaneSpec.class.getName() + "$newConnectionText";
	public static final String READ_ONLY_BUTTON_ID = LocalLoginPaneSpec.class.getName() + "$readOnlyButton";

	private final LocalPresentationModel model;
	
	public LocalLoginPaneSpec(LocalPresentationModel model) {
		this.model = model;
	}

	public ConnectionPresentationModel<FileConnectionParams> model() {
		return model;
	}
	
	public void create(final Composite parent, Composite innerComposite) {
		Label recentConnectionLabel = new Label(innerComposite, SWT.NONE);
		recentConnectionLabel.setText("Recent connections: ");

		Combo recentConnectionCombo = LoginDialogUtil.recentConnectionCombo(innerComposite, model);

		Label newConnectionLabel = new Label(innerComposite, SWT.NONE);
		newConnectionLabel.setText("New Connections:    ");

		final Text newConnectionText = new Text(innerComposite, SWT.BORDER);
		newConnectionText.setTextLimit(255);
		OMESWTUtil.assignWidgetId(newConnectionText, NEW_CONNECTION_TEXT_ID);

		Button browseBtn = new Button(innerComposite, SWT.PUSH);
		try {
			browseBtn.setImage(ImageUtility.getImage(OMPlusConstants.BROWSE_ICON));
		}
		catch(Exception exc) {
			browseBtn.setText("..");
		}
		browseBtn.setToolTipText("Browse");

		final Button readOnlyButton = new Button(innerComposite, SWT.CHECK);
		OMESWTUtil.assignWidgetId(readOnlyButton, READ_ONLY_BUTTON_ID);
		readOnlyButton.setText("read-only");

		newConnectionText.addModifyListener(new ModifyListener() {
			public void modifyText(ModifyEvent e) {
				model.path(newConnectionText.getText());
			}
		});
		readOnlyButton.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				model.readOnly(readOnlyButton.getSelection());
			}
		});

		model.addLocalSelectionListener(new LocalSelectionListener() {
			public void localSelection(String path, boolean readOnly) {
				newConnectionText(newConnectionText, path);
				if(readOnlyButton.getSelection() != readOnly) {
					readOnlyButton.setSelection(readOnly);
				}
			}
		});

		browseBtn.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				FileDialog fileChooser = new FileDialog(parent.getShell(), SWT.OPEN);
				String dbfile = fileChooser.open();
				if(dbfile != null){
					newConnectionText(newConnectionText, dbfile);
				}
			}
		});		
		
		GridLayoutFactory.swtDefaults().numColumns(3).equalWidth(false).applyTo(innerComposite);
		GridDataFactory.swtDefaults().align(SWT.LEFT, SWT.CENTER).applyTo(recentConnectionLabel);
		GridDataFactory.swtDefaults().span(2, 1).grab(true, false).align(SWT.FILL, SWT.CENTER).applyTo(recentConnectionCombo);
		GridDataFactory.swtDefaults().align(SWT.LEFT, SWT.CENTER).applyTo(newConnectionLabel);
		GridDataFactory.swtDefaults().grab(true, false).align(SWT.FILL, SWT.CENTER).applyTo(newConnectionText);
		GridDataFactory.swtDefaults().align(SWT.RIGHT, SWT.CENTER).applyTo(browseBtn);
		GridDataFactory.swtDefaults().align(SWT.LEFT, SWT.CENTER).applyTo(readOnlyButton);
		
		innerComposite.pack();
	}

	private void newConnectionText(Text newConnectionText, String path) {
		if(!newConnectionText.getText().equals(path)) {
			newConnectionText.setText(path);
			newConnectionText.setToolTipText(path);
		}
	}

}
