/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.reflect.*;

/**
 * @exclude
 */
public class YapObject extends YapMeta implements ObjectInfo{
    
	private YapClass i_yapClass;
	Object i_object;
	VirtualAttributes i_virtualAttributes;

	protected YapObject id_preceding;
	private YapObject id_subsequent;
	private int id_size;

	private YapObject hc_preceding;
	private YapObject hc_subsequent;
	private int hc_size;
	private int hc_code; // redundant hashCode
    
    public YapObject(){
    }
	
	YapObject(int a_id) {
		i_id = a_id;
	}

	YapObject(YapClass a_yapClass, int a_id) {
		i_yapClass = a_yapClass;
		i_id = a_id;
	}
	
	void activate(Transaction ta, Object a_object, int a_depth, boolean a_refresh) {
	    activate1(ta, a_object, a_depth, a_refresh);
		ta.i_stream.activate3CheckStill(ta);
	}
	
	void activate1(Transaction ta, Object a_object, int a_depth, boolean a_refresh) {
	    if(a_object instanceof Db4oTypeImpl){
	        a_depth = ((Db4oTypeImpl)a_object).adjustReadDepth(a_depth);
	    }
		if (a_depth > 0) {
		    YapStream stream = ta.i_stream;
		    if(a_refresh){
				if (stream.i_config.messageLevel() > YapConst.ACTIVATION) {
					stream.message("" + getID() + " refresh " + i_yapClass.getName());
				}
		    }else{
				if (isActive()) {
					if (a_object != null) {
						if (a_depth > 1) {
					        if (i_yapClass.i_config != null) {
					            a_depth = i_yapClass.i_config.adjustActivationDepth(a_depth);
					        }
							i_yapClass.activateFields(ta, a_object, a_depth);
						}
						return;
					}
				}
				if (stream.i_config.messageLevel() > YapConst.ACTIVATION) {
					stream.message("" + getID() + " activate " + i_yapClass.getName());
				}
		    }
			read(ta, null, a_object, a_depth, YapConst.ADD_MEMBERS_TO_ID_TREE_ONLY, false);
		}
	}

	final void addToIDTree(YapStream a_stream) {
		if (!(i_yapClass instanceof YapClassPrimitive)) {
			a_stream.idTreeAdd(this);
		}
	}

	/** return false if class not completely initialized, otherwise true **/
	boolean continueSet(Transaction a_trans, int a_updateDepth) {
		if (bitIsTrue(YapConst.CONTINUE)) {
		    if(! i_yapClass.stateOKAndAncestors()){
		        return false;
		    }
            
            if(DTrace.enabled){
                DTrace.CONTINUESET.log(getID());
            }
            
		    YapStream stream = a_trans.i_stream;
			bitFalse(YapConst.CONTINUE);
			Object obj = getObject();
			
			int id = getID();
			int length = ownLength();
			int address = -1;
			if(! stream.isClient()){
			    address = ((YapFile)stream).getSlot(length); 
			}
			a_trans.setPointer(id, address, length);
	        YapWriter writer = new YapWriter(a_trans, length);
	        writer.useSlot(id, address, length);
	        if (Deploy.debug) {
	            writer.writeBegin(getIdentifier(), length);
	        }
			writer.setUpdateDepth(a_updateDepth);
			writer.writeInt(i_yapClass.getID());
			
			i_yapClass.marshallNew(this, writer, obj);

			if (Deploy.debug) {
				writer.writeEnd();
				writer.debugCheckBytes();
			}

			stream.writeNew(i_yapClass, writer);

			i_yapClass.dispatchEvent(stream, obj, EventDispatcher.NEW);
			
			// TODO: Weak reference creation not necessary for
			//       primitive objects, since reference to YapObject
			//       should get lost immediately
			i_object = stream.i_references.createYapRef(this, obj);
			
			setStateClean();
			endProcessing();
		}
		return true;
	}

