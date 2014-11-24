/* Copyright (C) 2005   db4objects Inc.   http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.reflect.*;

/**
 * @exclude
 */
public class GenericReflector implements Reflector, DeepClone {
	
    private Reflector _delegate;
    private GenericArrayReflector _array;
    
    private final Hashtable4 _classByName = new Hashtable4(1);
    private final Hashtable4 _classByClass = new Hashtable4(1);
    private final Collection4 _classes = new Collection4();
    private final Hashtable4 _classByID = new Hashtable4(1);
    
    private Collection4 _collectionPredicates = new Collection4();
    private Collection4 _collectionUpdateDepths = new Collection4();
	private Collection4 _pendingClasses = new Collection4();
    
	private Transaction _trans;
	private YapStream _stream;
	
	public GenericReflector(Transaction trans, Reflector delegateReflector){
		setTransaction(trans);
		_delegate = delegateReflector;
        if(_delegate != null){
            _delegate.setParent(this);
        }
	}
	
	public Object deepClone(Object obj)  {
        GenericReflector myClone = new GenericReflector(null, (Reflector)_delegate.deepClone(this));
        myClone._collectionPredicates = (Collection4)_collectionPredicates.deepClone(myClone);
        myClone._collectionUpdateDepths = (Collection4)_collectionUpdateDepths.deepClone(myClone);
        
        
        // Interesting, adding the following messes things up.
        // Keep the code, since it may make sense to carry the
        // global reflectors into a running db4o session.
        
        
//        Iterator4 i = _classes.iterator();
//        while(i.hasNext()){
//            GenericClass clazz = (GenericClass)i.next();
//            clazz = (GenericClass)clazz.deepClone(myClone);
//            myClone._classByName.put(clazz.getName(), clazz);
//            myClone._classes.add(clazz);
//        }
        
		return myClone;
	}
	
	YapStream getStream(){
		return _stream;
	}

	public boolean hasTransaction(){
		return _trans != null;
	}
	
	public void setTransaction(Transaction trans){
		if(trans != null){
			_trans = trans;
			_stream = trans.i_stream;
		}
	}

    public ReflectArray array() {
        if(_array == null){
            _array = new GenericArrayReflector(this);
        }
        return _array;
    }

    public int collectionUpdateDepth(ReflectClass candidate) {
        Iterator4 i = _collectionUpdateDepths.iterator();
        while(i.hasNext()){
        	CollectionUpdateDepthEntry entry = (CollectionUpdateDepthEntry) i.next();
        	if (entry._predicate.match(candidate)) {
        		return entry._depth;
        	}
        }
        return 2;
        
        //TODO: will need knowledge for .NET collections here
    }

    public boolean constructorCallsSupported() {
        return _delegate.constructorCallsSupported();
    }

    GenericClass ensureDelegate(ReflectClass clazz){
        if(clazz == null){
        	return null;
        }
        GenericClass claxx = (GenericClass)_classByName.get(clazz.getName());
        if(claxx == null){
            //  We don't have to worry about the superclass, it can be null
            //  because handling is delegated anyway
            String name = clazz.getName();
            claxx = new GenericClass(this,clazz, name,null);
            _classes.add(claxx);
            _classByName.put(name, claxx);
        }
        return claxx;
    }
    
    public ReflectClass forClass(Class clazz) {
        if(clazz == null){
            return null;
        }
        ReflectClass claxx = (ReflectClass) _classByClass.get(clazz);
        if(claxx != null){
            return claxx;
        }
        claxx = forName(clazz.getName());
        if(claxx != null){
            _classByClass.put(clazz, claxx);
            return claxx;
        }
        claxx = _delegate.forClass(clazz);
        if(claxx == null){
            return null;
        }
        claxx = ensureDelegate(claxx);
        _classByClass.put(clazz, claxx);
        return claxx;
    }
    

    public ReflectClass forName(String className) {
        ReflectClass clazz = (ReflectClass)_classByName.get(className);
        if(clazz != null){
            return clazz;
        }
        clazz = _delegate.forName(className);
        if(clazz != null){
            return ensureDelegate(clazz);
        }
        
        if(_stream == null) {
        	return null;
        }
        
        if(_stream.i_classCollection != null){
            int classID = _stream.i_classCollection.getYapClassID(className);
            if(classID > 0){
                clazz = ensureClassInitialised(classID);
                _classByName.put(className, clazz);
                return clazz; 
            }
        }
        
        return null;
    }

