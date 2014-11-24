/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.jdk5;

import java.lang.annotation.*;

@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.TYPE)
public @interface Jdk5Annotation {
	boolean cascadeOnActivate() default false;
	boolean cascadeOnUpdate() default false;
	boolean cascadeOnDelete() default false;
	int minimumActivationDepth() default -1;
	int maximumActivationDepth() default -1;
}