	void deactivate(Transaction a_trans, int a_depth) {
		if (a_depth > 0) {
			Object obj = getObject();
			if (obj != null) {
			    if(obj instanceof Db4oTypeImpl){
			        ((Db4oTypeImpl)obj).preDeactivate();
			    }
			    YapStream stream = a_trans.i_stream;
				if (stream.i_config.messageLevel() > YapConst.ACTIVATION) {
					stream.message("" + getID() + " deactivate " + i_yapClass.getName());
				}

				setStateDeactivated();
				i_yapClass.deactivate(a_trans, obj, a_depth);
			}
		}
	}
	
	public byte getIdentifier() {
		return YapConst.YAPOBJECT;
	}

	public Object getObject() {
		if (Platform4.hasWeakReferences()) {
			return Platform4.getYapRefObject(i_object);
		}
		return i_object;
	}
    
    // this method will only work client-side or on
    // single ObjectContainers, after the YapClass
    // is set.
    public Transaction getTrans(){
        if(i_yapClass != null){
            YapStream stream = i_yapClass.getStream();
            if(stream != null){
                return stream.getTransaction();
            }
        }
        return null;
    }
    
    public Db4oUUID getUUID(){
        VirtualAttributes va = virtualAttributes(getTrans());
        if(va != null && va.i_database != null){
            return new Db4oUUID(va.i_uuid, va.i_database.i_signature);
        }
        return null;
    }
	
    public long getVersion(){
        VirtualAttributes va = virtualAttributes(getTrans());
        if(va == null) {
			return 0;
        }
		return va.i_version;
    }


	public YapClass getYapClass() {
		return i_yapClass;
	}

	public int ownLength() {
		return i_yapClass.objectLength();
	}

	final Object read(
		Transaction ta,
		YapWriter a_reader,
		Object a_object,
		int a_instantiationDepth,
		int addToIDTree,
        boolean checkIDTree) {

		// a_instantiationDepth is a way of overriding instantiation
		// in a positive manner

		if (beginProcessing()) {
		    
		    YapStream stream = ta.i_stream;

			if (a_reader == null) {
				a_reader = stream.readWriterByID(ta, getID());
			}
			if (a_reader != null) {
			    
				i_yapClass = readYapClass(a_reader);

				if (i_yapClass == null) {
					return null;
				}
                
                if(checkIDTree){
                    // the typical side effect: static fields and enums
                    YapObject classCreationSideEffect = stream.getYapObject(getID());
                    if(classCreationSideEffect != null){
                        Object obj = classCreationSideEffect.getObject();
                        if(obj != null){
                            return obj;
                        }
                        stream.yapObjectGCd(classCreationSideEffect);
                    }
                }

				a_reader.setInstantiationDepth(a_instantiationDepth);
				a_reader.setUpdateDepth(addToIDTree);
				
				if(addToIDTree == YapConst.TRANSIENT){
				    a_object = i_yapClass.instantiateTransient(this, a_object, a_reader);
				}else{
				    a_object = i_yapClass.instantiate(this, a_object, a_reader, addToIDTree == YapConst.ADD_TO_ID_TREE);
				}
				
			}
			endProcessing();
		}
		return a_object;
	}

	final Object readPrefetch(YapStream a_stream, Transaction ta, YapWriter a_reader) {

		Object readObject = null;
		if (beginProcessing()) {

			i_yapClass = readYapClass(a_reader);

			if (i_yapClass == null) {
				return null;
			}

			// We use an instantiationdepth of 1 only, if there is no special
			// configuration for the class. This is a quick fix due to a problem
			// instantiating Hashtables. There may be a better workaround that
			// works with configured objects only to make them fast also.
			//
			// An instantiation depth of 1 makes use of possibly prefetched strings
			// that are carried around in a_bytes.
			//
			// TODO: optimize  
			a_reader.setInstantiationDepth(i_yapClass.configOrAncestorConfig() == null ? 1 : 0);

			readObject = i_yapClass.instantiate(this, getObject(), a_reader, true);
			
			endProcessing();
		}
		return readObject;
	}

