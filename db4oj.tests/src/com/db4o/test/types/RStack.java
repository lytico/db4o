/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import java.util.*;

import com.db4o.*;
import com.db4o.test.*;

public class RStack extends RVector{

	public Object newInstance(){
		return new Stack();
	}

	public void specific(ObjectContainer con, int step){
		super.specific(con,step);
		TEntry entry = new TEntry().lastElement();
		Stack stack = new Stack();
		if(step > 0){
			stack.addElement(entry.key);
			ObjectSet set = con.queryByExample(stack);
			if(set.size() != step){
				Regression.addError("Stack member query not found");
			}else{
				Stack res = (Stack)set.next();
				if(! (stack.pop().equals(new TEntry().lastElement().key))){
					Regression.addError("Stack order changed.");
				}
			}
		}
	}
}
