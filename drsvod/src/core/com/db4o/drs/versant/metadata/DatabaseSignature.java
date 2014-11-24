/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.metadata;

import com.db4o.internal.encoding.*;

public class DatabaseSignature extends VodLoidAwareObject {
	
	private byte[] signature;
	
	public DatabaseSignature() {
	}
	
	public DatabaseSignature(byte[] signature){
		this.signature = signature;
	}
	
	public byte[] signature(){
		return signature;
	}
	
	@Override
	public String toString() {
		return "DatabaseSignature " 
			+ " signature:" 
			+ new LatinStringIO().read(signature);
	}

}
