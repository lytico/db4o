/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.ext.*;

/**
 * 
 */
class YapFieldVersion extends YapFieldVirtual {

    YapFieldVersion(YapStream stream) {
        super();
        i_name = VirtualField.VERSION;
        i_handler = new YLong(stream);
    }
    
    void addFieldIndex(YapWriter a_writer, boolean a_new) {
        YLong.writeLong(a_writer.getStream().bootRecord().nextVersion(), a_writer);
    }
    
    void delete(YapWriter a_bytes, boolean isUpdate) {
        a_bytes.incrementOffset(linkLength());
    }

    void instantiate1(Transaction a_trans, YapObject a_yapObject, YapReader a_bytes) {
        a_yapObject.i_virtualAttributes.i_version = YLong.readLong(a_bytes);
    }

    void marshall1(YapObject a_yapObject, YapWriter a_bytes, boolean a_migrating, boolean a_new) {
        YapStream stream = a_bytes.getStream().i_parent;
        PBootRecord br = stream.bootRecord();
        if (!a_migrating) {
            if (br != null) {
                a_yapObject.i_virtualAttributes.i_version = br.nextVersion();
            }
        }else{
            if(br != null){
                br.setDirty();
            }
        }
        if(a_yapObject.i_virtualAttributes == null){
            YLong.writeLong(0, a_bytes);
        }else{
            YLong.writeLong(a_yapObject.i_virtualAttributes.i_version, a_bytes);
        }
    }

    public int linkLength() {
        return YapConst.YAPLONG_LENGTH;
    }
    
    void marshallIgnore(YapWriter writer) {
        YLong.writeLong(0, writer);
    }


}