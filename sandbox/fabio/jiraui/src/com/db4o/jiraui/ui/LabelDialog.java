package com.db4o.jiraui.ui;
import org.eclipse.swt.SWT;
import org.eclipse.swt.events.*;
import org.eclipse.swt.graphics.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;

public class LabelDialog {

	private Text labelText;

	private final Display display;

	private String label;

	public LabelDialog(Display display, String label) {
		this.display = display;
		this.label = label;
	}

	protected Control createDialogArea(Composite parent) {
		final Composite comp = parent;

		GridLayout layout = new GridLayout();
		layout.numColumns = 2;
		comp.setLayout(layout);

		Label usernameLabel = new Label(comp, SWT.RIGHT);
		usernameLabel.setText("Label: ");

		labelText = new Text(comp, SWT.SINGLE);
		GridData data = new GridData(GridData.FILL_HORIZONTAL);
		labelText.setLayoutData(data);
		if (label != null) {
			labelText.setText(label);
			labelText.selectAll();
		}

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
				label = null;
				comp.getShell().dispose();
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
		
		Button login = new Button(buttons, SWT.PUSH);
		login.setText("Save");
		login.addSelectionListener(new SelectionListener() {
			
			@Override
			public void widgetSelected(SelectionEvent arg0) {
				label = labelText.getText();
				comp.getShell().dispose();
			}
			
			@Override
			public void widgetDefaultSelected(SelectionEvent arg0) {
			}
		});
		
		labelText.setFocus();
		
		return comp;
	}

	public String open() {
		
		Shell shell = new Shell(display, SWT.APPLICATION_MODAL | SWT.DIALOG_TRIM);
		
		createDialogArea(shell);
		
		shell.setSize(600, 150);
		
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
		
		return label;
		
	}

}