	public final void readThis(Transaction a_trans, YapReader a_bytes) {
		if (Deploy.debug) {
			System.out.println(
				"YapObject.readThis should never be called. All handling takes place in read");
		}
	}

	private final YapClass readYapClass(YapWriter a_reader) {
		if (Deploy.debug) {
			a_reader.readBegin(getID(), getIdentifier());
		}
		return a_reader.getStream().getYapClass(a_reader.readInt());
	}
    
	void setObjectWeak(YapStream a_stream, Object a_object) {
		if (a_stream.i_references._weak) {
			if(i_object != null){
				Platform4.killYapRef(i_object);
			}
			i_object = Platform4.createYapRef(a_stream.i_references._queue, this, a_object);
		} else {
			i_object = a_object;
		}
	}

	public void setObject(Object a_object) {
		i_object = a_object;
	}

	void setStateOnRead(YapWriter reader) {
		// Do nothing
	}
	
	/** return true for complex objects to instruct YapStream to add to lookup trees
	 * and to perform delayed storage through call to continueset further up the stack.
	 */ 
	boolean store(Transaction a_trans, YapClass a_yapClass, Object a_object, int a_updateDepth){
		
		i_object = a_object;
		writeObjectBegin();
		
		YapStream stream = a_trans.i_stream;

		i_yapClass = a_yapClass;

		if (i_yapClass.getID() == YapHandlers.ANY_ID) {
			// Storing naked objects does not make sense
			// TODO: why?
			throw new ObjectNotStorableException(i_yapClass.classReflector());
		}
		    
	    setID(stream.newUserObject());

	    // will be ended in continueset()
		beginProcessing();
		
		bitTrue(YapConst.CONTINUE);

		// We may still consider to have Arrays as full objects.
		// It would need special handling, to remove them from
		// hc_tree in the transaction, so currently it's ugly.
		
		// Removing SecondClass objects from the reference tree
		// causes problems in C/S cascaded delete.

		if (/*!(a_object instanceof SecondClass)  && */
			!(i_yapClass instanceof YapClassPrimitive) /*|| clazz.isArray()*/
			) {
		    
			return true;
		} else {
		    
			// Primitive Objects will not be stored
			// in the identity map.
			continueSet(a_trans, a_updateDepth);
		}
		return false;
	}
	
	public VirtualAttributes virtualAttributes(Transaction a_trans){
        if(a_trans == null){
            return i_virtualAttributes;
        }
	    if(i_virtualAttributes == null){ 
            if(i_yapClass.hasVirtualAttributes()){
                i_virtualAttributes = new VirtualAttributes();
                i_yapClass.readVirtualAttributes(a_trans, this);
            }
	    }else{
            if(! i_virtualAttributes.suppliesUUID()){
                if(i_yapClass.hasVirtualAttributes()){
                    i_yapClass.readVirtualAttributes(a_trans, this);
                }
            }
        }
	    return i_virtualAttributes;
	}
    
    public void setVirtualAttributes(VirtualAttributes at){
        i_virtualAttributes = at;
    }

	public void writeThis(Transaction trans, YapReader a_writer) {
		if (Deploy.debug) {
			System.out.println("YapObject.writeThis should never be called.");
		}
	}

	void writeUpdate(Transaction a_trans, int a_updatedepth) {

		continueSet(a_trans, a_updatedepth);
		// make sure, a concurrent new, possibly triggered by objectOnNew
		// is written to the file

		// preventing recursive
		if (beginProcessing()) {
		    
		    Object obj = getObject();
		    
		    if(i_yapClass.dispatchEvent(a_trans.i_stream, obj, EventDispatcher.CAN_UPDATE)){

				
				if ((!isActive()) || obj == null) {
					endProcessing();
					return;
				}
				if (Deploy.debug) {
					if (!(getID() > 0)) {
						System.out.println(
							"Object passed to set() with valid YapObject. YapObject had no ID.");
						throw new RuntimeException();
					}
					if (i_yapClass == null) {
						System.out.println(
							"Object passed to set() with valid YapObject. YapObject has no valid yapClass.");
						throw new RuntimeException();
					}
				}
				
				if (a_trans.i_stream.i_config.messageLevel() > YapConst.STATE) {
				    a_trans.i_stream.message("" + getID() + " update " + i_yapClass.getName());
				}
	
				setStateClean();
	
				a_trans.writeUpdateDeleteMembers(getID(), i_yapClass, a_trans.i_stream.i_handlers.arrayType(obj), 0);
	
				i_yapClass.marshallUpdate(a_trans, getID(), a_updatedepth, this, obj);
		    } else{
		        endProcessing();
		    }
		}
	}

