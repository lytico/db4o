/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations;

import java.lang.annotation.*;

/**
 * specifies the updateDepth for this class.
 * <br>
 * <br>
 * The default setting is 0: Only the object passed to
 * ObjectContainer.set(Object) will be updated.
 * 
 * @exclude
 */
@Documented
@Retention(RetentionPolicy.RUNTIME)
@Target( { ElementType.TYPE })
public @interface UpdatedDepth {
	int value() default 0;
}
