/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.foundation.*;

public class CallbacksTestCase 
{
    static boolean returnValue = true;
    static final Object lock = new Object();
    
	static final int ACTIVATE = 0;
	static final int DEACTIVATE = 1;
	static final int DELETE = 2;
	static final int NEW = 3;
	static final int UPDATE = 4;
	static final int CAN_ACTIVATE = 5;
	static final int CAN_DEACTIVATE = 6;
	static final int CAN_DELETE = 7;
	static final int CAN_NEW = 8;
	static final int CAN_UPDATE = 9;
    
    static boolean[] called = new boolean[CAN_UPDATE + 1];
    
    
    
	public String name;
	public CallbackHelper helper;
	
	public void storeOne(){
	    // helper = new CallbackHelper();
	    // helper.parent = this;
	    name = "stored";
	    noneCalled();
	}
	
	public void testOne(){
	    
	    ObjectContainer oc = Test.objectContainer();
	    
	    ensure(ACTIVATE);
	    ensureNot(DEACTIVATE);
	    ensureNot(DELETE);
	    ensure(NEW);
	    ensureNot(UPDATE);
	    
	    ensure(CAN_ACTIVATE);
	    ensureNot(CAN_DEACTIVATE);
	    ensureNot(CAN_DELETE);
	    ensure(CAN_NEW);
	    ensureNot(CAN_UPDATE);
	    noneCalled();
	    
	    setReturn(false);
	    oc.deactivate(this,3);
	    ensure(CAN_DEACTIVATE);
	    ensureNot(DEACTIVATE);
	    Test.ensure(name.equals("stored"));
	    noneCalled();
	    
	    setReturn(true);
	    oc.deactivate(this,3);
	    ensure(CAN_DEACTIVATE);
	    ensure(DEACTIVATE);
	    Test.ensure(name == null);
	    noneCalled();
	    
	    setReturn(false);
	    oc.activate(this,3);
	    ensure(CAN_ACTIVATE);
	    ensureNot(ACTIVATE);
	    Test.ensure(name == null);
	    noneCalled();
	    
	    setReturn(true);
	    oc.activate(this,3);
	    ensure(CAN_ACTIVATE);
	    ensure(ACTIVATE);
	    Test.ensure(name.equals("stored"));
	    noneCalled();
	    
	    setReturn(false);
	    name = "modified";
	    oc.store(this);
	    ensure(CAN_UPDATE);
	    ensureNot(UPDATE);
	    setReturn(true);
	    oc.ext().refresh(this, 3);
	    Test.ensure(name.equals("stored"));
	    noneCalled();
	    
	    setReturn(true);
	    name = "modified";
	    oc.store(this);
	    ensure(CAN_UPDATE);
	    ensure(UPDATE);
	    oc.ext().refresh(this, 3);
	    Test.ensure(name.equals("modified"));
	    noneCalled();
	    
	    // Test endless loops
	    helper = new CallbackHelper();
	    helper.name = "helper";
	    helper.parent = this;
	    oc.store(this);
	    oc.activate(this, 3);
	    oc.deactivate(this, 3);
	    
	    oc.activate(this, 1);
	    oc.deactivate(this.helper, 1);
	    setReturn(false);
	    noneCalled();
	    oc.activate(this, 3);
	    ensureNot(ACTIVATE);
	    
	    setReturn(true);
	    noneCalled();
	    oc.delete(this);
	    oc.commit();
	    
	    Runtime4.sleep(100);

	    ensure(CAN_DELETE);
	    ensure(DELETE);
	    
	    noneCalled();
	    setReturn(true);
	}
	
	static void setReturn(boolean val){
	    synchronized(lock){
	        returnValue = val;
	    }
	}
	
	static boolean getReturn(){
	    synchronized(lock){
	        return returnValue;
	    }
	}
	
	public boolean objectCanActivate(ObjectContainer container){
	    called[CAN_ACTIVATE] = true;
	    return getReturn();
	}

	public boolean objectCanDeactivate(ObjectContainer container){
	    called[CAN_DEACTIVATE] = true;
	    return getReturn();
	}
	
	public boolean objectCanDelete(ObjectContainer container){
	    called[CAN_DELETE] = true;
	    return getReturn();
	}
	
	public boolean objectCanNew(ObjectContainer container){
	    called[CAN_NEW] = true;
	    return getReturn();
	}
	
	public boolean objectCanUpdate(ObjectContainer container){
	    called[CAN_UPDATE] = true;
	    return getReturn();
	}
	
	public void objectOnActivate(ObjectContainer container){
	    called[ACTIVATE] = true;
	    if(helper != null){
	        container.activate(helper, 3);
	    }
	}
	
	public void objectOnDeactivate(ObjectContainer container){
	    called[DEACTIVATE] = true;
	    if(helper != null){
	        container.deactivate(helper, 3);
	    }
	}
	
	public void objectOnDelete(ObjectContainer container){
	    called[DELETE] = true;
	    if(helper != null){
	        container.delete(helper);
	    }
	}
	
	public void objectOnNew(ObjectContainer container){
	    called[NEW] = true;
	    if(helper != null){
	        container.store(helper);
	    }
	}	
	
	public void objectOnUpdate(ObjectContainer container){
	    called[UPDATE] = true;
	    if(helper != null){
	        container.store(helper);
	    }
	}
	
	private void ensure(int eventPos){
	    Test.ensure(called[eventPos]);
	}
	
	private void ensureNot(int eventPos){
	    Test.ensure(! called[eventPos]);
	}

	
	private void noneCalled(){
	    for (int i = 0; i <= CAN_UPDATE; i++) {
	        called[i] = false;
        }
	}
	
	
}

