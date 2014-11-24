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

import java.util.Date;
import java.util.Iterator;
import java.util.List;

/**
 * Persistent class that maintains the "shadow" synchronization data
 * for a class being synchronized by sync4o.
 */
class SyncClassInfo{

  private Date _lastSync;
  private SyncClassConfig _config;
  private List _objectInfos;

  SyncClassInfo(SyncClassConfig config, List objectInfos){
    
    if ((config == null) || (objectInfos == null)){
      throw new IllegalArgumentException("SyncClassInfo constructor arguments may not be null.");
    }
    
    _lastSync = null;
    _config = config;
    _objectInfos = objectInfos;
    
  }  
  
  Date getLastSync(){
  
    return _lastSync;
  
  }
  
  void setLastSync(Date lastSync){
  
    _lastSync = lastSync;
  
  }

  SyncClassConfig getConfig(){
  
    return _config;
    
  }

  List getObjectInfos(){
  
    return _objectInfos;
    
  }

  SyncObjectInfo findByKey(Object key){

    SyncObjectInfo found = null;
    
    for (Iterator j = _objectInfos.iterator(); (found == null) && j.hasNext();){
      
      SyncObjectInfo info = (SyncObjectInfo) j.next();
      if (key.equals(info.getKey())){
        
        found = info;
      }
      
    }
    
    return found;
    
  }
   
}
