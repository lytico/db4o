package com.db4o.omplus.ui.listeners.queryBuilder;

import java.util.ArrayList;

import org.eclipse.jface.viewers.TableViewer;
import org.eclipse.jface.viewers.ViewerDropAdapter;
import org.eclipse.swt.dnd.TransferData;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.ReflectHelper;
import com.db4o.omplus.datalayer.queryBuilder.QueryBuilderConstants;
import com.db4o.omplus.datalayer.queryBuilder.QueryClause;
import com.db4o.omplus.datalayer.queryBuilder.QueryGroup;
import com.db4o.omplus.ui.customisedcontrols.queryBuilder.QueryGroupComposite;
import com.db4o.omplus.ui.interfaces.IDropValidator;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;





public class TableDropAdapter extends ViewerDropAdapter
{
	@SuppressWarnings("unused")
	private int tableIndex;
	private TableViewer tableViewer;
	private QueryGroup queryGroup;
	private QueryGroupComposite queryGroupComposite;
	private IDropValidator dropValidator;
	
	public TableDropAdapter(int index,QueryGroup qg, TableViewer t, QueryGroupComposite c, IDropValidator dropValidator) {
		//TODO: Check if this super's call is needed
		super(t);
		tableIndex = index;
		queryGroup = qg;
		tableViewer = t;		
		queryGroupComposite = c;
		
		this.dropValidator = dropValidator;
		
	}

//	@Override  before 2 April 08
	/*public boolean performDrop(Object data)
	{
		String field = null;
		if(data != null){
			field =  data.toString();
			String className = field.split(OMPlusConstants.REGEX)[0];
			if(dropValidator.isDropValid(className)){
				QueryClause q = new QueryClause(field);
				q.setCondition(QueryBuilderConstants.STRING_CONDITION_ARRAY[0]);
				queryGroup.getQueryList().add(q);
				if(queryGroup.getQueryList().size() > 1)
				{
					queryGroup.getQueryList().get(queryGroup.getQueryList().size()-1).
												setOperator(queryGroup.getQueryList().get(0).getOperator());
					q.setOperator(queryGroup.getQueryList().get(0).getOperator());
				}
				else
				{
					queryGroup.getQueryList().get(queryGroup.getQueryList().size()-1).
												setOperator(QueryBuilderConstants.OPERATOR_ARRAY[1]);
					q.setOperator(QueryBuilderConstants.OPERATOR_ARRAY[1]);
				}
				tableViewer.add(q);		
				queryGroupComposite.resetTableHeight();
			}
		}
		return true;
	}*/
	
	public boolean performDrop(Object data)
	{
		String field = null;
		if(data != null){
			field =  data.toString();
			String className = field.split(OMPlusConstants.REGEX)[0];
			if(dropValidator.isDropValid(className)){
				QueryClause q = null;
				boolean updateUI = false;
				String operator = QueryBuilderConstants.OPERATOR_ARRAY[1];
				ArrayList<QueryClause> queryList = queryGroup.getQueryList();
				int queryListSize = queryList.size();
				if(queryListSize > 1)
				{
					operator = (((QueryClause)queryList.get(0)).getOperator());
				}
				else
				{
					operator = QueryBuilderConstants.OPERATOR_ARRAY[1];
				}
				if(field.equals(className)){
					ArrayList<QueryClause> clauses = getPrimitiveFields(className, operator);
					addQueryClause(clauses, queryList);
					updateUI = true;
				}
				else if(reflectHelper().getFieldTypeClass(field) != -1){
					q = newQueryClause(field, operator);
					queryList.add(q);
					addQueryClause(q);
					updateUI = true;
				}
				if(updateUI)
					queryGroupComposite.resetTableHeight();
			}
		}
		return true;
	}

	private ArrayList<QueryClause> getPrimitiveFields(String className, String operator)
	{
		ReflectClass clazz = reflectHelper().getReflectClazz(className);
		ReflectField [] fields = ReflectHelper.getDeclaredFieldsInHierarchy(clazz);
		ArrayList<QueryClause> list = new ArrayList<QueryClause>(fields.length);
		for(ReflectField f : fields){
			clazz = f.getFieldType();
			if(clazz.isPrimitive() || ReflectHelper.isWrapperClass(clazz.getName())){
				QueryClause q = newQueryClause(getFName(className, f.getName()), operator);
				list.add(q);
			}
		}
		return list;
	}

	private String getFName(String className, String name) {
		return new StringBuilder(className).append(OMPlusConstants.REGEX).append(name).toString();
	}

	private void addQueryClause(QueryClause q){
		tableViewer.add(q);tableViewer.getTable().getItems();	
	}
	
	private void addQueryClause(ArrayList<QueryClause> clauses, ArrayList<QueryClause> groupList){
		for(int count = 0; count < clauses.size(); count++){
			tableViewer.add(clauses.get(count));
			groupList.add(clauses.get(count));
		}
				
	}

	private QueryClause newQueryClause(String field, String operator){
		QueryClause q = new QueryClause(field);
		q.setOperator(operator);
		q.setCondition(QueryBuilderConstants.STRING_CONDITION_ARRAY[0]);
		return q;
	}

	@Override
	public boolean validateDrop(Object target, int operation,
			TransferData transferType) 
	{
		return true;
	}

	private ReflectHelper reflectHelper() {
		return Activator.getDefault().dbModel().db().reflectHelper();
	}
}
