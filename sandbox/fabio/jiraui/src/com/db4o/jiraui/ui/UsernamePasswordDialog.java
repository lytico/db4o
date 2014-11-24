package com.db4o.jiraui.ui;
import org.eclipse.swt.SWT;
import org.eclipse.swt.events.*;
import org.eclipse.swt.graphics.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;

public class UsernamePasswordDialog {

	private Text usernameField;

	private Text passwordField;

	private final Display display;

	String[] loginInfo = null;

	private final String username;

	public UsernamePasswordDialog(Display display, String username) {
		this.display = display;
		this.username = username;
	}

	protected Control createDialogArea(Composite parent) {
		final Composite comp = parent;

		GridLayout layout = new GridLayout();
		layout.numColumns = 2;
		comp.setLayout(layout);

		Label usernameLabel = new Label(comp, SWT.RIGHT);
		usernameLabel.setText("Username: ");

		usernameField = new Text(comp, SWT.SINGLE);
		GridData data = new GridData(GridData.FILL_HORIZONTAL);
		usernameField.setLayoutData(data);
		if (username != null) {
			usernameField.setText(username);
			usernameField.selectAll();
		}

		Label passwordLabel = new Label(comp, SWT.RIGHT);
		passwordLabel.setText("Password: ");

		passwordField = new Text(comp, SWT.SINGLE | SWT.PASSWORD);
		data = new GridData(GridData.FILL_HORIZONTAL);
		passwordField.setLayoutData(data);
		
		
		Composite buttons = new Composite(comp, SWT.NONE);
		GridData gridData = new GridData(GridData.FILL_HORIZONTAL);
		gridData.grabExcessHorizontalSpace = true;
		gridData.horizontalSpan  = 2;
		buttons.setLayoutData(gridData);
		
		RowLayout rowLayout = new RowLayout(SWT.HORIZONTAL);
		rowLayout.fill = true;
		buttons.setLayout(rowLayout);
		
		Button cancel = new Button(buttons, SWT.PUSH);
		cancel.setText("Cancel");
		cancel.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				comp.getShell().dispose();
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
		
		Button login = new Button(buttons, SWT.PUSH);
		login.setText("Login");
		login.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				loginInfo = new String[]{usernameField.getText(), passwordField.getText()};
				comp.getShell().dispose();
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
		
		usernameField.setFocus();
		
		return comp;
	}

	public String[] open() {
		
		Shell shell = new Shell(display, SWT.APPLICATION_MODAL | SWT.DIALOG_TRIM);
		
		createDialogArea(shell);
		
		shell.setSize(400, 250);
		
		Monitor primary = display.getPrimaryMonitor ();
		Rectangle bounds = primary.getBounds ();
		Rectangle rect = shell.getBounds ();
		int x = bounds.x + (bounds.width - rect.width) / 2;
		int y = bounds.y + (bounds.height - rect.height) / 2;
		shell.setLocation (x, y);
		
		shell.open();
		
		while(!shell.isDisposed()) {
			if (!display.readAndDispatch()) {
				display.sleep();
			}
		}
		
		return loginInfo;
		
	}

}
