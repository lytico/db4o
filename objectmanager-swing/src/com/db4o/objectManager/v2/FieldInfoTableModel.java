package com.db4o.objectManager.v2;

import com.db4o.Db4o;
import com.db4o.ext.StoredClass;
import com.db4o.ext.StoredField;
import com.db4o.objectmanager.api.helpers.ReflectHelper2;
import com.spaceprogram.db4o.sql.util.ReflectHelper;

import javax.swing.event.TableModelEvent;
import javax.swing.event.TableModelListener;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableModel;

/**
 * User: treeder
 * Date: Sep 11, 2006
 * Time: 3:02:44 PM
 */
public class FieldInfoTableModel extends DefaultTableModel implements TableModel, TableModelListener {
	private UISession session;
	private String className;
	static String columns[] = new String[]{
			"Name",
			"Type",
			"Indexed?",
	};
	private boolean editMode;
	private boolean working;


	public FieldInfoTableModel(UISession session, String className) {
		super(columns, 0);
		this.session = session;

		this.className = className;

		// not storing these as class fields so that if reopen is called, this stuff won't be disconnected.
		StoredField[] fields = getStoredFields(className);
		super.setRowCount(fields.length);
		int r = 0, c = 0;
		for (int i = 0; i < fields.length; i++) {
			StoredField field = fields[i];
			c = 0;
			//System.out.println("field " + field.getName() + ", storedType: " + field.getStoredType() + "  " + field);
			setValueAt(field.getName(), r, c++);
			// getStoredType returns null for primitive arrays.... hmmmm
			if (field.getStoredType() != null) {
				setValueAt(field.getStoredType().getName(), r, c++);
				// 	if not primitive or second class, then don't even show checkbox
				if (!(field.getStoredType().isCollection() || field.getStoredType().isArray())) {
					setValueAt(new Boolean(field.hasIndex()), r, c++);
				} else {
					setValueAt(null, r, c++);
				}
			} else {
				
			}
			r++;
		}
		addTableModelListener(this);
	}

	private StoredField[] getStoredFields(String className) {
		StoredClass storedClass = session.getObjectContainer().ext().storedClass(className);
		StoredField[] fields = ReflectHelper.getDeclaredFieldsInHeirarchy(storedClass); // todo: this is a problem if you've refactored classes that are available to OM, since it will use the available class, not the renamed stuff
		return fields;
	}


	public Class getColumnClass(int column) {
		if (column == 2) {
			return Boolean.class;
		}
		return super.getColumnClass(column);
	}

	/**
	 * Unfortunately, none of this schema evolution stuff works in c/s mode.
	 *
	 * @param row
	 * @param column
	 * @return
	 */
	public boolean isCellEditable(int row, int column) {
		if (!working && editMode) {
			if (column == 0) {
				// name
				return true;
			} else if (column == 2 && ReflectHelper2.isIndexable(getStoredFields(className)[row])) { // indexes
				return true;
			}
		}
		return false;
	}

	public void setEditMode(boolean editMode) {
		this.editMode = editMode;
	}

	public void tableChanged(TableModelEvent e) {
		working = true; // just so it will only do one thing at a time
		int row = e.getFirstRow();
		int column = e.getColumn();
		TableModel model = (TableModel) e.getSource();
		Object aValue = model.getValueAt(row, column);
		if (column == 0) {
			if (aValue instanceof String) {
				StoredField[] fields = getStoredFields(className);
				renameField(fields[row], (String) aValue);
			} else {
				System.err.println("Invalid type for renaming!!! Report bug at http://tracker.db4o.com/jira");
			}
			session.reopen();
		} else if (column == 2) {
			Boolean b = (Boolean) aValue;
			// for indexes, we'll have to reopen the objectcontainer and turn on the index before opening
			StoredField[] fields = getStoredFields(className);
			StoredField field = fields[row];
			Db4o.configure().objectClass(className).objectField(field.getName()).indexed(b.booleanValue());
			session.reopen();
		}
		working = false;
	}

	public void setValueAt(Object aValue, int row, int column) {
		super.setValueAt(aValue, row, column);
	}

	/**
	 * This will do a schema change in db4o.
	 *
	 * @param field
	 * @param newName
	 */
	private void renameField(StoredField field, String newName) {
		field.rename(newName);
		session.getObjectContainer().commit();
		// need to reopen ObjectContainer here to make it stick
		session.reopen();
	}
}
