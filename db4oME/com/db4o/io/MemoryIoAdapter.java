/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.io;

import java.io.*;

import com.db4o.foundation.*;

/**
 * IoAdapter for in-memory operation.
 * <br><br>Configure db4o to operate with this in-memory IoAdapter with
 * <code>MemoryIoAdapter memoryIoAdapter = new MemoryIoAdapter();<br>
 * Db4o.configure().io(memoryIoAdapter);</code><br>
 * <br><br>Use the normal #openFile() and #openServer() commands to 
 * open ObjectContainers and ObjectServers. The names specified as 
 * file names will be used to identify the 
 * <code>byte[]</code> content of the in-memory files in 
 * the _memoryFiles Hashtable in the adapter. After working with an 
 * in-memory ObjectContainer/ObjectServer the <code>byte[]</code> content
 * is available in the MemoryIoAdapter by using    
 * {@link #get(String)}. To add old existing database
 * <code>byte[]</code> content to a MemoryIoAdapter use 
 * {@link #put(String, byte[])}. To reduce memory consumption of memory
 * file names that will no longer be used call {@link #put(String, byte[])}
 * and pass an empty byte array.
 */
public class MemoryIoAdapter extends IoAdapter{

    private byte[] _bytes;
    private int _length;
    private int _seekPos;
    
    private Hashtable4 _memoryFiles;
    private int _growBy;
    
    public MemoryIoAdapter(){
        _memoryFiles = new Hashtable4(1);
        _growBy = 10000;
    }
    
    private MemoryIoAdapter(MemoryIoAdapter adapter, String name, byte[] bytes){
        _bytes = bytes;
        _length = bytes.length;
        _growBy = adapter._growBy;
    }
    
    private MemoryIoAdapter(MemoryIoAdapter adapter, String name, int initialLength){
        this(adapter, name, new byte[initialLength]);
    }
    
    /**
     * creates an in-memory database with the passed content bytes and
     * adds it to the adapter for the specified name. 
     * @param name the name to be use for #openFile() or #openServer() calls 
     * @param bytes the database content
     */
    public void put(String name, byte[] bytes){
        if(bytes == null){
            bytes = new byte[0];
        }
        _memoryFiles.put(name, new MemoryIoAdapter(this,name, bytes));
    }
    
    /**
     * returns the content bytes for a database with the given name. 
     * @param name the name to be use for #openFile() or #openServer() calls
     * @return the content bytes 
     */
    public byte[] get(String name){
        MemoryIoAdapter mia = (MemoryIoAdapter)_memoryFiles.get(name);
        if(mia == null){
            return null;
        }
        return mia._bytes;
    }
    
    /**
     * configures the length a memory file should grow, if no more 
     * free slots are found within.
     * <br><br>Specify a large value (100,000 or more) for best performance.
     * Specify a small value (100) for the smallest memory consumption. The
     * default setting is 10,000.
     * @param length the length in bytes
     */
    public void growBy(int length){
        if(length < 1){
            length = 1;
        }
        _growBy = length;
    }
    
    /**
     * for internal processing only.
     */
    public void close() throws IOException {
        // do nothing
    }
    
    public void delete(String path) {
    	_memoryFiles.remove(path);
    }
    
    /**
     * for internal processing only.
     */
    public boolean exists(String path){
        MemoryIoAdapter mia = (MemoryIoAdapter)_memoryFiles.get(path);
        if(mia == null){
            return false;
        }
        return  mia._length > 0;
    }


    /**
     * for internal processing only.
     */
    public long getLength() throws IOException {
        return _length;
    }

    /**
     * for internal processing only.
     */
    public IoAdapter open(String path, boolean lockFile, long initialLength) throws IOException {
        MemoryIoAdapter mia = (MemoryIoAdapter)_memoryFiles.get(path);
        if(mia == null){
            mia = new MemoryIoAdapter(this, path, (int)initialLength);
            _memoryFiles.put( path, mia);
        }
        return mia;
    }

    /**
     * for internal processing only.
     */
    public int read(byte[] bytes, int length) throws IOException {
        System.arraycopy(_bytes, _seekPos, bytes, 0, length);
        _seekPos += length;
        return length;
    }

    /**
     * for internal processing only.
     */
    public void seek(long pos) throws IOException {
        _seekPos = (int)pos;
    }

    /**
     * for internal processing only.
     */
    public void sync() throws IOException {
    }
    
    /**
     * for internal processing only.
     */
    public void write(byte[] buffer, int length) throws IOException {
        if(_seekPos + length > _bytes.length){
            int growBy = _growBy;
            if(_seekPos + length > growBy){
                growBy = _seekPos + length;
            }
            byte[] temp = new byte[_bytes.length + growBy];
            System.arraycopy(_bytes, 0, temp, 0, _length);
            _bytes = temp;
        }
        System.arraycopy(buffer, 0, _bytes, _seekPos, length);
        _seekPos += length;
        if(_seekPos > _length){
            _length = _seekPos;
        }
    }

}
