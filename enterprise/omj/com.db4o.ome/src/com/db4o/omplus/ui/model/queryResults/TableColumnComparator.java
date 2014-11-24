package com.db4o.omplus.ui.model.queryResults;

import java.math.BigDecimal;
import java.math.BigInteger;

import org.eclipse.jface.viewers.Viewer;
import org.eclipse.jface.viewers.ViewerComparator;

import com.db4o.omplus.datalayer.queryresult.QueryResultRow;
import com.db4o.reflect.generic.GenericObject;

public class TableColumnComparator extends ViewerComparator{
	
	public static final int ASCENDING = 0;   
	public static final int DESCENDING = 1; 
	
	private int columnIndex; 
	public int direction;
	
	public TableColumnComparator(int index, int direction){
		this.columnIndex = index - 1;
		this.direction = direction;
	}

	public int compare(Viewer viewer, Object e1, Object e2) {
		int cmp = 0;
		QueryResultRow row1 = (QueryResultRow)e1;
		QueryResultRow row2 = (QueryResultRow)e2;
		if(row1.getValues().length >= columnIndex){
			Object obj1 = row1.getValue(columnIndex);
			Object obj2 = row2.getValue(columnIndex);
			if(obj1 instanceof GenericObject || obj1 instanceof String ||
					obj1 instanceof Boolean || obj1 instanceof Character)
				cmp = compareString(obj1.toString(), obj2.toString());
			else if(obj1 instanceof Number && obj2 instanceof Number)
				cmp = compareNum((Number)obj1, (Number)obj2);
			if(direction == ASCENDING){
				return cmp;
			}
			else {
				return -1 * cmp;
			}
		}
		return 0;
	}

	private int compareNum(Number obj1, Number obj2) {
		if(obj1 instanceof Integer && obj2 instanceof Integer)
			return ((Integer)obj1).compareTo((Integer)obj2);
		else if(obj1 instanceof Double && obj2 instanceof Double)
			return ((Double)obj1).compareTo((Double)obj2);
		else if(obj1 instanceof Long && obj2 instanceof Long)
			return ((Long)obj1).compareTo((Long)obj2);
		else if(obj1 instanceof Float && obj2 instanceof Float)
			return ((Float)obj1).compareTo((Float)obj2);
		else if(obj1 instanceof Short && obj2 instanceof Short)
			return ((Short)obj1).compareTo((Short)obj2);
		else if(obj1 instanceof Byte && obj2 instanceof Byte)
			return ((Byte)obj1).compareTo((Byte)obj2);
		else if(obj1 instanceof BigDecimal && obj2 instanceof BigDecimal)
			return ((BigDecimal)obj1).compareTo((BigDecimal)obj2);
		else if(obj1 instanceof BigInteger && obj2 instanceof BigInteger)
			return ((BigInteger)obj1).compareTo((BigInteger)obj2);
		return 0;
	}

	private int compareString(String str, String str2) {
		return str.compareToIgnoreCase(str2);
	}
}
