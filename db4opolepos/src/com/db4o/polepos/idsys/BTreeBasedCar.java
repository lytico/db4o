/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import org.polepos.framework.*;

import com.db4o.config.*;


public class BTreeBasedCar extends IdSystemCar {
	
	public BTreeBasedCar(Team team){
		super(team);
	}


	@Override
	public String name() {
		return "btree-based id system";
	}

//	@Override
//	public IdSystem idSystem(LocalObjectContainer container) {
//		return new BTreeIdSystem(container, new InMemoryIdSystem(container));
//	}

	public void configure(IdSystemConfiguration config) {
		config.useStackedBTreeSystem();
	}

}
