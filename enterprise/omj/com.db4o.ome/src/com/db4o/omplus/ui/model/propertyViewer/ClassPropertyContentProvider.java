package com.db4o.omplus.ui.model.propertyViewer;

import org.eclipse.jface.viewers.IStructuredContentProvider;
import org.eclipse.jface.viewers.Viewer;

import com.db4o.omplus.datalayer.propertyViewer.classProperties.ClassProperties;

public class ClassPropertyContentProvider implements IStructuredContentProvider
{
	public Object[] getElements(Object inputElement) 
	{
		return ((ClassProperties)inputElement).getFields();
	}

	public void dispose() 
	{
		// Auto-generated method stub		
	}

	public void inputChanged(Viewer viewer, Object oldInput, Object newInput) 
	{
		// Auto-generated method stub
		
	}

}
