package com.db4o.omplus.ui.model.propertyViewer;

import org.eclipse.jface.viewers.ITableLabelProvider;
import org.eclipse.jface.viewers.LabelProvider;
import org.eclipse.swt.graphics.Image;

import com.db4o.omplus.datalayer.propertyViewer.PropertyViewerConstants;
import com.db4o.omplus.datalayer.propertyViewer.dbProperties.DBProperties;




public class DBPropertyLabelProvider extends LabelProvider implements ITableLabelProvider
{
	private final String NA = "NA";
	
	public Image getColumnImage(Object element, int columnIndex) {
		// Auto-generated method stub
		return null;
	}

	public String getColumnText(Object element, int columnIndex) {
		if(element instanceof DBProperties)
		{
			long property = 0;
			DBProperties properties = (DBProperties)element;
			switch(columnIndex)
			{
				case PropertyViewerConstants.DBSIZE_ID:
					property = properties.getDbSize();
					break;			
				case PropertyViewerConstants.NUM_CLASSES_ID:
					property = properties.getNoOfClasses();
					break;								
				case PropertyViewerConstants.FREE_SPACE_ID:
					property = properties.getFreeSpace();
					break;
			}
			if(property == 0)
				return NA;
			return new Long(property).toString();
		}
		return null;
	}

}
