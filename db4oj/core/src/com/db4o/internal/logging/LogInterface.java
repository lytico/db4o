package com.db4o.internal.logging;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.TYPE)
@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public @interface LogInterface {

}
