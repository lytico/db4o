package com.db4o.omplus.datalayer;

import java.util.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;

public class DbInterfaceImpl implements IDbInterface {
	
	private final ObjectContainer objContainer;
	private final String dbPath;
	
	public DbInterfaceImpl(ObjectContainer objContainer, String dbPath) {
		this.objContainer = objContainer;
		this.dbPath = dbPath;
	}

	public String getVersion(){
		return Db4o.version();
	}
	
	public Object[] getStoredClasses() {
		StoredClass []classes = null;
		try{
			classes = objContainer.ext().storedClasses();
		}catch(Exception ex){
			throw new RuntimeException(ex.getClass().getName()+ " occured when getting the"+
					" stored classes from db.");// OME currently doesn't handle translators, if configured.
		}
		TreeSet<String> temp = new TreeSet<String>();
		if(classes != null){
			for(StoredClass claxx: classes){
				String className = claxx.getName();
				if (! excludeClass(className)){
					temp.add(className);
				}
				
			}
		}
		return temp.toArray();
	}

	private boolean excludeClass(String className) {
		try {
			Class<?> clazz = Class.forName(className);
			return Internal4.class.isAssignableFrom(clazz);
		} catch (ClassNotFoundException e) {
			// Can happen if the class definition is not available.
		}
		return false;
	}
	
	public ObjectContainer getDB(){
		return objContainer;
	}
	
	public String getDbPath() {
		return dbPath;
	}

	public Reflector reflector(){
		return objContainer.ext().reflector();
	}
	
	public int getNumOfObjects(String className){
		try {
			if(className != null){
				StoredClass strClass = objContainer.ext().storedClass(className);
				if(strClass != null)
					return strClass.getIDs().length;
			}
		}catch(Exception ex){
			
		}
		return 0;
	}
	
	public void commit(){
		objContainer.commit();
	}
	
	public void close(){
		objContainer.close();
	}

	public long getDBSize() {
		if(!isClient()){
			SystemInfo sysInfo = objContainer.ext().systemInfo();
			if(sysInfo != null)
				return sysInfo.totalSize();
		}			
		return 0;
	}
	
	public long getFreespaceSize() {
		if(!isClient())
			return objContainer.ext().systemInfo().freespaceSize();
		return 0;
	}
	
	public boolean isClient(){
		return ((ObjectContainerBase)objContainer).isClient();
	}

	public Object getObjectById(long objId) {
		return objContainer.ext().getByID(objId);		
	}

	public void activate(Object resObj, int i) {
		objContainer.activate(resObj, i);		
	}

	public StoredClass getStoredClass(String name) {
		if(name == null)
			return null;
		return objContainer.ext().storedClass(name);
		
	}

	public void refreshObj(Object obj) {
		if(obj != null)
			objContainer.ext().refresh(obj, 1);
	}

	public boolean readOnly() {
		if(!(objContainer instanceof LocalObjectContainer)) {
			return false;
		}
		return ((LocalObjectContainer)objContainer).config().isReadOnly();
	}

	public ReflectHelper reflectHelper() {
		return new ReflectHelper(objContainer.ext().reflector());
	}

}
