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

import java.io.File;
import java.io.Serializable;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;

import com.db4o.Db4o;
import com.db4o.ObjectContainer;
import com.funambol.framework.core.StatusCode;
import com.funambol.framework.core.Sync4jException;


/**
 * Base class for both client and server implementations of Db4o SyncSource.
 * Contains basic SyncSource functionality common to both.
 */
public class Db4oSyncSourceBase implements Serializable{

  /** The file extension used to distinguish syn4o databases. */
  static private final String SYNC4O_SYNCDB_EXTENSION = ".s4o";

  /** The user-visible name for this instance. */
  private String _name;
  
  /** The URI by which this instance is identified. */
  private String _uri;
  
  /** The part of the URI by which this instance is identified that is a query. */
  private String _sourceQuery;
    
  /** Database file to be synchronized. */
  private String _dbFilename;

  /** Describes the classes within _dbfile that are to be synchronized. */
  private Set _classConfigs;

  /** Responsible for performing a synchronization. */
  private List _synchronizers;

  /** Manages access to data for synchronization. */
  private SyncDb _syncDb;
  
  /** Contains the status codes for SyncItems involved in the last operation. */
  private Map _statusMap;
 
  public Db4oSyncSourceBase(){
  
    _classConfigs = new HashSet();
    
    _statusMap = new HashMap();

    // important that this call is made as it sets up Db4o configuration
    // parameters for the synchronization database that must be setup
    // before the synchronization database is opened.
    SyncDb.configure();
    
  }

  /**
   * @return The name of this SyncSource.
   */
  public String getName(){

    return _name;
  
  }
  
  /**
   * Sets the name of this SyncSource
   * @param name The new name for the SyncSource.
   */
  public void setName(String name){
    
    _name = name;
    
  }

  /**
   * @return the URI that this SyncSource can be identified with and contacted at.
   */
  public String getSourceURI(){

    return _uri;
    
  }

  /**
   * Sets the URI used to identify this SyncSource.
   * 
   * @param uri The new URI for this SyncSource.
   */
  public void setSourceURI(String uri){
    
    int qMark = uri.indexOf('?');
    if (qMark == -1) {
    
      _uri = uri;
      _sourceQuery = "";
    
    }
    else {
    
      _uri = _uri.substring(0, qMark);
      _sourceQuery = _uri.substring(qMark);
    
    }
    
    _uri = uri;
    
  }
  
  /**
   * @return The type of data managed by this SyncSource.
   */
  public String getType(){

    return "application/octet-stream";
    
  }
  
  /**
   * @return Returns the database file being synchronized by this SyncSource.
   */
  public String getDbFilename(){

    return _dbFilename;
  
  }

  /**
   * Sets the full path to the db4o database file to be synchronized.
   * 
   * @param _dbFilename The database file to be synchronized.
   */
  public void setDbFilename(String dbFilename){

    _dbFilename = dbFilename;
  
  }

  /**
   * @return Returns the classConfigs.
   */
  public Set getClassConfigs(){

    return _classConfigs;
  
  }

  /**
   * @param classConfigs The classConfigs to set.
   */
  public void setClassConfigs(Set classConfigs){

    _classConfigs = classConfigs;
  
  }

  /**
   * @return The source query component of the URI for this SyncSource.
   */
  public String getSourceQuery(){

    return _sourceQuery;
    
  }

  /**
   * Sets up internal structures used by subclasses during synchronization.
   *
   */
  protected void beginSync(boolean purgeDeleted) throws Sync4jException{
    
    ObjectContainer db = Db4o.openFile(getDbFilename());
    if (db == null){
      
      throw new Sync4jException(String.format("Could not open database %1$s",
          new Object[]{getDbFilename()}));
      
    }
 
    ObjectContainer shadowDb = Db4o.openFile(getSyncDbFilename());
    if (shadowDb == null){
      
      throw new Sync4jException(String.format("Could not open synchronization shadow database %1$s",
          new Object[]{getSyncDbFilename()}));
      
    }
    
    _statusMap.clear();
    
    _syncDb = new SyncDb(db.ext(), shadowDb.ext());
    
    _synchronizers = new ArrayList();

    Map syncClasses = new HashMap();
    
    for (Iterator i = _classConfigs.iterator(); i.hasNext();){
      
      SyncClassConfig config = (SyncClassConfig) i.next();
      
      SyncClass syncClass = (SyncClass) syncClasses.get(config);
      if (syncClass == null){
        
        syncClass = new SyncClass(config, db.ext());
        syncClasses.put(config, syncClass);
        
      }
      
      if (purgeDeleted){
        _syncDb.purge(syncClass);
      }
      
      _synchronizers.add(new Synchronizer(_syncDb, syncClass));
      
    }

  }
  
  protected void finalizeSync() throws Sync4jException{
  
    // Update as SYNCHRONIZED all items that were signalled as successfully processed
    if (_statusMap.containsKey(new Integer(StatusCode.OK))){
      
      List ids = (List) _statusMap.get(new Integer(StatusCode.OK));
      for (Iterator i = ids.iterator(); i.hasNext();){
        
        SyncKey key = (SyncKey) i.next();
        Synchronizer synchronizer = getSynchronizer(key.getClassname());
        synchronizer.setSynchronized(key);
        
      }
      
    }
    
    for (Iterator i = _synchronizers.iterator(); i.hasNext();){
      
      Synchronizer synchronizer = (Synchronizer) i.next();
      synchronizer.close();
      
    }

    _synchronizers = null;
    
    _syncDb.close();
    
  }
  
  /**
   * @return The name of the synchronization database file to be used by this SyncSource.
   * @throws SyncSourceException
   */
  protected String getSyncDbFilename() throws Sync4jException{
    
    if (_dbFilename == null){
      
      throw new Sync4jException("Must supply a complete path for Db4o database filename.");
      
    }
    
    File dbFile = new File(_dbFilename);       
    File dir = dbFile.getParentFile();
    if (dir == null){
      
      dir = new File(".");
    
    }
    
    String s = dir.getAbsolutePath() + File.separator + dbFile.getName() + SYNC4O_SYNCDB_EXTENSION;
    
    return s;
    
  }
  
  protected Synchronizer getSynchronizer(String classname) throws Sync4jException{
    
    Synchronizer synchronizer = null;
    
    for (Iterator i = _synchronizers.iterator(); (synchronizer == null) && i.hasNext();){
      
      Synchronizer s = (Synchronizer) i.next();
      if (classname.equals(s.getSyncClass().getRealClass().getName())){
        
        synchronizer = s;
        
      }
      
    }
    
    if (synchronizer == null){
      
      throw new Sync4jException("sync4o provided with item or key of a type that it is not configured to synchronize");
      
    }
    
    return synchronizer;
    
  }
  
  protected List getAllSynchronizers(){
    
    return _synchronizers;
  
  }
  
  protected void updateStatus(List keys, int statusCode){

    Integer statusInt = new Integer(statusCode);
    if (_statusMap.containsKey(statusInt)){
      
      List existing = (List) _statusMap.get(statusInt);
      existing.addAll(keys);
      
    }
    else{
      
      _statusMap.put(statusInt, keys);
      
    }
    
  }
}
