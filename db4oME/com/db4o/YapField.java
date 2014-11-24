/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.inside.*;
import com.db4o.inside.ix.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;

/**
 * @exclude
 */
public class YapField implements StoredField {

    private YapClass         i_yapClass;

    //  position in YapClass i_fields
    private int              i_arrayPosition;

    protected String         i_name;

    private boolean          i_isArray;

    private boolean          i_isNArray;

    private boolean          i_isPrimitive;

    private ReflectField     i_javaField;

    TypeHandler4              i_handler;

    private int              i_handlerID;

    private int              i_state;

    private static final int NOT_LOADED  = 0;

    private static final int UNAVAILABLE = -1;

    private static final int AVAILABLE   = 1;

    protected Index4        i_index;

    private Config4Field     i_config;

    private Db4oTypeImpl     i_db4oType;

    static final YapField[]  EMPTY_ARRAY = new YapField[0];

    YapField(YapClass a_yapClass) {
        i_yapClass = a_yapClass;
    }

    YapField(YapClass a_yapClass, ObjectTranslator a_translator) {
        // for YapFieldTranslator only
        i_yapClass = a_yapClass;
        init(a_yapClass, a_translator.getClass().getName(), 0);
        i_state = AVAILABLE;
        YapStream stream =getStream(); 
        i_handler = stream.i_handlers.handlerForClass(
            stream, stream.reflector().forClass(a_translator.storedClass()));
    }

    YapField(YapClass a_yapClass, ReflectField a_field, TypeHandler4 a_handler) {
        init(a_yapClass, a_field.getName(), 0);
        i_javaField = a_field;
        i_javaField.setAccessible();
        i_handler = a_handler;
        
        // TODO: beautify !!!  possibly pull up isPrimitive to ReflectField
        boolean isPrimitive = false;
        if(a_field instanceof GenericField){
            isPrimitive  = ((GenericField)a_field).isPrimitive();
        }
        configure( a_field.getType(), isPrimitive);
        checkDb4oType();
        i_state = AVAILABLE;
    }

    void addFieldIndex(YapWriter a_writer, boolean a_new) {
        if (i_index == null) {
            a_writer.incrementOffset(linkLength());
        } else {
            try {
                addIndexEntry(a_writer, i_handler.readIndexValueOrID(a_writer));
            } catch (CorruptionException e) {
            }
        }
    }

    protected void addIndexEntry(YapWriter a_bytes, Object valueOrID) {
        addIndexEntry(a_bytes.getTransaction(), a_bytes.getID(), valueOrID);
    }

    void addIndexEntry(Transaction a_trans, int parentID, Object valueOrID) {
        i_handler.prepareLastIoComparison(a_trans, valueOrID);
        IndexTransaction ift = getIndex(a_trans).dirtyIndexTransaction(a_trans);
        ift.add(parentID, i_handler.indexEntry(valueOrID));
    }
    
    void removeIndexEntry(Transaction trans, int parentID, Object valueOrID){
        i_handler.prepareComparison(valueOrID);
        IndexTransaction ift = getIndex(trans).dirtyIndexTransaction(trans);
        ift.remove(parentID, i_handler.indexEntry(valueOrID));
    }
    

    public boolean alive() {
        if (i_state == AVAILABLE) {
            return true;
        }
        if (i_state == NOT_LOADED) {

            if (i_handler == null) {

                // this may happen if the local YapClassCollection has not
                // been updated from the server and presumably in some
                // refactoring cases. The origin is not verified but we
                // saw a database that had 0 in some wrapper IDs.

                // We try to heal the problem by re-reading the class.

                // This could be inherently dangerous, if the class type of
                // a field was modified.

                // TODO: add class refactoring features

                i_handler = loadJavaField1();
                if (i_handler != null) {
                    if (i_handlerID == 0) {
                        i_handlerID = i_handler.getID();
                    } else {
                        if (i_handler.getID() != i_handlerID) {
                            i_handler = null;
                        }
                    }
                }
            }

            loadJavaField();

            if (i_handler != null) {

                // TODO: This part is not quite correct.
                // We are using the old array information read from file to
                // wrap.

                // If a schema evolution changes an array to a different variable,
                // we are in trouble here.

                i_handler = wrapHandlerToArrays(getStream(), i_handler);

                i_state = AVAILABLE;
                checkDb4oType();
            } else {
                i_state = UNAVAILABLE;
            }
        }
        return i_state == AVAILABLE;

    }

