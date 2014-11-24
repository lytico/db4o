/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations;

import java.lang.annotation.*;

import com.db4o.config.*;

/**
 * generate version numbers for stored objects of this class.
 * @deprecated As of version 8.0 please use {@link FileConfiguration#generateCommitTimestamps(boolean)} instead
 * @exclude
 */
@Documented
@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
@Deprecated
public @interface GeneratedVersionNumbers {
	boolean value() default true;
}
