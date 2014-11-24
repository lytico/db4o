/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public final class BitMap4 {
    
    private final byte[] _bits;
    
    public BitMap4(int numBits){
        _bits = new byte[byteCount(numBits)];
    }

    /** "readFrom  buffer" constructor **/
    public BitMap4(byte[] buffer, int pos, int numBits){
        this(numBits);
        System.arraycopy(buffer, pos, _bits, 0, _bits.length);
    }
    
    public BitMap4(byte singleByte){
    	_bits = new byte[]{singleByte};
    }
    
    public boolean isTrue(int bit) {
        return ((_bits[arrayOffset(bit)]>>>byteOffset(bit))&1)!=0;
    }
    
    public boolean isFalse(int bit) {
        return ! isTrue(bit);
    }

    public int marshalledLength(){
        return _bits.length;
    }
    
    public void setFalse(int bit){
        _bits[arrayOffset(bit)] &= (byte)~bitMask(bit);
    }
    
    public void set(int bit, boolean val){
    	if(val){
    		setTrue(bit);
    	}else{
    		setFalse(bit);
    	}
    }
    
    public void setTrue(int bit){
        _bits[arrayOffset(bit)] |= bitMask(bit);
    }
    
    public void writeTo(byte[] bytes, int pos){
        System.arraycopy(_bits, 0, bytes, pos, _bits.length);
    }
    
	private byte byteOffset(int bit) {
		return (byte)(bit % 8);
	}

	private int arrayOffset(int bit) {
		return bit / 8;
	}
	
	private byte bitMask(int bit) {
		return (byte)(1 << byteOffset(bit));
	}
	
	private int byteCount(int numBits) {
		return (numBits + 7) / 8;
	}

	public byte getByte(int index) {
		return _bits[index];
	}
	
	public byte[] bytes(){
	    return _bits;
	}
	
}
