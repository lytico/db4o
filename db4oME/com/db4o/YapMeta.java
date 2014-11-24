/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.inside.slots.*;

/**
 * @exclude
 */
public abstract class YapMeta {
    
    int i_id; // UID and address of pointer to the object in our file

    protected int i_state = 2; // DIRTY and ACTIVE

    final boolean beginProcessing() {
        if (bitIsTrue(YapConst.PROCESSING)) {
            return false;
        }
        bitTrue(YapConst.PROCESSING);
        return true;
    }

    final void bitFalse(int bitPos) {
        i_state &= ~(1 << bitPos);
    }
    
    final boolean bitIsFalse(int bitPos) {
        return (i_state | (1 << bitPos)) != i_state;
    }

    final boolean bitIsTrue(int bitPos) {
        return (i_state | (1 << bitPos)) == i_state;
    }

    final void bitTrue(int bitPos) {
        i_state |= (1 << bitPos);
    }

    void cacheDirty(Collection4 col) {
        if (!bitIsTrue(YapConst.CACHED_DIRTY)) {
            bitTrue(YapConst.CACHED_DIRTY);
            col.add(this);
        }
    }

    void endProcessing() {
        bitFalse(YapConst.PROCESSING);
    }

    public int getID() {
        return i_id;
    }

    public abstract byte getIdentifier();

    public final boolean isActive() {
        return bitIsTrue(YapConst.ACTIVE);
    }

    public boolean isDirty() {
        return bitIsTrue(YapConst.ACTIVE) && (!bitIsTrue(YapConst.CLEAN));
    }
    
    public boolean isNew(){
        return i_id == 0;
    }

    public int linkLength() {
        return YapConst.YAPID_LENGTH;
    }

    final void notCachedDirty() {
        bitFalse(YapConst.CACHED_DIRTY);
    }

    public abstract int ownLength();

    public void read(Transaction a_trans) {
        try {
            if (beginProcessing()) {
                YapReader reader = a_trans.i_stream.readReaderByID(a_trans, getID());
                if (reader != null) {
                    if (Deploy.debug) {
                        reader.readBegin(getID(), getIdentifier());
                    }
                    readThis(a_trans, reader);
                    setStateOnRead(reader);
                }
                endProcessing();
            }
        } catch (LongJumpOutException ljoe) {
            throw ljoe;
        } catch (Throwable t) {
            if (Debug.atHome) {
                t.printStackTrace();
            }
        }
    }
    
    public abstract void readThis(Transaction a_trans, YapReader a_reader);


    public void setID(int a_id) {
        i_id = a_id;
    }

    public final void setStateClean() {
        bitTrue(YapConst.ACTIVE);
        bitTrue(YapConst.CLEAN);
    }

    public final void setStateDeactivated() {
        bitFalse(YapConst.ACTIVE);
    }

    public void setStateDirty() {
        bitTrue(YapConst.ACTIVE);
        bitFalse(YapConst.CLEAN);
    }

    void setStateOnRead(YapReader reader) {
        if (Deploy.debug) {
            reader.readEnd();
        }
        if (bitIsTrue(YapConst.CACHED_DIRTY)) {
            setStateDirty();
        } else {
            setStateClean();
        }
    }

    public final void write(Transaction a_trans) {
        
        if (! writeObjectBegin()) {
            return;
        }
            
        YapFile stream = (YapFile)a_trans.i_stream;
        
        int address = 0;
        int length = ownLength();
        
        YapReader writer = new YapReader(length);
        
        if(isNew()){
            Pointer4 ptr = stream.newSlot(a_trans, length);
            setID(ptr._id);
            address = ptr._address;
            
            // FIXME: Free everything on rollback here ?
        }else{
            address = stream.getSlot(length);
            a_trans.slotFreeOnRollbackCommitSetPointer(i_id, address, length);
        }
        
        if (Deploy.debug) {
            writer.writeBegin(getIdentifier(), length);
        }

        writeThis(a_trans, writer);

        if (Deploy.debug) {
            writer.writeEnd();
        }

        ((YapFile)stream).writeObject(this, writer, address);

        if (isActive()) {
            setStateClean();
        }
        endProcessing();

    }

    boolean writeObjectBegin() {
        if (isDirty()) {
            return beginProcessing();
        }
        return false;
    }

    void writeOwnID(Transaction trans, YapReader a_writer) {
        write(trans);
        a_writer.writeInt(getID());
    }

    public abstract void writeThis(Transaction trans, YapReader a_writer);

}
