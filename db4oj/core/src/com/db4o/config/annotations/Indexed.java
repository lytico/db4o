/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations;

import java.lang.annotation.*;

/**
 * {@code  @Indexed } annotation turns indexing on and can be applied to
 * class fields. Its functionality is equivalent to {@link com.db4o.config.ObjectField#indexed(boolean)}. <br>
 * {@code  @Indexed } annotation is used without any parameters and cannot
 * be used to disable indexing.
 *@see com.db4o.config.ObjectField#indexed(boolean)
 */
@Documented
@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface Indexed {
	
}
