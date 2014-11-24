/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com

This file is part of the open source sync4o connector written to enable
the Funambol data synchronization client and server to support the
db4o object database.

sync4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

sync4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.sync4o;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;

import org.apache.commons.codec.binary.Base64;

/**
 * Contains the data that form the contents of a Sync4o SyncItemKey.
 * 
 * This class abstracts the concept of a SyncItemKey (which has 
 * completely separate definitions in the client and server Funambol
 * APIs), so that the SyncDb and Synchronizer classes are client/server agnostic.
 */
public class SyncKey implements Serializable{

  private final String _classname;
  private final Object _key;
  
  static public String toEncodedString(SyncKey key) throws Exception{
      
    ByteArrayOutputStream bs = new ByteArrayOutputStream();
    ObjectOutputStream os = new ObjectOutputStream(bs);
    os.writeObject(key);
    os.flush();
    os.close();

    return new String(Base64.encodeBase64(bs.toByteArray()));
    
  }
  
  static public SyncKey fromEncodedString(String s) throws Exception{
    
    ObjectInputStream ois = new ObjectInputStream(new ByteArrayInputStream(Base64.decodeBase64(s.getBytes())));
    SyncKey key = (SyncKey) ois.readObject();
    ois.close();

    return key;
    
  }
  
  /**
   * Completely initialize a new instance.
   * 
   * @param classname The name of the class referred to by the key.
   * @param key The key contents.
   */
  public SyncKey(String classname, Object key){
    
    if (!isValidClassname(classname) || !isValidKey(key)){
      
      throw new IllegalArgumentException();
      
    }
    
    _classname = classname;
    _key = key;
    
  }
  
  /**
   * @return The name of the class referred to by this key.
   */
  public String getClassname(){
  
    return _classname;
  
  }
  
  /**
   * @return The real key contents.
   */
  public Object getKey(){
  
    return _key;

  }
 
  public boolean equals(Object o){

    boolean equal = false;
    if ((o != null) && (o instanceof SyncKey)){
      
      SyncKey key = (SyncKey) o;
      equal = (_classname.equals(key._classname) && _key.equals(key._key));
      
    }
    
    return equal;
    
  }

  public String toString(){

    StringBuilder b = new StringBuilder("Sync4o SyncKey: ");
    
    b.append(_classname);
    b.append(":");
    b.append(_key.toString());
    
    return b.toString();
  }

  /**
   * Tests a class name for validity.
   * 
   * @param classname The class name to be tested. 
   * @return true if the class name is valid, else false.
   */
  static private boolean isValidClassname(String classname){

    return ((classname != null) && (classname.length() > 0));
    
  }

  /**
   * Tests a key for validity.
   * 
   * @param key The key to be tested.
   * @return true if the key is valid, else false.
   */
  static private boolean isValidKey(Object key){

    return (key != null);
    
  }

  public int hashCode(){

    return _key.hashCode();
    
  }
  
}
