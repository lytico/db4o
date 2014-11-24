/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class NtoNParent extends RTest
{
	public NtoNChild[] children;

	public void set(int ver){
		children = new NtoNChild[3];
		for(int i =0; i < 3; i++){
			children[i] = new NtoNChild();
			children[i].parents = new NtoNParent[2];
			children[i].parents[0] = this;
			children[i].parents[1] = new NtoNParent();
			children[i].parents[1].children = new NtoNChild[1];
			children[i].parents[1].children[0] = children[i];
			children[i].name = "ver" + ver + "child" + i;
		}
	}
}
