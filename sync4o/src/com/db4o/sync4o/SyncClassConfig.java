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

/**
 * Simple container class.
 * 
 * Holds all the data required to be able to synchronize a class stored
 * in a db4o database.
 */
final public class SyncClassConfig implements Serializable {

  private static final long serialVersionUID = 7132118838682873736L;

  /**
   * The name of the class to be synchronized.
   */
  private String _className;

  /**
   * The name of the field in the class that can be used
   * to uniquely identify instances.
   */
  private String _uniqueField;

  /**
   * Default constructor - supports JavaBean XMLEncoder persistence
   * used by Funambol server for SyncSource and its contained classes. 
   */
  public SyncClassConfig() {

  }

  /**
   * @return Returns the class.
   */
  public String getClassName() {

    return _className;
  }

  /**
   * @param className The class to set.
   */
  public void setClassName(String className) {

    _className = className;
  }

  /**
   * @return Returns the uniqueField.
   */
  public String getUniqueField() {

    return _uniqueField;
  }

  /**
   * @param uniqueField The uniqueField to set.
   */
  public void setUniqueField(String uniqueField) {

    _uniqueField = uniqueField;
  }
  
  public String toString(){
    
    StringBuilder b = new StringBuilder();
    
    b.append("SyncClassConfig {");
    b.append(_className + ",");
    b.append(_uniqueField);
    b.append("}");
    
    return b.toString();
    
  }
  
  public boolean equals(Object o) {
    
    if ((o != null) && (o instanceof SyncClassConfig)){
      
      SyncClassConfig c = (SyncClassConfig) o;
      return ((c._className.equals(_className)) &&
          (c._uniqueField.equals(_uniqueField)));
      
    }
    else{
      
      return false;
      
    }
    
  }
  
}
