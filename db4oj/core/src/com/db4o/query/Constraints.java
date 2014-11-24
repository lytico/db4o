/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.query;

/**
 * A set of {@link Constraint} objects.
 * <br><br>This extension of the {@link Constraint} interface allows
 * setting the evaluation mode of all contained {@link Constraint}
 * objects with single calls.
 * <br><br>
 * See also {@link Query#constraints()}.
 */
public interface Constraints extends Constraint
{
	
	/**
	 * returns an array of the contained {@link Constraint} objects.
	 * @return  an array of the contained {@link Constraint} objects.
	 */
	public Constraint[] toArray();
}
