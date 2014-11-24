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
import java.security.Principal;

import java.sql.Timestamp;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.Iterator;
import java.util.List;
import java.util.logging.Logger;

import com.funambol.framework.core.Sync4jException;
import com.funambol.framework.engine.SyncItem;
import com.funambol.framework.engine.SyncItemKey;
import com.funambol.framework.engine.SyncItemState;
import com.funambol.framework.engine.source.SyncSource;
import com.funambol.framework.engine.source.SyncSourceInfo;
import com.funambol.framework.engine.source.SyncSourceException;
import com.funambol.framework.engine.SyncItemImpl;
import com.funambol.framework.logging.Sync4jLogger;
import com.funambol.framework.engine.source.SyncContext;

/**
 * This class implements a <i>SyncSource</i> that supports db4o databases.
 */
public class Db4oSyncSource extends Db4oSyncSourceBase implements SyncSource, Serializable{

  private static final long serialVersionUID = 5968694905122732457L;

  private Logger _log;

  /** The description for the SyncSource. */
  private SyncSourceInfo _info;
  
  /** The principal for whom the sync operation is being performed. */
  private Principal _principal;
  
  /**
   * Creates a new instance.
   * 
   * NB: Configuration of the instance is done via its properties
   * rather than via constructor arguments. This is the pattern
   * for a Funambol Connector.
   */
  public Db4oSyncSource(){

    super();
    
    _log = Sync4jLogger.getLogger("server");
    
  }

  /**
   * @return Describe this SyncSource. 
   */
  public SyncSourceInfo getInfo(){

    return _info;
  
  }
  
  /** 
   * Sets the description for this SyncSource.
   * 
   * @param info The new description.
   */
  public void setInfo(SyncSourceInfo info){
    
    _info = info;
    
  }  

  public void beginSync(SyncContext syncContext)
		throws SyncSourceException{
	_log.info(String.format("Db4oSyncSource#beginSync(%1$s, %2$d, %3$s); ",
			new Object[]{
				syncContext.getPrincipal(), 
				new Integer(syncContext.getSyncMode()), 
				syncContext.getFilterClause()}));

	try{
  
		super.beginSync(false);

	}
	catch (Sync4jException e){

		translateException(e);

	}

  }
  
  public void commitSync() throws SyncSourceException{
	  
  }

  public void endSync() throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#endSync(); principal: %1$s", new Object[]{_principal}));
    
