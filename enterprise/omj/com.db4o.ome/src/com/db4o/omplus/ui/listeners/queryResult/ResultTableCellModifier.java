package com.db4o.omplus.ui.listeners.queryResult;

import java.util.*;

import org.eclipse.jface.viewers.*;
import org.eclipse.swt.widgets.*;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.queryBuilder.*;
import com.db4o.omplus.datalayer.queryresult.*;
import com.db4o.omplus.ui.interfaces.*;
import com.db4o.omplus.ui.model.*;

public class ResultTableCellModifier implements ICellModifier 
{
	private final String []BOOLEAN_VALUES = {Boolean.TRUE.toString(), Boolean.FALSE.toString()};
	
	private QueryResultList queryResultList;
	private QueryResultTableModifiedList modifiedObjList;
	private TableViewer tableViewer;
	
	private IChildModifier childModifier;
	private HashMap<String, String> columnMap;
	
	private QueryPresentationModel queryModel;

	private final ReflectHelper reflectHelper;	

	public ResultTableCellModifier(ReflectHelper reflectHelper, QueryPresentationModel queryModel, TableViewer tableViewer, QueryResultList queryResultList,
									QueryResultTableModifiedList modifiedObjList, 
									IChildModifier childModifier, 
									HashMap<String, String> columnNameMap) {
		this.reflectHelper = reflectHelper;
		this.queryModel = queryModel;
		this.tableViewer = tableViewer;
		this.queryResultList = queryResultList;
		this.modifiedObjList = modifiedObjList;
		this.childModifier = childModifier;
		this.columnMap = columnNameMap;
	}

	public boolean canModify(Object element, String property1) 
	{
		String completeFieldName = columnMap.get(property1);
		QueryResultRow row = (QueryResultRow)element;
		return row.getIsCellModifiable(queryResultList.getFieldIndex(completeFieldName));		
	}

	public Object getValue(Object element, String property) 
	{
		if(element instanceof QueryResultRow)
		{
			String completeFieldName = columnMap.get(property);
			QueryResultRow row = (QueryResultRow)element;
			Object obj = row.getValue(queryResultList.getFieldIndex(completeFieldName));
			int type = getDataType(completeFieldName);
			if(type == QueryBuilderConstants.DATATYPE_BOOLEAN){
				return getIndex(obj);
			}else if(type == QueryBuilderConstants.DATATYPE_DATE_TIME)
				return obj;
			return obj.toString();
		}
		return "";
	}
	
	private Object getIndex(Object value){
		if(value.equals(BOOLEAN_VALUES[1]))
			value = 1;
		return 0;
	}

	public void modify(Object element, String property, Object value) 
	{
		String completeFieldName = columnMap.get(property);
		int datatype = getDataType(completeFieldName);
		if(!validateEditing(completeFieldName, datatype, value) 
			|| ( datatype != QueryBuilderConstants.DATATYPE_STRING
			&&	value.toString().equals(OMPlusConstants.NULL_VALUE)) )
			return;
		
		TableItem item = (TableItem) element;
		QueryResultRow row = (QueryResultRow) item.getData();
		if(datatype == QueryBuilderConstants.DATATYPE_BOOLEAN){
			Integer i = 0;
			try {
				i = new Integer(value.toString());
			}catch(NumberFormatException ex){
			}
			value = BOOLEAN_VALUES[i];
		}
		// handled update in updatevalue()
		Object parentObj = row.updateValue(completeFieldName, value, queryResultList);
		if(parentObj != null) {
			//Value modified so enable the Save button.
			childModifier.objectTableModified(row.getResultObj());
		
			//			ModifiedList should come here
			if(modifiedObjList != null)
				modifiedObjList.addObjectToList(parentObj);
			tableViewer.refresh();		
		}
	}
		
	/**
	 * Validate if editing is to be allowed
	 * @param element
	 * @param value
	 * @return
	 */
	private boolean validateEditing(String property, int datatype, Object value)
	{
		switch(datatype)
		{
			case QueryBuilderConstants.DATATYPE_CHARACTER:
				return validateCharacter(value);
			/*case QueryBuilderConstants.DATATYPE_BOOLEAN:
				return validateBoolean(value);	*/		
			case QueryBuilderConstants.DATATYPE_NUMBER:
				return validateNumber(property,value);					
			case QueryBuilderConstants.DATATYPE_DATE_TIME:
				return (value instanceof Date);
			default://Everything else is String
				return true;
		}		
	}
	
	private int getDataType(String fieldName){
		return reflectHelper.getFieldTypeClass(fieldName);
	}
	

	/**
	 * Validate Numeric values acc to their correct datatype
	 * 
	 * @param element
	 * @param value
	 * @return
	 */
	@SuppressWarnings("unused")
	// FIXME validation code seems to be duplicated in QueryBuilderValueEditor and ObjectTreeEditor
	private boolean validateNumber(String property, Object value) 
	{
		
		int numericDatatype = reflectHelper.getNumberType(property);
		switch (numericDatatype)
		{
			case QueryBuilderConstants.DATATYPE_BYTE:
				try
				{
					
					Byte byte1 = new Byte(value.toString());
				}
				catch(NumberFormatException e)
				{
					err().error("Invalid byte value: " + value, e);
					return false;
				}
				break;
	
			case QueryBuilderConstants.DATATYPE_SHORT:
				try
				{
					Short byte1 = new Short(value.toString());
				}
				catch(NumberFormatException e)
				{
					err().error("Invalid short value: " + value, e);
					return false;
				}
				break;
			case QueryBuilderConstants.DATATYPE_INT:
				try
				{
					Integer byte1 = new Integer(value.toString());
				}
				catch(NumberFormatException e)
				{
					err().error("Invalid integer value: " + value, e);
					return false;
				}
				break;
			case QueryBuilderConstants.DATATYPE_LONG:
				try
				{
					Long byte1 = new Long(value.toString());
				}
				catch(NumberFormatException e)
				{
					err().error("Invalid long value: " + value, e);
					return false;
				}
				break;
			case QueryBuilderConstants.DATATYPE_DOUBLE:
				try
				{
					Double byte1 = new Double(value.toString());
				}
				catch(NumberFormatException e)
				{
					err().error("Invalid double value: " + value, e);
					return false;
				}
			case QueryBuilderConstants.DATATYPE_FLOAT:	
				try
				{
					Float byte1 = new Float(value.toString());
				}
				catch(NumberFormatException e)
				{
					err().error("Invalid float value: " + value, e);
					return false;
				}						
		}	
		return true;
	}
	
	private ErrorMessageHandler err()
	{
		return queryModel.err();
	}

	/**
	 * Validate boolean values
	 * @param value
	 * @return
	 */
	@SuppressWarnings("unused")
	private boolean validateBoolean(Object value)
	{
		//LOGIC: Boolean doesnt throw parseException if string is not true/false. Hence check it yourself
		if ("true".equalsIgnoreCase(value.toString()) ||
			"false".equalsIgnoreCase(value.toString()))
		{
			return true;
		}
		else
		{
			err().error("Invalid boolean string: " + value);
			return false;
		}
			
	}
	
	/**
	 * Validate character values
	 * @param value
	 * @return
	 */
	private boolean validateCharacter(Object value)
	{
		
		if (value.toString().length()==1)
		{
			return true;
		}
		else
		{
			err().error("Invalid character value: " + value);
			return false;
		}			
	}
}
