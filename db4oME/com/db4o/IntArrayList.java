/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public class IntArrayList {
    
    static final int INC = 20;
    
    protected int[] i_content;
    
    private int i_current;
    private int i_count;
    
    public IntArrayList(){
        this(INC);
    }
    
    public IntArrayList(int initialSize){
        i_content = new int[initialSize];
    }
    
    public void add(int a_value){
        if(i_count >= i_content.length){
            int[] temp = new int[i_content.length + INC];
            System.arraycopy(i_content, 0, temp, 0, i_content.length);
            i_content = temp;
        }
        i_content[i_count++] = a_value;
    }
    
    public int indexOf(int a_value) {
        for (int i = 0; i < i_count; i++) {
            if (i_content[i] == a_value){
                return i;
            }
        }
        return -1;
    }
    
    public int size(){
        return i_count;
    }
    
    public void reset() {
        i_current = i_count - 1;
    }
    
    public boolean hasNext(){
        return i_current >= 0;
    }
    
    public int nextInt(){
        return i_content[i_current --];
    }
    
    public long[] asLong(){
        long[] longs = new long[i_count];
        for (int i = 0; i < i_count; i++) {
            longs[i] = i_content[i]; 
        }
        return longs;
    }

}
