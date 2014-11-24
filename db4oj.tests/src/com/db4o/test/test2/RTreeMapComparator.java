/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.test2;

import java.util.*;

import com.db4o.*;
import com.db4o.test.*;
import com.db4o.test.types.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class RTreeMapComparator extends RMap{

	TEntry entry(){
		return new IntEntry();
	}

	public Object newInstance(){
		return new TreeMap(new CustomComparator());
	}

	public void specific(ObjectContainer con, int step){
		if(step > 0){
			int foundComparators = 0;
			ObjectSet set = con.queryByExample(new TreeMap());
			while(set.hasNext()){
				TreeMap tm = (TreeMap)set.next();
				Comparator cmp = tm.comparator();
				if(cmp != null){
					if(cmp instanceof CustomComparator){
						foundComparators ++;
						if(foundComparators >= step){
							return;
						}
					}
				}
			}
			Regression.addError("RTreeMapComparator comparator lost on the way");
		}
	}
}
