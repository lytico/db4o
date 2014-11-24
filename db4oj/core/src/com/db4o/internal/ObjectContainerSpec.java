/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;



/**
 * Workaround to provide the Java 5 version with a hook to add ExtObjectContainer.
 * (Generic method declarations won't match ungenerified YapStreamBase implementations
 * otherwise and implementing it directly kills .NET conversion.)
 * 
 * @exclude
 */
@decaf.IgnoreImplements(platforms={decaf.Platform.JDK11, decaf.Platform.JDK12})
public interface ObjectContainerSpec extends InternalObjectContainer {
}