    public void appendEmbedded2(YapWriter a_bytes) {
        if (alive()) {
            i_handler.appendEmbedded3(a_bytes);
        } else {
            a_bytes.incrementOffset(linkLength());
        }
    }
    
    boolean canAddToQuery(String fieldName){
        if(! alive()){
            return false;
        }
        return fieldName.equals(getName())  && getParentYapClass() != null && !getParentYapClass().isInternal(); 
    }

    boolean canHold(ReflectClass claxx) {
        // alive() is checked in QField caller
        if (claxx == null) {
            return !i_isPrimitive;
        }
        return i_handler.canHold(claxx);
    }

    public Object coerce(ReflectClass claxx, Object obj) {
        // alive() is checked in QField caller
        
        if (claxx == null || obj == null) {
            return i_isPrimitive ? No4.INSTANCE : obj;
        }
        return i_handler.coerce(claxx, obj);
    }

    public boolean canLoadByIndex(QConObject a_qco, QE a_evaluator) {
        if (i_handler instanceof YapClass) {
            YapClass yc = (YapClass) i_handler;
            if(yc.isArray()){
                return false;
            }
            if (a_evaluator instanceof QEIdentity) {
                
                yc.i_lastID = a_qco.getObjectID();
            }
        }
        return true;
    }

    void cascadeActivation(Transaction a_trans, Object a_object, int a_depth,
        boolean a_activate) {
        if (alive()) {
            try {
                Object cascadeTo = getOrCreate(a_trans, a_object);
                if (cascadeTo != null && i_handler != null) {
                    i_handler.cascadeActivation(a_trans, cascadeTo, a_depth,
                        a_activate);
                }
            } catch (Exception e) {
            }
        }
    }

    private void checkDb4oType() {
        if (i_javaField != null) {
            if (getStream().i_handlers.ICLASS_DB4OTYPE.isAssignableFrom(i_javaField.getType())) {
                i_db4oType = YapHandlers.getDb4oType(i_javaField.getType());
            }
        }
    }

    void collectConstraints(Transaction a_trans, QConObject a_parent,
        Object a_template, Visitor4 a_visitor) {
        Object obj = getOn(a_trans, a_template);
        if (obj != null) {
            Collection4 objs = Platform4.flattenCollection(a_trans.i_stream, obj);
            Iterator4 j = objs.iterator();
            while (j.hasNext()) {
                obj = j.next();
                if (obj != null) {
                    if (i_isPrimitive) {
                        if (i_handler instanceof YapJavaClass) {
                            if (obj.equals(((YapJavaClass) i_handler)
                                .primitiveNull())) {
                                return;
                            }
                        }
                    }
                    if(Deploy.csharp){
                        if(Platform4.ignoreAsConstraint(obj)){
                            return;
                        }
                    }
                    if (!a_parent.hasObjectInParentPath(obj)) {
                        a_visitor.visit(new QConObject(a_trans, a_parent,
                            qField(a_trans), obj));
                    }
                }
            }
        }
    }

    TreeInt collectIDs(TreeInt tree, YapWriter a_bytes) {
        if (alive()) {
            if (i_handler instanceof YapClass) {
                tree = (TreeInt) Tree.add(tree, new TreeInt(a_bytes.readInt()));
            } else if (i_handler instanceof YapArray) {
                tree = ((YapArray) i_handler).collectIDs(tree, a_bytes);
            }
        }
        return tree;

    }

