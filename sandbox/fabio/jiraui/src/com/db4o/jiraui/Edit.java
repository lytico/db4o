package com.db4o.jiraui;

import java.util.*;

import org.eclipse.swt.*;
import org.eclipse.swt.events.*;
import org.eclipse.swt.graphics.*;
import org.eclipse.swt.layout.*;
import org.eclipse.swt.widgets.*;

import com.db4o.jiraui.api.*;
import com.db4o.jiraui.ui.*;


public class Edit {

	static final String TRACKER_URL = "http://tracker.db4o.com/rpc/soap/jirasoapservice-v2";

	public static void main(String[] args) {
		
		Locale.setDefault(Locale.UK);
		
		Display d = Display.getDefault();
		Shell s = new Shell(d);

		s.setText("Task fine-grained priorization");

		s.setLayout(new FillLayout());

		final Root root = new Root();

		EditTasksControllerImpl controller = new EditTasksControllerImpl(d, root);
		EditTasks edit = new EditTasks(s, SWT.NONE, controller);
		controller.view = edit;

		s.setSize(Math.max(700, edit.getSuggestedWidth() + 50), 700);

		Monitor primary = d.getPrimaryMonitor ();
		Rectangle bounds = primary.getBounds ();
		Rectangle rect = s.getBounds ();
		int x = bounds.x + (bounds.width - rect.width) / 2;
		int y = bounds.y + (bounds.height - rect.height) / 2;
		s.setLocation (x, y);

		s.setVisible(true);
		
		s.addDisposeListener(new DisposeListener() {
			
			@Override
			public void widgetDisposed(DisposeEvent disposeevent) {
				root.close();
			}
		});

		while (!s.isDisposed()) {
			if (!d.readAndDispatch()) {
				d.sleep();
			}
		}

		d.dispose();
		
	}

}
