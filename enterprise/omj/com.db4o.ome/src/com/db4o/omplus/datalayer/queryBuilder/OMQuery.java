package com.db4o.omplus.datalayer.queryBuilder;

import java.util.ArrayList;

import com.db4o.omplus.datalayer.OMPlusConstants;

/**
 * A class that stores a list of all querygroups which would finally make the
 * entire constraint
 */

@SuppressWarnings("unchecked")
public class OMQuery 
{
	/**
	 * List of QueryGroups
	 */
	private ArrayList<QueryGroup> constraintList = new ArrayList<QueryGroup>();
	
	private String [] attributeList;
	
	private String className;

	public ArrayList<QueryGroup> getConstraintList()
	{
		return constraintList;
	}

	public void setConstraintList(ArrayList<QueryGroup> constraintList) 
	{
		this.constraintList = constraintList;
	}
	
	public void initQueryGroup(int index)
	{
		QueryGroup q = new QueryGroup();
		constraintList.add(index, q);
	}
	
//	TODO Alreadyimplemented this in QueryParser which has to be removed 
//	 
	public String getQueryClass(){
		if(className != null)
			return className;
		if(constraintList !=  null && constraintList.size()>0)
		{
			QueryGroup qg = constraintList.get(0);
			if(qg != null && qg.getQueryList().size()>0)
			{
				QueryClause clause = qg.getQueryList().get(0);
				if (clause != null){
					String str = clause.getField();
					return str.split(OMPlusConstants.REGEX)[0];
				}
			}
		}
		return null;
	}
	
	// Only called for ViewAllobjects
	public void setQueryClass(String cname) {
		if(cname != null && cname.trim().length() > 0)
			className = cname;
	}
	
	public QueryGroup getQueryGroup(int index)
	{
		return constraintList.get(index);
	}

	/*public void updateQuery(QueryClause query, int index) 
	{
		QueryGroup  queryGroup = getQueryGroup(index);
		queryGroup.updateData(query);		
	}*/
	
//	TODO Required for RecentQuery to display as a List Item or equivalent.
	public String toString()
	{
		StringBuilder sb = new StringBuilder();
		int size = constraintList.size();
		if(size > 0) 
		{
			for(int i = 0; i < size; i++)
			{
				QueryGroup group = ((QueryGroup)constraintList.get(i));
				sb.append(group.toString());
				if(size != (i+1))
					sb.append(((QueryGroup)constraintList.get(i+1)).getGroupOperator());
			}
		}
		return sb.toString();
	}
	
	public void updateOperatorForGroup(int index, int selectionIndex) 
	{
		QueryGroup  queryGroup = getQueryGroup(index);
		queryGroup.setGroupOperator(QueryBuilderConstants.OPERATOR_ARRAY[selectionIndex]);
	}

	public void addNewQuery(int index, QueryClause q) 
	{
		QueryGroup  queryGroup = getQueryGroup(index);
		queryGroup.getQueryList().add(q);
	}

	public void deleteDataFromGroup(int groupIndex, int rowIndex[])
	{
		QueryGroup queryGroup = getQueryGroup(groupIndex);
		queryGroup.deleteClause(rowIndex);
		
	}
	
	public void removeQueryGroup(int index) 
	{
		constraintList.remove(index);
		
	}

	public String [] getAttributeList() {
		return attributeList;
	}

	public void setAttributeList(String [] attributeList) {
		this.attributeList = attributeList;
	}

}
