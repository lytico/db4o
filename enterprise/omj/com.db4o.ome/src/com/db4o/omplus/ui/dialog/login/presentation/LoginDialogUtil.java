/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.presentation;

import org.eclipse.swt.*;
import org.eclipse.swt.widgets.*;

import com.db4o.omplus.connection.*;
import com.db4o.omplus.ui.*;
import com.db4o.omplus.ui.dialog.login.model.*;
import com.db4o.omplus.ui.dialog.login.model.ConnectionPresentationModel.ConnectionPresentationListener;

public class LoginDialogUtil {

	public static final String RECENT_CONNECTION_COMBO_ID = LoginDialogUtil.class.getName() + "$recentConnectionCombo";

	public static <P extends ConnectionParams> Combo recentConnectionCombo(Composite parent, final ConnectionPresentationModel<P> model) {
		final Combo combo = new Combo(parent, SWT.NONE);
		// FIXME not a unique id, obviously
		OMESWTUtil.assignWidgetId(combo, RECENT_CONNECTION_COMBO_ID);
		String[] items = model.recentConnections();
		combo.setItems(items);
		combo.setToolTipText("no selection");
		combo.addListener(SWT.Selection, new Listener(){
			public void handleEvent(Event event) {
				model.select(combo.getSelectionIndex());
				combo.setToolTipText(combo.getItem(combo.getSelectionIndex()));
			}
		});
		model.addConnectionPresentationListener(new ConnectionPresentationListener() {
			public void connectionPresentationState(boolean editEnabled, int jarPathCount, int configuratorCount) {
				if(editEnabled) {
					combo.deselectAll();
					combo.setToolTipText("no selection");
				}
			}
		});
		return combo;
	}

}
