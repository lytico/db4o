/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.reflect.*;

/**
 * @exclude
 */
public final class YapClassCollection extends YapMeta implements UseSystemTransaction {

    private YapClass i_addingMembersTo;

    private Collection4 i_classes;
    private Hashtable4 i_creating;
    
    final YapStream i_stream;
    final Transaction i_systemTrans;

    private Hashtable4 i_yapClassByBytes;
    private Hashtable4 i_yapClassByClass;
    private Hashtable4 i_yapClassByID;
    
    private int i_yapClassCreationDepth;
    private Queue4 i_initYapClassesOnUp;
	
	private final PendingClassInits _classInits; 


    YapClassCollection(Transaction a_trans) {
        i_systemTrans = a_trans;
        i_stream = a_trans.i_stream;
        i_initYapClassesOnUp = new Queue4();
		_classInits = new PendingClassInits(this);
    }

    void addYapClass(YapClass yapClass) {
        i_stream.setDirty(this);
        i_classes.add(yapClass);
        if(yapClass.stateUnread()){
            i_yapClassByBytes.put(yapClass.i_nameBytes, yapClass);
        }else{
            i_yapClassByClass.put(yapClass.classReflector(), yapClass);
        }
        if (yapClass.getID() == 0) {
            yapClass.write(i_systemTrans);
        }
        i_yapClassByID.put(yapClass.getID(), yapClass);
    }
    
    private byte[] asBytes(String str){
        return i_stream.stringIO().write(str);
    }

    void attachQueryNode(final String fieldName, final Visitor4 a_visitor) {
        YapClassCollectionIterator i = iterator();
        while (i.hasNext()) {
            final YapClass yc = i.readNextClass();
            if(! yc.isInternal()){
                yc.forEachYapField(new Visitor4() {
                    public void visit(Object obj) {
                        YapField yf = (YapField)obj;
                        if(yf.canAddToQuery(fieldName)){
                            a_visitor.visit(new Object[] {yc, yf});
                        }
                    }
                });
            }
        }
    }

    void checkChanges() {
        Iterator4 i = i_classes.iterator();
        while (i.hasNext()) {
            ((YapClass)i.next()).checkChanges();
        }
    }
    
    final boolean createYapClass(YapClass a_yapClass, ReflectClass a_class) {
        i_yapClassCreationDepth++;
        ReflectClass superClass = a_class.getSuperclass();
        YapClass superYapClass = null;
        if (superClass != null && ! superClass.equals(i_stream.i_handlers.ICLASS_OBJECT)) {
            superYapClass = getYapClass(superClass, true);
        }
        boolean ret = i_stream.createYapClass(a_yapClass, a_class, superYapClass);
        i_yapClassCreationDepth--;
        initYapClassesOnUp();
        return ret;
    }


    boolean fieldExists(String a_field) {
        YapClassCollectionIterator i = iterator();
        while (i.hasNext()) {
            if (i.readNextClass().getYapField(a_field) != null) {
                return true;
            }
        }
        return false;
    }

    Collection4 forInterface(ReflectClass claxx) {
        Collection4 col = new Collection4();
        YapClassCollectionIterator i = iterator();
        while (i.hasNext()) {
            YapClass yc = i.readNextClass();
            ReflectClass candidate = yc.classReflector();
            if(! candidate.isInterface()){
                if (claxx.isAssignableFrom(candidate)) {
                    col.add(yc);
                    Iterator4 j = col.iterator();
                    while (j.hasNext()) {
                        YapClass existing = (YapClass)j.next();
                        if(existing != yc){
                            YapClass higher = yc.getHigherHierarchy(existing);
                            if (higher != null) {
                                if (higher == yc) {
                                    col.remove(existing);
                                }else{
                                    col.remove(yc);
                                }
                            }
                        }
                    }
                }
            }
        }
        return col;
    }

    public byte getIdentifier() {
        return YapConst.YAPCLASSCOLLECTION;
    }
    
    YapClass getActiveYapClass(ReflectClass a_class) {
        return (YapClass)i_yapClassByClass.get(a_class);
    }

