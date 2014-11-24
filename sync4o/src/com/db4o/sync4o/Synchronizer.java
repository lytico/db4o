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
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.Set;

import com.funambol.framework.core.Sync4jException;


/**
 *  Class that contains logic to perform a synchronization
 *  for a single class within a db4o database.
 * 
 *  Responsible for maintaining all state associated with the operation. 
 */
class Synchronizer{

  /** The sync database for synchronization of _db. */
  private SyncDb _syncDb;
  
  /** The class being synchronized. */
  private SyncClass _syncClass;

  /**
   * Initializes the synchronizer, acquiring the resources
   * necessary to carry out synchronization.
   *
   * @param syncDb The source of synchronization data.
   * @param syncClass The configuration data describing the synchronization to be 
   * performed by this instance.
   * @throws Sync4jException if resources cannot be acquired or if
   * an unsupported syncMode is supplied.
   */
  Synchronizer(SyncDb syncDb, SyncClass syncClass)
  throws Sync4jException{
       
    if ((syncDb == null) || (syncClass == null)){
      
      throw new IllegalArgumentException();
      
    }
         
    _syncDb = syncDb;
    _syncClass = syncClass;

    // Update the sync database to reflect the changes
    // that have occurred in _db since the last sync operation
    _syncDb.updateSyncData(_syncClass);      
    
  }

  /**
   * @return The SyncClass that this instance manages synchronization for.
   */
  public SyncClass getSyncClass(){
  
    return _syncClass;
    
  }

  /**
   * Releases any resources used to perform synchronization.
   * 
   * It is a logical error to call any other methods on this
   * instance once close() has been called.
   */
  void close() throws Sync4jException{

    _syncDb.resetSyncData(_syncClass);
    _syncDb = null;
    
  }
  
  void setAllSynchronized() throws Sync4jException{
    
    _syncDb.setState(_syncClass, SyncState.SYNCHRONIZED);
    
  }
  
  void setSynchronized(SyncKey key) throws Sync4jException{
    
    _syncDb.setState(_syncClass, key.getKey(), SyncState.SYNCHRONIZED);
    
  }

  List getAllSyncItemKeys() throws Sync4jException{

    return makeSyncKeys(_syncDb.getAllKeys(_syncClass));

  }

  List getSyncItemsForKeys(List syncKeys) throws Sync4jException{

    List items = new ArrayList(syncKeys.size());
    for (Iterator i = syncKeys.iterator(); i.hasNext();){
      
      items.add(_syncDb.getAsSyncItem(_syncClass, (SyncKey) i.next()));
      
    }
    
    return items;

  }
  
  List getNewSyncItemKeys(Timestamp since, Timestamp until)
      throws Sync4jException{

    return makeSyncKeys(_syncDb.getFilteredKeys(_syncClass, SyncState.NEW, since, until));

  }

  List getDeletedSyncItemKeys(Timestamp since, Timestamp until)
      throws Sync4jException{

    return makeSyncKeys(_syncDb.getFilteredKeys(_syncClass, SyncState.DELETED, since, until));
    
  }

  List getUpdatedSyncItemKeys(Timestamp since, Timestamp until)
      throws Sync4jException{

    return makeSyncKeys(_syncDb.getFilteredKeys(_syncClass, SyncState.UPDATED, since, until));
    
  }

  SyncData getSyncItemFromId(SyncKey syncKey) throws Sync4jException{

    return _syncDb.getAsSyncItem(_syncClass, syncKey);

  }

  void deleteSyncItem(SyncKey syncKey) throws Sync4jException{
    
    _syncDb.delete(_syncClass, syncKey);
    
  }
  
  void removeSyncItem(SyncKey syncKey, Timestamp time) throws Sync4jException{

    _syncDb.remove(_syncClass, syncKey, time);
    
  }

  void removeSyncItem(SyncKey syncKey, Timestamp time, boolean softDelete) throws Sync4jException{
    
    // sync4o does not support soft deletions
    if (!softDelete){
      
      removeSyncItem(syncKey, time);
      
    }

  }

  SyncData updateSyncItem(SyncData syncItem) throws Sync4jException{
   
    return _syncDb.set(_syncClass, syncItem);
    
  }

  SyncData addSyncItem(SyncData syncItem) throws Sync4jException{
    
    return _syncDb.set(_syncClass, syncItem);
    
  }

  List getSyncItemKeysFromTwin(SyncData syncItem) throws Sync4jException{

    if (syncItem == null) {
      
      throw new IllegalArgumentException();
      
    }
    
    List twins = _syncDb.findTwins(_syncClass, syncItem);
    
    List keys = new ArrayList();
    for (Iterator i = twins.iterator(); i.hasNext();){
      
      Object key = _syncClass.extractKey(i.next());
      keys.add(new SyncKey(_syncClass.getRealClass().getName(), key));
      
    }
    
    return keys;

  }

  char getSyncItemStateFromId(SyncKey key) throws Sync4jException{
    
    if (key == null){
      
      throw new IllegalArgumentException();
      
    }
    
    return _syncDb.getState(_syncClass, key.getKey());
    
  }

  SyncData mergeSyncItems(SyncKey serverKey, SyncData clientItem) throws Sync4jException{
    
    if ((serverKey == null) || (clientItem == null)){
      
      throw new IllegalArgumentException();
      
    }
    
    return _syncDb.merge(_syncClass, serverKey, clientItem);
  
  }

  private List makeSyncKeys(Set keys){
    
    List syncKeys = new ArrayList(keys.size());
    for (Iterator i = keys.iterator(); i.hasNext();){
      
      syncKeys.add(makeSyncKey(i.next()));
      
    }
    
    return syncKeys;
    
  }
  
  private SyncKey makeSyncKey(Object keyValue){
    
    return new SyncKey(_syncClass.getRealClass().getName(), keyValue);
    
  }
  
}
