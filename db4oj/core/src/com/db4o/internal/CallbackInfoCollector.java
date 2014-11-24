/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal;

public interface CallbackInfoCollector {

	void added(int id);

	void updated(int id);

	void deleted(int id);

}