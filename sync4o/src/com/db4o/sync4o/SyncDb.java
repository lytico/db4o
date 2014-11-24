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
import java.util.Date;
import java.util.Calendar;
import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Set;

import org.apache.commons.codec.binary.Base64;

import com.db4o.Db4o;
import com.db4o.ObjectContainer;
import com.db4o.ext.ExtDb4o;
import com.db4o.ext.ExtObjectContainer;
import com.db4o.ext.MemoryFile;
import com.db4o.query.Candidate;
import com.db4o.query.Evaluation;
import com.db4o.query.Query;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;
import com.funambol.framework.core.Sync4jException;

/**
 * Class that manages the "real" database (the one being synchronized)
 * and the "shadow" database (the one with the synchronization state) during
 * synchronization. 
 */
class SyncDb{

  /** The main database containing user-defined objects that is being synchronized. */
  private final ExtObjectContainer _db;
  
  /** The shadow database that mantains synchronization state for the main database. */
  private final ExtObjectContainer _shadowDb;
  
  /**
   * Sets up Db4o configuration parameters that must be set before the 
   * shadow database is opened. This method must be called prior to 
   * constructing a SyncDb instance.
   */
  static public void configure(){

    Db4o.configure().objectClass(SyncClassInfo.class).cascadeOnDelete(true);
    Db4o.configure().objectClass(SyncClassInfo.class).cascadeOnUpdate(true);

  }
  
  public SyncDb(ExtObjectContainer db, ExtObjectContainer shadowDb){

    if ((db == null) || (db.ext().isClosed()) ||
        (shadowDb == null) || (shadowDb.ext().isClosed())){

      throw new IllegalArgumentException();

    }

    _db = db;
    _shadowDb = shadowDb;
    
  }
  
  /**
   * Releases all external database handles.
   * Once called, this instance should no longer have any other
   * methods called on it as they cannot perform reliably.
   */
  public void close(){
    
    _db.close();
    _shadowDb.close();

  }

  /**
   * Purges all the SyncObjectInfos with state DELETED from the
   * shadow database. Essentially, this is "forgetting" all the records
   * that have been deleted from the primary database.
   * 
   * @param syncClass 
   *          Represents the class whose instances are to have their sync
   *          metadata updated.
   * @throws Sync4jException If any processing errors occur.
   */
  public void purge(final SyncClass syncClass) throws Sync4jException{
    
    if (syncClass == null){
      
      throw new IllegalArgumentException();
  
    }

    SyncClassInfo syncClassInfo = getClassInfo(syncClass.getConfig());
    List toDelete = new ArrayList();
    List syncInfos = syncClassInfo.getObjectInfos();
    
    for (Iterator i = syncInfos.iterator(); i.hasNext();){
    
      SyncObjectInfo info = (SyncObjectInfo) i.next();
      
      if (info.getSyncState() == SyncState.DELETED){
        
        toDelete.add(info);
      
      }
      
    }
    
    for (Iterator i = toDelete.iterator(); i.hasNext();){
      
      syncClassInfo.getObjectInfos().remove(i.next());
      
    }
    
    // commit all the changes we have made
    setClassInfo(syncClassInfo);
    
  }
  
