/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package  com.db4o.query;

import com.db4o.*;


/**
 * handle to a node in a S.O.D.A. query graph.
 * <br><br>
 * A node in the query graph can represent multiple 
 * classes, one class or an attribute of a class.<br><br>The graph 
 * is automatically extended with attributes of added constraints 
 * (see {@link #constrain(java.lang.Object)}) and upon calls to  {@link #descend(java.lang.String)} that request nodes that do not yet exist.
 * <br><br>
 * References to joined nodes in the query graph can be obtained
 * by "walking" along the nodes of the graph with the method 
 * {@link #descend(java.lang.String)}.
 * <br><br>
 * {@link #execute()}
 * evaluates the entire graph against all persistent objects. 
 * <br><br>
 * {@link #execute()} can be called from any {@link Query} node
 * of the graph. It will return an {@link ObjectSet} filled with
 * objects of the class/classes that the node, it was called from,
 * represents.<br><br>
 * <b>Note:<br>
 * {@link Predicate Native queries} are the recommended main query 
 * interface of db4o.</b> 
 */
public interface Query {


    /**
	 * adds a constraint to this node.
	 * <br><br>
	 * If the constraint contains attributes that are not yet
	 * present in the query graph, the query graph is extended
	 * accordingly.
	 * <br><br>
	 * Special behaviour for:
	 * <ul>
	 * <li> class {@link Class}: confine the result to objects of one
	 * class or to objects implementing an interface.</li>
	 * <li> interface {@link Evaluation}: run
	 * evaluation callbacks against all candidates.</li>
	 * </ul>
     * @param constraint the constraint to be added to this Query.
     * @return {@link Constraint} a new {@link Constraint} for this
     * query node or <code>null</code> for objects implementing the 
     * {@link Evaluation} interface.
     */
    public Constraint constrain (Object constraint);

    
    /**
     * returns a {@link Constraints}
     * object that holds an array of all constraints on this node.
     * @return {@link Constraints} on this query node.
     */
    public Constraints constraints();


    /**
	 * returns a reference to a descendant node in the query graph.
	 * <br><br>If the node does not exist, it will be created.
	 * <br><br>
	 * All classes represented in the query node are tested, whether
	 * they contain a field with the specified field name. The
	 * descendant Query node will be created from all possible candidate
	 * classes.
     * @param fieldName path to the descendant.
     * @return descendant {@link Query} node
     */
    public Query descend (String fieldName);


    /**
	 * executes the {@link Query}.
     * @return {@link ObjectSet} - the result of the {@link Query}.
     */
    public ObjectSet execute ();

    
    /**
	 * adds an ascending ordering criteria to this node of
	 * the query graph. Multiple ordering criteria will be applied
	 * in the order they were called.
     * @return this {@link Query} object to allow the chaining of method calls.
     */
    public Query orderAscending ();


    /**
	 * adds a descending order criteria to this node of
	 * the query graph. Multiple ordering criteria will be applied
	 * in the order they were called.
     * @return this {@link Query} object to allow the chaining of method calls.
     */
    public Query orderDescending ();
    
    /**
     * Sort the resulting ObjectSet by the given comparator.
     * 
     * @param comparator The comparator to apply.
     * @return this {@link Query} object to allow the chaining of method calls.
     */
    public Query sortBy(QueryComparator comparator);
    
//    /**
//     * defines a Query node to be represented as a column in the array
//     * returned in every element of the ObjectSet upon query execution. 
//     * @param node the Query node to be represented
//     * @param column the column in the result array 
//     */
//    public void result(Query node, int column);
//
}

