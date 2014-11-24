package com.db4o.internal;

public interface EventDispatcher {

	boolean dispatch(Transaction trans, Object obj, int eventID);

	boolean hasEventRegistered(int eventID);

}