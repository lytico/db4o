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

import java.util.Date;
import java.util.List;

import com.db4o.Db4o;
import com.db4o.ObjectContainer;
import com.db4o.query.Query;
import com.example.Customer;

/** 
 * Utility that allows test databases to be easily manipulated and viewed.
 */
public class DataGenerator{

  static public void main(String[] args){
  
    if (args.length == 0){
      System.out.println("Usage error: please refer to source for correct usage.");
      System.exit(-1);
    }
    
    String action = args[0].toLowerCase().trim();
    if (action.equals("-create")){
        create(Integer.parseInt(args[1]), Integer.parseInt(args[2]), args[3]);
    }
    else if (action.equals("-delete")){
        delete(Integer.parseInt(args[1]), Integer.parseInt(args[2]), args[3]);
    }
    else if (action.equals("-update")){
        update(Integer.parseInt(args[1]), Integer.parseInt(args[2]), args[4], args[3]);
    }        
    else if (action.equals("-list")){
        list(args[1]);
    }
    else{
        System.out.println("Usage error: please refer to source for correct usage.");
        System.out.println("Arg was" + args[0]);
        System.exit(-1);
    }
    
    System.exit(0);
    
  }
  
  static private void list(String filename){

    System.out.println("Listing records...");
    ObjectContainer db = Db4o.openFile(filename);
    List l = db.query(Customer.class);
    for (int i = 0; i < l.size(); i++){
      
      Customer c = (Customer) l.get(i);
      long version = db.ext().getObjectInfo(c).getVersion();
      Date lastModified = new Date(version >> 15);
      System.out.println(c + " lastMod: " + lastModified);
      
    }
    
    System.out.println(l.size() + " records");
    
    db.close();
  }
  
  static private void create(int startId, int count, String filename){
    
    System.out.println("Generating " + count + " records...");
    ObjectContainer db = Db4o.openFile(filename);
    db.ext().configure().generateVersionNumbers(Integer.MAX_VALUE);

    for (int i = startId; i < (startId + count); i++){
      
      String s = Integer.toString(i);
      Customer c = new Customer(i, "Name " + s, "Location " + s);
    
      db.ext().set(c, Integer.MAX_VALUE);
      System.out.print(".");
      
    }

    System.out.println("Done.");
    db.commit();
    db.close();

  }
  
  static private void delete (int startId, int count, String filename){

    System.out.println("Deleting " + count + " records, starting at " + startId + "...");
    ObjectContainer db = Db4o.openFile(filename);
    db.ext().configure().generateVersionNumbers(Integer.MAX_VALUE);
    
    for (int i = startId; i < (startId + count); i++){
      
      Integer id = new Integer(i);
      Query q = db.query();
      q.constrain(Customer.class);
      q.descend("_customerId").constrain(id);
      List l = q.execute();
      if (l.size() != 1){
        
        System.out.println("Could not locate record with id " + i);
        
      }
      else{
        
        db.delete(l.get(0));
        System.out.print(".");
        
      }
      
    }

    System.out.println("Done.");
    db.commit();
    db.close();
    
  }
  
  static private void update(int startId, int count, String filename, String newName){

    System.out.println("Updating " + count + " records, starting at " + startId + "...");
    ObjectContainer db = Db4o.openFile(filename);
    db.ext().configure().generateVersionNumbers(Integer.MAX_VALUE);
    
    for (int i = startId; i < (startId + count); i++){
      
      Integer id = new Integer(i);
      Query q = db.query();
      q.constrain(Customer.class);
      q.descend("_customerId").constrain(id);
      List l = q.execute();
      if (l.size() != 1){
        
        System.out.println("Could not locate record with id " + i);
        
      }
      else{
        
        Customer c = (Customer) l.get(0);
        c.setName(newName);
        db.ext().set(c, Integer.MAX_VALUE);
        System.out.print(".");
        
      }
      
    }

    System.out.println("Done.");
    db.commit();
    db.close();
    
  }
  
}
