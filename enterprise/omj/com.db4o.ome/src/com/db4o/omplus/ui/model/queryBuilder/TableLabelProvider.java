package com.db4o.omplus.ui.model.queryBuilder;

import org.eclipse.jface.viewers.ITableLabelProvider;
import org.eclipse.jface.viewers.LabelProvider;
import org.eclipse.swt.graphics.Image;

import com.db4o.omplus.datalayer.queryBuilder.QueryClause;
import com.db4o.omplus.datalayer.queryBuilder.QueryGroup;



@SuppressWarnings("unused")
public class TableLabelProvider extends LabelProvider implements ITableLabelProvider 
{

	private QueryGroup queryGroup;
	private int tableIndex;
	
	public TableLabelProvider(QueryGroup q, int i)
	{
		queryGroup = q;
		tableIndex = i;
	}

	public Image getColumnImage(Object element, int columnIndex) {
		// Auto-generated method stub
		return null;
	}

	public String getColumnText(Object element, int columnIndex) 
	{
		if(columnIndex == 0)
			//return ((QueryClause)element).getField().toString();
			return ((QueryClause)element).getFieldNameWithoutPackageInfo();
		else if(columnIndex == 1)
			return ((QueryClause)element).getCondition();			
		else if(columnIndex == 2){
			Object value = ((QueryClause)element).getValue();
			if(value != null)
				return value.toString();
			else 
				return "";
		}	else if(columnIndex == 3)
			return ((QueryClause)element).getOperator();
		else
			return null;	
	}

}
