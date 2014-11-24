package com.db4o.io;

import java.io.*;

import com.db4o.*;

/**
 * Base class for database file adapters, both for file and memory 
 * databases.
 */
public abstract class IoAdapter {
    
    private static final int COPY_SIZE = 4096;
    
	private int _blockSize;
    
    /**
     * converts address and address offset to an absolute address 
     */
    protected final long regularAddress(int blockAddress, int blockAddressOffset){
        return (long)blockAddress * _blockSize + blockAddressOffset;
    }
    
    /**
     * copies a block within a file in block mode 
     */
    public void blockCopy(int oldAddress, int oldAddressOffset, int newAddress, int newAddressOffset, int length) throws IOException{
        copy(regularAddress(oldAddress, oldAddressOffset), regularAddress(newAddress, newAddressOffset), length);
    }
    
    /**
     * sets the read/write pointer in the file using block mode 
     */
	public void blockSeek(int address) throws IOException {
		blockSeek(address,0);
	}

    /**
     * sets the read/write pointer in the file using block mode 
     */
	public void blockSeek(int address, int offset)
			throws IOException {		
		seek(regularAddress(address,offset));
	}

	/**
     * outside call to set the block size of this adapter 
	 */
    public void blockSize(int blockSize) {
		_blockSize=blockSize;
	}

	/**
     * implement to close the adapter 
	 */
    public abstract void close() throws IOException;

    /**
     * copies a block within a file in absolute mode 
     */
    public void copy(long oldAddress, long newAddress, int length) throws IOException{
        
        if(DTrace.enabled){
            DTrace.IO_COPY.logLength(newAddress, length);
        }
        
        if(length > COPY_SIZE){
            byte[] buffer = new byte[COPY_SIZE];
            int pos = 0;
            while(pos + COPY_SIZE < length){
                copy(buffer, oldAddress + pos, newAddress + pos);
                pos+= COPY_SIZE;
            }
            oldAddress += pos;
            newAddress += pos;
            length -= pos;
        }
        
        copy(new byte[length], oldAddress, newAddress);
    }
    
    
    private void copy(byte[] buffer, long oldAddress, long newAddress) throws IOException {
        seek(oldAddress);
        read(buffer);
        seek(newAddress);
        write(buffer);
    }
    
    /**
     * deletes the given path from whatever 'file system' is addressed
     */
    public abstract void delete(String path);
    
    /**
     * checks whether a file exists 
     */
    public abstract boolean exists(String path);
    
	/**
     * implement to return the absolute length of the file 
	 */
    public abstract long getLength() throws IOException;

    /**
     * implement to open the file 
     */
    public abstract IoAdapter open(String path, boolean lockFile, long initialLength) throws IOException;

    /**
     * reads a buffer at the seeked address 
     */
	public int read(byte[] buffer) throws IOException {
		return read(buffer,buffer.length);		
	}

    /**
     * implement to read a buffer at the seeked address 
     */
	public abstract int read(byte[] bytes, int length) throws IOException;

    /**
     * implement to set the read/write pointer in the file, absolute mode 
     */
	public abstract void seek(long pos) throws IOException;

	/**
     * implement to flush the file contents to storage 
	 */
    public abstract void sync() throws IOException;

	/**
     * writes a buffer to the seeked address 
	 */
    public void write(byte[] bytes) throws IOException {
		write(bytes,bytes.length);
	}

    /**
     * implement to write a buffer at the seeked address 
     */
	public abstract void write(byte[] buffer, int length) throws IOException;
	
	/**
     * returns the block size currently used 
	 */
    public int blockSize() {
		return _blockSize;
	}


}