  /**
   * Updates the SyncDb so that it contains updated sync data for all the
   * current instances of the class represented by SyncClass.
   * 
   * @param syncClass
   *          Represents the class whose instances are to have their sync
   *          metadata updated.
   * @throws Sync4jException If any processing errors occur.
   * @throws IllegalArgumentException If syncClass is null.
   */
  public void updateSyncData(final SyncClass syncClass) throws Sync4jException{
  
    if (syncClass == null){
  
      throw new IllegalArgumentException();
  
    }
  
    // NB: Sets are deliberately used in this method as our key
    // operations are all supposedly on *unique* keys. Sets will
    // help expose any runtime violations of this constraint.
    
    // All operations in this method are performed in memory, as the entire
    // shadow database needs to be activated to be updated.
  
    // First, assemble the set of keys that we already have in the syncdb
    // We also keep a map of keys to ObjectInfo's, to make subsequent
    // operations on the ObjectInfoList easier and quicker.
    SyncClassInfo syncClassInfo = getClassInfo(syncClass.getConfig());
    List syncInfos = syncClassInfo.getObjectInfos();
    Set syncDbKeys = getKeySet(syncInfos);
  
    // Create a set of keys for all the objects in the target db
    // to make subsequent set operations a little easier
    Set dbKeys = getAllKeys(syncClass);

    // find records that have appeared in the db since
    // we last saw it and mark them as SyncItemState.NEW
    Date newTimestamp = null;
    if (syncClassInfo.getLastSync() == null){
      newTimestamp = new Date();
    }    
    
    Set newKeys = new HashSet(dbKeys);
    newKeys.removeAll(syncDbKeys);
    for (Iterator i = newKeys.iterator(); i.hasNext();){
  
      Object key = i.next();
      Object o = get(syncClass, key);
      
      SyncObjectInfo info = new SyncObjectInfo(syncClassInfo, key,
          (newTimestamp == null) ? syncClass.extractTimestamp(o) : newTimestamp, SyncState.NEW);
      syncInfos.add(info);
  
    }
    
    // find records that have been removed from the db since
    // we last saw it and mark them as SyncItemState.DELETED
    Date deleteTimestamp = null;
    if (syncClassInfo.getLastSync() != null){
      Calendar cal = Calendar.getInstance();
      cal.setTime(syncClassInfo.getLastSync());
      cal.add(Calendar.SECOND, 1);
      deleteTimestamp = new Date(cal.getTimeInMillis());
    }
    
    Set deletedKeys = new HashSet(syncDbKeys);
    deletedKeys.removeAll(dbKeys);
    for (Iterator i = deletedKeys.iterator(); i.hasNext();){
  
      SyncObjectInfo info = syncClassInfo.findByKey(i.next()); 
      info.setSyncState(SyncState.DELETED);
      info.setTimestamp(deleteTimestamp);
  
    }

    // find records that have been modified since we last saw
    // them and mark them as SyncItemState.UPDATED
    final Date lastSyncTime = syncClassInfo.getLastSync();
    if (lastSyncTime != null){
  
      for (Iterator i = syncInfos.iterator(); i.hasNext();){
  
        SyncObjectInfo info = (SyncObjectInfo) i.next();
        
        // Need to get the actual object here so we can compare 
        // its last update to the lastSyncTime
        if ((info.getSyncState() == SyncState.SYNCHRONIZED) ||
             (info.getSyncState() == SyncState.UPDATED)){
          
          List objects = findByKey(syncClass, info.getKey());
          if ((objects != null) && objects.size() > 0){
            
            Object o = objects.get(0);

            Date d = syncClass.extractTimestamp(o);
            if (d != null){

              info.setTimestamp(d);
            
              if ((info.getTimestamp().after(lastSyncTime))){
              
                info.setSyncState(SyncState.UPDATED);
              
              }

            }
            
          }
          
        }
  
      }
  
    }
    
    // commit all the changes we have made
    setClassInfo(syncClassInfo);
  
  }

  public Object get(final SyncClass syncClass, final Object key) throws Sync4jException{
  
    if ((syncClass == null) || (key == null)){
      
      throw new IllegalArgumentException();
      
    }
    
    List objects = findByKey(syncClass, key);
    if (objects.size() > 1){
      
      throw new Sync4jException("Multiple objects matched a key. Consider reconfiguring the unique key field that sync4o uses.");
      
    }
    
    return (objects.size() > 0) ? objects.get(0) : null;
  }

  public char getState(final SyncClass syncClass, final Object key) throws Sync4jException{
    
    if ((syncClass == null) || (key == null)){
      
      throw new IllegalArgumentException();
      
    }
    
    char state = SyncState.NOT_EXISTING; 
    
    List objects = findObjectInfoByKey(syncClass, key);
    if (objects.size() == 1){
  
      SyncObjectInfo info = (SyncObjectInfo) objects.get(0);
      state = info.getSyncState();
      
    }
    else if (objects.size() > 1){
      
      throw new Sync4jException("Multiple objects matched a key. Consider reconfiguring the unique key field that sync4o uses.");
      
    }
    
    return state;
    
  }

  public Date getSyncTimestamp(final SyncClass syncClass, final Object key) throws Sync4jException{
    
    if ((syncClass == null) || (key == null)){
      
      throw new IllegalArgumentException();
      
    }
    
    Date timestamp = null; 
    
    List objects = findObjectInfoByKey(syncClass, key);
    if (objects.size() == 1){
  
      SyncObjectInfo info = (SyncObjectInfo) objects.get(0);
      timestamp = info.getTimestamp();
      
    }
    else if (objects.size() > 1){
      
      throw new Sync4jException("Multiple objects matched a key. Consider reconfiguring the unique key field that sync4o uses.");
      
    }
    
    return timestamp;
    
  }
  
