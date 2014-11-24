/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.cobra;

import com.db4o.drs.versant.*;
import com.db4o.drs.versant.metadata.*;

public class Item extends VodLoidAwareObject{
	
	@Indexed
	private String name;
	
	private long[] longs;
	
	public Item() {
		
	}
	
	public Item(String name){
		this.name = name;
	}
	
	public void setLongs(long [] longs){
		this.longs = longs;
	}
	
	public String getName(){
		return name;
	}
	
	public long[] getLongs() {
		return longs;
	}

}
