/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations;

import java.lang.annotation.*;

/**
 * generate UUIDs for stored objects of this class.
 * @exclude
 */
@Documented
@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface GeneratedUUIDs {
	boolean value() default true;
}
