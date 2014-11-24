/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import java.io.*;

import org.polepos.framework.*;

public class IdSystemTeam extends Team {

	private final static String PATH = "data/idsystem.db4o";
	
	// private final IdSystemEngine _engine = new IdSystemEngine(new MemoryStorage(), PATH);
	
	private final IdSystemEngine _engine = new IdSystemEngine(PATH);
	
	@Override
	public Car[] cars() {
		return new Car[] {
			new PointerBasedCar(this),
			new BTreeBasedCar(this),
		};
	}

	@Override
	public String databaseFile() {
		return PATH;
	}

	@Override
	public String description() {
		return "db4o id systems";
	}

	@Override
	public DriverBase[] drivers() {
		return new DriverBase[] {
			new PlainAllocateDriver(_engine),
			new PlainCommitDriver(_engine),
			new PlainLookupDriver(_engine),
		};
	}

	@Override
	public String name() {
		return "db4o id systems";
	}

	@Override
	public String website() {
		return "http://www.db4o.com/";
	}

	@Override
	public void setUp() {
		try {
			_engine.clear();
		} catch (IOException exc) {
			exc.printStackTrace();
		}
	}
}
