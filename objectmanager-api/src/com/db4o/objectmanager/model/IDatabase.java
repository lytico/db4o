package com.db4o.objectmanager.model;

//import org.eclipse.ve.sweet.objectviewer.IObjectViewer;

import com.db4o.ObjectSet;
import com.db4o.query.Query;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.Reflector;



public interface IDatabase {
	
	void open(Db4oConnectionSpec spec);
    
    void reopen();

	/**
	 * Method close.  Close the current YAP file if one is open.
	 */
	public abstract void closeIfOpen();

	public abstract DatabaseGraphIterator graphIterator();

	public abstract DatabaseGraphIterator graphIterator(String name);

	public abstract ObjectSet instances(ReflectClass clazz);
	
	public abstract long getId(Object object);
    
	public abstract Object byId(long id);
    
	long[] instanceIds(ReflectClass clazz);

	public abstract void activate(Object object);

	public abstract void setInitialActivationDepth(int initialActivationDepth);

    public abstract Reflector reflector();

    public abstract Query query();

  /*  public abstract IObjectViewer construct();
    
    public abstract IObjectViewer construct(Object toEdit);*/
    
    void delete(Object obj);
    
    void rollback();
    void commit();
}