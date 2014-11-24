package com.db4o.omplus.ui.listeners.queryBuilder;

import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.events.SelectionListener;
import org.eclipse.swt.widgets.Combo;

import com.db4o.omplus.datalayer.queryBuilder.QueryBuilderConstants;
import com.db4o.omplus.datalayer.queryBuilder.QueryGroup;



public class TableComboSelectionListener implements SelectionListener 
{
	@SuppressWarnings("unused")
	private int tableIndex; 
	//private OMQuery constaintList;
	Combo parentCombo;
	private QueryGroup queryGroup;
	
	//public TableComboSelectionListener(int index,OMQuery c,Combo p )
	public TableComboSelectionListener(int index,Combo p, QueryGroup qg )
	{
		tableIndex = index;
		//constaintList = c;
		parentCombo = p;
		queryGroup = qg;
	}

	public void widgetDefaultSelected(SelectionEvent e) {
		System.out.println("WARNING: TableComboSelectionListener -> widgetDefaultSelected() should be implemneted");
		
	}

	public void widgetSelected(SelectionEvent e) 
	{
		//constaintList.updateOperatorForGroup(tableIndex,parentCombo.getSelectionIndex());
		queryGroup.setGroupOperator(QueryBuilderConstants.OPERATOR_ARRAY[parentCombo.getSelectionIndex()]);
	}

}
