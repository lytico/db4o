/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.convert;

import com.db4o.internal.*;

/**
 * @exclude
 */
public abstract class ConversionStage {
	
	public final static class ClassCollectionAvailableStage extends ConversionStage {
		
		public ClassCollectionAvailableStage(LocalObjectContainer file) {
			super(file);
		}

		public void accept(Conversion conversion) {
			conversion.convert(this);
		}
	}

	public final static class SystemUpStage extends ConversionStage {
		public SystemUpStage(LocalObjectContainer file) {
			super(file);
		}
		public void accept(Conversion conversion) {
			conversion.convert(this);
		}
	}

	private LocalObjectContainer _file;
	
	protected ConversionStage(LocalObjectContainer file) {
		_file = file;
	}

	public LocalObjectContainer file() {
		return _file;
	}

	public int converterVersion() {
		return _file.systemData().converterVersion();
	}
	
    public abstract void accept(Conversion conversion);

}