	/***** HCTREE *****/

	public YapObject hc_add(YapObject a_add) {
		Object obj = a_add.getObject();
		if (obj != null) {
			a_add.hc_init(obj);
			return hc_add1(a_add);
		} else {
			return this;
		}
	}
    
    public void hc_init(Object obj){
        hc_preceding = null;
        hc_subsequent = null;
        hc_size = 1;
        hc_code = hc_getCode(obj);
    }
    
	private YapObject hc_add1(YapObject a_new) {
		int cmp = hc_compare(a_new);
		if (cmp < 0) {
			if (hc_preceding == null) {
				hc_preceding = a_new;
				hc_size++;
			} else {
				hc_preceding = hc_preceding.hc_add1(a_new);
				if (hc_subsequent == null) {
					return hc_rotateRight();
				} else {
					return hc_balance();
				}
			}
		} else {
			if (hc_subsequent == null) {
				hc_subsequent = a_new;
				hc_size++;
			} else {
				hc_subsequent = hc_subsequent.hc_add1(a_new);
				if (hc_preceding == null) {
					return hc_rotateLeft();
				} else {
					return hc_balance();
				}
			}
		}
		return this;
	}

	private YapObject hc_balance() {
		int cmp = hc_subsequent.hc_size - hc_preceding.hc_size;
		if (cmp < -2) {
			return hc_rotateRight();
		} else if (cmp > 2) {
			return hc_rotateLeft();
		} else {
			hc_size = hc_preceding.hc_size + hc_subsequent.hc_size + 1;
			return this;
		}
	}

	private void hc_calculateSize() {
		if (hc_preceding == null) {
			if (hc_subsequent == null) {
				hc_size = 1;
			} else {
				hc_size = hc_subsequent.hc_size + 1;
			}
		} else {
			if (hc_subsequent == null) {
				hc_size = hc_preceding.hc_size + 1;
			} else {
				hc_size = hc_preceding.hc_size + hc_subsequent.hc_size + 1;
			}
		}
	}

	private int hc_compare(YapObject a_to) {
	    int cmp = a_to.hc_code - hc_code;
	    if(cmp == 0){
	        cmp = a_to.i_id - i_id;
	    }
		return cmp;
	}

	public YapObject hc_find(Object obj) {
		return hc_find(hc_getCode(obj), obj);
	}

	private YapObject hc_find(int a_id, Object obj) {
		int cmp = a_id - hc_code;
		if (cmp < 0) {
			if (hc_preceding != null) {
				return hc_preceding.hc_find(a_id, obj);
			}
		} else if (cmp > 0) {
			if (hc_subsequent != null) {
				return hc_subsequent.hc_find(a_id, obj);
			}
		} else {
			if (obj == getObject()) {
				return this;
			}
			if (hc_preceding != null) {
				YapObject inPreceding = hc_preceding.hc_find(a_id, obj);
				if (inPreceding != null) {
					return inPreceding;
				}
			}
			if (hc_subsequent != null) {
				return hc_subsequent.hc_find(a_id, obj);
			}
		}
		return null;
	}

	private int hc_getCode(Object obj) {
		int hcode = System.identityHashCode(obj);
		if (hcode < 0) {
			hcode = ~hcode;
		}
		return hcode;
	}

