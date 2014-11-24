/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.qlin;

import static com.db4o.qlin.QLinSupport.*;

import java.util.*;


/**
 * @sharpen.if !SILVERLIGHT
 */
@decaf.Remove(decaf.Platform.JDK11)
public class Closures extends BasicQLinTestCase {
	
	public static class Closure {
	
	}
	
	public void with(Object obj, Object obj2){
		
	}
	
	public List<Cat> listOf(Cat...cats){
		return null;
	}
	
	public void closureSample(){
		// List<Cat> occamAndZora = occamAndZora();
			
	final Cat cat = prototype(Cat.class);
	
	List<Cat> cats = listOf(new Cat("Zora"), new Cat("Wolke"));
	with(cats, new Closure(){{ cat.feed(); }});
            
            
//            
//            Iterable<Cat> query = occamAndZora();
//            
//            with(db().from(Cat.class).select()).feed();
//        
//        
//            query = occamAndZora();
//            
//            Iterable<Color> colors = map(db().from(Cat.class).select(), cat.color());
//        


		
//		final Cat cat = prototype(Cat.class);
//		List<Cat> occamAndZora = occamAndZora();
//		with(occamAndZora, new Closure { cat.feed() } );
		
		
	}

	
//	   public <T> T with(Iterable<T> withOn){
//	        // magic goes here
//	        return null;
//	    }
//
//	    public <T,TResult> Iterable<TResult> map(Iterable<T> withOn,TResult projection ){
//	        // magic goe here
//	        return null;
//	    }

	

	
	
	
	private void with(List<Cat> occamAndZora, Object closure) {
		
	}


}