    public ReflectClass forObject(Object obj) {
        if (obj instanceof GenericObject){
            return ((GenericObject)obj)._class;
        }
        return _delegate.forObject(obj);
    }
    
    public Reflector getDelegate(){
        return _delegate;
    }

    public boolean isCollection(ReflectClass candidate) {
        //candidate = candidate.getDelegate(); 
        Iterator4 i = _collectionPredicates.iterator();
        while(i.hasNext()){
            if (((ReflectClassPredicate)i.next()).match(candidate)) {
            	return true;
            }
        }
        return _delegate.isCollection(candidate.getDelegate());
        
        //TODO: will need knowledge for .NET collections here
        // possibility: call registercollection with strings
    }

    public void registerCollection(Class clazz) {
		registerCollection(classPredicate(clazz));
    }

	public void registerCollection(ReflectClassPredicate predicate) {
		_collectionPredicates.add(predicate);
	}

	private ReflectClassPredicate classPredicate(Class clazz) {
		final ReflectClass collectionClass = forClass(clazz);
		ReflectClassPredicate predicate = new ReflectClassPredicate() {
			public boolean match(ReflectClass candidate) {
	            return collectionClass.isAssignableFrom(candidate);
			}
		};
		return predicate;
	}

    public void registerCollectionUpdateDepth(Class clazz, int depth) {
		registerCollectionUpdateDepth(classPredicate(clazz), depth);
    }

	public void registerCollectionUpdateDepth(ReflectClassPredicate predicate, int depth) {
        _collectionUpdateDepths.add(new CollectionUpdateDepthEntry(predicate, depth));
	}
    
    public void register(GenericClass clazz) {
    	String name = clazz.getName();
    	if(_classByName.get(name) == null){
    		_classByName.put(name, clazz);
    		_classes.add(clazz);
    	}
    }
    
	public ReflectClass[] knownClasses(){
		readAll();
        
        Collection4 classes = new Collection4();
		
		Iterator4 i = _classes.iterator();
		while(i.hasNext()){
            GenericClass clazz = (GenericClass)i.next();
            if(! _stream.i_handlers.ICLASS_INTERNAL.isAssignableFrom(clazz)){
                if(! clazz.isSecondClass()){
					if(! clazz.isArray()){
						classes.add(clazz);
					}
                }
            }
		}
        
        ReflectClass[] ret = new ReflectClass[classes.size()];
        int j = 0;
        i = classes.iterator();
        while(i.hasNext()){
            ret[j++] = (ReflectClass)i.next();
        }
        return ret;
	}
	
	private void readAll(){
		int classCollectionID = _stream.i_classCollection.getID();
		YapWriter classcollreader = _stream.readWriterByID(_trans, classCollectionID);
        if (Deploy.debug) {
            classcollreader.readBegin(classCollectionID, YapConst.YAPCLASSCOLLECTION);
        }
        
		int numclasses = classcollreader.readInt();
		int[] classIDs = new int[numclasses];
		
		for (int classidx = 0; classidx < numclasses; classidx++) {
			classIDs[classidx] = classcollreader.readInt(); 
			ensureClassAvailability(classIDs[classidx]);
		}

		for (int classidx = 0; classidx < numclasses; classidx++) {
			ensureClassRead(classIDs[classidx]);
		}
	}
	
	private GenericClass ensureClassInitialised (int id) {
		GenericClass ret = ensureClassAvailability(id);
		while(_pendingClasses.size() > 0) {
			Collection4 pending = _pendingClasses;
			_pendingClasses = new Collection4();
			Iterator4 i = pending.iterator();
			while(i.hasNext()) {
				ensureClassRead(((Integer)i.next()).intValue());
			}
		}
		return ret;
	}
	
