package com.db4o.db4ounit.common.io;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public interface ThrowCondition {
	boolean shallThrow(long pos, int numBytes);
}
