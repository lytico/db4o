/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * public for .NET conversion reasons.
 * 
 * @exclude
 */
public class YapReader {
	
	// for coding convenience, we allow objects to grab into the buffer
	byte[] _buffer;
	public int _offset;

	
	YapReader(){
	}
	
	public YapReader(int a_length){
		_buffer = new byte[a_length];
	}
	
    void append(byte a_byte) {
        _buffer[_offset++] = a_byte;
    }

    void append(byte[] a_bytes) {
        System.arraycopy(a_bytes, 0, _buffer, _offset, a_bytes.length);
        _offset += a_bytes.length;
    }
    
	final boolean containsTheSame(YapReader other) {
	    if(other != null) {
	        byte[] otherBytes = other._buffer;
	        if(_buffer == null) {
	            return otherBytes == null;
	        }
	        if(otherBytes != null && _buffer.length == otherBytes.length) {
	            int len = _buffer.length;
	            for (int i = 0; i < len; i++) {
	                if(_buffer[i] != otherBytes[i]) {
	                    return false;
	                }
	            }
	            return true;
	        }
	    }
	    return false;
	}
	
	public int getLength() {
		return _buffer.length;
	}
	
    public void incrementOffset(int a_by) {
        _offset += a_by;
    }
    
    /**
     * non-encrypted read, used for indexes
     * @param a_stream
     * @param a_address
     */
    public void read(YapStream a_stream, int a_address, int addressOffset){
        a_stream.readBytes(_buffer, a_address, addressOffset, getLength());
    }
	
	void readBegin(byte a_identifier) {
		if (Deploy.debug) {
			if (Deploy.brackets) {
				if (readByte() != YapConst.YAPBEGIN) {
					throw new RuntimeException("YapBytes.readBegin() YAPBEGIN expected.");
				}
			}
			if (Deploy.identifiers) {
				byte readB = readByte();
				if (readB != a_identifier) {
					throw new RuntimeException("YapBytes.readBegin() wrong identifier");
				}
			}
		}
	}

    public void readBegin(int a_id, byte a_identifier) {
        if (Deploy.debug) {
            readBegin(a_identifier);
        }
    }
	
	public byte readByte() {
		return _buffer[_offset++];
	}
	
	byte[] readBytes(int a_length){
	    byte[] bytes = new byte[a_length];
		readBytes(bytes);
	    return bytes;
	}
	
	void readBytes(byte[] bytes) {
		int length = bytes.length;
	    System.arraycopy(_buffer, _offset, bytes, 0, length);
	    _offset += length;
	}
    
	final YapReader readEmbeddedObject(Transaction a_trans) {
		return a_trans.i_stream.readObjectReaderByAddress(readInt(), readInt());
	}
	
	void readEncrypt(YapStream a_stream, int a_address) {
		a_stream.readBytes(_buffer, a_address, getLength());
		a_stream.i_handlers.decrypt(this);
	}

    void readEnd() {
        if (Deploy.debug && Deploy.brackets) {
            if (readByte() != YapConst.YAPEND) {
                throw new RuntimeException("YapBytes.readEnd() YAPEND expected");
            }
        }
    }

    public final int readInt() {
        if (Deploy.debug) {
            return YInt.readInt(this);
        } else {
            
            // if (YapConst.INTEGER_BYTES == 4) {
                
                int o = (_offset += 4) - 1;
                return (_buffer[o] & 255) | (_buffer[--o] & 255)
                    << 8 | (_buffer[--o] & 255)
                    << 16 | _buffer[--o]
                    << 24;
                
//            } else {
//            	int ret = 0;
//                int ii = _offset + YapConst.INTEGER_BYTES;
//                while (_offset < ii) {
//                    ret = (ret << 8) + (_buffer[_offset++] & 0xff);
//                }
//				return ret;
//            }
                
        }
		
    }

    void replaceWith(byte[] a_bytes) {
        System.arraycopy(a_bytes, 0, _buffer, 0, getLength());
    }
    
    String toString(Transaction a_trans) {
        try {
            return (String)a_trans.i_stream.i_handlers.i_stringHandler.read1(this);
        } catch (Exception e) {
            if(Deploy.debug || Debug.atHome) {
                e.printStackTrace();
            }
        }
        return "";
    }
    
    void writeBegin(byte a_identifier) {
        if (Deploy.debug) {
            if (Deploy.brackets) {
                append(YapConst.YAPBEGIN);
            }
            if (Deploy.identifiers) {
                append(a_identifier);
            }
        }
    }
    
    void writeBegin(byte a_identifier, int a_length) {
        if (Deploy.debug) {
            writeBegin(a_identifier);
        }
    }
    
    void writeEnd() {
        if (Deploy.debug && Deploy.brackets) {
            append(YapConst.YAPEND);
        }
    }
    
    public final void writeInt(int a_int) {
        
        
		// FIXME: variable ii declared outside the loop to circumvent mono bug
		// int ii = YapConst.WRITE_LOOP;
        
        if (Deploy.debug) {
            YInt.writeInt(a_int, this);
        } else {
            
//            if (YapConst.INTEGER_BYTES == 4) {
                
                int o = _offset + 4;
                _offset = o;
                byte[] b = _buffer;
                b[--o] = (byte)a_int;
                b[--o] = (byte) (a_int >>= 8);
                b[--o] = (byte) (a_int >>= 8);
                b[--o] = (byte) (a_int >>= 8);
                
//            } else {
//                for (; ii >= 0; ii -= 8) {
//                    _buffer[_offset++] = (byte) (a_int >> ii);
//                }
//            }
                
        }
    }
    
    public void writeIDOf(Transaction trans, Object obj) {
        if(obj == null){
            writeInt(0);
            return;
        }
        
        if(obj instanceof YapMeta){
            ((YapMeta)obj).writeOwnID(trans, this);
            return;
        }
        
        writeInt(((Integer)obj).intValue());
    }
    
    public void writeIDOf(Transaction trans, YapMeta yapMeta) {
        if(yapMeta == null){
            writeInt(0);
            return;
        }
        yapMeta.writeOwnID(trans, this);
    }
    
    public void writeIDOf(Transaction trans, Integer i) {
        writeInt(i.intValue());
    }
    
    void writeShortString(Transaction trans, String a_string) {
        trans.i_stream.i_handlers.i_stringHandler.writeShort(a_string, this);
    }
    
}
