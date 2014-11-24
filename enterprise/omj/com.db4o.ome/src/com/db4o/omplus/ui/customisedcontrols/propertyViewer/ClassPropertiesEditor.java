package com.db4o.omplus.ui.customisedcontrols.propertyViewer;

import org.eclipse.jface.viewers.CellEditor;
import org.eclipse.jface.viewers.ComboBoxCellEditor;
import org.eclipse.jface.viewers.EditingSupport;
import org.eclipse.jface.viewers.TableViewer;
import org.eclipse.swt.SWT;

import com.db4o.omplus.datalayer.propertyViewer.classProperties.FieldProperties;

public class ClassPropertiesEditor extends EditingSupport 
{
	private TableViewer tableViewer;
	private ComboBoxCellEditor editor;
	private final String [] comboValues = {"true", "false"};
	
	public ClassPropertiesEditor(TableViewer viewer)
	{
		super(viewer);
		this.tableViewer = viewer;
		editor = new ComboBoxCellEditor(viewer.getTable(),comboValues,SWT.READ_ONLY);	
	}

	@Override
	protected boolean canEdit(Object element)
	{
		return true;
	}

	@Override
	protected CellEditor getCellEditor(Object element) 
	{	
		//return comboBoxCellEditor;
		return editor;
	}

	@Override
	protected Object getValue(Object element) 
	{
		FieldProperties fieldProperties = (FieldProperties) element;
		
		String[] items = editor.getItems(); 
		
		Boolean boolean1 = new Boolean(fieldProperties.isIndexed());
		
		for( int i = 0; i < items.length; i++ ) 
		{
	        if( items[i].equalsIgnoreCase(boolean1.toString())) 
	        {
	            return new Integer(i);
	        }
		}
		return new Integer(-1);
	}

	@Override
	protected void setValue(Object element, Object value) 
	{
		FieldProperties fieldProperties = (FieldProperties) element;
		if(value!= null) {
			int index = ((Integer)value).intValue();
			if(index == 0){
				fieldProperties.setIndexed(true);
			}else if(index == 1){
				fieldProperties.setIndexed(false);
			}
			tableViewer.refresh();
		}
	}
}
