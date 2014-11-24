/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.internal.*;
import com.db4o.io.*;


public class IdSystemEngine {
	
	private final String _path;
	private final Storage _storage;
	
	public IdSystemEngine(Storage storage, String path) {
		_storage = storage;
		_path = path;
		new File(path).mkdirs();
	}

	public IdSystemEngine(String path) {
		this(null, path);
	}
	
	public LocalObjectContainer open(IdSystemConfigurator configurator) {
		return (LocalObjectContainer) Db4oEmbedded.openFile(config(configurator), _path);
	}

	public void clear() throws IOException {
		if(_storage == null){
			new File(_path).delete();
			return;
		}
		_storage.delete(_path);
	}
	
	private EmbeddedConfiguration config(IdSystemConfigurator configurator) {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		if(_storage != null){
			config.file().storage(_storage);
		}
		configurator.configure(config.idSystem());
		return config;
	}

}
