/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.jdk5;

import java.util.*;

@Jdk5Annotation(cascadeOnActivate=true, cascadeOnUpdate=true, maximumActivationDepth=3)
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class Jdk5Data<Item> {
    private Item item;
    // JDK1.5: typesafe enums
    private Jdk5Enum type;
    // JDK1.5: generics
    private List<Integer> list;
    
    public Jdk5Data(Item item,Jdk5Enum type) {
        this.item=item;
        this.type=type;
        list=new ArrayList<Integer>();
    }

    // JDK1.5: varargs
    public void add(int ... is) {
        // JDK1.5: enhanced for with array
        for(int i : is) {
            // JDK1.5: boxing
            list.add(i);
        }
    }
    
    public int getMax() {
        int max=Integer.MIN_VALUE;
        // JDK1.5: enhanced for with collection / unboxing
        
        for(int i : list) {
            max=Math.max(i,max);
        }
        
        return max;
    }
    
    public int getSize() {
        return list.size();
    }
    
    public Item getItem() {
        return item;
    }
    
    public Jdk5Enum getType() {
        return type;
    }
}
