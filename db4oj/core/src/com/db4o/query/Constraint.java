/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package  com.db4o.query;

/**
 * Constraint to limit the objects returned upon
 * {@link Query#execute() query execution}.
 * <br><br>
 * Constraints are constructed by calling 
 * {@link Query#constrain(Object)}.
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
	 * Links two Constraints for AND evaluation.
	 * For example:<br>
	 * <pre class="prettyprint"><code> query.constrain(Pilot.class);
	 * query.descend("points").constrain(101).smaller().and(query.descend("name").constrain("Test Pilot0"));</code></pre><br>
	 * will retrieve all pilots with points less than 101 and name as "Test Pilot0"<br>
     * @param with the other {@link Constraint}
     * @return a new {@link Constraint}, that can be used for further calls
	 * to {@link #and(Constraint)} and {@link #or(Constraint)}
     */
    public Constraint and(Constraint with);


    /**
	 * Links two Constraints for OR evaluation.
	 * For example:<br><br>
	 * <pre class="prettyprint"><code> query.constrain(Pilot.class);
	 * query.descend("points").constrain(101).greater().or(query.descend("name").constrain("Test Pilot0"));</code></pre><br>
	 * will retrieve all pilots with points more than 101 or pilots with the name "Test Pilot0"<br>
     * @param with the other {@link Constraint}
     * @return a new {@link Constraint}, that can be used for further calls
     * to {@link #and(Constraint)} and {@link #or(Constraint)}
     */
    public Constraint or(Constraint with);


    /**
     * Used in conjunction with {@link #smaller()} or {@link #greater()} to create constraints
     * like "smaller or equal", "greater or equal".
     * For example:<br>
     * <pre class="prettyprint"><code> query.constrain(Pilot.class);
	 * query.descend("points").constrain(101).smaller().equal();</code></pre><br>
	 * will return all pilots with points &lt;= 101.<br>
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint equal();


    /**
     * Sets the evaluation mode to &gt;.
     * For example:<br>
     * <pre class="prettyprint"><code> query.constrain(Pilot.class);
	 * query.descend("points").constrain(101).greater()</code></pre><br>
	 * will return all pilots with points &gt; 101.<br>
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint greater();

    /**
     * Sets the evaluation mode to &lt;.
     * For example:<br>
     * <pre class="prettyprint"><code> query.constrain(Pilot.class);
	 * query.descend("points").constrain(101).smaller()</code></pre><br>
	 * will return all pilots with points &lt; 101.<br>
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint smaller();


    /**
     * Sets the evaluation mode to identity comparison. In this case only
     * objects having the same database identity will be included in the result set.
     * For example:<br>
     * <pre class="prettyprint"><code> Pilot pilot = new Pilot("Test Pilot1", 100);
	 * Car car = new Car("Ferrari", pilot);
	 * container.store(car);
	 * Car otherCar = new Car("Ferrari", pilot);
	 * container.store(otherCar);
	 * Query query = container.query();
	 * query.constrain(Car.class);
	 * // All cars having pilot with the same database identity
	 * // will be retrieved.
	 * query.descend("pilot").constrain(pilot).identity();</code></pre><br><br>
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint identity();
	
    /**
     * Set the evaluation mode to object comparison (query by example).
     * 
     * @return this to allow the chaining of method calls.
     */
	public Constraint byExample();
	
    /**
     * Sets the evaluation mode to "like" comparison.
     * This is a contains comparison which is case insensitive. This only works on strings. This mode will include
     * all objects having the constrain expression somewhere inside the string field.
     * For example:<br>
     * <pre class="prettyprint"><code> Pilot pilot = new Pilot("Test Pilot1", 100);
	 * container.store(pilot);
	 *  ...
     * query.constrain(Pilot.class);
	 * // All pilots with the name containing "est" will be retrieved
	 * query.descend("name").constrain("est").like();</code></pre><br>
     * @return this to allow the chaining of method calls.
     */
    public Constraint like();
	
	
    /**
     * Sets the evaluation mode to string contains comparison. This only works on strings.
     * The contains comparison is case sensitive.
     * For example:<br>
     * <pre class="prettyprint"><code> Pilot pilot = new Pilot("Test Pilot1", 100);
     * container.store(pilot);
     *  ...
     * query.constrain(Pilot.class);
     * // All pilots with the name containing "est" will be retrieved
     * query.descend("name").constrain("est").contains();</code></pre><br>
     * @see #like() like() for case insensitive string comparison
     * @return this to allow the chaining of method calls.
     */
    public Constraint contains();

    /**
     * Sets the evaluation mode to string startsWith comparison.
     * For example:<br>
     * <pre class="prettyprint"><code> Pilot pilot = new Pilot("Test Pilot0", 100);
     * container.store(pilot);
	 *  ...
     * query.constrain(Pilot.class);
	 * query.descend("name").constrain("Test").startsWith(true);</code></pre><br>
     * @param caseSensitive comparison will be case sensitive if true, case insensitive otherwise
     * @return this to allow the chaining of method calls.
     */
    public Constraint startsWith(boolean caseSensitive);

    /**
     * Sets the evaluation mode to string endsWith comparison.
     * For example:<br>
     * <pre class="prettyprint"><code> Pilot pilot = new Pilot("Test Pilot0", 100);
     * container.store(pilot);
	 *  ...</code><br>
     * query.constrain(Pilot.class);
	 * query.descend("name").constrain("T0").endsWith(false);</code></pre><br>
     * @param caseSensitive comparison will be case sensitive if true, case insensitive otherwise
     * @return this to allow the chaining of method calls.
     */
    public Constraint endsWith(boolean caseSensitive);


    /**
     * Turns on not() comparison. All objects not full filling the constrain condition will be returned.
     * For example:<br>
     * <pre class="prettyprint"><code> Pilot pilot = new Pilot("Test Pilot1", 100);
     * container.store(pilot);
	 *  ...
     * query.constrain(Pilot.class);
	 * query.descend("name").constrain("t0").endsWith(true).not();</code></pre><br>
     * @return this {@link Constraint} to allow the chaining of method calls.
     */
    public Constraint not();
    
    
    /**
     * Returns the Object the query graph was constrained with to
     * create this {@link Constraint}.
     * @return Object the constraining object.
     */
    public Object getObject();

}
