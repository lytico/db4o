/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import java.io.*;

import com.db4o.foundation.*;
import com.db4o.foundation.network.*;

/**
 * public for .NET conversion reasons
 * 
 * @exclude
 */
public final class YapWriter extends YapReader {

    private int i_address;
    private int _addressOffset;

    private int i_cascadeDelete; 

    private Tree i_embedded;
    private int i_id;

    // carries instantiation depth through the reading process
    private int i_instantionDepth;
    private int i_length;

    Transaction i_trans;

    // carries updatedepth depth through the update process
    // and carries instantiation information through the reading process 
    private int i_updateDepth = 1;

    public YapWriter(Transaction a_trans, int a_initialBufferSize) {
        i_trans = a_trans;
        i_length = a_initialBufferSize;
        _buffer = new byte[i_length];
    }

    public YapWriter(Transaction a_trans, int a_address, int a_initialBufferSize) {
        this(a_trans, a_initialBufferSize);
        i_address = a_address;
    }

    YapWriter(YapWriter parent, YapWriter[] previousRead, int previousCount) {
        previousRead[previousCount++] = this;
        int parentID = parent.readInt();
        i_length = parent.readInt();
        i_id = parent.readInt();
        previousRead[parentID].addEmbedded(this);
        i_address = parent.readInt();
        i_trans = parent.getTransaction();
        _buffer = new byte[i_length];
        System.arraycopy(parent._buffer, parent._offset, _buffer, 0, i_length);
        parent._offset += i_length;
        if (previousCount < previousRead.length) {
            new YapWriter(parent, previousRead, previousCount);
        }
    }

    //	void debug(){
    //		if(Debug.current){
    //			System.out.println("Address: " + i_address + " ID:" + i_id);
    //			if(i_embedded != null){
    //				System.out.println("Children:");
    //				Iterator i = i_embedded.iterator();
    //				while(i.hasNext()){
    //					((YapBytes)(i.next())).debug();
    //				}
    //			}
    //		}
    //	}

    void addEmbedded(YapWriter a_bytes) {
        i_embedded = Tree.add(i_embedded, new TreeIntObject(a_bytes.getID(), a_bytes));
    }


    int appendTo(final YapWriter a_bytes, int a_id) {
        a_id++;
        a_bytes.writeInt(i_length);
        a_bytes.writeInt(i_id);
        a_bytes.writeInt(i_address);
        a_bytes.append(_buffer);
        final int[] newID = { a_id };
        final int myID = a_id;
        forEachEmbedded(new VisitorYapBytes() {
            public void visit(YapWriter a_embedded) {
                a_bytes.writeInt(myID);
                newID[0] = a_embedded.appendTo(a_bytes, newID[0]);
            }
        });
        return newID[0];
    }

    int cascadeDeletes() {
        return i_cascadeDelete;
    }

    void debugCheckBytes() {
        if (Debug.xbytes) {
            if (_offset != i_length) {
                // Db4o.log("!!! YapBytes.debugCheckBytes not all bytes used");
                // This is normal for writing The FreeSlotArray, becauce one
                // slot is possibly reserved by it's own pointer.
            }
        }
    }

    int embeddedCount() {
        final int[] count = { 0 };
        forEachEmbedded(new VisitorYapBytes() {
            public void visit(YapWriter a_bytes) {
                count[0] += 1 + a_bytes.embeddedCount();
            }
        });
        return count[0];
    }

    int embeddedLength() {
        final int[] length = { 0 };
        forEachEmbedded(new VisitorYapBytes() {
            public void visit(YapWriter a_bytes) {
                length[0] += a_bytes.getLength() + a_bytes.embeddedLength();
            }
        });
        return length[0];
    }

    void forEachEmbedded(final VisitorYapBytes a_visitor) {
        if (i_embedded != null) {
            i_embedded.traverse(new Visitor4() {
                public void visit(Object a_object) {
                    a_visitor.visit((YapWriter) ((TreeIntObject)a_object)._object);
                }
            });
        }
    }

    public int getAddress() {
        return i_address;
    }
    
    public int addressOffset(){
        return _addressOffset;
    }

    int getID() {
        return i_id;
    }

    int getInstantiationDepth() {
        return i_instantionDepth;
    }

    public int getLength() {
        return i_length;
    }

    public YapStream getStream() {
        return i_trans.i_stream;
    }

    public Transaction getTransaction() {
        return i_trans;
    }

    int getUpdateDepth() {
        return i_updateDepth;
    }
    
    byte[] getWrittenBytes(){
        byte[] bytes = new byte[_offset];
        System.arraycopy(_buffer, 0, bytes, 0, _offset);
        return bytes;
    }


