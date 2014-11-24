/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.presentation;

import org.eclipse.jface.layout.*;
import org.eclipse.swt.*;
import org.eclipse.swt.widgets.*;

import com.db4o.omplus.ui.*;
import com.db4o.omplus.ui.dialog.login.model.*;
import com.db4o.omplus.ui.dialog.login.model.CustomConfigModel.CustomConfigListener;

public class CustomConfigPane extends Composite {

	public static interface JarPathSource {
		String jarPath();
	}
	
	public static final String JAR_LIST_ID = CustomConfigPane.class.getName() + "$jarList";
	public static final String CONFIGURATOR_LIST_ID = CustomConfigPane.class.getName() + "$configuratorList";
	public static final String ADD_JAR_BUTTON_ID = CustomConfigPane.class.getName() + "$addJarButton";
	public static final String REMOVE_JAR_BUTTON_ID = CustomConfigPane.class.getName() + "$removeJarButton";
	public static final String CANCEL_BUTTON_ID = CustomConfigPane.class.getName() + "$cancelButton";
	public static final String OK_BUTTON_ID = CustomConfigPane.class.getName() + "$okButton";

	private final CustomConfigModel model;
	private final JarPathSource jarPathSource;
	
	private Button removeButton;
	private List jarList;
	
	public CustomConfigPane(Shell dialog, Composite parent, CustomConfigModel model, JarPathSource jarPathSource) {
		super(parent, SWT.NONE);
		this.model = model;
		this.jarPathSource = jarPathSource;
		createContents(dialog, parent);
	}

	private void createContents(final Shell dialog, final Composite parent) {
		Label jarLabel = label("Jars:");
		Label confLabel = label("Configurators:");
		jarList = new List (this, SWT.BORDER | SWT.MULTI | SWT.V_SCROLL | SWT.H_SCROLL);
		final List confList = new List (this, SWT.BORDER | SWT.MULTI | SWT.V_SCROLL | SWT.H_SCROLL);
		Button addButton = button("Add");
		removeButton = button("Remove");
		Button okButton = button("OK");
		Button cancelButton = button("Cancel");
		
		OMESWTUtil.assignWidgetId(jarList, JAR_LIST_ID);
		OMESWTUtil.assignWidgetId(confList, CONFIGURATOR_LIST_ID);
		OMESWTUtil.assignWidgetId(addButton, ADD_JAR_BUTTON_ID);
		OMESWTUtil.assignWidgetId(removeButton, REMOVE_JAR_BUTTON_ID);
		OMESWTUtil.assignWidgetId(cancelButton, CANCEL_BUTTON_ID);
		OMESWTUtil.assignWidgetId(okButton, OK_BUTTON_ID);

		confList.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				model.selectConfigClassNames(confList.getSelection());
			}
		});		
		jarList.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				updateJarRemovalState();
			}
		});
		addButton.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				String jarPath = jarPathSource.jarPath();
				if(jarPath != null){
					model.addJarPaths(jarPath);
				}
			}
		});		
		removeButton.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				String[] selectedItems = jarList.getSelection();
				if(selectedItems.length > 0) {
					model.removeJarPaths(selectedItems);
				}
			}
		});		
		removeButton.setEnabled(false);
		okButton.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				model.commit();
				dialog.close();
				dialog.dispose();
			}
		});
		cancelButton.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				dialog.close();
				dialog.dispose();
			}
		});
		
		model.addListener(new CustomConfigListener() {
			public void customConfig(String[] jarPaths, String[] configClassNames, String[] selectedConfigNames) {
				jarList.setItems(jarPaths);
				confList.setItems(configClassNames);
				confList.setSelection(selectedConfigNames);
				updateJarRemovalState();
			}
		});
		
		GridLayoutFactory.swtDefaults().numColumns(4).equalWidth(true).applyTo(this);
		GridDataFactory.swtDefaults().span(2, 1).align(SWT.LEFT, SWT.CENTER).applyTo(jarLabel);
		GridDataFactory.swtDefaults().span(2, 1).align(SWT.LEFT, SWT.CENTER).applyTo(confLabel);
		GridDataFactory.swtDefaults().minSize(400, 100).span(2, 1).grab(true, true).align(SWT.FILL, SWT.FILL).applyTo(jarList);
		GridDataFactory.swtDefaults().minSize(400, 100).span(2, 1).grab(true, true).align(SWT.FILL, SWT.FILL).applyTo(confList);
		GridDataFactory.swtDefaults().grab(true, false).align(SWT.FILL, SWT.CENTER).applyTo(addButton);
		GridDataFactory.swtDefaults().grab(true, false).align(SWT.FILL, SWT.CENTER).applyTo(removeButton);
		GridDataFactory.swtDefaults().grab(true, false).align(SWT.FILL, SWT.CENTER).applyTo(okButton);
		GridDataFactory.swtDefaults().grab(true, false).align(SWT.FILL, SWT.CENTER).applyTo(cancelButton);
	}

	private Label label(String text) {
		Label label = new Label(this, SWT.NONE);
		label.setText(text);
		return label;
	}
	
	private Button button(String text) {
		Button button = new Button(this, SWT.PUSH);
		button.setText(text);
		return button;
	}
	
	private void updateJarRemovalState() {
		removeButton.setEnabled(jarList.getSelectionCount() > 0 );
	}
}
