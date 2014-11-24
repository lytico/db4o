/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

class YapFieldNull extends YapField{
    
    public YapFieldNull(){
        super(null);
    }
    
	YapComparable prepareComparison(Object obj){
		return Null.INSTANCE;
	}
	
	Object read(YapWriter a_bytes) {
		return null;
	}
	
	Object readQuery(Transaction a_trans, YapReader a_reader) throws CorruptionException {
		return null;
	}
}
