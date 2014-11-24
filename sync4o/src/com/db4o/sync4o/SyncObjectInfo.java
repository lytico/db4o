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

import java.io.Serializable;
import java.util.Date;


/**
 * A record that describes the synchronization state for a user-defined object.
 * Instances are stored in a "shadow" synchronization database separate
 * from the "main" database that stores the user-defined objects.
 */
class SyncObjectInfo implements Serializable{

  private static final long serialVersionUID = -6545349386106764810L;

  /**
   * Reference back to parent to make db4o queries work while bugid 1 is open.
   */
  private SyncClassInfo _classInfo;
  
  /**
   * The "primary key" of the object this instance refers to.
   */
  private Object _key;
  
  /**
   * The last time the object this instance refers to was modified.
   */
  private Date _timestamp;
  
  /**
   * The current known synchronization state of the object
   * this instance refers to.
   */
  private char _syncState;

  public SyncObjectInfo(SyncClassInfo classInfo, Object key, Date timestamp, char syncState){
    
    if ((key == null) || (timestamp == null)){
      
      throw new IllegalArgumentException();
      
    }
    
    _classInfo = classInfo;
    _key = key;
    _timestamp = timestamp;
    _syncState = syncState;
    
  }
  
  public SyncClassInfo getClassInfo(){
    
    return _classInfo;
    
  }
  
  public Object getKey(){
  
    return _key;
    
  }

  public Date getTimestamp(){
    
    return _timestamp;
  }
  
  public void setTimestamp(Date timestamp){
  
    _timestamp = timestamp;
  }
  
  public char getSyncState(){
  
    return _syncState;
    
  }
  
  public void setSyncState(char syncState){
  
    _syncState = syncState;
    
  }

}
