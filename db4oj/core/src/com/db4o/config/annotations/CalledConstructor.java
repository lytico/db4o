/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations;

import java.lang.annotation.*;

/**
 * switches calling constructors on <br> <br>
 * @exclude
 */
@Documented
@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface CalledConstructor {
	boolean value() default true;
}
