/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations;

import java.lang.annotation.*;

/**
 * sets the maximum activation depth to the desired value.
 * @exclude
 */
@Documented
@Retention(RetentionPolicy.RUNTIME)
@Target( { ElementType.TYPE })
public @interface MaximumActivationDepth {
	int value();
}