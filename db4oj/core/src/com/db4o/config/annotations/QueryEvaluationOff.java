/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config.annotations;

import java.lang.annotation.*;

/**
 * turns query evaluation of specific fields off.
 * <br><br>
 * All fields are evaluated by default.
 * 
 * @exclude
 */
@Documented
@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface QueryEvaluationOff {
}