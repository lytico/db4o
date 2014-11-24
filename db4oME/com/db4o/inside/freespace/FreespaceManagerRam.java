/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.freespace;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.inside.slots.*;


public class FreespaceManagerRam extends FreespaceManager {
    
    private final TreeIntObject _finder   = new TreeIntObject(0);

    private Tree _freeByAddress;
    
    private Tree _freeBySize;
    
    public FreespaceManagerRam(YapFile file){
        super(file);
    }
    
    private void addFreeSlotNodes(int a_address, int a_length) {
        FreeSlotNode addressNode = new FreeSlotNode(a_address);
        addressNode.createPeer(a_length);
        _freeByAddress = Tree.add(_freeByAddress, addressNode);
        _freeBySize = Tree.add(_freeBySize, addressNode._peer);
    }

    public void beginCommit() {
        // do nothing
    }
    
    public void debug(){
        if(Debug.freespace){
            System.out.println("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            System.out.println("Dumping RAM based address index");
            _freeByAddress.traverse(new Visitor4() {
            
                public void visit(Object a_object) {
                    System.out.println(a_object);
                }
            
            });
            System.out.println("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            System.out.println("Dumping RAM based length index");
            _freeBySize.traverse(new Visitor4() {
                  public void visit(Object a_object) {
                      System.out.println(a_object);
                  }
              });
            System.out.println("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        }
    }
    
    public void endCommit() {
        // do nothing
    }
    
    public void free(int a_address, int a_length) {
        
        if (a_address <= 0) {
            return;
        }
        
        if (a_length <= discardLimit()) {
            return;
        }
        
        if(DTrace.enabled){
            DTrace.FREE_RAM.logLength(a_address, a_length);
        }
        
        a_length = _file.blocksFor(a_length);
        _finder._key = a_address;
        FreeSlotNode sizeNode;
        FreeSlotNode addressnode = (FreeSlotNode) Tree.findSmaller(_freeByAddress, _finder);
        if ((addressnode != null)
            && ((addressnode._key + addressnode._peer._key) == a_address)) {
            sizeNode = addressnode._peer;
            _freeBySize = _freeBySize.removeNode(sizeNode);
            sizeNode._key += a_length;
            FreeSlotNode secondAddressNode = (FreeSlotNode) Tree
                .findGreaterOrEqual(_freeByAddress, _finder);
            if ((secondAddressNode != null)
                && (a_address + a_length == secondAddressNode._key)) {
                sizeNode._key += secondAddressNode._peer._key;
                _freeBySize = _freeBySize
                    .removeNode(secondAddressNode._peer);
                _freeByAddress = _freeByAddress
                    .removeNode(secondAddressNode);
            }
            sizeNode.removeChildren();
            _freeBySize = Tree.add(_freeBySize, sizeNode);
        } else {
            addressnode = (FreeSlotNode) Tree.findGreaterOrEqual(
                _freeByAddress, _finder);
            if ((addressnode != null)
                && (a_address + a_length == addressnode._key)) {
                sizeNode = addressnode._peer;
                _freeByAddress = _freeByAddress.removeNode(addressnode);
                _freeBySize = _freeBySize.removeNode(sizeNode);
                sizeNode._key += a_length;
                addressnode._key = a_address;
                addressnode.removeChildren();
                sizeNode.removeChildren();
                _freeByAddress = Tree.add(_freeByAddress, addressnode);
                _freeBySize = Tree.add(_freeBySize, sizeNode);
            } else {
                addFreeSlotNodes(a_address, a_length);
            }
        }
        if (Debug.xbytes) {
            if(! Debug.freespaceChecker){
                _file.writeXBytes(a_address, a_length * blockSize());
            }
        }
    }
    
    public void freeSelf() {
        // Do nothing.
        // The RAM manager frees itself on reading.
    }

    
    public int getSlot(int length) {
        int address = getSlot1(length);
        
        if(address != 0){
            
            if(DTrace.enabled){
                DTrace.GET_FREESPACE_RAM.logLength(address, length);
            }
        }
        return address;
    }
    
    public int getSlot1(int length) {
        length = _file.blocksFor(length);
        _finder._key = length;
        _finder._object = null;
        _freeBySize = FreeSlotNode.removeGreaterOrEqual((FreeSlotNode) _freeBySize, _finder);

        if (_finder._object == null) {
            return 0;
        }
            
        FreeSlotNode node = (FreeSlotNode) _finder._object;
        int blocksFound = node._key;
        int address = node._peer._key;
        _freeByAddress = _freeByAddress.removeNode(node._peer);
        if (blocksFound > length) {
            addFreeSlotNodes(address + length, blocksFound - length);
        }
        return address;
    }
    

    public void migrate(final FreespaceManager newFM) {
        if(_freeByAddress != null){
            _freeByAddress.traverse(new Visitor4() {
                public void visit(Object a_object) {
                    FreeSlotNode fsn = (FreeSlotNode)a_object;
                    int address = fsn._key;
                    int length = fsn._peer._key;
                    newFM.free(address, length);
                }
            });
        }
    }
    
    public void read(int freeSlotsID) {
        if (freeSlotsID <= 0){
            return;
        }
        if(discardLimit() == Integer.MAX_VALUE){
            return;
        }
        YapWriter reader = _file.readWriterByID(trans(), freeSlotsID);
        if (reader == null) {
            return;
        }

        FreeSlotNode.sizeLimit = discardLimit();

        _freeBySize = new TreeReader(reader, new FreeSlotNode(0), true).read();

        final Tree[] addressTree = new Tree[1];
        if (_freeBySize != null) {
            _freeBySize.traverse(new Visitor4() {

                public void visit(Object a_object) {
                    FreeSlotNode node = ((FreeSlotNode) a_object)._peer;
                    addressTree[0] = Tree.add(addressTree[0], node);
                }
            });
        }
        _freeByAddress = addressTree[0];
        
        if(! Debug.freespace){
          _file.free(freeSlotsID, YapConst.POINTER_LENGTH);
          _file.free(reader.getAddress(), reader.getLength());
        }
    }
    
    public void start(int slotAddress) {
        // this is done in read(), nothing to do here
    }
    
    public byte systemType() {
        return FM_RAM;
    }
    
    private final Transaction trans(){
        return _file.i_systemTrans;
    }

    public int write(boolean shuttingDown){
        if(! shuttingDown){
            return 0;
        }
        int freeBySizeID = 0;
        int length = Tree.byteCount(_freeBySize);
        
        Pointer4 ptr = _file.newSlot(trans(), length); 
        freeBySizeID = ptr._id;
        YapWriter sdwriter = new YapWriter(trans(), length);
        sdwriter.useSlot(freeBySizeID, ptr._address, length);
        Tree.write(sdwriter, _freeBySize);
        sdwriter.writeEncrypt();
        trans().writePointer(ptr._id, ptr._address, length);
        return freeBySizeID;
    }


}
