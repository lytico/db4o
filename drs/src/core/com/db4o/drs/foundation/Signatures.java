/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.foundation;

import java.util.*;

public class Signatures {
	
	private final Map<Signature, Long> _loidBySignature = new HashMap<Signature, Long>();

	private final Map<Long, Signature> _signatureByLoid = new HashMap<Long, Signature>();

	public void add(Signature signature, long signatureLoid) {
		_loidBySignature.put(signature, signatureLoid);
		_signatureByLoid.put(signatureLoid, signature);
	}
	
	public Long loidForSignature(Signature signature){
		return _loidBySignature.get(signature);
	}
	
	public Signature signatureForLoid(long signatureLoid){
		return _signatureByLoid.get(signatureLoid);
	}

}
