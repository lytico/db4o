/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.fileheader;

import com.db4o.internal.*;

/**
 * @exclude
 */
public class FileHeader3 extends FileHeader2 {

	@Override
	public FileHeaderVariablePart createVariablePart(LocalObjectContainer file) {
		return new FileHeaderVariablePart3(file);
	}
	
	@Override
	protected byte version() {
		return (byte) 3;
	};
	
	@Override
	protected NewFileHeaderBase createNew() {
		return new FileHeader3();
	}
	
	@Override
	public FileHeader convert(LocalObjectContainer file) {
		return this;
	}
	
}
