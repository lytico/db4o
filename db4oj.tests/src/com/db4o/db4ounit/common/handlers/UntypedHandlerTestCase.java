/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;



public class UntypedHandlerTestCase extends TypeHandlerTestCaseBase {

    public static void main(String[] args) {
        new UntypedHandlerTestCase().runSolo();
    }
    
    public static class Item  {
        
        public Object _member;
        
        public Item(Object member) {
            _member = member;
        }
        
        public boolean equals(Object obj) {
            if(obj == this){
                return true;
            }
            if (!(obj instanceof Item)) {
                return false;
            }
            Item other = (Item)obj;
            if(this._member.getClass().isArray()){
                return arraysEquals((Object[])this._member, (Object[])other._member);
            }
            return this._member.equals(other._member);
            
        }
        
        private boolean arraysEquals(Object[] arr1, Object[] arr2){
            if(arr1.length != arr2.length){
                return false;
            }
            for (int i = 0; i < arr1.length; i++) {
                if(! arr1[i].equals(arr2[i])){
                    return false;
                }
            }
            return true;
        }
        
        public int hashCode() {
            int hash = 7;
            hash = 31 * hash + (null == _member ? 0 : _member.hashCode());
            return hash;
        }
        
        public String toString() {
            return "[" + _member + "]";
        }
    }
    
    public void testStoreIntItem() throws Exception{
        doTestStoreObject(new Item(new Integer(3355)));
    }
    
    public void testStoreStringItem() throws Exception{
        doTestStoreObject(new Item("one"));
    }
    
    public void testStoreArrayItem() throws Exception{
        doTestStoreObject(new Item(new String[]{"one", "two", "three"}));
    }

}