	private YapObject hc_rotateLeft() {
		YapObject tree = hc_subsequent;
		hc_subsequent = tree.hc_preceding;
		hc_calculateSize();
		tree.hc_preceding = this;
		if(tree.hc_subsequent == null){
			tree.hc_size = 1 + hc_size;
		}else{
			tree.hc_size = 1 + hc_size + tree.hc_subsequent.hc_size;
		}
		return tree;
	}

	private YapObject hc_rotateRight() {
		YapObject tree = hc_preceding;
		hc_preceding = tree.hc_subsequent;
		hc_calculateSize();
		tree.hc_subsequent = this;
		if(tree.hc_preceding == null){
			tree.hc_size = 1 + hc_size;
		}else{
			tree.hc_size = 1 + hc_size + tree.hc_preceding.hc_size;
		}
		return tree;
	}

	private YapObject hc_rotateSmallestUp() {
		if (hc_preceding != null) {
			hc_preceding = hc_preceding.hc_rotateSmallestUp();
			return hc_rotateRight();
		}
		return this;
	}

	YapObject hc_remove(YapObject a_find) {
		if (this == a_find) {
			return hc_remove();
		}
		int cmp = hc_compare(a_find);
		if (cmp <= 0) {
			if (hc_preceding != null) {
				hc_preceding = hc_preceding.hc_remove(a_find);
			}
		}
		if (cmp >= 0) {
			if (hc_subsequent != null) {
				hc_subsequent = hc_subsequent.hc_remove(a_find);
			}
		}
		hc_calculateSize();
		return this;
	}
    
    public void hc_traverse(Visitor4 visitor){
        if(hc_preceding != null){
            hc_preceding.hc_traverse(visitor);
        }
        visitor.visit(this);
        if(hc_subsequent != null){
            hc_subsequent.hc_traverse(visitor);
        }
    }

	private YapObject hc_remove() {
		if (hc_subsequent != null && hc_preceding != null) {
			hc_subsequent = hc_subsequent.hc_rotateSmallestUp();
			hc_subsequent.hc_preceding = hc_preceding;
			hc_subsequent.hc_calculateSize();
			return hc_subsequent;
		}
		if (hc_subsequent != null) {
			return hc_subsequent;
		}
		return hc_preceding;
	}

	/***** IDTREE *****/

	YapObject id_add(YapObject a_add) {
		a_add.id_preceding = null;
		a_add.id_subsequent = null;
		a_add.id_size = 1;
		return id_add1(a_add);
	}

	private YapObject id_add1(YapObject a_new) {
		int cmp = a_new.i_id - i_id;
		if (cmp < 0) {
			if (id_preceding == null) {
				id_preceding = a_new;
				id_size++;
			} else {
				id_preceding = id_preceding.id_add1(a_new);
				if (id_subsequent == null) {
					return id_rotateRight();
				} else {
					return id_balance();
				}
			}
		} else {
			if (id_subsequent == null) {
				id_subsequent = a_new;
				id_size++;
			} else {
				id_subsequent = id_subsequent.id_add1(a_new);
				if (id_preceding == null) {
					return id_rotateLeft();
				} else {
					return id_balance();
				}
			}
		}
		return this;
	}

	private YapObject id_balance() {
		int cmp = id_subsequent.id_size - id_preceding.id_size;
		if (cmp < -2) {
			return id_rotateRight();
		} else if (cmp > 2) {
			return id_rotateLeft();
		} else {
			id_size = id_preceding.id_size + id_subsequent.id_size + 1;
			return this;
		}
	}

	private void id_calculateSize() {
		if (id_preceding == null) {
			if (id_subsequent == null) {
				id_size = 1;
			} else {
				id_size = id_subsequent.id_size + 1;
			}
		} else {
			if (id_subsequent == null) {
				id_size = id_preceding.id_size + 1;
			} else {
				id_size = id_preceding.id_size + id_subsequent.id_size + 1;
			}
		}
	}

