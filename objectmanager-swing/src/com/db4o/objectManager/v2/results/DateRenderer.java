package com.db4o.objectManager.v2.results;

import java.util.*;

import javax.swing.table.*;

import com.db4o.objectManager.v2.*;

/**
 * User: treeder
 * Date: Oct 16, 2006
 * Time: 10:13:46 PM
 */
public class DateRenderer extends DefaultTableCellRenderer implements TableCellRenderer {

	public DateRenderer() {
		super();
	}

	public void setValue(Object value) {
		setText((value == null) ? "" : MainPanel.dateFormatter.display((Date) value));
	}

}
