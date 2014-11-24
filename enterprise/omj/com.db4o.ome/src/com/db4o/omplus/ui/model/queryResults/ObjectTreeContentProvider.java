package com.db4o.omplus.ui.model.queryResults;

import org.eclipse.jface.viewers.ITreeContentProvider;
import org.eclipse.jface.viewers.Viewer;


import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.queryresult.ObjectTreeNode;
import com.db4o.omplus.ui.customisedcontrols.queryResult.ObjectViewer;
import com.db4o.omplus.ui.customisedcontrols.queryResult.ObjectViewerTab;

/**
 * Class to set the contents for the tree that displays the Object in every QueryResult 
 * @author prameela_nair
 *
 */

@SuppressWarnings("unused")
public class ObjectTreeContentProvider implements ITreeContentProvider 
{
	private ObjectViewer objectViewer;
	private ObjectViewerTab objectViewerTab;
	
	public ObjectTreeContentProvider(ObjectViewer ob, ObjectViewerTab t)
	{
		objectViewer = ob;
		objectViewerTab = t; 
	}
	
	public Object[] getChildren(Object parentElement)
	{
		if(parentElement instanceof ObjectTreeNode)
		{
			ObjectTreeNode node = (ObjectTreeNode)parentElement;			
//			ObjectTreeNode [] treeNodes =  objectViewer.getObjectTreeBuilder().getObjectTree(node.getType(),node.getValue());
			ObjectTreeNode [] treeNodes =  objectViewer.getObjectTreeBuilder().getObjectTree(node);
	        return treeNodes;
		}
		else
		{
			System.out.println("Error: ObjectTreeContentProvider -> getChildren()");
			return null;
		}
		
	}

	public Object getParent(Object element) {
		// Auto-generated method stub
		return null;
	}

	public boolean hasChildren(Object element)
	{
		if(element instanceof ObjectTreeNode)
		{
			ObjectTreeNode node = (ObjectTreeNode)element;
			String type = node.getType();
			// currently added StringBuffer & StringBuilder checks. if there are more separate method
			// for this would be better
			if(node.isPrimitive() || node.getValue().equals(OMPlusConstants.NULL_VALUE)
				|| type.equals(StringBuffer.class.getName()) || type.equals(StringBuilder.class.getName()) )
			{
				return false;
			}
			else
				return true;
		}
		else
			return false;
	}

	public Object[] getElements(Object inputElement)
	{
		return (ObjectTreeNode[])inputElement;
	}

	public void dispose() {
	}

	public void inputChanged(Viewer viewer, Object oldInput, Object newInput) {
	}

}	

