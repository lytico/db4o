/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.presentation;

import org.eclipse.swt.widgets.*;

import com.db4o.omplus.connection.*;
import com.db4o.omplus.ui.dialog.login.model.*;

public interface LoginPaneSpec<P extends ConnectionParams> {

	ConnectionPresentationModel<P> model();
	void create(Composite parent, Composite composite);
	
}
