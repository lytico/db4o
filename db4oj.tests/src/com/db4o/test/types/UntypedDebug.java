/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class UntypedDebug extends RTest
{

    public Object[] oObject;
	public Object[] nObject;

	public void set(int ver){
		if(ver == 1){
			
			oObject = new ObjectSimplePublic[]{new ObjectSimplePublic("so"), null, new ObjectSimplePublic("far"), new ObjectSimplePublic("O.K.")};
			nObject = null;
		}else{
		oObject = new ObjectSimplePublic[]{new ObjectSimplePublic("works"),  new ObjectSimplePublic("far"), new ObjectSimplePublic("excellent")};
			nObject = new ObjectSimplePublic[]{};
		}
	}
}
