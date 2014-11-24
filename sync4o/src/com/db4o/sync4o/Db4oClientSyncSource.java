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
import java.util.Date;
import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Set;
import java.util.StringTokenizer;

import com.funambol.framework.core.Sync4jException;
import com.funambol.syncclient.spds.engine.SyncItem;
import com.funambol.syncclient.spds.engine.SyncItemKey;
import com.funambol.syncclient.spds.SyncException;
import com.funambol.syncclient.spds.engine.SyncItemImpl;
import com.funambol.syncclient.spds.engine.SyncItemProperty;
import com.funambol.syncclient.spds.engine.SyncSource;
import com.funambol.syncclient.common.logging.*;

/**
 * The client side SyncSource for Sync4o. Communicates with
 * a server side Db4oSyncSource to perform synchronization.
 * 
 * @version $Id$
 */
public class Db4oClientSyncSource extends Db4oSyncSourceBase implements SyncSource, Serializable {

  public Db4oClientSyncSource(){
    
    super();
  
  }
 
  public void setClassConfigsList (String configs){
    
    Set configSet = new HashSet();
    if (configs != null){
      
      StringTokenizer list = new StringTokenizer(configs, ";");
      while (list.hasMoreTokens()) {
        
        String config = list.nextToken();
        StringTokenizer configFields = new StringTokenizer(config, ",");
        if (configFields.countTokens() != 2){
  
          throw new IllegalArgumentException("Badly formed input. Should be two fields: classname, keyfield, separated by commas.");
        
        }
        
        SyncClassConfig c = new SyncClassConfig();
        c.setClassName(configFields.nextToken());
        c.setUniqueField(configFields.nextToken());
        configSet.add(c);
        
      }
    
    }
    
    setClassConfigs(configSet);
    
  }
  
  /**
   * @see com.funambol.syncclient.spds.engine.SyncSource#beginSync(int)
   */
  public void beginSync(int syncMode) throws SyncException{

    Logger.debug(String.format("beginSync(%1$d); [%2$s]",
        new Object[] { new Integer(syncMode), getSourceURI() }));
    
    try{
      
      super.beginSync(true);
      
    }
    catch (Sync4jException e){
      
      translateException(e);
      
    }
   
  }

  /**
   * @see com.funambol.syncclient.spds.engine.SyncSource#commitSync()
   */
  public void commitSync() throws SyncException{

    Logger.debug(String.format("commitSync(); [%1$s]",
        new Object[] { getSourceURI() }));
    
    try{
      
      // When the client commits a sync, it is assumed that all changes are synchronized.
      for (Iterator i = getAllSynchronizers().iterator(); i.hasNext();){
        
        Synchronizer synchronizer = (Synchronizer) i.next();
        
        synchronizer.setAllSynchronized();
          
      }
      
      super.finalizeSync();
      
    }
    catch (Sync4jException e){
      
      translateException(e);
      
    }

  }
  
  /**
   * @return All sync items available on this client.
   * @see com.funambol.syncclient.spds.engine.SyncSource#getAllSyncItems()
   */
  public SyncItem[] getAllSyncItems(Principal principal) throws SyncException{

    Logger.debug(String.format("Calling getAllSyncItems() [%1$s]",
        new Object[] { getSourceURI() }));
    
    List data = new ArrayList();

    try{
      
      for (Iterator i = getAllSynchronizers().iterator(); i.hasNext();){
        
        Synchronizer synchronizer = (Synchronizer) i.next();
        data.addAll(synchronizer.getSyncItemsForKeys(synchronizer.getAllSyncItemKeys()));
        
      }
    
    }    
    catch (Sync4jException e){
      
      translateException(e);
      
    }

    return makeSyncItems(data);
    
  }

  /**
   * @see com.funambol.syncclient.spds.engine.SyncSource#getDeletedSyncItems(java.security.Principal, java.util.Date)
   */
  public SyncItem[] getDeletedSyncItems(Principal principal, Date date) throws SyncException{

    Logger.debug(String.format("getDeletedSyncItems(%1$s, %2$s); [%3$s]",
        new Object[] { principal, date, getSourceURI() }));

    List data = new ArrayList();

    try{
      
      for (Iterator i = getAllSynchronizers().iterator(); i.hasNext();){
        
        Synchronizer synchronizer = (Synchronizer) i.next();
        data.addAll(synchronizer.getSyncItemsForKeys(
            synchronizer.getDeletedSyncItemKeys(new Timestamp(date.getTime()), null)));
        
      }
    
    }    
    catch (Sync4jException e){
      
      translateException(e);
      
    }
    
    return makeSyncItems(data);

  }

  /**
   * @see com.funambol.syncclient.spds.engine.SyncSource#getNewSyncItems(java.security.Principal, java.util.Date)
   */
  public SyncItem[] getNewSyncItems(Principal principal, Date date) throws SyncException{

    Logger.debug(String.format("getNewSyncItems(%1$s, %2$s); [%3$s]",
        new Object[] { principal, date, getSourceURI() }));

    List data = new ArrayList();

    try{
      
      for (Iterator i = getAllSynchronizers().iterator(); i.hasNext();){
        
        Synchronizer synchronizer = (Synchronizer) i.next();
        data.addAll(synchronizer.getSyncItemsForKeys(
            synchronizer.getNewSyncItemKeys(new Timestamp(date.getTime()), null)));
        
      }
    
    }    
    catch (Sync4jException e){
      
      translateException(e);
      
    }
    
    return makeSyncItems(data);

  }

