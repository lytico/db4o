/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.test.data;

public class SimpleArrayHolder {
    
    private String name;
    
    private SimpleArrayContent[] arr;
    
    public SimpleArrayHolder() {
        
    }
    
    public SimpleArrayHolder(String name) {
        this.name = name;
    }

    
    public SimpleArrayContent[] getArr() {
        return arr;
    }
    
    public void setArr(SimpleArrayContent[] arr) {
        this.arr = arr;
    }
    
    public String getName() {
        return name;
    }
    
    public void setName(String name) {
        this.name = name;
    }
    
    public void add(SimpleArrayContent sac){
        if(arr == null){
            arr = new SimpleArrayContent[]{sac};
            return;
        }
        SimpleArrayContent[] temp = arr;
        arr = new SimpleArrayContent[temp.length + 1];
        System.arraycopy(temp, 0, arr, 0, temp.length);
        arr[temp.length] = sac;
    }
    
    

}
