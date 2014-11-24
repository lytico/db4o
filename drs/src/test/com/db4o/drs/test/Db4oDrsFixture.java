/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.test;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.inside.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.reflect.*;

public class Db4oDrsFixture implements DrsProviderFixture {
	
	protected String _name;
	
	
	// TODO: No need to maintain the database here. It can be in the provider. 
	protected ExtObjectContainer _db;
	
	protected TestableReplicationProviderInside _provider;
	
	protected final File testFile;
	
	private Configuration _config;
	
	static final String RAM_DRIVE_PROPERTY = "db4o.drs.path";
	
	private static final String PATH;
	
	static{
		
		String path = System.getProperty(RAM_DRIVE_PROPERTY);
		
		if(path == null){
			path = ramDrivePath();
		}
		
		if(path == null || path.length() == 0){
			System.out.println("You can tune dRS tests by setting the environment variable ");
			System.out.println(RAM_DRIVE_PROPERTY);
			System.out.println("to your RAM drive.");
			path = ".";
		}
		PATH = path;
	}

	/** @sharpen.remove null */
	private static String ramDrivePath() {
		return System.getenv(RAM_DRIVE_PROPERTY);
	}	
	
	public Db4oDrsFixture(String name) {
		this(name, null);
	}
	
	public Db4oDrsFixture(String name, Reflector reflector) {
		_name = name;
		
		File folder = new File(PATH);
		if (! folder.exists()){
			System.out.println("Path " + PATH + " does not exist. Using current working folder.");
			System.out.println("Check the " + RAM_DRIVE_PROPERTY + " environment variable on your system.");
			folder = new File(".");
		}
		testFile = new File(folder.getPath() + "/drs_cs_" + _name + ".db4o");
		
		if (reflector != null) {
			config().reflectWith(reflector);
		}
	}
	
	public TestableReplicationProviderInside provider() {
		return _provider;
	}

	public void clean() {
		testFile.delete();
		_config = null;
	}

	public void close() {
		_provider.destroy();
		_db.close();
		_provider = null;
	}

	public void open() {
		_db = Db4o.openFile(cloneConfiguration(), testFile.getPath()).ext();
		_provider = Db4oProviderFactory.newInstance(_db, _name);
	}

	private Configuration cloneConfiguration() {
	    return (Configuration) ((DeepClone)config()).deepClone(null);
    }
	
	public Configuration config() {
		if(_config == null) {
			_config = Db4o.newConfiguration();
		}
		return _config;
	}
	
	public void destroy(){
		
	}

}