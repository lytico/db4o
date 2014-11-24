/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.reflect.*;


/**
 * YapString
 * Legacy rename for C# obfuscator production trouble
 * 
 * @exclude
 */
public final class YapString extends YapIndependantType {
    
    public YapStringIO i_stringIo; 
    
    public YapString(YapStream stream, YapStringIO stringIO) {
        super(stream);
        i_stringIo = stringIO;
    }
    
    public void appendEmbedded3(YapWriter a_bytes) {
        YapWriter bytes = a_bytes.readEmbeddedObject();
        if (bytes != null) {
            a_bytes.addEmbedded(bytes);
        }
    }

    public boolean canHold(ReflectClass claxx) {
        return claxx.equals(classReflector());
    }

    public void cascadeActivation(
        Transaction a_trans,
        Object a_object,
        int a_depth,
        boolean a_activate) {
        // default: do nothing
    }

    public ReflectClass classReflector(){
    	return _stream.i_handlers.ICLASS_STRING;
    }

    public Object comparableObject(Transaction a_trans, Object a_object){
        if(a_object != null){
            int[] slot = (int[]) a_object;
            return a_trans.i_stream.readObjectReaderByAddress(slot[0], slot[1]);
        }
        return null;
    }
    
    public boolean equals(TypeHandler4 a_dataType) {
        return (this == a_dataType);
    }

    public int getID() {
        return 9;
    }

    byte getIdentifier() {
        return YapConst.YAPSTRING;
    }

    public YapClass getYapClass(YapStream a_stream) {
        return a_stream.i_handlers.i_yapClasses[getID() - 1];
    }

    public boolean indexNullHandling() {
        return true;
    }

    public Object read(YapWriter a_bytes) throws CorruptionException {
        i_lastIo = a_bytes.readEmbeddedObject();
        return read1(i_lastIo);
    }
    
    Object read1(YapReader bytes) throws CorruptionException {
		if (bytes == null) {
			return null;
		}
		if (Deploy.debug) {
			bytes.readBegin(0, YapConst.YAPSTRING);
		}
		String ret = readShort(bytes);
		if (Deploy.debug) {
			bytes.readEnd();
		}
		return ret;
    }
    
    public TypeHandler4 readArrayWrapper(Transaction a_trans, YapReader[] a_bytes) {
        // virtual and do nothing
        return null;
    }

    public void readCandidates(YapReader a_bytes, QCandidates a_candidates) {
        // do nothing
    }

    public Object readIndexEntry(YapReader a_reader) {
        return new int[] {a_reader.readInt(), a_reader.readInt()};
    }

	public Object readQuery(Transaction a_trans, YapReader a_reader, boolean a_toArray) throws CorruptionException{
	    YapReader reader = a_reader.readEmbeddedObject(a_trans);
	    if(a_toArray) {
	        if(reader != null) {
	            return reader.toString(a_trans);
	        }
	    }
	    return reader;
	}
	
    final String readShort(YapReader a_bytes) throws CorruptionException {
        int length = a_bytes.readInt();
        if (length > YapConst.MAXIMUM_BLOCK_SIZE) {
            throw new CorruptionException();
        }
        if (length > 0) {
            String str = i_stringIo.read(a_bytes, length);
            if(! Deploy.csharp){
                if(_stream.i_config.internStrings()){
                    str = str.intern();
                }
            }
            return str;
        }
        return "";
    }
    
    void setStringIo(YapStringIO a_io) {
        i_stringIo = a_io;
    }
    
    public boolean supportsIndex() {
        return true;
    }

    public void writeIndexEntry(YapReader a_writer, Object a_object) {
        if(a_object == null){
            a_writer.writeInt(0);
            a_writer.writeInt(0);
        }else{
            int[] slot = (int[])a_object;
            a_writer.writeInt(slot[0]);
            a_writer.writeInt(slot[1]);
        }
    }
    
    public int writeNew(Object a_object, YapWriter a_bytes) {
        if (a_object == null) {
            a_bytes.writeEmbeddedNull();
        } else {
            String str = (String) a_object;
            int length = i_stringIo.length(str);
            YapWriter bytes = new YapWriter(a_bytes.getTransaction(), length);
            if (Deploy.debug) {
                bytes.writeBegin(YapConst.YAPSTRING, length);
            }
            bytes.writeInt(str.length());
            i_stringIo.write(bytes, str);
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

    final void writeShort(String a_string, YapReader a_bytes) {
        if (a_string == null) {
            a_bytes.writeInt(0);
        } else {
            a_bytes.writeInt(a_string.length());
            i_stringIo.write(a_bytes, a_string);
        }
    }

    public int getType() {
        return YapConst.TYPE_SIMPLE;
    }


    // Comparison_______________________

    private YapReader i_compareTo;

    private YapReader val(Object obj) {
        if(obj instanceof YapReader) {
            return (YapReader)obj;
        }
        if(obj instanceof String) {
            String str = (String)obj;
            YapReader reader = new YapReader(i_stringIo.length(str));
            if(Deploy.debug) {
                reader.writeBegin(YapConst.YAPSTRING, i_stringIo.length(str));
            }
            writeShort(str, reader);
            if(Deploy.debug) {
                reader.writeEnd();
            }
            return reader;
        }
        return null;
    }
    
	public void prepareLastIoComparison(Transaction a_trans, Object obj) {
	    if(obj == null) {
	        i_compareTo = null;    
	    }else {
	        i_compareTo = i_lastIo;
	    }
	}

    public YapComparable prepareComparison(Object obj) {
        if (obj == null) {
            i_compareTo = null;
            return Null.INSTANCE;
        }
        i_compareTo = val(obj);
        return this;
    }
    
    public Object current(){
        return i_compareTo;
    }
    
    public int compareTo(Object obj) {
        if(i_compareTo == null) {
            if(obj == null) {
                return 0;
            }
            return 1;
        }
        return compare(i_compareTo, val(obj));
    }

    public boolean isEqual(Object obj) {
        if(i_compareTo == null){
            return obj == null;
        }
        return i_compareTo.containsTheSame(val(obj));
    }

    public boolean isGreater(Object obj) {
        if(i_compareTo == null){
            // this should be called for indexing only
            // object is always greater
            return obj != null;
        }
        return compare(i_compareTo, val(obj)) > 0;
    }

    public boolean isSmaller(Object obj) {
        if(i_compareTo == null){
            // this should be called for indexing only
            // object is always greater
            return false;
        }
        return compare(i_compareTo, val(obj)) < 0;
    }

    /** 
     * returns: -x for left is greater and +x for right is greater 
     *
     * TODO: You will need collators here for different languages.  
     */
    final int compare(YapReader a_compare, YapReader a_with) {
        if (a_compare == null) {
            if (a_with == null) {
                return 0;
            }
            return 1;
        }
        if (a_with == null) {
            return -1;
        }
        return compare(a_compare._buffer, a_with._buffer);
    }
    
    static final int compare(byte[] compare, byte[] with){
        int min = compare.length < with.length ? compare.length : with.length;
        int start = YapConst.YAPINT_LENGTH;
        if(Deploy.debug) {
            start += YapConst.LEADING_LENGTH;
            min -= YapConst.BRACKETS_BYTES;
        }
        for(int i = start;i < min;i++) {
            if (compare[i] != with[i]) {
                return with[i] - compare[i];
            }
            
        }
        return with.length - compare.length;
    }

}
