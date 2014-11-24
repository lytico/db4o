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
import com.db4o.ext.ExtObjectContainer;
import com.db4o.ext.ObjectInfo;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;
import com.funambol.framework.core.Sync4jException;


/**
 * Represents a stored class (in a db4o database) that will be synchronized by
 * sync4o.
 */
final class SyncClass{

  private ExtObjectContainer _db;
  
  private SyncClassConfig _config;
  
  private ReflectClass _class;
  
  private Class _realClass;

  private ReflectField _uniqueField;

  public SyncClass(SyncClassConfig config, ExtObjectContainer db) throws Sync4jException{

    _db = db;
    _db.configure().generateVersionNumbers(Integer.MAX_VALUE);
    _config = config;
    _class = _db.ext().reflector().forName(config.getClassName());
    if (_class == null){
      
      String s = String.format("Could not obtain a ReflectClass for class %1s in database %2s.",
          new Object[] {config.getClassName(), db.toString()});
      throw new Sync4jException(s);
      
    }

    _uniqueField = _class.getDeclaredField(config.getUniqueField());
    if (_uniqueField == null){
      
      String s = String.format(
          "Could not obtain a StoredField for key field %1$ on class %2$ in database %3$.",
          new Object[] {config.getUniqueField(), _class.getName(), db.toString()});
      throw new Sync4jException(s);
    }
    _uniqueField.setAccessible();

    try{
      
      _realClass = Class.forName(_class.getName());
      
    }
    catch (ClassNotFoundException e){
      
      throw new Sync4jException("Could not locate class to be synchronized.", e);
      
    }
    
  }  
  
  public SyncClassConfig getConfig(){
  
    return _config;
  }

  public Object extractKey(Object o){
    
    return _uniqueField.get(o);
 
  }
  
  public Date extractTimestamp(Object o){

    ObjectInfo info = _db.getObjectInfo(o);
    // this bitshift rule comes from com.db4o.foundation.TimeStampIdGenerator
    Date d = new Date(info.getVersion() >> 15);
    return d;
 
  }
  
  public boolean isInstance(Object o){
    
    return getRealClass().isInstance(o);
    
  }
  
  public Class getRealClass(){
  
    return _realClass;
    
  }
    
}
