/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.fieldindex;

import com.db4o.internal.query.processor.*;

class QEBitmap {
	public static QEBitmap forQE(QE qe) {
    	boolean[] bitmap = new boolean[4];
    	qe.indexBitMap(bitmap);
    	return new QEBitmap(bitmap);
    }
	
	private QEBitmap(boolean[] bitmap) {
		_bitmap = bitmap;
	}
	
	private boolean[] _bitmap;

	public boolean takeGreater() {
		return _bitmap[QE.GREATER];
	}
	
	public boolean takeEqual() {
		return _bitmap[QE.EQUAL];
	}

	public boolean takeSmaller() {
		return _bitmap[QE.SMALLER];
	}
}