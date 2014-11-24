/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package com.db4o.drs.test.data;

import com.db4o.activation.*;
import com.db4o.ta.*;

/**
* @exclude
*/
public class ActivatableItem implements Activatable {
	
	private String name;
	
	private transient Activator _activator;
	
	public ActivatableItem(){
		
	}

	public ActivatableItem(String name){
		this.name = name;
	}
	
	public void activate(ActivationPurpose purpose) {
		if(_activator != null){
			_activator.activate(purpose);
		}
	}

	public void bind(Activator activator) {
		_activator = activator;
	}

	public Object name() {
		activate(ActivationPurpose.READ);
		return name;		
	}
	
	public String getName() {
		activate(ActivationPurpose.READ);
		return name;
	}

	public void setName(String name) {
		activate(ActivationPurpose.WRITE);
		this.name = name;
	}
	
}