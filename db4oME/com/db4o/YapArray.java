/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.inside.slots.*;
import com.db4o.reflect.*;

class YapArray extends YapIndependantType {
	
	final YapStream _stream;
    final TypeHandler4 i_handler;
    final boolean i_isPrimitive;
    final ReflectArray _reflectArray;

    YapArray(YapStream stream, TypeHandler4 a_handler, boolean a_isPrimitive) {
        super(stream);
    	_stream = stream;
        i_handler = a_handler;
        i_isPrimitive = a_isPrimitive;
        _reflectArray = stream.reflector().array();
    }

    Object[] allElements(Object a_object) {
        Object[] all = new Object[_reflectArray.getLength(a_object)];
        for (int i = all.length - 1; i >= 0; i--) {
            all[i] = _reflectArray.get(a_object, i);
        }
        return all;
    }

    public void appendEmbedded3(YapWriter a_bytes) {
        a_bytes.incrementOffset(linkLength());
    }

    public boolean canHold(ReflectClass claxx) {
        return i_handler.canHold(claxx);
    }

    public final void cascadeActivation(
        Transaction a_trans,
        Object a_object,
        int a_depth,
        boolean a_activate) {
        // We simply activate all Objects here
        if (i_handler instanceof YapClass) {
            
            a_depth --;
            
            Object[] all = allElements(a_object);
            if (a_activate) {
                for (int i = all.length - 1; i >= 0; i--) {
                    _stream.stillToActivate(all[i], a_depth);
                }
            } else {
                for (int i = all.length - 1; i >= 0; i--) {
                  _stream.stillToDeactivate(all[i], a_depth, false);
                }
            }
        }
    }
    
    public ReflectClass classReflector(){
    	return i_handler.classReflector();
    }

    TreeInt collectIDs(TreeInt tree, YapWriter a_bytes){
        Transaction trans = a_bytes.getTransaction();
        YapReader bytes = a_bytes.readEmbeddedObject(trans);
        if (bytes != null) {
            int count = elementCount(trans, bytes);
            for (int i = 0; i < count; i++) {
                tree = (TreeInt)Tree.add(tree, new TreeInt(bytes.readInt()));
            }
        }
        return tree;
    }

    public final void deleteEmbedded(YapWriter a_bytes) {
        int address = a_bytes.readInt();
        int length = a_bytes.readInt();
        if (address > 0) {
        	Transaction trans = a_bytes.getTransaction();
            if (a_bytes.cascadeDeletes() > 0 && i_handler instanceof YapClass) {
                YapWriter bytes =
                    a_bytes.getStream().readObjectWriterByAddress(
                        trans,
                        address,
                        length);
                if (bytes != null) {
                    if (Deploy.debug) {
                        bytes.readBegin(bytes.getAddress(), identifier());
                    }
                    bytes.setCascadeDeletes(a_bytes.cascadeDeletes());
                    for (int i = elementCount(trans, bytes); i > 0; i--) {
                        i_handler.deleteEmbedded(bytes);
                    }
                }
            }
            trans.slotFreeOnCommit(address, address, length);
        }
    }

    public final void deletePrimitiveEmbedded(
        YapWriter a_bytes,
        YapClassPrimitive a_classPrimitive) {
        int address = a_bytes.readInt();
        int length = a_bytes.readInt();
        if (address > 0) {
            Transaction trans = a_bytes.getTransaction();
            YapWriter bytes =
                a_bytes.getStream().readObjectWriterByAddress(trans, address, length);
            if (bytes != null) {
                if (Deploy.debug) {
                    bytes.readBegin(bytes.getAddress(), identifier());
                }
                for (int i = elementCount(trans, bytes); i > 0; i--) {
                    int id = bytes.readInt();
                    Slot slot = trans.getSlotInformation(id);
					a_classPrimitive.free(trans, id, slot._address,slot._length);
                }
            }
            trans.slotFreeOnCommit(address, address, length);
        }
    }

    int elementCount(Transaction a_trans, YapReader a_bytes) {
        if (Debug.arrayTypes) {
            int typeOrLength = a_bytes.readInt();
            if (typeOrLength >= 0) {
                return typeOrLength;
            }
        }
        return a_bytes.readInt();
    }

    public final boolean equals(TypeHandler4 a_dataType) {
        if (a_dataType instanceof YapArray) {
            if (((YapArray) a_dataType).identifier() == identifier()) {
                return (i_handler.equals(((YapArray) a_dataType).i_handler));
            }
        }
        return false;
    }

    public final int getID() {
        return i_handler.getID();
    }

    public int getType() {
        return i_handler.getType();
    }

