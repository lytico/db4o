/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import org.polepos.framework.*;

import com.db4o.internal.*;
import com.db4o.internal.ids.*;

public abstract class IdSystemCar extends Car implements IdSystemConfigurator {
	
	public IdSystemCar(Team team){
		super(team, "555555");
	}

	public IdSystem idSystem(LocalObjectContainer container) {
		return container.idSystem();
	}

}
