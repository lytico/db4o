package com.db4o.omplus.ui.model.queryBuilder;

import org.eclipse.jface.viewers.IStructuredContentProvider;
import org.eclipse.jface.viewers.Viewer;

import com.db4o.omplus.datalayer.queryBuilder.QueryGroup;


public class TableContentProvider implements IStructuredContentProvider 
{
	private QueryGroup queryGroup;
	@SuppressWarnings("unused")
	private int tableIndex;
	
	public TableContentProvider(QueryGroup q, int i)
	{
		queryGroup = q;
		tableIndex = i;
	}
	
	public Object[] getElements(Object inputElement) {
		return queryGroup.getQueryList().toArray();
	}

	public void dispose() {
		// Auto-generated method stub
		
	}

	public void inputChanged(Viewer viewer, Object oldInput, Object newInput) {
		// Auto-generated method stub
		
	}

}
