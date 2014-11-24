package com.db4o.db4ounit.jre5.collections;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class Product extends ActivatableImpl {
	private String _code;
	private String _description;
	
	public Product(String code, String description) {
		_code = code;
		_description = description;
	}
	
	public String code() {
		activate(ActivationPurpose.READ);
		return _code;
	}
	
	public String description() {
		activate(ActivationPurpose.READ);
		return _description;
	}
	
	public boolean equals(Object p) {
		activate(ActivationPurpose.READ);
		
		if (p == null) return false;
		if (p.getClass() != this.getClass()) return false;
		
		Product rhs = (Product) p;
		return  rhs._code == _code;
	}
}
