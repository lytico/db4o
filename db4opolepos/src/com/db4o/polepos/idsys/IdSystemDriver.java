/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import org.polepos.framework.*;

import com.db4o.internal.*;
import com.db4o.internal.ids.*;

public abstract class IdSystemDriver extends DriverBase {

	private final IdSystemEngine _engine;
	
	private IdSystemHandle _handle;
	
	protected IdSystemDriver(IdSystemEngine engine) {
		_engine = engine;
		_handle = null;
	}
	
	@Override
	public void closeDatabase() {
		close();
	}

	@Override
	public void prepare() {
		close();
		IdSystemCar car = (IdSystemCar)car();
		LocalObjectContainer container = _engine.open(car);
		_handle = new IdSystemHandle(container, car.idSystem(container));
	}
	
	protected IdSystem idSystem() {
		return _handle.idSystem();
	}
	
	private void close() {
		if(_handle == null) {
			return;
		}
		_handle.close();
	}

}