    try{
    
      super.finalizeSync();
    
    }
    catch (Sync4jException e){
      
      translateException(e);
    
    }

  }

  public SyncItemKey[] getAllSyncItemKeys() throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#getAllSyncItemKeys(); principal: %1$s", new Object[]{_principal}));
    
    List keys = new ArrayList();
    
    try{
    
      for (Iterator i = getAllSynchronizers().iterator(); i.hasNext();){
        
        Synchronizer synchronizer = (Synchronizer) i.next();
        keys.addAll(synchronizer.getAllSyncItemKeys());
        
      }

    }
    catch (Sync4jException e){
      
      translateException(e);
    
    }
    
    _log.info(String.format("Db4oSyncSource#getAllSyncItemKeys: returned %1$d keys; principal %2$s",
        new Object[] { new Integer(keys.size()), _principal }));
    
    return makeSyncItemKeys(keys);

  }

  public SyncItemKey[] getNewSyncItemKeys(Timestamp since, Timestamp until)
      throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#getNewSyncItemKeys(%2$s, %3$s); principal: %1$s",
        new Object[] { _principal, since, until }));
    
    List keys = new ArrayList();
    
    try {
    
      for (Iterator i = getAllSynchronizers().iterator(); i.hasNext();){
      
        Synchronizer synchronizer = (Synchronizer) i.next();
        keys.addAll(synchronizer.getNewSyncItemKeys(since, until));
  
      }

    }
    catch (Sync4jException e){
      
      translateException(e);
    
    }
    
    _log.info(String.format("Db4oSyncSource#getNewSyncItemKeys() returned %1$d keys; principal: %2$s",
        new Object[] { new Integer(keys.size()), _principal }));

    return makeSyncItemKeys(keys);

  }

  public SyncItemKey[] getDeletedSyncItemKeys(Timestamp since, Timestamp until)
      throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#getDeletedSyncItemKeys(%2$s, %3$s); principal: %1$s",
        new Object[] { _principal, since, until }));
    
    List keys = new ArrayList();
    
    try{

      for (Iterator i = getAllSynchronizers().iterator(); i.hasNext();){
        
        Synchronizer synchronizer = (Synchronizer) i.next();
        keys.addAll(synchronizer.getDeletedSyncItemKeys(since, until));
  
      }
      
    }
    catch (Sync4jException e){
      
      translateException(e);
    
    }

    _log.info(String.format("Db4oSyncSource#getDeletedSyncItemKeys() returned %1$d keys; principal: %2$s",
        new Object[] { new Integer(keys.size()), _principal }));
    
    return makeSyncItemKeys(keys);

  }

  public SyncItemKey[] getUpdatedSyncItemKeys(Timestamp since, Timestamp until)
      throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#getUpdatedSyncItemKeys(%2$s, %3$s); principal: %1$s",
        new Object[] { _principal, since, until }));
    
    List keys = new ArrayList();
    try{
      
      for (Iterator i = getAllSynchronizers().iterator(); i.hasNext();){
        
        Synchronizer synchronizer = (Synchronizer) i.next();
        keys.addAll(synchronizer.getUpdatedSyncItemKeys(since, until));
  
      }
      
    }
    catch (Sync4jException e){
      
      translateException(e);
    
    }
    
    _log.info(String.format("Db4oSyncSource#getUpdatedSyncItemKeys() returned %1$d keys; principal: %2$s",
        new Object[] { new Integer(keys.size()), _principal }));

    return makeSyncItemKeys(keys);

  }

  public SyncItem getSyncItemFromId(SyncItemKey syncItemKey) throws SyncSourceException{
      
    _log.info(String.format("Db4oSyncSource#getSyncItemFromId(%1$s); principal: %2$s",
        new Object[] { syncItemKey, _principal }));

    SyncItem item = null;
    
    if (syncItemKey != null){
        
      try{

        SyncKey syncKey = getSyncKey(syncItemKey);
        Synchronizer synchronizer = getSynchronizer(syncKey.getClassname());
        
        item = makeSyncItem(synchronizer.getSyncItemFromId(syncKey));
        
      }
      catch (Sync4jException e){
        
        _log.info(e.getMessage());
        translateException(e);
      
      }

    }
    
    return item;
    
  }

  public void removeSyncItem(SyncItemKey syncItemKey, Timestamp time) throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#removeSyncItem(%2$s, %3$s); principal: %1$s",
        new Object[] { _principal, syncItemKey, time }));
    
    if (syncItemKey != null){
    
      Timestamp ts = time;
      if (ts == null){
      
        ts = new Timestamp(new Date().getTime());
        
      }
      
      try{
       
        SyncKey syncKey = getSyncKey(syncItemKey);
        Synchronizer synchronizer = getSynchronizer(syncKey.getClassname());

        synchronizer.removeSyncItem(syncKey, ts);
      
      }
      catch (Sync4jException e){
        
        translateException(e);
      
      }
      
    }
    
  }

  public void removeSyncItem(SyncItemKey syncItemKey, Timestamp time, boolean softDelete)
      throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#removeSyncItem(%2$s, %3$s, %4$s); principal: %1$s",
        new Object[] { _principal, syncItemKey, time, new Boolean(softDelete) }));
    
    // We ignore softDelete, as it requires support in the schema of the database being sync'd
    if (!softDelete){
      
      removeSyncItem(syncItemKey, time);
      
    }
    
  }

  public SyncItem updateSyncItem(SyncItem syncItem) throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#updateSyncItem(%1$s); principal: %2$s",
        new Object[] { syncItem, _principal }));
    
    SyncItem item = null;
    
    if ((syncItem != null) && (syncItem.getKey() != null)){
    
      try{
       
        SyncKey syncKey = getSyncKey(syncItem.getKey());
        Synchronizer synchronizer = getSynchronizer(syncKey.getClassname());

        item = makeSyncItem(synchronizer.updateSyncItem(makeSyncData(syncItem)));
      
      }
      catch (Sync4jException e){
        
        translateException(e);
      
      }
      
    }
    
    return item;
    
  }

  public SyncItem addSyncItem(SyncItem syncItem) throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#addSyncItem(%1$s); principal: %2$s",
        new Object[] { syncItem, _principal }));

    SyncItem item = null;
    
    if ((syncItem != null) && (syncItem.getKey() != null)){
      
      try{
        
        SyncKey syncKey = getSyncKey(syncItem.getKey());
        Synchronizer synchronizer = getSynchronizer(syncKey.getClassname());

        item = makeSyncItem(synchronizer.addSyncItem(makeSyncData(syncItem)));
      
      }
      catch (Sync4jException e){
        
        translateException(e);
      
      }
      
    }
    
    return item;
    
  }

  public SyncItemKey[] getSyncItemKeysFromTwin(SyncItem syncItem) throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#getSyncItemsFromTwin(%1$s); principal: %2$s",
        new Object[] { syncItem, _principal }));

    SyncItemKey[] keys = new SyncItemKey[0]; 
    
    if ((syncItem != null) && (syncItem.getKey() != null)){
    
      try{
        
        SyncKey syncKey = getSyncKey(syncItem.getKey());
        Synchronizer synchronizer = getSynchronizer(syncKey.getClassname());

        keys = makeSyncItemKeys(synchronizer.getSyncItemKeysFromTwin(makeSyncData(syncItem)));
      
      }
      catch (Sync4jException e){
        
        translateException(e);
      
      }
      
    }
    
    return keys;
    
  }

  public void setOperationStatus(String operation, int statusCode, SyncItemKey[] keyArray){

    _log.info(String.format("Db4oSyncSource#setOperationStatus(%1$s, %2$d, %3%s); principal: %4$s",
        new Object[] { operation, new Integer(statusCode), Arrays.toString(keyArray), _principal }));

    List keys = new ArrayList();
    for (int i = 0; i < keyArray.length; i++){
    
      try{
        
        keys.add(getSyncKey(keyArray[i]));
      
      }
      catch (SyncSourceException e){
      
        _log.info("Db4oSyncSource#setOperationStatus() failed: " + e.getMessage());
        
      }
      
    }
        
    updateStatus(keys, statusCode);
    
  }

  public char getSyncItemStateFromId(SyncItemKey syncItemKey) throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#getSyncItemStateFromId(%1$s); principal: %2$s",
        new Object[] { syncItemKey, _principal }));

    char state = SyncItemState.UNKNOWN;
    
    if (syncItemKey != null){
      
      try{
        
        SyncKey syncKey = getSyncKey(syncItemKey);
        Synchronizer synchronizer = getSynchronizer(syncKey.getClassname());

        state = synchronizer.getSyncItemStateFromId(syncKey);
      
      }
      catch (Sync4jException e){
        
        translateException(e);
      
      }
      
    }
    
    return state;
    
  }

  public boolean mergeSyncItems(SyncItemKey serverKey, SyncItem clientItem) throws SyncSourceException{

    _log.info(String.format("Db4oSyncSource#mergeSyncItems(%1$s, %2$s); principal: %3$s",
        new Object[] { serverKey, clientItem, _principal }));

    boolean clientItemChanged = false;
    
    if ((serverKey != null) && (clientItem != null)){
    
      SyncKey syncKey = getSyncKey(serverKey);
      try{

        Synchronizer synchronizer = getSynchronizer(syncKey.getClassname());
        SyncData update = null;
      
        update = synchronizer.mergeSyncItems(syncKey, makeSyncData(clientItem));
        if (update != null){
          
          clientItemChanged = true;
          clientItem.setContent(update.getContent());
          clientItem.setFormat(update.getFormat());
          clientItem.setType(getType());
          clientItem.setState(update.getState());
          clientItem.setTimestamp(update.getTimestamp());
          
        }
      
      }
      catch (Sync4jException e){
        
        translateException(e);
      
      }
            
    }
    
    return clientItemChanged;

  }

  public boolean isSyncItemInFilterClause(SyncItem arg0) throws SyncSourceException{

    // sync4o currently does not support filters, so we always return true.
    return true;

  }

  public boolean isSyncItemInFilterClause(SyncItemKey arg0) throws SyncSourceException{

    // sync4o currently does not support filters, so we always return true.
    return true;

  }
   
  /**
   * Checks to ensure a SyncItemKey was generated by an instance of sync4o.
   * If not, throws a SyncSourceException.
   * 
   * @return the SyncKey contained within syncItemKey.
   * @param syncItemKey The SyncItemKey to be tested.
   * @throw SyncSourceException if syncItemKey was not generated by sync4o.
   */
  static private SyncKey getSyncKey(SyncItemKey syncItemKey) throws SyncSourceException{

    Object o = syncItemKey.getKeyValue();
    if (!(o instanceof String)){
      
      throw new SyncSourceException("SyncItemKey passed to sync4o was not generated by sync4o. " + o.getClass().getName());
    
    }

    SyncKey key = null;
    
    try{
      
      key = SyncKey.fromEncodedString((String) o);
      
    }
    catch (Exception e){
      
      throw new SyncSourceException(e);
    
    }
    
    return key;
    
  }

  /** 
   * Exists to translate the Sync4jExceptions thrown by the generic Sync4o classes
   * into the specific type of exception (SyncSourceException) thrown by a server-side
   * SyncSource.
   * 
   * @param e The exception to be wrapped and thrown.
   * @throws SyncSourceException
   */
  static private void translateException(Sync4jException e) throws SyncSourceException{
  
    throw new SyncSourceException(e.getMessage());
  
  }

  /**
   * Converts a generic Sync4o SyncKey into a server-side SyncSource SyncItemKey.
   * 
   * @param key The key to be converted.
   * @return A SyncItemKey containing the SyncKey data.
   * @throws SyncSourceException If anything goes wrong during processing.
   */
  static private SyncItemKey makeSyncItemKey(SyncKey key) throws SyncSourceException{
    
    try{
   
      return new SyncItemKey(SyncKey.toEncodedString(key));
      
    }
    catch (Exception e){

      throw new SyncSourceException(e);
      
    }
    
  }
  
  static private SyncItemKey[] makeSyncItemKeys(List keys) throws SyncSourceException{

    SyncItemKey[] itemKeys = new SyncItemKey[keys.size()];
    for (int i = 0; i < keys.size(); i++){
      
      itemKeys[i] = makeSyncItemKey((SyncKey) keys.get(i)); 
    
    }
    
    return itemKeys;

  }

  private SyncItem makeSyncItem(SyncData data) throws Sync4jException{
  
    SyncItem item = null;
    if (data != null){
      
      try{
      
        item = new SyncItemImpl(this, SyncKey.toEncodedString(data.getKey()), data.getState());
      
      }
      catch (Exception e){
        
        throw new Sync4jException(e);
      
      }
      
      item.setContent(data.getContent());
      item.setFormat(data.getFormat());
      item.setTimestamp(data.getTimestamp());
      item.setType(getType());
    
    }
    
    return item;
    
  }
  
  private SyncData makeSyncData(SyncItem item) throws SyncSourceException{
    
    SyncData data = null;
    if (item != null){
      
      data = new SyncData(getSyncKey(item.getKey()), item.getContent(), item.getTimestamp(), item.getState());
    }
    
    return data;
    
  }
  
}
