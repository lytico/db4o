package com.db4o.omplus.ui.customisedcontrols.queryBuilder;

import org.eclipse.jface.viewers.*;
import org.eclipse.swt.*;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.queryBuilder.*;

public class QueryBuilderComboEditor extends EditingSupport
{
	private ComboBoxCellEditor comboBoxCellEditor;
	
	private QueryGroup queryGroup;
	private TableViewer tableViewer;
	private int columnIndex;

	public QueryBuilderComboEditor(TableViewer viewer, QueryGroup  group, int i) 
	{
		super(viewer);
		this.queryGroup = group;
		this.tableViewer = viewer;
		this.columnIndex = i;
		//Create a combo with some default values
		comboBoxCellEditor = new ComboBoxCellEditor(tableViewer.getTable(),new String[] {"i1","i2"},SWT.READ_ONLY);		
	}

	@Override
	protected boolean canEdit(Object element)
	{
		return true;
	}

	@Override
	protected CellEditor getCellEditor(Object element) 
	{
		QueryClause queryClause = (QueryClause)element;
		int datatype = reflectHelper().getFieldTypeClass(queryClause.getField());
		String[] choices = null;
		
		switch(columnIndex)
		{
			case QueryBuilderConstants.CONDITION:
			{	
				switch(datatype)
				{
					case QueryBuilderConstants.DATATYPE_BOOLEAN:
						choices = QueryBuilderConstants.BOOLEAN_CONDITION_ARRAY;
						break;
					case QueryBuilderConstants.DATATYPE_CHARACTER:
						choices = QueryBuilderConstants.CHARACTER_CONDITION_ARRAY;
						break;
					case QueryBuilderConstants.DATATYPE_DATE_TIME:
						choices = QueryBuilderConstants.DATE_TIME_CONDITION_ARRAY;
						break;
					case QueryBuilderConstants.DATATYPE_NUMBER:
						choices = QueryBuilderConstants.NUMERIC_CONDITION_ARRAY;
						break;
					case QueryBuilderConstants.DATATYPE_STRING:
						choices = QueryBuilderConstants.STRING_CONDITION_ARRAY;
						break;	
				} 	
				break;
			}
			case QueryBuilderConstants.OPERATOR:
				
				int objectIndex = queryGroup.getQueryList().indexOf(queryClause);
				if(objectIndex == 0)
				{
					choices = QueryBuilderConstants.OPERATOR_ARRAY; 
				}
				else
				{
					choices = new String[] {queryGroup.getQueryList().get(0).getOperator()};
				}				
				break;
		}		
		
		comboBoxCellEditor.setItems(choices);		
		return comboBoxCellEditor;
	}

	@Override
	protected Object getValue(Object element) 
	{
		String[] items = comboBoxCellEditor.getItems(); 
		QueryClause query = (QueryClause)element;
		
		String searchVal = null;
		if(columnIndex == QueryBuilderConstants.CONDITION)
			searchVal = query.getCondition();
		else if(columnIndex == QueryBuilderConstants.OPERATOR)
			searchVal = query.getOperator();
		
		for( int i = 0; i < items.length; i++ ) 
		{
	        if( items[i].equals(searchVal) ) 
	        {
	            return new Integer(i);
	        }
		}
		return new Integer(-1);
	}

	@Override
	protected void setValue(Object element, Object value) 
	{
		if(((Integer)value).intValue()<0) //For any other values typed in return -1;
			return;
		
		QueryClause query = (QueryClause)element;
		int datatype = reflectHelper().getFieldTypeClass(query.getField());
		
		switch(columnIndex)
		{
			case QueryBuilderConstants.CONDITION:
			{	
				switch(datatype)
				{
					case QueryBuilderConstants.DATATYPE_BOOLEAN:
						query.setCondition(QueryBuilderConstants.BOOLEAN_CONDITION_ARRAY[((Integer)value).intValue()]);
						break;
					case QueryBuilderConstants.DATATYPE_CHARACTER:
						query.setCondition(QueryBuilderConstants.CHARACTER_CONDITION_ARRAY[((Integer)value).intValue()]);
						break;
					case QueryBuilderConstants.DATATYPE_DATE_TIME:
						query.setCondition(QueryBuilderConstants.DATE_TIME_CONDITION_ARRAY[((Integer)value).intValue()]);
						break;
					case QueryBuilderConstants.DATATYPE_NUMBER:
						query.setCondition(QueryBuilderConstants.NUMERIC_CONDITION_ARRAY[((Integer)value).intValue()]);
						break;
					case QueryBuilderConstants.DATATYPE_STRING:
						query.setCondition(QueryBuilderConstants.STRING_CONDITION_ARRAY[((Integer)value).intValue()]);
						break;						
				}	
				break;
			}
			case QueryBuilderConstants.OPERATOR:
				int indexObject = queryGroup.getQueryList().indexOf(query);
				//LOGIC: Make every other row in a QueryGroup show the same operator as the first row/clause
				if(indexObject == 0)
				{
					//System.out.println("Index 0");
					query.setOperator(QueryBuilderConstants.OPERATOR_ARRAY[((Integer)value).intValue()]);
				}
				else
				{
					//System.out.println(indexObject);
					query.setOperator(queryGroup.getQueryList().get(0).getOperator());
				}

			 	break;
		}		
		int objectIndex = queryGroup.getQueryList().indexOf(query);
		queryGroup.updateData(query,objectIndex);
		queryGroup.resetAllOperators();
		tableViewer.refresh();		
	}

	private ReflectHelper reflectHelper() {
		return Activator.getDefault().dbModel().db().reflectHelper();
	}
}