  /**
   * @see com.funambol.syncclient.spds.engine.SyncSource#getUpdatedSyncItems(java.security.Principal, java.util.Date)
   */
  public SyncItem[] getUpdatedSyncItems(Principal principal, Date date) throws SyncException{

    Logger.debug(String.format("getUpdatedSyncItems(%1$s, %2$s); [%3$s]",
        new Object[] { principal, date, getSourceURI() }));

    List data = new ArrayList();

    try{
      
      for (Iterator i = getAllSynchronizers().iterator(); i.hasNext();){
        
        Synchronizer synchronizer = (Synchronizer) i.next();
        data.addAll(synchronizer.getSyncItemsForKeys(
            synchronizer.getUpdatedSyncItemKeys(new Timestamp(date.getTime()), null)));
        
      }
    
    }    
    catch (Sync4jException e){
      
      translateException(e);
      
    }
    
    return makeSyncItems(data);

  }

  /**
   * @see com.funambol.syncclient.spds.engine.SyncSource#removeSyncItem(java.security.Principal, com.funambol.syncclient.spds.engine.SyncItem)
   */
  public void removeSyncItem(Principal principal, SyncItem item) throws SyncException{

    Logger.debug(String.format("removeSyncItem(%1$s, %2$s); [%3$s]",
        new Object[] { principal, item, getSourceURI() }));

    if (item == null){
      
      return;
      
    }
    
    try{
      
      SyncKey key = getSyncKey(item.getKey());
      Synchronizer synchronizer = getSynchronizer(key.getClassname());
        
      synchronizer.deleteSyncItem(key);
        
    }    
    catch (Sync4jException e){
      
      translateException(e);
      
    }
    
  }

  /**
   * @see com.funambol.syncclient.spds.engine.SyncSource#setSyncItem(java.security.Principal, com.funambol.syncclient.spds.engine.SyncItem)
   */
  public SyncItem setSyncItem(Principal principal, SyncItem item) throws SyncException{

    Logger.debug(String.format("setSyncItem(%1$s, %2$s); [%3$s]",
        new Object[] { principal, item, getSourceURI() }));

    if ((item != null) && (item.getKey() != null)){
      
      try{
        
        SyncKey key = getSyncKey(item.getKey());
        Synchronizer synchronizer = getSynchronizer(key.getClassname());
          
        item = makeSyncItem(synchronizer.updateSyncItem(makeSyncData(item)));
          
      }    
      catch (Sync4jException e){
        
        translateException(e);
        
      }
      
    }

    return item;

  }

  static private void translateException(Sync4jException e) throws SyncException{
    
    throw new SyncException(e.getMessage(), e.getCause());
  
  }
  
  /**
   * Checks to ensure a SyncItemKey was generated by an instance of sync4o.
   * If not, throws a SyncSourceException.
   * 
   * @return the SyncKey contained within syncItemKey.
   * @param syncItemKey The SyncItemKey to be tested.
   * @throw SyncSourceException if syncItemKey was not generated by sync4o.
   */
  static private SyncKey getSyncKey(SyncItemKey syncItemKey) throws SyncException{

    Object o = syncItemKey.getKeyValue();
    if (!(o instanceof String)){
      
      throw new SyncException("SyncItemKey passed to sync4o was not generated by sync4o.");
    
    }
    
    SyncKey key = null;
    try{
    
      key = SyncKey.fromEncodedString((String) o);
    
    }
    catch (Exception e){
      
      throw new SyncException(e.getMessage());
      
    }
    
    return key;
    
  }
  
  private SyncItem[] makeSyncItems(List data) throws SyncException{
  
    SyncItem[] items = null;
    
    try{
      
      items = new SyncItem[data.size()];
      for (int i = 0; i < data.size(); i++){
        
        items[i] = makeSyncItem((SyncData) data.get(i));
        
      }
      
    }
    catch (Sync4jException e){
      
      translateException(e);
      
    }
    
    return items;
    
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
      
      item.setProperty(new SyncItemProperty(SyncItem.PROPERTY_BINARY_CONTENT, data.getContent()));
      item.setProperty(new SyncItemProperty(SyncItem.PROPERTY_TIMESTAMP, data.getTimestamp()));
    
    }
    
    return item;
    
  }
    
  private SyncData makeSyncData(SyncItem item) throws SyncException{
    
    SyncData data = null;
    if (item != null){
      
      data = new SyncData(getSyncKey(item.getKey()),
          (byte[]) item.getPropertyValue(SyncItem.PROPERTY_BINARY_CONTENT),
          new Timestamp(((Date) item.getPropertyValue(SyncItem.PROPERTY_TIMESTAMP)).getTime()),
          item.getState());
    }
    
    return data;
    
  }
  
}
