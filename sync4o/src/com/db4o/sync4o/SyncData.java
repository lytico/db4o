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

import java.sql.Timestamp;

import org.apache.commons.codec.binary.Base64;


/**
 * Contains the data that form the contents of a Sync4o SyncItem.
 * 
 * This class abstracts the concept of a SyncItem (which has 
 * completely separate definitions in the client and server Funambol
 * APIs), so that the SyncDb and Synchronizer classes are client/server agnostic.
 */
class SyncData{

  static private final String STANDARD_FORMAT = "base64";
  
  private final SyncKey _key;
  private final Timestamp _timestamp;
  private final char _state;
  private final byte[] _data;
  
  public SyncData(SyncKey key, byte[] data, Timestamp ts, char state){
  
    if ((key == null) || (data == null) || !Base64.isArrayByteBase64(data) || (ts == null)){
      
      throw new IllegalArgumentException();
      
    }
    
    _key = key;
    _timestamp = (Timestamp) ts.clone();
    _state = state;
    _data = data;
    
  }

  public byte[] getContent(){
    
    return (byte[]) _data.clone();
    
  }
  
  public char getState(){
  
    return _state;
  
  }
  
  public Timestamp getTimestamp(){
  
    return _timestamp;
  
  }

  public String getFormat(){
    
    return STANDARD_FORMAT;
    
  }

  
  public SyncKey getKey(){
  
    return _key;
  }
}