    public void read() {
        i_trans.i_stream.readBytes(_buffer, i_address,_addressOffset, i_length);
    }

    final boolean read(YapSocket sock) throws IOException {
        int offset = 0;
        int length = i_length;
        while (length > 0) {
            int read = sock.read(_buffer, offset, length);
			if(read<0) {
				return false;
			}
            offset += read;
            length -= read;
        }
		return true;
    }

    final YapWriter readEmbeddedObject() {
        int id = readInt();
        int length = readInt();
        Tree tio = TreeInt.find(i_embedded, id);
        if (tio != null) {
            return (YapWriter) ((TreeIntObject)tio)._object;
        }
        YapWriter bytes = i_trans.i_stream.readObjectWriterByAddress(i_trans, id, length);
        if (bytes != null) {
            bytes.setID(id);
        }
        return bytes;
    }

    final YapWriter readYapBytes() {
        int length = readInt();
        if (length == 0) {
            return null;
        }
        YapWriter yb = new YapWriter(i_trans, length);
        System.arraycopy(_buffer, _offset, yb._buffer, 0, length);
        _offset += length;
        return yb;
    }

    void removeFirstBytes(int aLength) {
        i_length -= aLength;
        byte[] temp = new byte[i_length];
        System.arraycopy(_buffer, aLength, temp, 0, i_length);
        _buffer = temp;
        _offset -= aLength;
        if (_offset < 0) {
            _offset = 0;
        }
    }

    void address(int a_address) {
        i_address = a_address;
    }

    void setCascadeDeletes(int depth) {
        i_cascadeDelete = depth;
    }

    public void setID(int a_id) {
        i_id = a_id;
    }

    void setInstantiationDepth(int a_depth) {
        i_instantionDepth = a_depth;
    }

    void setTransaction(Transaction aTrans) {
        i_trans = aTrans;
    }

    void setUpdateDepth(int a_depth) {
        i_updateDepth = a_depth;
    }

    public void slotDelete() {
        i_trans.slotDelete(i_id, i_address, i_length);
    }
    
    void trim4(int a_offset, int a_length) {
        byte[] temp = new byte[a_length];
        System.arraycopy(_buffer, a_offset, temp, 0, a_length);
        _buffer = temp;
        i_length = a_length;
    }

    void useSlot(int a_adress) {
        i_address = a_adress;
        _offset = 0;
    }

    void useSlot(int a_adress, int a_length) {
        i_address = a_adress;
        _offset = 0;
        if (a_length > _buffer.length) {
            _buffer = new byte[a_length];
        }
        i_length = a_length;
    }

    public void useSlot(int a_id, int a_adress, int a_length) {
        i_id = a_id;
        useSlot(a_adress, a_length);
    }

    void write() {
        if (Debug.xbytes) {
            debugCheckBytes();
        }
        i_trans.i_file.writeBytes(this, i_address, _addressOffset);
    }

    void writeEmbedded() {
        final YapWriter finalThis = this;
        forEachEmbedded(new VisitorYapBytes() {
            public void visit(YapWriter a_bytes) {
                a_bytes.writeEmbedded();
                i_trans.i_stream.writeEmbedded(finalThis, a_bytes);
            }
        });
        
        // TODO: It may be possible to remove the following to 
        // allow indexes to be created from the bytes passed
        // from the client without having to reread. Currently
        // the bytes don't seem to be found and there is a
        // problem with encryption during the write process.

        i_embedded = null; // no reuse !!!
    }

    void writeEmbeddedNull() {
        writeInt(0);
        writeInt(0);
    }

    public void writeEncrypt() {
        if (Deploy.debug) {
            debugCheckBytes();
        }
        i_trans.i_stream.i_handlers.encrypt(this);
        i_trans.i_file.writeBytes(this, i_address, _addressOffset);
        i_trans.i_stream.i_handlers.decrypt(this);
    }


    // turning writing around since our Collection world is the wrong
    // way around
    // TODO: optimize
    final void writeQueryResult(QueryResultImpl a_qr) {
        int size = a_qr.size();
        writeInt(size);
        _offset += (size - 1) * YapConst.YAPID_LENGTH;
        int dec = YapConst.YAPID_LENGTH * 2;
        for (int i = 0; i < size; i++) {
            writeInt(a_qr.nextInt());
            _offset -= dec;
        }
    }

    void writeShortString(String a_string) {
        writeShortString(i_trans, a_string);
    }

    public void moveForward(int length) {
        _addressOffset += length;
    }
    
    public void writeForward() {
        write();
        _addressOffset += i_length;
        _offset = 0;
    }
    
    public String toString(){
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
        return "id " + i_id + " adr " + i_address + " len " + i_length;
    }

}