    public YapClass getYapClass(YapStream a_stream) {
        return i_handler.getYapClass(a_stream);
    }

    byte identifier() {
        return YapConst.YAPARRAY;
    }
    
    public boolean indexNullHandling() {
        return i_handler.indexNullHandling();
    }
    
    public Object comparableObject(Transaction a_trans, Object a_object){
        throw YapConst.virtualException();
    }

    int objectLength(Object a_object) {
        return YapConst.OBJECT_LENGTH
            + YapConst.YAPINT_LENGTH * (Debug.arrayTypes ? 2 : 1)
            + (_reflectArray.getLength(a_object) * i_handler.linkLength());
    }
    
	public void prepareLastIoComparison(Transaction a_trans, Object obj) {
	    prepareComparison(obj);
	}

    public Object read(YapWriter a_bytes) throws CorruptionException{
		YapWriter bytes = a_bytes.readEmbeddedObject();
		i_lastIo = bytes;
		if (bytes == null) {
			return null;
		}
		if (Deploy.debug) {
			bytes.readBegin(bytes.getAddress(), identifier());
		}
		bytes.setUpdateDepth(a_bytes.getUpdateDepth());
		bytes.setInstantiationDepth(a_bytes.getInstantiationDepth());
        Object array = read1(bytes);
        if (Deploy.debug) {
            bytes.readEnd();
        }
        return array;
    }
    
    public Object readIndexEntry(YapReader a_reader) {
        // TODO: implement
        throw YapConst.virtualException();
    }
    
	public Object readQuery(Transaction a_trans, YapReader a_reader, boolean a_toArray) throws CorruptionException{
		YapReader bytes = a_reader.readEmbeddedObject(a_trans);
		if (bytes == null) {
			return null;
		}
		if(Deploy.debug){
		    bytes.readBegin(identifier());
		}
		Object array = read1Query(a_trans, bytes);
		if (Deploy.debug) {
			bytes.readEnd();
		}
		return array;
	}
	
	Object read1Query(Transaction a_trans, YapReader a_reader) throws CorruptionException{
		int[] elements = new int[1];
        Object ret = readCreate(a_trans, a_reader, elements);
		if(ret != null){
			for (int i = 0; i < elements[0]; i++) {
                _reflectArray.set(ret, i, i_handler.readQuery(a_trans, a_reader, true));
			}
		}
		return ret;
	}

    Object read1(YapWriter a_bytes) throws CorruptionException{
		int[] elements = new int[1];
		Object ret = readCreate(a_bytes.getTransaction(), a_bytes, elements);
		if (ret != null){
            if(i_handler.readArray(ret, a_bytes)){
                return ret;
            }
			for (int i = 0; i < elements[0]; i++) {
				_reflectArray.set(ret, i, i_handler.read(a_bytes));
			}	
		}
        return ret;
    }

	private Object readCreate(Transaction a_trans, YapReader a_reader, int[] a_elements) {
		ReflectClass[] clazz = new ReflectClass[1];
		a_elements[0] = readElementsAndClass(a_trans, a_reader, clazz);
		if (i_isPrimitive) {
			return _reflectArray.newInstance(i_handler.primitiveClassReflector(), a_elements[0]);
		} else {
			if (clazz[0] != null) {
				return _reflectArray.newInstance(clazz[0], a_elements[0]);	
			}
		}
		return null;
	}

    public TypeHandler4 readArrayWrapper(Transaction a_trans, YapReader[] a_bytes) {
        return this;
    }

    public void readCandidates(YapReader a_bytes, QCandidates a_candidates) {
		YapReader bytes = a_bytes.readEmbeddedObject(a_candidates.i_trans);
		if (bytes != null) {
		    if(Deploy.debug){
		        bytes.readBegin(identifier());
		    }
            int count = elementCount(a_candidates.i_trans, bytes);
            for (int i = 0; i < count; i++) {
                a_candidates.addByIdentity(new QCandidate(a_candidates, null, bytes.readInt(), true));
            }
        }
    }
    
