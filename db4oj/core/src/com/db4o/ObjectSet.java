/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package  com.db4o;

import com.db4o.ext.ExtObjectSet;

import java.util.List;

/**
 * An ObjectSet is a representation for a set of objects returned  by a query.
 * <br><br>
 * ObjectSet extends the list interface. It is
 * recommended to never reference ObjectSet directly in code but to use the list interface instead.
 * <br><br>
 * Note that the underlying
 * {@link ObjectContainer ObjectContainer} of an ObjectSet
 * needs to remain open as long as an ObjectSet is used. This is necessary
 * for lazy instantiation. The objects in an ObjectSet are only instantiated
 * when they are actually being used by the application. In case you want to use a query
 * result after the object container has been closed, you need to copy the result set.
 *
 * @see ExtObjectSet for extended functionality.
 * 
 */
@decaf.IgnoreImplements(value=decaf.Platform.JDK11, interfaces={List.class})
public interface ObjectSet<T> extends List<T>, Iterable<T> {
	
	
	/**
     * Returns an ObjectSet with extended functionality.
     * <br><br>
     * Every ObjectSet that db4o provides can be casted to
     * an ExtObjectSet. This method is supplied for your convenience
     * to work without a cast.
     * <br><br>
     * The ObjectSet functionality is split to two interfaces
     * to allow newcomers to focus on the essential methods.
     */
    public ExtObjectSet ext();
	
	
    /**
	 * Returns true if the ObjectSet has more elements.
	 *
     * @return boolean - true if the ObjectSet has more
	 * elements.
     */
    public boolean hasNext ();

    /**
	 * Returns the next object in the ObjectSet.
	 * <br><br>
	 * Before returning the object, next() triggers automatic activation of the
	 * object with the respective
	 * {@link com.db4o.config.CommonConfiguration#activationDepth global} or
	 * {@link com.db4o.config.ObjectClass#maximumActivationDepth class specific}
	 * setting.
     * <br><br>
     * @return the next object in the ObjectSet.
     */
    public T next ();

    /**
	 * Resets the ObjectSet cursor before the first element.
     * A subsequent call to {@link #next()} will return the first element.
     */
    public void reset ();

    /**
	 * Returns the number of elements in the ObjectSet.
     * @return the number of elements in the ObjectSet.
     * 
     * @sharpen.ignore
     */
    public int size ();
}