    void configure(ReflectClass a_class, boolean isPrimitive) {
        i_isPrimitive = isPrimitive | a_class.isPrimitive();
        i_isArray = a_class.isArray();
        if (i_isArray) {
            ReflectArray reflectArray = getStream().reflector().array();
            i_isNArray = reflectArray.isNDimensional(a_class);
            a_class = reflectArray.getComponentType(a_class);
            if (Deploy.csharp) {
            } else {
                i_isPrimitive = a_class.isPrimitive();
            }
            if (i_isNArray) {
                i_handler = new YapArrayN(getStream(), i_handler, i_isPrimitive);
            } else {
                i_handler = new YapArray(getStream(), i_handler, i_isPrimitive);
            }
        }
    }

    void deactivate(Transaction a_trans, Object a_onObject, int a_depth) {
        if (!alive()) {
        	return;
        }
        try {
            boolean isEnumClass = i_yapClass.isEnum();
			if (i_isPrimitive && !i_isArray) {
                if(!isEnumClass) {
                    i_javaField.set(a_onObject, ((YapJavaClass) i_handler)
                        .primitiveNull());
                }
                return;
			}
            if (a_depth > 0) {
                cascadeActivation(a_trans, a_onObject, a_depth, false);
            }
            if(!isEnumClass) {
            	i_javaField.set(a_onObject, null);
            }
        } catch (Throwable t) {
        }
    }

    void delete(YapWriter a_bytes, boolean isUpdate) {
        if (alive()) {
            if (i_index != null) {
                int offset = a_bytes._offset;
                Object obj = null;
                try {
                    obj = i_handler.readIndexValueOrID(a_bytes);
                } catch (CorruptionException e) {
                    if(Debug.atHome){
                        e.printStackTrace();
                    }
                }
                removeIndexEntry(a_bytes.getTransaction(), a_bytes.getID(), obj);
                a_bytes._offset = offset;
            }
            
            boolean dotnetValueType = false;
            if(Deploy.csharp){
            	dotnetValueType = Platform4.isValueType(i_handler.classReflector());	
            }
            
            if ((i_config != null && i_config.cascadeOnDelete() == YapConst.YES)
                || dotnetValueType) {
                int preserveCascade = a_bytes.cascadeDeletes();
                a_bytes.setCascadeDeletes(1);
                i_handler.deleteEmbedded(a_bytes);
                a_bytes.setCascadeDeletes(preserveCascade);
            }else if(i_config != null && i_config.cascadeOnDelete() == YapConst.NO){
                int preserveCascade = a_bytes.cascadeDeletes();
                a_bytes.setCascadeDeletes(0);
                i_handler.deleteEmbedded(a_bytes);
                a_bytes.setCascadeDeletes(preserveCascade);
            } else {
                i_handler.deleteEmbedded(a_bytes);
            }
        }
    }

    public boolean equals(Object obj) {
        if (obj instanceof YapField) {
            YapField yapField = (YapField) obj;
            yapField.alive();
            alive();
            return yapField.i_isPrimitive == i_isPrimitive
                && yapField.i_handler.equals(i_handler)
                && yapField.i_name.equals(i_name);
        }
        return false;
    }