  public List getAll(SyncClass syncClass) throws Sync4jException{
  
    if (syncClass == null){
  
      throw new IllegalArgumentException();
      
    }
  
    return _db.query(syncClass.getRealClass());
  
  }

  public Set getAllKeys(SyncClass syncClass) throws Sync4jException{
  
    if (syncClass == null){
  
      throw new IllegalArgumentException();
  
    }
  
    Set keys = new HashSet();
    List objects = getAll(syncClass);
  
    for (Iterator i = objects.iterator(); i.hasNext();){

      Object o = i.next();
      _db.activate(o, 2);
      Object key = syncClass.extractKey(o);
      if (!keys.add(key)){
  
        throw new Sync4jException(
            String.format(
                "Database contains duplicate keys. Please reconsider selection of unique key %1$s for class %1$s.",
                new Object[] { syncClass.getConfig().getUniqueField(),
                    syncClass.getConfig().getClassName() }));
  
      }
  
    }
  
    return keys;
  
  }

  public Set getFilteredKeys(final SyncClass syncClass, final char syncItemState,
      final Date since, final Date until)  throws Sync4jException{
    
    Set keys = new HashSet();
    
    List infos = findKeysBetween(syncClass, syncItemState, since, until);
    for (Iterator i = infos.iterator(); i.hasNext();){
      
      SyncObjectInfo info = (SyncObjectInfo) i.next();
      if (!keys.add(info.getKey())){
        
        throw new Sync4jException("Shadow database appears to contain duplicate keys. Please reinitialize sync4o shadow database.");
        
      }
      
    }
    
    return keys;
    
  }

  public SyncData getAsSyncItem(SyncClass syncClass, SyncKey syncKey) throws Sync4jException{
  
    SyncData item = null;
        
    Object key = syncKey.getKey();
    Object o = get(syncClass, key);
    char state = getState(syncClass, key);
    if (o != null){
      
      Timestamp timestamp = null;
      Date dt = syncClass.extractTimestamp(o);
      timestamp = (dt != null) ? new Timestamp(dt.getTime()) : null;

      MemoryFile data = new MemoryFile();
      ObjectContainer tempDb = ExtDb4o.openMemoryFile(data);
      if (tempDb == null){
        
        throw new Sync4jException("Could not create in-memory database for SyncItem transfer.");
        
      }
      
      tempDb.ext().migrateFrom(_db);
      tempDb.set(o);
      tempDb.commit();
      tempDb.ext().migrateFrom(null);
      tempDb.close();
      tempDb = null;
      
      item = new SyncData(syncKey, Base64.encodeBase64(data.getBytes()), timestamp, state);  
    
    }
    else if (state == SyncState.DELETED){
    
      item = new SyncData(syncKey, new byte[0], new Timestamp(getSyncTimestamp(syncClass, key).getTime()), state);
    }
    
    return item;
  
  }

  public void remove(SyncClass syncClass, SyncKey syncKey, Date time) throws Sync4jException{
  
    if ((syncClass == null) || (syncKey == null) || (time == null)){
      
      throw new IllegalArgumentException();
    
    }
       
    Object key = syncKey.getKey();
    Object item = get(syncClass, key);
    if (item != null){
      
      _db.delete(item);
      _db.commit();
      
      SyncClassInfo syncClassInfo = getClassInfo(syncClass.getConfig());
      SyncObjectInfo info = syncClassInfo.findByKey(key);
      if (info != null){
      
        info.setSyncState(SyncState.DELETED);
        info.setTimestamp(time);
        
      }
      else{
      
        info = new SyncObjectInfo(syncClassInfo, key, time, SyncState.DELETED);
        syncClassInfo.getObjectInfos().add(info);
        
      }
      
      setClassInfo(syncClassInfo);

    }
    
  }

