package com.db4o.omplus.ui.model.queryResults;

import java.util.Collection;
import java.util.Map;

import org.eclipse.jface.viewers.ITableLabelProvider;
import org.eclipse.jface.viewers.LabelProvider;
import org.eclipse.swt.graphics.Image;

import com.db4o.omplus.datalayer.ImageUtility;
import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.queryresult.ObjectTreeNode;
import com.db4o.omplus.ui.customisedcontrols.queryResult.ObjectViewer;
import com.db4o.omplus.ui.customisedcontrols.queryResult.ObjectViewerTab;
import com.db4o.omplus.ui.model.ArrayLabelProvider;


@SuppressWarnings("unused")
public class ObjectTreeLabelProvider extends LabelProvider implements ITableLabelProvider 
{
	private ObjectViewer objectViewer;
	private ObjectViewerTab objectViewerTab;

	public ObjectTreeLabelProvider(ObjectViewer ob, ObjectViewerTab t)
	{
		objectViewer = ob;
		objectViewerTab = t;
	}
	public String getText(Object property)
	{
		return property.toString();
	}

	public Image getColumnImage(Object element, int columnIndex) 
	{
		if(columnIndex == 0)
		{
			ObjectTreeNode node = (ObjectTreeNode)element;			
			
			switch(node.getNodeType())
			{
				case OMPlusConstants.PRIMITIVE:
					return ImageUtility.getImage(OMPlusConstants.PRIMITIVE_ICON);
				case OMPlusConstants.COLLECTION:
					return ImageUtility.getImage(OMPlusConstants.COLLECTION_ICON);	
				case OMPlusConstants.COMPLEX:
					return ImageUtility.getImage(OMPlusConstants.COMPLEX_ICON);
				default:
					return null;		 
			}			
		}
		
		return null;
	}

	@SuppressWarnings("unchecked")
	public String getColumnText(Object element, int columnIndex)
	{
		if(columnIndex == 0){
			String[] strArray = ((ObjectTreeNode)element).getName().split(OMPlusConstants.REGEX);
			int length = strArray.length;
			return strArray[length - 1];
		}
		else if(columnIndex == 1){
			ObjectTreeNode node = (ObjectTreeNode)element;
			Object value = node.getValue();
			if(node.getNodeType() == OMPlusConstants.COLLECTION ){
				if(value instanceof Collection)
					return getString( ((Collection)value).size() );
				else if(value instanceof Map)
					return getString( ((Map)value).size() );
				else {
					return getString( node.getArrayLength());
				}
			}
			
			return ( (node.getValue() != null)? node.getValue().toString():"");
		}
			
		else if(columnIndex == 2){
			String type = ((ObjectTreeNode)element).getType();
			String arrType = ArrayLabelProvider.lookUp(type);
			if( arrType != null)
				return arrType;
			return type;
		}
		else
			return "";
	}
	
	private String getString(int size){
		StringBuilder sb = new StringBuilder();
		sb.append(size);
		sb.append(" items");
		return sb.toString();
	}
}