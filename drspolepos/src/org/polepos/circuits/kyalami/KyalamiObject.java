/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package org.polepos.circuits.kyalami;

import org.polepos.framework.*;


public class KyalamiObject implements CheckSummable{

    public int _val;
    
	public KyalamiObject() {
	}

	public KyalamiObject(int i) {
        _val = i;
    }

    
	public String toString() {
		return "KyalamiObject " +
				"_val='" + _val + '\'';
	}

    public long checkSum() {
        return _val;
    }

    
    public int getVal() {
        return _val;
    }
    
    public void setVal(int val) {
        _val = val;
    }
}