	private GenericClass ensureClassAvailability (int id) {

        if(id == 0){
            return null;
        }
		
        GenericClass ret = (GenericClass)_classByID.get(id);
		if(ret != null){
			return ret;
		}
        
		YapWriter classreader=_stream.readWriterByID(_trans,id);
        if (Deploy.debug) {
            classreader.readBegin(id, YapConst.YAPCLASS);
        }
		int namelength= classreader.readInt();
		String classname= _stream.stringIO().read(classreader,namelength);
		
		ret = (GenericClass)_classByName.get(classname);
		if(ret != null){
			_classByID.put(id, ret);
			_pendingClasses.add(new Integer(id));
			return ret;
		}
		
		classreader.incrementOffset(YapConst.YAPINT_LENGTH); // skip empty unused int slot
        
		int ancestorid=classreader.readInt();
		int fieldCount=classreader.readInt();
		
		ReflectClass nativeClass = _delegate.forName(classname);
		ret = new GenericClass(this, nativeClass,classname, ensureClassAvailability(ancestorid));
		ret.setDeclaredFieldCount(fieldCount);
		
		// step 1 only add to _classByID, keep the class out of _classByName and _classes
        _classByID.put(id, ret);
		_pendingClasses.add(new Integer(id));
		
		return ret;
	}
	
	private void ensureClassRead(int id) {

		GenericClass clazz = (GenericClass)_classByID.get(id);
		
		YapWriter classreader=_stream.readWriterByID(_trans,id);
        if (Deploy.debug) {
            classreader.readBegin(id, YapConst.YAPCLASS);
        }
		int namelength= classreader.readInt();
		String classname= _stream.stringIO().read(classreader,namelength);
		
		// Having the class in the _classByName Map for now indicates
		// that the class is fully read. This is breakable if we start
		// returning GenericClass'es in other methods like forName
		// even if a native class has not been found
		if(_classByName.get(classname) != null){
			return;
		}
		
        // step 2 add the class to _classByName and _classes to denote reading is completed
        _classByName.put(classname, clazz);
		_classes.add(clazz);
		
		// skip empty unused int slot, ancestor, index
		classreader.incrementOffset(YapConst.YAPINT_LENGTH * 3);
		
		int numfields=classreader.readInt();
		
		
		GenericField[] fields=new GenericField[numfields];
		for (int i = 0; i < numfields; i++) {
			String fieldname=null;
			int fieldnamelength= classreader.readInt();
			fieldname = _stream.stringIO().read(classreader,fieldnamelength);
            
            if (fieldname.indexOf(YapConst.VIRTUAL_FIELD_PREFIX) == 0) {
                fields[i] = new GenericVirtualField(fieldname);
            }else{
                
                GenericClass fieldClass = null;
    			
    		    int handlerid=classreader.readInt();
                
                // need to take care of special handlers here
                switch (handlerid){
                    case YapHandlers.ANY_ID:
                        fieldClass = (GenericClass)forClass(Object.class);
                        break;
                    case YapHandlers.ANY_ARRAY_ID:
                        fieldClass = ((GenericClass)forClass(Object.class)).arrayClass();
                        break;
                    default:
						ensureClassAvailability(handlerid);
                        fieldClass = (GenericClass)_classByID.get(handlerid);        
                }
    		    
    		    YapBit attribs=new YapBit(classreader.readByte());
    		    boolean isprimitive=attribs.get();
    		    boolean isarray = attribs.get();
    		    boolean ismultidimensional=attribs.get();
    		    
    			fields[i]=new GenericField(fieldname,fieldClass, isprimitive, isarray, ismultidimensional );
            }
		}
		
        clazz.initFields(fields);
	}
    

	public void registerPrimitiveClass(int id, String name, GenericConverter converter) {
        GenericClass existing = (GenericClass)_classByID.get(id);
		if (existing != null) {
			if (null != converter) {
				existing.setSecondClass();
			} else {
				existing.setConverter(null);
			}
			return;
		}
		ReflectClass clazz = _delegate.forName(name);
		
		GenericClass claxx = null;
		if(clazz != null) {
	        claxx = ensureDelegate(clazz);
		}else {
	        claxx = new GenericClass(this, null, name, null);
	        _classByName.put(name, claxx);
		    claxx.initFields(new GenericField[] {new GenericField(null, null, true, false, false)});
		    claxx.setConverter(converter);
	        _classes.add(claxx);
		}
	    claxx.setSecondClass();
	    claxx.setPrimitive();
	    _classByID.put(id, claxx);
	}

    public void setParent(Reflector reflector) {
        // do nothing, the generic reflector does not have a parant
    }

}
