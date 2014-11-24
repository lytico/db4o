/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous.filealgebra;

import java.io.*;

public class TakeFirstSingleFileSource implements SingleFileSource {

	private FileSource _source;
	
	public TakeFirstSingleFileSource(FileSource source) {
		_source = source;
	}
	
	public File file() {
		return _source.files().get(0);
	}

}
