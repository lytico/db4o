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
package com.db4o.sync4o.test;

import java.util.Properties;

import com.funambol.syncclient.spdm.SimpleDeviceManager;
import com.funambol.syncclient.spds.*;

public class TestSync{ 

  public static void main(String[] args){
  
  
    try{

      // For JDK 1.1.8 compatibility we cannot use System.setPropery()...
      Properties props = System.getProperties();
      
      // This sets the root of our config tree to be ./config
      props.put(SimpleDeviceManager.PROP_DM_DIR_BASE, "config");
      
      System.setProperties(props);
      
      // the "." argument specifies the current working directory
      // as the root for sync operations
      SyncManager syncManager = SyncManager.getSyncManager(".");
      
      syncManager.sync();
    
    }
    catch (Exception e){
      
      e.printStackTrace();
      System.exit(-1);
      
    }
  } 

}
