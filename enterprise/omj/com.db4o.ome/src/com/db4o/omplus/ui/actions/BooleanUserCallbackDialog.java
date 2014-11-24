/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.actions;

import org.eclipse.jface.dialogs.*;
import org.eclipse.swt.widgets.*;

import com.db4o.foundation.*;
import com.db4o.omplus.datalayer.*;

public class BooleanUserCallbackDialog implements Function4<String, Boolean> {

	private final Shell shell;
	
	public BooleanUserCallbackDialog(Shell shell) {
		this.shell = shell;
	}

	public Boolean apply(String msg) {
		return MessageDialog.openQuestion(shell, OMPlusConstants.DIALOG_BOX_TITLE, msg);
	}

}
