package com.db4o.omplus.ui.model.propertyViewer;

import org.eclipse.jface.viewers.ITableLabelProvider;
import org.eclipse.jface.viewers.LabelProvider;
import org.eclipse.swt.graphics.Image;

import com.db4o.omplus.datalayer.propertyViewer.PropertyViewerConstants;
import com.db4o.omplus.datalayer.propertyViewer.objectProperties.ObjectProperties;

public class ObjectPropertyLabelProvider extends LabelProvider implements ITableLabelProvider
{

	private final String NA = "NA";
	
	public Image getColumnImage(Object element, int columnIndex) 
	{
		// Auto-generated method stub
		return null;
	}

	public String getColumnText(Object element, int columnIndex) 
	{
		if(element instanceof ObjectProperties)
		{
			ObjectProperties properties = (ObjectProperties)element;
			switch(columnIndex)
			{
				case PropertyViewerConstants.UUID_ID:
					return properties.getUuidAsString();
												
				case PropertyViewerConstants.LOCAL_IDENTIFIER_ID:
					if(properties.getLocalID() == 0)
						return NA;
					return new Long(properties.getLocalID()).toString();
								
				case PropertyViewerConstants.DEPTH_ID:
					if(properties.getDepth() == 0)
						return NA;
					return new Integer(properties.getDepth()).toString();
					
				case PropertyViewerConstants.VERSION_ID:
					if(properties.getVersion() == 0)
						return NA;
					return new Long(properties.getVersion()).toString();	
					
			}
		}
		return null;
	}

}
