package com.db4o.objectManager.v2.query;

import com.db4o.objectManager.v2.MainPanel;
import com.db4o.objectManager.v2.util.Converter;
import com.db4o.objectManager.v2.util.Log;

import javax.swing.*;
import javax.swing.table.TableCellEditor;
import java.awt.Component;
import java.util.Date;

/**
 * User: treeder
 * Date: Mar 9, 2007
 * Time: 12:12:30 PM
 */
public class DateEditor extends DefaultCellEditor implements TableCellEditor {
	public DateEditor(JTextField textField) {
		super(textField);
	}


	public Object getCellEditorValue() {
		try {
			return Converter.convertFromString(Date.class, (String) super.getCellEditorValue());
		} catch(Exception e) {
			e.printStackTrace();
			Log.addException(e);
		}
		return null;
	}

	public Component getTableCellEditorComponent(JTable table, Object value, boolean isSelected, int row, int column) {
		value = MainPanel.dateFormatter.edit((Date) value);
		return super.getTableCellEditorComponent(table, value, isSelected, row, column);
	}

}
