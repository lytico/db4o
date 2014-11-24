/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.KeySpec;
import com.db4o.foundation.KeySpecHashtable4;

abstract class Config4Abstract
{
	protected KeySpecHashtable4 _config;

	private final static KeySpec CASCADE_ON_ACTIVATE=new KeySpec(YapConst.DEFAULT);
    
	private final static KeySpec CASCADE_ON_DELETE=new KeySpec(YapConst.DEFAULT);
    
	private final static KeySpec CASCADE_ON_UPDATE=new KeySpec(YapConst.DEFAULT);

    private final static KeySpec NAME=new KeySpec(null);

	public Config4Abstract() {
		this(new KeySpecHashtable4(10));
	}
	
	protected Config4Abstract(KeySpecHashtable4 config) {
		_config=(KeySpecHashtable4)config.deepClone(this);
	}
	
	public void cascadeOnActivate(boolean flag){
		putThreeValued(CASCADE_ON_ACTIVATE,flag);
	}
	
	public void cascadeOnDelete(boolean flag){
		putThreeValued(CASCADE_ON_DELETE,flag);
	}
	
	public void cascadeOnUpdate(boolean flag){
		putThreeValued(CASCADE_ON_UPDATE,flag);
	}

	protected void putThreeValued(KeySpec spec,boolean flag) {
		_config.put(spec, flag ? YapConst.YES : YapConst.NO);
	}
	
	public int cascadeOnActivate(){
		return cascade(CASCADE_ON_ACTIVATE);
	}
	
	public int cascadeOnDelete(){
		return cascade(CASCADE_ON_DELETE);
	}
	
	public int cascadeOnUpdate(){
		return cascade(CASCADE_ON_UPDATE);
	}

	private int cascade(KeySpec spec) {
		return _config.getAsInt(spec);
	}
	
	abstract String className();
	
	public boolean equals(Object obj){
		return getName().equals(((Config4Abstract)obj).getName());
	}

	public String getName(){
		return _config.getAsString(NAME);
	}
	
	protected void setName(String name) {
		_config.put(NAME,name);
	}
}
