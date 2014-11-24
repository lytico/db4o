/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import com.db4o.internal.*;
import com.db4o.internal.ids.*;

public class IdSystemHandle {

	private LocalObjectContainer _container;
	private IdSystem _idSystem;

	public IdSystemHandle(LocalObjectContainer container, IdSystem idSystem) {
		_container = container;
		_idSystem = idSystem;
	}
	
	public void close() {
		_idSystem.close();
		_container.close();
	}

	public IdSystem idSystem() {
		return _idSystem;
	}

}
