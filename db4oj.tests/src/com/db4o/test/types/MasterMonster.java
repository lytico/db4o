/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import com.db4o.internal.*;
import com.db4o.test.*;

public class MasterMonster extends RTest
{
	public Object[] ooo;

	public void set(int ver){
		Object[] classes = allClassesButThis();
		ooo = new Object[classes.length];
		for(int i = 0;i < classes.length; i++){
			try{
				RTestable test = (RTestable)classes[i];
				if(Platform4.canSetAccessible() || !test.jdk2() ){
					ooo[i] = test.newInstance();
					test.set(ooo[i], ver);
				}
			}catch (Exception e){
				throw new RuntimeException("MasterMonster instantiation failed.");
			}
		}
	}

	Object[] allClassesButThis(){
		Object[] all = Regression.allClasses();
		Object[] classes = new Object[all.length - 1];
		System.arraycopy(all,0,classes,0,all.length - 1);
		return classes;
	}
}
