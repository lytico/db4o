package com.db4o.omplus.ui.customisedcontrols.queryBuilder;

import java.util.Date;

import org.eclipse.jface.viewers.CellEditor;
import org.eclipse.jface.viewers.EditingSupport;
import org.eclipse.jface.viewers.TableViewer;
import org.eclipse.jface.viewers.TextCellEditor;

import com.db4o.omplus.datalayer.queryBuilder.QueryBuilderConstants;
import com.db4o.omplus.datalayer.queryBuilder.QueryClause;
import com.db4o.omplus.datalayer.queryBuilder.QueryGroup;

public class QueryBuilderTextEditor extends EditingSupport 
{
	/**
	 * Text editor
	 */	
	private CellEditor textEditor;
	
	
	
	private QueryGroup queryGroup;
	private TableViewer tableViewer;
	private int columnIndex;
	
	//TODO: add support for OPerator column too ...
	
	public QueryBuilderTextEditor(TableViewer viewer, QueryGroup group, int i) 
	{
		super(viewer);
		this.queryGroup = group;
		this.tableViewer = viewer;
		this.columnIndex = i;
		textEditor = new TextCellEditor(viewer.getTable());			
	}

	protected boolean canEdit(Object element) 
	{
		return true;
	}

	protected CellEditor getCellEditor(Object element) 
	{

		return textEditor;
	}
	
	protected Object getValue(Object element) 
	{
		QueryClause query = ((QueryClause) element);
		Object result = null;	
		switch(columnIndex)
		{
			case QueryBuilderConstants.FIELD: 
				 	result = query.getField();
				 	break;
			case QueryBuilderConstants.VALUE:
				result = query.getValue();
			 	break;
		}
		return result;
	}

	protected void setValue(Object element, Object value) 
	{
		QueryClause query = (QueryClause) element;
		
		switch(columnIndex)
		{
			case QueryBuilderConstants.FIELD: 
					query.setField(((String)value).trim());
				 	break;
			case QueryBuilderConstants.VALUE:
				if(value instanceof Date)
					query.setValue(value);
				else
					query.setValue(((String)value).trim());
			 	break;
		}	
		int objectIndex = queryGroup.getQueryList().indexOf(query);
		queryGroup.updateData(query,objectIndex);
		//Important else display not updated
		//tableViewer.refresh(query);		
		tableViewer.refresh();
		
	}
	
}