  public void delete(SyncClass syncClass, SyncKey syncKey) throws Sync4jException{
    
    if ((syncClass == null) || (syncKey == null)){
      
      throw new IllegalArgumentException();
    
    }
       
    Object key = syncKey.getKey();
    Object item = get(syncClass, key);
    if (item != null){
      
      _db.delete(item);
      _db.commit();
      
      SyncClassInfo syncClassInfo = getClassInfo(syncClass.getConfig());
      SyncObjectInfo info = syncClassInfo.findByKey(key);
      if (info != null){
      
        syncClassInfo.getObjectInfos().remove(info);
        
      }
      
      setClassInfo(syncClassInfo);

    }
    
  }
  
  public SyncData set(SyncClass syncClass, SyncData item) throws Sync4jException{
    
    if ((syncClass == null) || (item == null)){
      
      throw new IllegalArgumentException();
      
    }
    
    // First, we store the item in the database    
    Object updatedObject;
    
    MemoryFile data = new MemoryFile(Base64.decodeBase64(item.getContent()));
    ObjectContainer tempDb = ExtDb4o.openMemoryFile(data);
    if (tempDb == null){
      
      throw new Sync4jException("Could not create in-memory database from SyncItem contents.");
      
    }
    
    List objects = tempDb.get(syncClass.getRealClass());
    if ((objects == null) || (objects.size() == 0)){
      
      throw new Sync4jException("SyncItem contained an empty database.");
      
    }
    
    updatedObject = objects.get(0);

    _db.ext().migrateFrom(tempDb);
    
    Object keyValue = item.getKey().getKey(); 
    Object o = get(syncClass, keyValue);
    if (o != null){
      
      _db.delete(o);
      
    }

    _db.set(updatedObject);
    _db.commit();
    
    _db.ext().migrateFrom(null);

    tempDb.close();
    tempDb = null;
    
    Timestamp ts = item.getTimestamp();
    if (ts != null){
      
      ts = new Timestamp(new Date().getTime());
    
    }
    
    // Last, we update our synchronization database with metadata about the operation
    SyncClassInfo syncClassInfo = getClassInfo(syncClass.getConfig());
    
    SyncObjectInfo info = syncClassInfo.findByKey(keyValue);
    if (info != null){
      
      info.setSyncState(SyncState.UPDATED);
      info.setTimestamp(ts);
      
    }
    else{
      
      info = new SyncObjectInfo(syncClassInfo, keyValue, ts, SyncState.NEW);
      syncClassInfo.getObjectInfos().add(info);
      
    }
    
    setClassInfo(syncClassInfo);

    return new SyncData(item.getKey(), Base64.encodeBase64(data.getBytes()), ts, info.getSyncState());
    
    
  }
  
  public void setState(final SyncClass syncClass, final Object key, final char state) throws Sync4jException{
    
    if ((syncClass == null) || (key == null)){
      
      throw new IllegalArgumentException();
      
    }

    SyncClassInfo syncClassInfo = getClassInfo(syncClass.getConfig());
    
    SyncObjectInfo info = syncClassInfo.findByKey(key);
    if (info != null){
    
      info.setSyncState(state);
      
    }
      
    setClassInfo(syncClassInfo);
      
  }

  public void setState(final SyncClass syncClass, final char state) throws Sync4jException{
    
    if (syncClass == null){
      
      throw new IllegalArgumentException();
      
    }

    SyncClassInfo syncClassInfo = getClassInfo(syncClass.getConfig());
    
    for (Iterator i = syncClassInfo.getObjectInfos().iterator(); i.hasNext();){
      
      SyncObjectInfo info = (SyncObjectInfo) i.next();
      info.setSyncState(state);
      
    }
    
    setClassInfo(syncClassInfo);
      
  }
  
  public SyncData merge(SyncClass syncClass, SyncKey syncKey, SyncData item) throws Sync4jException{

    SyncData updatedItem = null;
    
    SyncClassInfo syncClassInfo = getClassInfo(syncClass.getConfig());
    SyncObjectInfo info = syncClassInfo.findByKey(syncKey.getKey());
    if (info != null){
    
      if (info.getTimestamp().before(item.getTimestamp())) {
      
        set(syncClass, item);
        
      }
      else{
        
        getAsSyncItem(syncClass, syncKey);
        
        updatedItem = new SyncData(syncKey, item.getContent(), item.getTimestamp(), SyncState.UPDATED);
        
      }
      
    }
    else{
      
      info = new SyncObjectInfo(syncClassInfo, syncKey.getKey(), item.getTimestamp(), SyncState.NEW);
      syncClassInfo.getObjectInfos().add(info);
      
    }
    
    setClassInfo(syncClassInfo);
    
    return updatedItem;
    
  }
  