    YapClass getYapClass(ReflectClass a_class, boolean a_create) {
    	
    	YapClass yapClass = (YapClass)i_yapClassByClass.get(a_class);
        
        if (yapClass == null) {        	        	
            yapClass = (YapClass)i_yapClassByBytes.remove(getNameBytes(a_class.getName()));
            readYapClass(yapClass, a_class);
        }

        if (yapClass != null || (!a_create)) {
            return yapClass;
        }

        yapClass = (YapClass)i_creating.get(a_class);
        
        if(yapClass != null){
            return yapClass;
        }
        
        yapClass = new YapClass(i_stream, a_class);
        
        i_creating.put(a_class, yapClass);
        
        if(! createYapClass(yapClass, a_class)){
            i_creating.remove(a_class);
            return null;
        }

        // YapStream#createYapClass may add the YapClass already,
        // so we have to check again
        
        boolean addMembers = false;
        
        if (i_yapClassByClass.get(a_class) == null) {
            addYapClass(yapClass);
            addMembers = true;
        }

        int id = yapClass.getID();
        if(id == 0){
            yapClass.write(i_stream.getSystemTransaction());
            id = yapClass.getID();
        }
        
        if(i_yapClassByID.get(id) == null){
            i_yapClassByID.put(id, yapClass);
            addMembers = true;
        }
        
        if(addMembers || yapClass.i_fields == null){
			_classInits.process(yapClass);
        }
        
        i_creating.remove(a_class);
        
        i_stream.setDirty(this);
        
        return yapClass;
    }    
    
	YapClass getYapClass(int a_id) {
        return readYapClass((YapClass)i_yapClassByID.get(a_id), null);
    }

    public YapClass getYapClass(String a_name) {
        YapClass yapClass = (YapClass)i_yapClassByBytes.remove(getNameBytes(a_name));
        readYapClass(yapClass, null);
        if (yapClass == null) {
            YapClassCollectionIterator i = iterator();
            while (i.hasNext()) {
                yapClass = (YapClass)i.next();
                if (a_name.equals(yapClass.getName())) {
                    readYapClass(yapClass, null);
                    return yapClass;
                }
            }
            return null;
        }
        return yapClass;
    }
    
    public int getYapClassID(String name){
        YapClass yc = (YapClass)i_yapClassByBytes.get(getNameBytes(name));
        if(yc != null){
            return yc.getID();
        }
        return 0;
    }

	private byte[] getNameBytes(String name) {		
		return asBytes(resolveAlias(name));
	}

	private String resolveAlias(String name) {
		return i_stream.i_config.resolveAlias(name);
	}

    void initOnUp(Transaction systemTrans) {
        i_yapClassCreationDepth++;
        systemTrans.i_stream.showInternalClasses(true);
        Iterator4 i = i_classes.iterator();
        while (i.hasNext()) {
            ((YapClass)i.next()).initOnUp(systemTrans);
        }
        systemTrans.i_stream.showInternalClasses(false);
        i_yapClassCreationDepth--;
        initYapClassesOnUp();
    }

    void initTables(int a_size) {
        i_classes = new Collection4();
        i_yapClassByBytes = new Hashtable4(a_size);
        if (a_size < 16) {
            a_size = 16;
        }
        i_yapClassByClass = new Hashtable4(a_size);
        i_yapClassByID = new Hashtable4(a_size);
        i_creating = new Hashtable4(1);
    }
    
    private void initYapClassesOnUp() {
        if(i_yapClassCreationDepth == 0){
            YapClass yc = (YapClass)i_initYapClassesOnUp.next();
            while(yc != null){
                yc.initOnUp(i_systemTrans);
                yc = (YapClass)i_initYapClassesOnUp.next();
            }
        }
    }
    
    YapClassCollectionIterator iterator(){
        return new YapClassCollectionIterator(this, i_classes._first);
    }

    public int ownLength() {
        return YapConst.OBJECT_LENGTH
            + YapConst.YAPINT_LENGTH
            + (i_classes.size() * YapConst.YAPID_LENGTH);
    }

    void purge() {
        Iterator4 i = i_classes.iterator();
        while (i.hasNext()) {
            ((YapClass)i.next()).purge();
        }
    }

