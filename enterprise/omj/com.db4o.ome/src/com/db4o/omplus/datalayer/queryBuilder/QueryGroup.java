package com.db4o.omplus.datalayer.queryBuilder;

import java.util.ArrayList;
import java.util.ListIterator;

/**
 * Class storing a list of queries
 * @author prameela_nair
 *
 */
public class QueryGroup
{
	/**
	 * List of queries in a group
	 */
	private ArrayList<QueryClause> queryList = new ArrayList<QueryClause>();
	/**
	 * The oerator that should be applied to the QueryGroup 
	 * previous to current group. null if the this is first query group
	 */
	private String groupOperator;
	
	//change this : added one element by default
	public QueryGroup()
	{
/*		QueryClause a = new QueryClause("check "+QueryBuilderConstants.rand.nextInt(50));
		a.setCondition(QueryBuilderConstants.CONSTRAINTS_ARRAY[QueryBuilderConstants.rand.nextInt(3)]);
		a.setOperator(QueryBuilderConstants.OPERATOR_ARRAY[QueryBuilderConstants.rand.nextInt(2)]);
		a.setValue("100");
		queryList.add(a);		
*/	}
	
	public ArrayList<QueryClause> getQueryList() 
	{
		return queryList;
	}
	public void setQueryList(ArrayList<QueryClause> queryList) 
	{
		this.queryList = queryList;
	}
	public String getGroupOperator() {
		return groupOperator;
	}
	public void setGroupOperator(String groupOperator) {
		this.groupOperator = groupOperator;
	}

	@SuppressWarnings("unchecked")
	public void updateData(QueryClause q, int objectIndex) 
	{
		//TODO: Check if this method is right...unnecessarily modifying whole object for
		//one column change
		
		/*Iterator iterator = queryList.iterator();
		while(iterator.hasNext())
		{
			QueryClause existing = (QueryClause)iterator.next();
			if(existing.getField().equals(q.getField()))
			{
				existing.setCondition(q.getCondition());
				existing.setOperator(q.getOperator());
				existing.setValue(q.getValue());
				break;
			}
		}*/
		QueryClause existing = queryList.get(objectIndex);
		existing.setCondition(q.getCondition());
		existing.setOperator(q.getOperator());
		existing.setValue(q.getValue());		
	}

	/**
	 * Deelete clauses specified by the clauseIndices
	 * @param clauseIndices
	 */
	public void deleteClause(int[] clauseIndices) 
	{
		for(int i = clauseIndices.length-1; i >=0 ; i--)
		{
			queryList.remove(clauseIndices[i]);
		}
		
	}
	
	/**
	 * Reset all the operators to the first Clause operator 
	 */
	public void resetAllOperators()
	{
		if(queryList.size() > 1)
		{
			QueryClause clause = queryList.get(0);
			for(int i = 1; i < queryList.size(); i++)
			{
				QueryClause toChangeClause = queryList.get(i);
				toChangeClause.setOperator(clause.getOperator());
			}
		}
	}
	
	public String toString(){
		StringBuilder sb = new StringBuilder();
		int size = queryList.size();
		int count = 0;
		if(size > 0) {
			ListIterator<QueryClause> iterator = queryList.listIterator();
			sb.append("(");
			while(iterator.hasNext()) {
				QueryClause clause = ((QueryClause)iterator.next());
				sb.append(clause.toString());
				if(size > 1 && count < size - 1 )
					sb.append(clause.getOperator());
				count++;
			}
			sb.append(")");
		}
		return sb.toString();
	}
	

}
