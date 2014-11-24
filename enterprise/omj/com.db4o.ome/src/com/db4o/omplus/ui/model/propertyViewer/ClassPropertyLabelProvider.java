package com.db4o.omplus.ui.model.propertyViewer;

import org.eclipse.jface.viewers.ITableLabelProvider;
import org.eclipse.jface.viewers.LabelProvider;
import org.eclipse.swt.graphics.Image;

import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.propertyViewer.PropertyViewerConstants;
import com.db4o.omplus.datalayer.propertyViewer.classProperties.FieldProperties;
import com.db4o.omplus.ui.model.ArrayLabelProvider;




public class ClassPropertyLabelProvider extends LabelProvider implements ITableLabelProvider
{
	private final String PUBLIC = "Public";

	public Image getColumnImage(Object element, int columnIndex) {
		// Auto-generated method stub
		return null;
	}

	public String getColumnText(Object element, int columnIndex) 
	{
		
		if(element instanceof FieldProperties)
		{
			FieldProperties properties = (FieldProperties)element;
			switch(columnIndex)
			{
				case PropertyViewerConstants.FIELD_ID:
					return properties.getFieldName();
//				GA not handled
				case PropertyViewerConstants.DATATYPE_ID:
					String type = properties.getFieldDataType();
					String arrType = ArrayLabelProvider.lookUp(type);
					if( arrType != null)
						return arrType;
					return type;
					
					
				case PropertyViewerConstants.INDEXED_ID:
				{
					/*if(properties.isIndexed())
						return OMPlusConstants.YES;
					else
						return OMPlusConstants.NO;*/
					return new Boolean(properties.isIndexed()).toString();
				}

				case PropertyViewerConstants.ACCESS_MODIFIER_ID:
				{
					//NOTE: Ideally the datamodel at back end should make this boolean
					//But future if there is any way of showing Pvt/Default etc. hence kep as string
					//and at UI level for version 1.0 just change teh dispaly string acc to UI
					if(properties.getAccessModifier().equalsIgnoreCase(PUBLIC))
						return OMPlusConstants.YES;
					else
						return OMPlusConstants.NO;
					
				}
					
			}
		}
		return null;		
	}

}