	YapObject id_find(int a_id) {
		int cmp = a_id - i_id;
		if (cmp > 0) {
			if (id_subsequent != null) {
				return id_subsequent.id_find(a_id);
			}
		} else if (cmp < 0) {
			if (id_preceding != null) {
				return id_preceding.id_find(a_id);
			}
		} else {
			return this;
		}
		return null;
	}

	private YapObject id_rotateLeft() {
		YapObject tree = id_subsequent;
		id_subsequent = tree.id_preceding;
		id_calculateSize();
		tree.id_preceding = this;
		if(tree.id_subsequent == null){
			tree.id_size = id_size + 1;
		}else{
			tree.id_size = id_size + 1 + tree.id_subsequent.id_size;
		}
		return tree;
	}

	private YapObject id_rotateRight() {
		YapObject tree = id_preceding;
		id_preceding = tree.id_subsequent;
		id_calculateSize();
		tree.id_subsequent = this;
		if(tree.id_preceding == null){
			tree.id_size = id_size + 1;
		}else{
			tree.id_size = id_size + 1 + tree.id_preceding.id_size;
		}
		return tree;
	}

	private YapObject id_rotateSmallestUp() {
		if (id_preceding != null) {
			id_preceding = id_preceding.id_rotateSmallestUp();
			return id_rotateRight();
		}
		return this;
	}

	YapObject id_remove(int a_id) {
		int cmp = a_id - i_id;
		if (cmp < 0) {
			if (id_preceding != null) {
				id_preceding = id_preceding.id_remove(a_id);
			}
		} else if (cmp > 0) {
			if (id_subsequent != null) {
				id_subsequent = id_subsequent.id_remove(a_id);
			}
		} else {
			return id_remove();
		}
		id_calculateSize();
		return this;
	}

	private YapObject id_remove() {
		if (id_subsequent != null && id_preceding != null) {
			id_subsequent = id_subsequent.id_rotateSmallestUp();
			id_subsequent.id_preceding = id_preceding;
			id_subsequent.id_calculateSize();
			return id_subsequent;
		}
		if (id_subsequent != null) {
			return id_subsequent;
		}
		return id_preceding;
	}
	
	public String toString(){
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
	    try{
		    int id = getID();
		    String str = "YapObject\nID=" + id;
		    if(i_yapClass != null){
		        YapStream stream = i_yapClass.getStream();
		        if(stream != null && id > 0){
		            YapWriter writer = stream.readWriterByID(stream.getTransaction(), id);
		            if(writer != null){
		                str += "\nAddress=" + writer.getAddress();
		            }
		            YapClass yc = readYapClass(writer);
		            if(yc != i_yapClass){
		                str += "\nYapClass corruption";
		            }else{
		                str += yc.toString(writer, this, 0, 5);
		            }
		        }
		    }
		    Object obj = getObject();
		    if(obj == null){
		        str += "\nfor [null]";
		    }else{
		        String objToString ="";
			    try{
			        objToString = obj.toString();
			    }catch(Exception e){
			    }
			    ReflectClass claxx = getYapClass().reflector().forObject(obj);
			    str += "\n" + claxx.getName() + "\n" + objToString;
		    }
		    return str;
	    }catch(Exception e){
	        // e.printStackTrace();
	    }
	    return "Exception in YapObject analyzer";
	}

// Unused, keep for possible future use
    
//    public void remarshall(Transaction trans){
//        YapStream stream = trans.i_stream;
//        YapWriter writer = stream.readWriterByID(trans, getID());
//        writer._offset = 0;
//        if (Deploy.debug) {
//            writer.writeBegin(YapConst.YAPOBJECT, writer.getLength());
//        }
//        writer.writeInt(i_yapClass.getID());
//        i_yapClass.marshall(this, getObject(), writer, false);
//        if (Deploy.debug) {
//            writer.writeEnd();
//            writer.debugCheckBytes();
//        }
//        
//        if(Deploy.debug){
//            if(stream instanceof YapFile){
//                YapFile yf = (YapFile)stream;
//                yf.writeXBytes(writer.getAddress(), writer.getLength());
//            }
//        }
//        writer.writeEncrypt();
//    }
    


    
}