    public Object get(Object a_onObject) {
        if (i_yapClass != null) {
            YapStream stream = i_yapClass.getStream();
            if (stream != null) {
                synchronized (stream.i_lock) {
                    stream.checkClosed();
                    YapObject yo = stream.getYapObject(a_onObject);
                    if (yo != null) {
                        int id = yo.getID();
                        if (id > 0) {
                            YapWriter writer = stream.readWriterByID(stream
                                .getTransaction(), id);
                            if (writer != null) {
                                if (i_yapClass.findOffset(writer, this)) {
                                    try {
                                        return read(writer);
                                    } catch (CorruptionException e) {
                                        if (Debug.atHome) {
                                            e.printStackTrace();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    public String getName() {
        return i_name;
    }

    YapClass getFieldYapClass(YapStream a_stream) {
        // alive needs to be checked by all callers: Done
		return i_handler.getYapClass(a_stream);
    }
    
    Index4 getIndex(Transaction a_trans){
        return i_index;
    }

    Tree getIndexRoot(Transaction a_trans) {
        return getIndex(a_trans).indexTransactionFor(a_trans).getRoot();
    }

    TypeHandler4 getHandler() {
        // alive needs to be checked by all callers: Done
        return i_handler;
    }

    Object getOn(Transaction a_trans, Object a_OnObject) {
        if (alive()) {
            try {
                return i_javaField.get(a_OnObject);
            } catch (Throwable t) {
            }
            // this is typically the case, if a field is removed from an
            // object.
        }
        return null;
    }

    /**
     * dirty hack for com.db4o.types some of them need to be set automatically
     * TODO: Derive from YapField for Db4oTypes
     */
    Object getOrCreate(Transaction a_trans, Object a_OnObject) {
        if (alive()) {
            try {
                Object obj = i_javaField.get(a_OnObject);
                if (i_db4oType != null) {
                    if (obj == null) {
                        obj = i_db4oType.createDefault(a_trans);
                        i_javaField.set(a_OnObject, obj);
                    }
                }
                return obj;
            } catch (Throwable t) {
                if(Debug.atHome){
                    t.printStackTrace();
                }
            }
            // this is typically the case, if a field is removed from an
            // object.
        }
        return null;
    }

    YapClass getParentYapClass() {
        // alive needs to be checked by all callers: Done
        return i_yapClass;
    }

    public ReflectClass getStoredType() {
        if (!Deploy.csharp) {
            if (i_isPrimitive) {
                return i_handler.primitiveClassReflector();
            }
        }
        return i_handler.classReflector();
    }
    
    public YapStream getStream(){
    	if(i_yapClass == null){
    		return null;
    	}
    	return i_yapClass.getStream();
    }

    boolean hasIndex() {
        // alive needs to be checked by all callers: Done
        return i_index != null;
    }

    void incrementOffset(YapReader a_bytes) {
        a_bytes.incrementOffset(linkLength());
    }

    void init(YapClass a_yapClass, String a_name, int syntheticforJad) {
        i_yapClass = a_yapClass;
        i_name = a_name;
        if (a_yapClass.i_config != null) {
            i_config = a_yapClass.i_config.configField(a_name);
            if (Debug.configureAllFields) {
                if (i_config == null) {
                    i_config = (Config4Field) a_yapClass.i_config
                        .objectField(i_name);
                }
            }
            if (Debug.indexAllFields) {
                i_config.indexed(true);
            }
        }
    }

    void initConfigOnUp(Transaction trans) {
        if (i_config != null) {
            i_config.initOnUp(trans, this);
        }
    }

    void initIndex(Transaction systemTrans, MetaIndex metaIndex) {
        if (supportsIndex()) {
            i_index = new Index4(systemTrans, getHandler(), metaIndex, i_handler.indexNullHandling());
        }
    }

    void instantiate(YapObject a_yapObject, Object a_onObject, YapWriter a_bytes)
        throws CorruptionException {
        if (alive()) {
            Object toSet = null;
            try {
                toSet = read(a_bytes);
            } catch (Exception e) {
                e.printStackTrace();
                throw new CorruptionException();
            }
            if (i_db4oType != null) {
                if (toSet != null) {
                    ((Db4oTypeImpl) toSet).setTrans(a_bytes.getTransaction());
                }
            }
            try {
                i_javaField.set(a_onObject, toSet);
            } catch (Throwable t) {
                if(Debug.atHome){
                    t.printStackTrace();
                }
            }
        }
    }

    public boolean isArray() {
        return i_isArray;
    }

    public int linkLength() {
        alive();
        if (i_handler == null) {
            // must be a YapClass
            return YapConst.YAPID_LENGTH;
        }
        return i_handler.linkLength();
    }

    void loadHandler(YapStream a_stream) {
        if (i_handlerID < 1) {
            i_handler = null;
        } else if (i_handlerID <= a_stream.i_handlers.maxTypeID()) {
            i_handler = a_stream.i_handlers.getHandler(i_handlerID);
        } else {
            i_handler = a_stream.getYapClass(i_handlerID);
        }
    }

    private void loadJavaField() {
        TypeHandler4 handler = loadJavaField1();
        if (handler == null || (!handler.equals(i_handler))) {
            i_javaField = null;
            i_state = UNAVAILABLE;
        }
    }

    private TypeHandler4 loadJavaField1() {
        try {
        	YapStream stream = i_yapClass.getStream();
            i_javaField = i_yapClass.classReflector().getDeclaredField(
                i_name);
            if (i_javaField == null) {
                return null;
            }
            i_javaField.setAccessible();
            stream.showInternalClasses(true);
            TypeHandler4 handler = stream.i_handlers.handlerForClass(stream,
                i_javaField.getType());
            stream.showInternalClasses(false);
            return handler;
        } catch (Exception e) {
            if (Debug.atHome) {
                e.printStackTrace();
            }
        }
        return null;
    }

    void marshall(YapObject a_yapObject, Object a_object, YapWriter a_bytes,
        Config4Class a_config, boolean a_new) {
        // alive needs to be checked by all callers: Done
		
		int memberId = 0;

        if (a_object != null
            && ((a_config != null && (a_config.cascadeOnUpdate() == YapConst.YES)) || (i_config != null && (i_config.cascadeOnUpdate() == YapConst.YES)))) {
            int min = 1;
            if (i_yapClass.isCollection(a_object)) {
            	GenericReflector reflector = i_yapClass.reflector();
                min = reflector.collectionUpdateDepth(reflector.forObject(a_object));
            }
            int updateDepth = a_bytes.getUpdateDepth();
            if (updateDepth < min) {
                a_bytes.setUpdateDepth(min);
            }
            memberId = i_handler.writeNew(a_object, a_bytes);
            a_bytes.setUpdateDepth(updateDepth);
        } else {
            memberId = i_handler.writeNew(a_object, a_bytes);
        }
        if (i_index != null) {
            if(memberId == -1){
                // primitive
                addIndexEntry(a_bytes, a_object);
            }else{
                // first class object
                addIndexEntry(a_bytes, new Integer(memberId));
            }
        }
    }

    int ownLength(YapStream a_stream) {
        return a_stream.stringIO().shortLength(i_name) + 1
            + YapConst.YAPID_LENGTH;
    }

    YapComparable prepareComparison(Object obj) {
        if (alive()) {
            i_handler.prepareComparison(obj);
            return i_handler;
        }
        return null;
    }

    QField qField(Transaction a_trans) {
        int yapClassID = 0;
        if(i_yapClass != null){
            yapClassID = i_yapClass.getID();
        }
        return new QField(a_trans, i_name, this, yapClassID, i_arrayPosition);
    }

    Object read(YapWriter a_bytes) throws CorruptionException {
        if (!alive()) {
            return null;
        }
        return i_handler.read(a_bytes);
    }

    Object readQuery(Transaction a_trans, YapReader a_reader)
        throws CorruptionException {
        return i_handler.readQuery(a_trans, a_reader, false);
    }

    YapField readThis(YapStream a_stream, YapReader a_reader) {
        try {
            i_name = a_stream.i_handlers.i_stringHandler.readShort(a_reader);
        } catch (CorruptionException ce) {
            i_handler = null;
            return this;
        }
        if (i_name.indexOf(YapConst.VIRTUAL_FIELD_PREFIX) == 0) {
            YapFieldVirtual[] virtuals = a_stream.i_handlers.i_virtualFields;
            for (int i = 0; i < virtuals.length; i++) {
                if (i_name.equals(virtuals[i].i_name)) {
                    return virtuals[i];
                }
            }
        }
        init(i_yapClass, i_name, 0);
        i_handlerID = a_reader.readInt();
        YapBit yb = new YapBit(a_reader.readByte());
        i_isPrimitive = yb.get();
        i_isArray = yb.get();
        i_isNArray = yb.get();
        return this;
    }
    
    public void readVirtualAttribute(Transaction a_trans, YapReader a_reader, YapObject a_yapObject) {
        a_reader.incrementOffset(i_handler.linkLength());
    }

    void refresh() {
        TypeHandler4 handler = loadJavaField1();
        if (handler != null) {
            handler = wrapHandlerToArrays(getStream(), handler);
            if (handler.equals(i_handler)) {
                return;
            }
        }
        i_javaField = null;
        i_state = UNAVAILABLE;
    }

    public void rename(String newName) {
        YapStream stream = i_yapClass.getStream();
        if (! stream.isClient()) {
            i_name = newName;
            i_yapClass.setStateDirty();
            i_yapClass.write(stream.getSystemTransaction());
        } else {
            Exceptions4.throwRuntimeException(58);
        }
    }

    void setArrayPosition(int a_index) {
        i_arrayPosition = a_index;
    }

    void setName(String a_name) {
        i_name = a_name;
    }

    boolean supportsIndex() {
        return alive() && i_handler.supportsIndex();
    }

    private final TypeHandler4 wrapHandlerToArrays(YapStream a_stream, TypeHandler4 a_handler) {
        if (i_isNArray) {
            a_handler = new YapArrayN(a_stream, a_handler, i_isPrimitive);
        } else {
            if (i_isArray) {
                a_handler = new YapArray(a_stream, a_handler, i_isPrimitive);
            }
        }
        return a_handler;
    }

    void writeThis(Transaction trans, YapReader a_writer, YapClass a_onClass) {
        alive();
        a_writer.writeShortString(trans, i_name);
        if (i_handler instanceof YapClass) {
            if (i_handler.getID() == 0) {
                trans.i_stream.needsUpdate(a_onClass);
            }
        }
        int wrapperID = 0;
        try {
            // The wrapper can be null and it can fail to
            // deliver the ID.

            // In this case the field is dead.

            wrapperID = i_handler.getID();
        } catch (Exception e) {
            if (Debug.atHome) {
                e.printStackTrace();
            }
        }
        if (wrapperID == 0) {
            wrapperID = i_handlerID;
        }
        a_writer.writeInt(wrapperID);
        YapBit yb = new YapBit(0);
        yb.set(i_handler instanceof YapArrayN); // keep the order
        yb.set(i_handler instanceof YapArray);
        yb.set(i_isPrimitive);
        a_writer.append(yb.getByte());
    }

    public String toString() {
        StringBuffer sb = new StringBuffer();
        if (Debug.prettyToStrings) {
            sb.append("YapField ");
            sb.append(i_name);
            sb.append("\n");
            if (i_index != null) {
                sb.append(i_index.toString());
            }

        } else {
            if (i_yapClass != null) {
                sb.append(i_yapClass.getName());
                sb.append(".");
                sb.append(getName());
            }
        }
        return sb.toString();
    }

    public String toString(YapWriter writer, YapObject yapObject, int depth, int maxDepth) throws CorruptionException {
        String str = "\n Field " + i_name;
        if (! alive()) {
            writer.incrementOffset(linkLength());
        }else{
            Object obj = null;
            try{
                obj = read(writer);
            }catch(Exception e){
                // can happen
            }
            if(obj == null){
                str += "\n [null]";
            }else{
                str+="\n  " + obj.toString();
            }
        }
        return str;
    }

}