  public void resetSyncData(SyncClass syncClass) throws Sync4jException{
   
    SyncClassInfo syncClassInfo = getClassInfo(syncClass.getConfig());
    syncClassInfo.setLastSync(new Date());
    setClassInfo(syncClassInfo);
    
  }
  
  public List findTwins(SyncClass syncClass, SyncData item) throws Sync4jException{

    // Extract the object to be matched against
    MemoryFile data = new MemoryFile(Base64.decodeBase64(item.getContent()));
    ObjectContainer tempDb = ExtDb4o.openMemoryFile(data);
    if (tempDb == null){
      
      throw new Sync4jException("Could not create in-memory database from SyncItem contents.");
      
    }
    
    List objects = tempDb.get(syncClass.getRealClass());
    if ((objects == null) || (objects.size() == 0)){
      
      throw new Sync4jException("SyncItem contained an empty database.");
      
    }
    
    Object o = objects.get(0);
    
    // Assemble the list of fields to be matched on
    ReflectClass cls = tempDb.ext().reflector().forClass(syncClass.getRealClass());
    ReflectField[] allFields = cls.getDeclaredFields();
    
    Query q = _db.query();
    q.constrain(syncClass.getRealClass());
    
    for (int i = 0; i < allFields.length; i++){
      
      String fieldName = allFields[i].getName(); 
      if ((fieldName != syncClass.getConfig().getUniqueField())) {
      
        q.descend(fieldName).constrain(allFields[i].get(o));
        
      }
      
    }
    
    return q.execute();
  
  }

  private SyncClassInfo getClassInfo(final SyncClassConfig config){

    if (config == null){

      throw new IllegalArgumentException();

    }

    SyncClassInfo info;

    // Find the SyncClassInfo (if there is one) for the supplied config
    List classInfos = findClassInfos(config);
    if ((classInfos != null) && (classInfos.size() == 1)){

      info = (SyncClassInfo) classInfos.get(0);

    }
    else{

      info = new SyncClassInfo(config, _shadowDb.ext().collections().newLinkedList());
      setClassInfo(info);

    }

    return info;

  }

  private void setClassInfo(final SyncClassInfo info){

    _shadowDb.ext().set(info, Integer.MAX_VALUE);
    _shadowDb.commit();

  }
  
  private Set getKeySet(List syncInfos){

    Set set = new HashSet(syncInfos.size());
    for (Iterator i = syncInfos.iterator(); i.hasNext();){
      
      SyncObjectInfo info = (SyncObjectInfo) i.next();
      set.add(info.getKey());
           
    }
    
    return set;
    
  }
  
  private List findClassInfos(SyncClassConfig config){

    Query query = _shadowDb.query();
    query.constrain(SyncClassInfo.class);
    query.descend("_config").constrain(config);
    
    Db4o.configure().activationDepth(10);
    List list = query.execute(); 
    Db4o.configure().activationDepth(5);
    
    return list;

  }

  private List findKeysBetween(final SyncClass syncClass, final char state, final Date start, final Date finish){

    Query query = _shadowDb.query();
    query.constrain(SyncObjectInfo.class);
    query.constrain(new Evaluation(){

      public void evaluate(Candidate c){

        boolean matches = false;
        SyncObjectInfo info = (SyncObjectInfo) c.getObject();
        if (syncClass.getConfig().equals(info.getClassInfo().getConfig()) &&
            info.getSyncState() == state) {

          Date timestamp = info.getTimestamp();
          matches = ((start == null) || start.before(timestamp)) &&
              ((finish == null) || finish.after(timestamp));
          
        }

        c.include(matches);
        
      }
      
    });

    return query.execute();

  }
  
  private List findByKey(final SyncClass syncClass, final Object key){
  
    Query query = _db.query();
    query.constrain(syncClass.getRealClass());
    query.descend(syncClass.getConfig().getUniqueField()).constrain(key).equal();
    
    return query.execute();
    
  }

  private List findObjectInfoByKey(SyncClass syncClass, Object key){

    Query query = _shadowDb.query();
    
    query.constrain(SyncObjectInfo.class);
    query.descend("_key").constrain(key).equal();
    
    return query.execute();
  }
  
}
