/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.freespace;

import com.db4o.*;


public abstract class FreespaceManager {
    
    final YapFile     _file;

    public static final byte FM_DEFAULT = 0;
    public static final byte FM_LEGACY_RAM = 1;
    public static final byte FM_RAM = 2;
    public static final byte FM_IX = 3;
    
    private static final int INTS_IN_SLOT = 12;
    
    FreespaceManager(YapFile file){
        _file = file;
    }

    public static byte checkType(byte systemType){
        if(systemType == FM_DEFAULT){
            return FM_RAM;    
        }
        return systemType;
    }
    
    public static FreespaceManager createNew(YapFile file, byte systemType){
        systemType = checkType(systemType);
        switch(systemType){
            case FM_LEGACY_RAM:
            case FM_RAM:
                return new FreespaceManagerRam(file);
            default:
                return new FreespaceManagerIx(file);
        }
    }
    
    public static int initSlot(YapFile file){
        int address = file.getSlot(slotLength());
        slotEntryToZeroes(file, address);
        return address;
    }
    
    static void slotEntryToZeroes(YapFile file, int address){
        YapWriter writer = new YapWriter(file.i_systemTrans, address, slotLength());
        for (int i = 0; i < INTS_IN_SLOT; i++) {
            writer.writeInt(0);
        }
        if (Debug.xbytes) {
            writer.setID(YapConst.IGNORE_ID);  // no XBytes check
        }
        writer.writeEncrypt();
    }
    
    
    final static int slotLength(){
        return YapConst.YAPINT_LENGTH * INTS_IN_SLOT;
    }
    
    public abstract void beginCommit();
    
    final int blockSize(){
        return _file.blockSize();
    }
    
    public abstract void debug();
    
    final int discardLimit(){
        return _file.i_config.discardFreeSpace();
    }
    
    public abstract void endCommit();
    
    public abstract void free(int a_address, int a_length);
    
    public abstract void freeSelf();
    
    public abstract int getSlot(int length);
    
    public abstract void migrate(FreespaceManager newFM);
    
    public abstract void read(int freeSlotsID);
    
    public abstract void start(int slotAddress);
    
    public abstract byte systemType();
    
    public abstract int write(boolean shuttingDown);
    
}
