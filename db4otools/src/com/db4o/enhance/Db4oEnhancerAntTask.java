/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.enhance;

import com.db4o.instrumentation.ant.*;
import com.db4o.nativequery.main.*;
import com.db4o.ta.instrumentation.ant.*;


/**
 * Ant task to enhance class files for db4o.
 */
public class Db4oEnhancerAntTask extends Db4oFileEnhancerAntTask {
    
    private boolean _nq = true;
	private boolean _ta = true;
	private boolean _collections = true;

	public Db4oEnhancerAntTask(){
    }

	/**
	 * @param nq true if native query optimization instrumentation should take place, false otherwise
	 */
    public void setNq(boolean nq) {
    	_nq = nq;
    }
    
	/**
	 * @param ta true if transparent activation/persistence instrumentation should take place, false otherwise
	 */
    public void setTa(boolean ta) {
    	_ta = ta;
    }

	/**
	 * @param collections true if native collections should be instrumented for transparent activation/persistence, false otherwise
	 */
    public void setCollections(boolean collections) {
    	_collections = collections;
    }
    
    public void execute() {
    	if(_nq) {
            add(new NQAntClassEditFactory());
    	}
    	if(_ta) {
            add(new TAAntClassEditFactory(_collections));
    	}
    	super.execute();
    }
}
