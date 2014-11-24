package com.db4o.omplus.ui.model;

import org.eclipse.jface.viewers.LabelProvider;
import org.eclipse.swt.graphics.Image;

import com.db4o.omplus.datalayer.ImageUtility;
import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.classviewer.ClassTreeNode;

public class ClassViewerLabelProvider extends LabelProvider
{
	@SuppressWarnings("unused")
	private String view;
	
//	TODO: str is null when set. passing just string is blunder
	public ClassViewerLabelProvider(String str){
		view = str;
	}
	
	public String getText(Object property)
	{
		if(property instanceof ClassTreeNode){
			ClassTreeNode node = (ClassTreeNode)property;
			if(node.getNodeType() == OMPlusConstants.PACKAGE_NODE)
				return ((ClassTreeNode)property).getName();
			else if(node.getNodeType() == OMPlusConstants.CLASS_NODE){
//				if(view.equals(ClassViewer.FLAT_VIEW))
					return ((ClassTreeNode)property).getName();	
//				else
//					return getClassName(((ClassTreeNode)property).getName());
			}
			else
				return getFieldName(node.getName());
		}
		return "";
	}
	
	private String getFieldName(String name){
		String strArray [] = ((String)name).split(OMPlusConstants.REGEX);
		return strArray[strArray.length - 1];
	}
	
/*	private String getClassName(String name){
		int index = ((String)name).lastIndexOf(OMPlusConstants.DOT_OPERATOR);
		String className = ((String)name).substring(index +1); 
		return className;
	}*/

	public Image getImage(Object element) {
		if(element instanceof ClassTreeNode){
			ClassTreeNode node = (ClassTreeNode)element;
			if(node.getNodeType() == OMPlusConstants.PACKAGE_NODE)
				return ImageUtility.getImage(OMPlusConstants.COMPLEX_ICON);
			else if(node.getNodeType() == OMPlusConstants.CLASS_NODE)
					return ImageUtility.getImage(OMPlusConstants.COMPLEX_ICON);
			else if(node.getNodeType() == OMPlusConstants.CLASS_FIELD_NODE) {
				int fieldType = node.getFieldNodeType();
				switch(fieldType)
				{
					case OMPlusConstants.PRIMITIVE:
						return ImageUtility.getImage(OMPlusConstants.PRIMITIVE_ICON);
					case OMPlusConstants.COLLECTION:
						return ImageUtility.getImage(OMPlusConstants.COLLECTION_ICON);	
					case OMPlusConstants.COMPLEX:
						return ImageUtility.getImage(OMPlusConstants.COMPLEX_ICON);
					 
				}
			}
//			return ImageUtility.getImage(OMPlusConstants.PRIMITIVE_ICON);
			}
			
		return null;
	}
}
