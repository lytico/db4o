package com.db4o.objectManager.v2;

import com.db4o.objectmanager.api.DatabaseInspector;
import com.db4o.reflect.ReflectClass;

import javax.swing.table.TableModel;
import javax.swing.table.DefaultTableModel;
import java.util.List;

/**
 * User: treeder
 * Date: Sep 3, 2006
 * Time: 5:03:28 PM
 */
public class ClassStatsTableModel extends DefaultTableModel implements TableModel {
    private DatabaseInspector databaseInspector;
    static String columns[] = new String[]{
            "Class",
            "Objects",
            /*"Data size",
            "Index size",*/
    };
	private List<ReflectClass> storedClasses;

	public ClassStatsTableModel(DatabaseInspector databaseInspector) {
        super(columns, 0);
		storedClasses = databaseInspector.getClassesStored();
		super.setRowCount(storedClasses.size());
        this.databaseInspector = databaseInspector;
        int r=0,c=0;
        for (int i = 0; i < storedClasses.size(); i++) {
            ReflectClass storedClass = storedClasses.get(i);
            c=0;
            setValueAt(storedClass.getName(),r,c++);
            setValueAt(databaseInspector.getNumberOfObjectsForClass(storedClass.getName()),r,c++);
            //setValueAt(databaseInspector.getSpaceUsedByClass(storedClass.getName()),r,c++);
            //setValueAt(databaseInspector.getSpaceUsedByClassIndexes(storedClass.getName()), r, c++);
            r++;
        }
	}

    public String getColumnName(int column) {
        return columns[column];
    }

    public Class<?> getColumnClass(int columnIndex) {
        if(columnIndex == 0){
            return String.class;
        } else {
            return Number.class;
        }
    }

    public boolean isCellEditable(int row, int column) {
        return false;
    }

	public List<ReflectClass> getStoredClasses() {
		return storedClasses;
	}
}
