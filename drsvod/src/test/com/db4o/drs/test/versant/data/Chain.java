/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant.data;

public class Chain {
	
	private int _id;
	
	private Chain _next;
	
	public Chain() {
	}
	
	public Chain(int id, Chain chain) {
		_id = id;
		_next = chain;
	}

	public static Chain newChainWithLength(int length){
		Chain chain = null;
		for (int i = length-1; i >= 0; i--) {
			chain = new Chain(i, chain); 
		}
		return chain;
		
	}
	
	public int length(){
		int length = 0;
		Chain chain = this;
		while(chain != null){
			length++;
			chain = chain._next;
		}
		return length;
	}

}
