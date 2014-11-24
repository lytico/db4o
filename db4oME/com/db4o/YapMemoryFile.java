/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import java.io.*;

import com.db4o.ext.*;
import com.db4o.inside.*;

/**
 * @exclude
 */
public class YapMemoryFile extends YapFile {
    
    private boolean i_closed = false;
    final MemoryFile i_memoryFile;
    private int i_length = 0;

    protected YapMemoryFile(YapStream a_parent, MemoryFile memoryFile) {
        super(a_parent);
        this.i_memoryFile = memoryFile;
        try {
            open();
        } catch (Exception e) {
            Exceptions4.throwRuntimeException(22, e);
        }
        initialize3();
    }

    YapMemoryFile(MemoryFile memoryFile) {
        this(null, memoryFile);
    }
    
    public void backup(String path)throws IOException{
        Exceptions4.throwRuntimeException(60);
    }

    void checkDemoHop() {
        // do nothing
    }

    boolean close2() {
        i_entryCounter++;
        if (Deploy.debug) {
            write(true);
        } else {
            try {
                write(true);
            } catch (Throwable t) {
                fatalException(t);
            }
        }
        super.close2();
        i_entryCounter--;
        if (i_closed == false) {
            byte[] temp = new byte[i_length];
            System.arraycopy(i_memoryFile.getBytes(), 0, temp, 0, i_length);
            i_memoryFile.setBytes(temp);
        }
        i_closed = true;
        return true;
    }

    public void copy(int oldAddress, int oldAddressOffset, int newAddress, int newAddressOffset, int length) {
        byte[] bytes = memoryFileBytes(newAddress + newAddressOffset + length);
        System.arraycopy(bytes, oldAddress + oldAddressOffset, bytes, newAddress + newAddressOffset, length);
    }

    void emergencyClose() {
        super.emergencyClose();
        i_closed = true;
    }

    long fileLength() {
        return i_length;
    }

    String fileName() {
        return "Memory File";
    }

    boolean hasShutDownHook() {
        return false;
    }

    boolean needsLockFileThread() {
        return false;
    }

    private void open() {
        byte[] bytes = i_memoryFile.getBytes();
        if (bytes == null || bytes.length == 0) {
            i_memoryFile.setBytes(new byte[i_memoryFile.getInitialSize()]);
            configureNewFile();
            write(false);
            writeHeader(false);
        } else {
            i_length = bytes.length;
            readThis();
        }
    }

    void readBytes(byte[] a_bytes, int a_address, int a_length) {
        try {
            System.arraycopy(i_memoryFile.getBytes(), a_address, a_bytes, 0, a_length);
        } catch (Exception e) {
            Exceptions4.throwRuntimeException(13, e);
        }
    }
    
    void readBytes(byte[] bytes, int address, int addressOffset, int length){
        readBytes(bytes, address + addressOffset, length);
    }

    public void syncFiles() {
    }

    boolean writeAccessTime() {
        return true;
    }

    void writeBytes(YapReader a_bytes, int address, int addressOffset) {
        int fullAddress = address + addressOffset;
        int length = a_bytes.getLength(); 
        System.arraycopy(a_bytes._buffer, 0, memoryFileBytes(fullAddress + length), fullAddress , length);
    }

    private byte[] memoryFileBytes(int a_lastByte) {
        byte[] bytes = i_memoryFile.getBytes();
        if (a_lastByte > i_length) {
            if (a_lastByte > bytes.length) {
                int increase = a_lastByte - bytes.length;
                if (increase < i_memoryFile.getIncrementSizeBy()) {
                    increase = i_memoryFile.getIncrementSizeBy();
                }
                byte[] temp = new byte[bytes.length + increase];
                System.arraycopy(bytes, 0, temp, 0, bytes.length);
                i_memoryFile.setBytes(temp);
                bytes = temp;
            }
            i_length = a_lastByte;
        }
        return bytes;
    }
    
    public void writeXBytes(int a_address, int a_length) {
        // do nothing
    }

}