    int readElementsAndClass(Transaction a_trans, YapReader a_bytes, ReflectClass[] clazz){
        int elements = a_bytes.readInt();
        clazz[0] = i_handler.classReflector();
        if (Debug.arrayTypes && elements < 0) {
            
            // TODO: This one is a terrible low-frequency blunder !!!
            // If YapClass-ID == 99999 then we will get ignore.
            
            if(elements != YapConst.IGNORE_ID){
                boolean primitive = false;
                if(!Deploy.csharp){
                    if(elements < YapConst.PRIMITIVE){
                        primitive = true;
                        elements -= YapConst.PRIMITIVE;
                    }
                }
                YapClass yc = a_trans.i_stream.getYapClass(- elements);
                if (yc != null) {
                    if(primitive){
                    	clazz[0] = yc.primitiveClassReflector();
                    }else{
                        clazz[0] = yc.classReflector();
                    }
                }
            }
            elements = a_bytes.readInt();
        }
        if(Debug.exceedsMaximumArrayEntries(elements, i_isPrimitive)){
            return 0;
        }
        return elements;
    }
    
    
    static Object[] toArray(YapStream a_stream, Object a_object) {
        if (a_object != null) {
        	ReflectClass claxx = a_stream.reflector().forObject(a_object);
            if (claxx.isArray()) {
                YapArray ya;
                if(a_stream.reflector().array().isNDimensional(claxx)){
                    ya = new YapArrayN(a_stream, null, false);
                } else {
                    ya = new YapArray(a_stream, null, false);
                }
                return ya.allElements(a_object);
            }
        }
        return new Object[0];
    }

    void writeClass(Object a_object, YapWriter a_bytes){
        if (Debug.arrayTypes) {
            int yapClassID = 0;
            
            Reflector reflector = a_bytes.i_trans.reflector();
            
            ReflectClass claxx = _reflectArray.getComponentType(reflector.forObject(a_object));
            
            boolean primitive = false;
            if(! Deploy.csharp){
                if(claxx.isPrimitive()){
                    primitive = true;
                }
            }
            YapStream stream = a_bytes.getStream();
            if(primitive){
                claxx = stream.i_handlers.handlerForClass(stream,claxx).classReflector();
            }
            YapClass yc = stream.getYapClass(claxx, true);
            if (yc != null) {
                yapClassID = yc.getID();
            }
            if(yapClassID == 0){
                
                // TODO: This one is a terrible low-frequency blunder !!!
                // If YapClass-ID == 99999 then we will get IGNORE back.
                // Discovered on adding the primitives
                yapClassID = - YapConst.IGNORE_ID;
                
            } else{
                if(primitive){
                    yapClassID -= YapConst.PRIMITIVE;
                }
            }

            a_bytes.writeInt(- yapClassID);
        }
    }
    
    public void writeIndexEntry(YapReader a_writer, Object a_object) {
        // TODO: implement
        throw YapConst.virtualException();
    }
    
    public int writeNew(Object a_object, YapWriter a_bytes) {
        if (a_object == null) {
            a_bytes.writeEmbeddedNull();
        } else {
            int length = objectLength(a_object);
            YapWriter bytes = new YapWriter(a_bytes.getTransaction(), length);
            bytes.setUpdateDepth(a_bytes.getUpdateDepth());
            if (Deploy.debug) {
                bytes.writeBegin(identifier(), length);
            }
            writeNew1(a_object, bytes);
            if (Deploy.debug) {
                bytes.writeEnd();
            }
            bytes.setID(a_bytes._offset);
            i_lastIo = bytes;
            a_bytes.getStream().writeEmbedded(a_bytes, bytes);
            a_bytes.incrementOffset(YapConst.YAPID_LENGTH);
            a_bytes.writeInt(length);
        }
		return -1;
    }

    void writeNew1(Object a_object, YapWriter a_bytes) {
        writeClass(a_object, a_bytes);
		
		int elements = _reflectArray.getLength(a_object);
        a_bytes.writeInt(elements);
        
        if(i_handler.writeArray(a_object, a_bytes)){
            return;
        }
        
        for (int i = 0; i < elements; i++) {
            i_handler.writeNew(_reflectArray.get(a_object, i), a_bytes);
        }
    }

    // Comparison_______________________

    public YapComparable prepareComparison(Object obj) {
        i_handler.prepareComparison(obj);
        return this;
    }
    
    public Object current(){
        return i_handler.current();
    }
    
    public int compareTo(Object a_obj) {
        return -1;
    }
    
    public boolean isEqual(Object obj) {
        if(obj == null){
            return false;
        }
        Object[] compareWith = allElements(obj);
        for (int j = 0; j < compareWith.length; j++) {
            if (i_handler.isEqual(compareWith[j])) {
                return true;
            }
        }
        return false;
    }

    public boolean isGreater(Object obj) {
        Object[] compareWith = allElements(obj);
        for (int j = 0; j < compareWith.length; j++) {
            if (i_handler.isGreater(compareWith[j])) {
                return true;
            }
        }
        return false;
    }

    public boolean isSmaller(Object obj) {
        Object[] compareWith = allElements(obj);
        for (int j = 0; j < compareWith.length; j++) {
            if (i_handler.isSmaller(compareWith[j])) {
                return true;
            }
        }
        return false;
    }

    public boolean supportsIndex() {
        return false;
    }
}