    public final void readThis(Transaction a_trans, YapReader a_reader) {
        int classCount = a_reader.readInt();

        initTables(classCount);

        for (int i = classCount; i > 0; i--) {
            YapClass yapClass = new YapClass(i_stream, null);
            int id = a_reader.readInt();
            yapClass.setID(id);
            i_classes.add(yapClass);
            i_yapClassByID.put(id, yapClass);
            i_yapClassByBytes.put(yapClass.readName(a_trans), yapClass);
        }
        
        final Hashtable4 readAs = i_stream.i_config.readAs(); 
        
        readAs.forEachKey(new Visitor4() {
            public void visit(Object a_object) {
                String dbName = (String)a_object;
                byte[] dbbytes = getNameBytes(dbName);
                String useName = (String)readAs.get(dbName);
                byte[] useBytes = getNameBytes(useName);
                if(i_yapClassByBytes.get(useBytes) == null){
                    YapClass yc = (YapClass)i_yapClassByBytes.get(dbbytes);
                    if(yc != null){
                        yc.i_nameBytes = useBytes;
                        yc.setConfig(i_stream.i_config.configClass(dbName));
                        i_yapClassByBytes.put(dbbytes, null);
                        i_yapClassByBytes.put(useBytes, yc);
                    }else{
                        int xxx = 1;
                    }
                }
            }
        });
    }

    YapClass readYapClass(YapClass yapClass, ReflectClass a_class) {
        if (yapClass != null  && ! yapClass.stateUnread()) {
            return yapClass;
        }
        i_yapClassCreationDepth++;
        if (yapClass != null  && yapClass.stateUnread()) {
            yapClass.createConfigAndConstructor(i_yapClassByBytes, i_stream, a_class);
            ReflectClass claxx = yapClass.classReflector();
            if(claxx != null){
                i_yapClassByClass.put(claxx, yapClass);
                yapClass.readThis();
                yapClass.checkChanges();
                i_initYapClassesOnUp.add(yapClass);
            }
        }
        i_yapClassCreationDepth--;
        initYapClassesOnUp();
        return yapClass;
    }

    void refreshClasses() {
        YapClassCollection rereader = new YapClassCollection(i_systemTrans);
        rereader.i_id = i_id;
        rereader.read(i_stream.getSystemTransaction());
        Iterator4 i = rereader.i_classes.iterator();
        while (i.hasNext()) {
            YapClass yc = (YapClass)i.next();
            if (i_yapClassByID.get(yc.getID()) == null) {
                i_classes.add(yc);
                i_yapClassByID.put(yc.getID(), yc);
                if(yc.stateUnread()){
                    i_yapClassByBytes.put(yc.readName(i_systemTrans), yc);
                }else{
                    i_yapClassByClass.put(yc.classReflector(), yc);
                }
            }
        }
        i = i_classes.iterator();
        while (i.hasNext()) {
            YapClass yc = (YapClass)i.next();
            yc.refresh();
        }
    }

    void reReadYapClass(YapClass yapClass){
        if(yapClass != null){
            reReadYapClass(yapClass.i_ancestor);
            yapClass.readName(i_systemTrans);
            yapClass.forceRead();
            yapClass.setStateClean();
            yapClass.bitFalse(YapConst.CHECKED_CHANGES);
            yapClass.bitFalse(YapConst.READING);
            yapClass.bitFalse(YapConst.CONTINUE);
            yapClass.bitFalse(YapConst.DEAD);
            yapClass.checkChanges();
        }
    }
    
    public StoredClass[] storedClasses() {
        Collection4 classes = new Collection4();
        Iterator4 i = i_classes.iterator();
        while (i.hasNext()) {
            YapClass yc = (YapClass)i.next();
            readYapClass(yc, null);
            if(yc.classReflector() == null){
                yc.forceRead();
            }
            classes.add(yc);
        }
        StoredClass[] sclasses = new StoredClass[classes.size()];
        classes.toArray(sclasses);
        return sclasses;
    }

    public void writeThis(Transaction trans, YapReader a_writer) {
        a_writer.writeInt(i_classes.size());
        Iterator4 i = i_classes.iterator();
        while (i.hasNext()) {
            a_writer.writeIDOf(trans, i.next());
        }
    }

	public String toString(){
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
		String str = "";
		Iterator4 i = i_classes.iterator();
		while(i.hasNext()){
			YapClass yc = (YapClass)i.next();
			str += yc.getID() + " " + yc + "\r\n";
		}
		return str;
	}

}
