/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package  com.db4o.query;

/**
 * constraint to limit the objects returned upon
 * {@link Query#execute() query execution}.
 * <br><br>
 * Constraints are constructed by calling 
 * {@link Query#constrain Query.constrain()}.
 * <br><br>
 * Constraints can be joined with the methods {@link #and}
 * and {@link #or}.
 * <br><br>
 * The methods to modify the constraint evaluation algorithm may
 * be merged, to construct combined evaluation rules.
 * Examples:
 * <ul>
 *   <li> <code>Constraint#smaller().equal()</code> for "smaller or equal" </li>
 *   <li> <code>Constraint#not().like()</code> for "not like" </li>
 *   <li> <code>Constraint#not().greater().equal()</code> for "not greater or equal" </li>
 * </ul>
 */
public interface Constraint {

    /**
	 * links two Constraints for AND evaluation.
     * @param with the other {@link Constraint}
     * @return a new {@link Constraint}, that can be used for further calls
	 * to {@link #and and()} and {@link #or or()}
     */
    public Constraint and (Constraint with);


    /**
	 * links two Constraints for OR evaluation.
     * @param with the other {@link Constraint}
     * @return a new {@link Constraint}, that can be used for further calls
     * to {@link #and and()} and {@link #or or()}
     */
    public Constraint or (Constraint with);


    /**
     * sets the evaluation mode to <code>==</code>.
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint equal ();


    /**
     * sets the evaluation mode to <code>&gt;</code>.
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint greater ();

    /**
     * sets the evaluation mode to <code>&lt;</code>.
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint smaller ();


    /**
     * sets the evaluation mode to identity comparison.
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint identity ();
	
	
    /**
     * sets the evaluation mode to "like" comparison.
     * <br><br>Constraints are compared to the first characters of a field.<br><br> 
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint like ();
	
	
    /**
     * sets the evaluation mode to containment comparison.
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint contains ();

    /**
     * sets the evaluation mode to string startsWith comparison.
     * @param caseSensitive comparison will be case sensitive if true, case insensitive otherwise
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint startsWith(boolean caseSensitive);

    /**
     * sets the evaluation mode to string endsWith comparison.
     * @param caseSensitive comparison will be case sensitive if true, case insensitive otherwise
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint endsWith(boolean caseSensitive);


    /**
     * turns on not() comparison.
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint not ();
    
    
    /**
     * returns the Object the query graph was constrained with to
     * create this {@link Constraint}.
     * @return Object the constraining object.
     */
    public Object getObject();

}
