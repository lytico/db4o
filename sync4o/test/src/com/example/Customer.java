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
package com.example;

import java.util.Date;

/** 
 * Sample class used by sync4o tests.
 */
public class Customer {
  
  private int _customerId;
  private String _name;
  private String _location;
  private Date _lastUpdate;

  public Customer(int customerId, String name, String location) {
    
    _customerId = customerId;
    _name = name;
    _location = location;
    _lastUpdate = new Date();
  }
  
  public String getName() {
  
    return _name;
  }
 
  public void setName(String name) {
  
    _name = name;
    rememberUpdate();
  }

  public int getCustomerId() {
  
    return _customerId;
  }
  
  public void setLocation(String location) {

    _location = location;
    rememberUpdate();
  }

  public String getLocation() {

    return _location;
  }
 
  public Date getLastUpdate() {
    return _lastUpdate;
  }

  private void rememberUpdate() {
    setLastUpdate(new Date());
  }
  
  private void setLastUpdate(Date ts) {
    _lastUpdate = ts;
  }
  
  public String toString(){
    
    StringBuilder b = new StringBuilder("Customer: ");
    
    b.append(_customerId);
    b.append(", ");
    b.append(_name);
    b.append(", ");
    b.append(_location);
    b.append(", ");
    b.append(_lastUpdate);
    
    return b.toString();
    
  }
}
