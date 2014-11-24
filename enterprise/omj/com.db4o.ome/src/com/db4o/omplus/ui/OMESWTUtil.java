/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui;

import org.eclipse.swt.widgets.*;

public class OMESWTUtil {

	public static void assignWidgetId(Widget widget, String id) {
		widget.setData(OMESWTUtil.WIDGET_NAME_KEY, id);
	}

	// FIXME localize usage in separate class
	public static final String WIDGET_NAME_KEY = "ome_widget_name";
	
	private OMESWTUtil() {
	}
}
