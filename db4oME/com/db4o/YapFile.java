/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import java.io.*;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.inside.*;
import com.db4o.inside.btree.*;
import com.db4o.inside.freespace.*;
import com.db4o.inside.slots.*;
import com.db4o.reflect.*;

/**
 * @exclude
 */
public abstract class YapFile extends YapStream {

    protected YapConfigBlock    _configBlock;
    
    private PBootRecord         _bootRecord;

    private Collection4         i_dirty;
    
    private FreespaceManager _freespaceManager;
    
    // can be used to check freespace system
    private FreespaceManager _fmChecker;

    private boolean             i_isServer = false;

    private Tree                i_prefetchedIDs;

    private Hashtable4          i_semaphores;

    int                         i_writeAt;
    
    private Tree                _freeOnCommit;
    

    YapFile(YapStream a_parent) {
        super(a_parent);
    }
    
    public byte blockSize(){
        return 1;
    }

    
    void blockSize(int blockSize){
        // do nothing, overwridden in YapRandomAccessFile 
    }
    
    public PBootRecord bootRecord(){
        return _bootRecord;
    }
    
    boolean close2() {
        boolean ret = super.close2();
        i_dirty = null;
        return ret;
    }

    void commit1() {
        checkClosed();
        i_entryCounter++;
        try {
            write(false);
        } catch (Throwable t) {
            fatalException(t);
        }
        i_entryCounter--;
    }

    void configureNewFile() {
        
        _freespaceManager = FreespaceManager.createNew(this, i_config.freespaceSystem());
        
        if(Debug.freespaceChecker){
            _fmChecker = new FreespaceManagerRam(this);
        }
        
        blockSize(i_config.blockSize());
        i_writeAt = blocksFor(HEADER_LENGTH);
        _configBlock = new YapConfigBlock(this);
        _configBlock.write();
        _configBlock.go();
        initNewClassCollection();
        initializeEssentialClasses();
        initBootRecord();
        _freespaceManager.start(_configBlock._freespaceAddress);
        
        if(Debug.freespace  && Debug.freespaceChecker){
            _fmChecker.start(0);
        }
        
    }

    long currentVersion() {
        return _bootRecord.currentVersion();
    }

    void initNewClassCollection() {
        // overridden in YapObjectCarrier to do nothing
        i_classCollection.initTables(1);
    }

    final ClassIndex createClassIndex(YapClass yapClass) {
        return new ClassIndex(yapClass);
    }
    
    final BTree createBTreeClassIndex(YapClass a_yapClass, int id){
        BTree btree = new BTree(id, new YInt(this), null);
        if(id == 0){
            btree.write(getSystemTransaction());
        }
        return btree; 
    }

    final QueryResultImpl createQResult(Transaction a_ta) {
        return new QueryResultImpl(a_ta);
    }

    final boolean delete5(Transaction ta, YapObject yo, int a_cascade, boolean userCall) {
        int id = yo.getID();
        YapWriter reader = readWriterByID(ta, id);
        if (reader != null) {
            Object obj = yo.getObject();
            if (obj != null) {
                if ((!showInternalClasses())
                    && YapConst.CLASS_INTERNAL.isAssignableFrom(obj.getClass())) {
                    return false;
                }
            }
            reader.setCascadeDeletes(a_cascade);
            reader.slotDelete();
            YapClass yc = yo.getYapClass();
            yc.delete(reader, obj);

            // The following will not work with this approach.
            // Free blocks are identified in the Transaction by their ID.
            // TODO: Add a second tree specifically to free pointers.

            //			if(SecondClass.class.isAssignableFrom(yc.getJavaClass())){
            //				ta.freePointer(id);
            //			}

            return true;
        }
        return false;
    }

    abstract long fileLength();

    abstract String fileName();
    
    public void free(Slot slot) {
        if(slot == null){
            return;
        }
        if(slot._address == 0){
            return;
        }
        free(slot._address, slot._length);
    }

    public void free(int a_address, int a_length) {
        if(DTrace.enabled){
            DTrace.FILE_FREE.logLength(a_address, a_length);
        }
        _freespaceManager.free(a_address, a_length);
        if(Debug.freespace && Debug.freespaceChecker){
            _fmChecker.free(a_address, a_length);
        }
    }

    final void freePrefetchedPointers() {
        if (i_prefetchedIDs != null) {
            i_prefetchedIDs.traverse(new Visitor4() {

                public void visit(Object a_object) {
                    free(((TreeInt) a_object)._key, YapConst.POINTER_LENGTH);
                }
            });
        }
        i_prefetchedIDs = null;
    }
    
    final void freeSpaceBeginCommit(){
        if(_freespaceManager == null){
            return;
        }
        _freespaceManager.beginCommit();
    }
    
    final void freeSpaceEndCommit(){
        if(_freespaceManager == null){
            return;
        }
        _freespaceManager.endCommit();
    }


    void getAll(Transaction ta, final QueryResultImpl a_res) {

        // duplicates because of inheritance hierarchies
        final Tree[] duplicates = new Tree[1];

        YapClassCollectionIterator i = i_classCollection.iterator();
        while (i.hasNext()) {
            YapClass yapClass = i.readNextClass();
            if (yapClass.getName() != null) {
                ReflectClass claxx = yapClass.classReflector();
                if (claxx == null
                    || !( i_handlers.ICLASS_INTERNAL.isAssignableFrom(claxx))) {
                    
                    if(Debug.useBTrees){
                        BTree btree = yapClass.index();
                        if(btree != null){
                            btree.traverseKeys(ta, new Visitor4() {
                                public void visit(Object obj) {
                                    int id = ((Integer)obj).intValue();
                                    TreeInt newNode = new TreeInt(id);
                                    duplicates[0] = Tree
                                        .add(duplicates[0], newNode);
                                    if (newNode.size() != 0) {
                                        a_res.add(id);
                                    }
                                }
                            });
                            
                        }
                    }
                    
                    if(Debug.useOldClassIndex && ! Debug.useBTrees){
                    
                        Tree tree = yapClass.getIndex(ta);
                        if (tree != null) {
                            tree.traverse(new Visitor4() {
    
                                public void visit(Object obj) {
                                    int id = ((TreeInt) obj)._key;
                                    TreeInt newNode = new TreeInt(id);
                                    duplicates[0] = Tree
                                        .add(duplicates[0], newNode);
                                    if (newNode.size() != 0) {
                                        a_res.add(id);
                                    }
                                }
                            });
                        }
                    }
                    
                    
                }
            }
        }
        a_res.reset();
    }

    final int getPointerSlot() {
        int id = getSlot(YapConst.POINTER_LENGTH);

        // write a zero pointer first
        // to prevent delete interaction trouble
        i_systemTrans.writePointer(id, 0, 0);
        
        
        // We have to make sure that object IDs do not collide
        // with built-in type IDs.
        if(id <= i_handlers.maxTypeID()){
            return getPointerSlot();
        }
            
        return id;
    }
    
    public int blocksFor(long bytes){
        int blockLen = blockSize();
        int result = (int)(bytes / blockLen);
        if (bytes % blockLen != 0) result++;
        return result;
    }
    
    public int getSlot(int a_length){
        
        if(! DTrace.enabled){
            return getSlot1(a_length);
        }
        
        int address = getSlot1(a_length);
        DTrace.GET_SLOT.logLength(address, a_length);
        return address;
    }

    private final int getSlot1(int bytes) {
        
        if(Deploy.debug){
            if(bytes <= 0){
                throw new RuntimeException("Who wants invalid zero or smaller slots ?");
            }
        }
        
        if(_freespaceManager != null){
            
            int freeAddress = _freespaceManager.getSlot(bytes);
            
            if(Debug.freespace && Debug.freespaceChecker){
                if(freeAddress > 0){
                    Collection4 wrongOnes = new Collection4();
                    int freeCheck = _fmChecker.getSlot(bytes);
                    
                    while(freeCheck != freeAddress  && freeCheck > 0){
                        // System.out.println("Freecheck alternative found: "  + freeCheck);
                        wrongOnes.add(new int[]{freeCheck,bytes});
                        freeCheck = _fmChecker.getSlot(bytes);
                    }
                    Iterator4 i = wrongOnes.iterator();
                    while(i.hasNext()){
                        int[] adrLength = (int[])i.next();
                        _fmChecker.free(adrLength[0], adrLength[1]);
                    }
                    if(freeCheck == 0){
                        _freespaceManager.debug();
                        _fmChecker.debug();
                    }
                }
            }
            
            if(freeAddress > 0){
                return freeAddress;
            }
        }
        
        int blocksNeeded = blocksFor(bytes);
        if (Debug.xbytes && Deploy.overwrite) {
            writeXBytes(i_writeAt, blocksNeeded * blockSize());
        }
        int address = i_writeAt;
        i_writeAt += blocksNeeded;
        return address;
    }
    
    void ensureLastSlotWritten(){
        if (!Debug.xbytes){
            if(Deploy.overwrite){
                if(i_writeAt > blocksFor(fileLength())){
                    YapWriter writer = getWriter(i_systemTrans, i_writeAt - 1, blockSize());
                    writer.write();
                }
            }
        }
    }

    public Db4oDatabase identity() {
        if(_bootRecord == null){
            return null;  // early access for internal stuff, no identity needed
        }
        return _bootRecord.i_db;
    }

    void initialize2() {
        i_dirty = new Collection4();
        super.initialize2();
    }
    
    private void initBootRecord() {
        showInternalClasses(true);
        _bootRecord = new PBootRecord();
        _bootRecord.i_stream = this;
        _bootRecord.init(i_config);
        setInternal(i_systemTrans, _bootRecord, false);
        _configBlock._bootRecordID = getID1(i_systemTrans, _bootRecord);
        _configBlock.write();
        showInternalClasses(false);
    }

    boolean isServer() {
        return i_isServer;
    }

    final YapWriter newObject(Transaction a_trans, YapMeta a_object) {
        int length = a_object.ownLength();
        Pointer4 ptr = newSlot(a_trans, length);
        a_object.setID(ptr._id);
        YapWriter writer = new YapWriter(a_trans, length);
        writer.useSlot(ptr._id, ptr._address, length);
        if (Deploy.debug) {
            writer.writeBegin(a_object.getIdentifier(), length);
        }
        return writer;
    }

    public final Pointer4 newSlot(Transaction a_trans, int a_length) {
        int id = getPointerSlot();
        int address = getSlot(a_length);
        a_trans.setPointer(id, address, a_length);
        return new Pointer4(id, address);
    }

    final int newUserObject() {
        return getPointerSlot();
    }

    void prefetchedIDConsumed(int a_id) {
        i_prefetchedIDs = i_prefetchedIDs.removeLike(new TreeIntObject(a_id));
    }

    int prefetchID() {
        int id = getPointerSlot();
        i_prefetchedIDs = Tree.add(i_prefetchedIDs, new TreeInt(id));
        return id;
    }
    
    public ReferencedSlot produceFreeOnCommitEntry(int id){
        Tree node = TreeInt.find(_freeOnCommit, id);
        if (node != null) {
            return (ReferencedSlot) node;
        }
        ReferencedSlot slot = new ReferencedSlot(id);
        _freeOnCommit = Tree.add(_freeOnCommit, slot);
        return slot;
    }
    
    public void reduceFreeOnCommitReferences(ReferencedSlot slot){
        if(slot.removeReferenceIsLast()){
            _freeOnCommit = _freeOnCommit.removeNode(slot);
        }
    }
    
    public void freeDuringCommit(ReferencedSlot referencedSlot, Slot slot){
        _freeOnCommit = referencedSlot.free(this, _freeOnCommit, slot);
    }

    public void raiseVersion(long a_minimumVersion) {
        _bootRecord.raiseVersion(a_minimumVersion);
    }

    public YapWriter readWriterByID(Transaction a_ta, int a_id) {
        return (YapWriter)readReaderOrWriterByID(a_ta, a_id, false);    
    }

    public YapReader readReaderByID(Transaction a_ta, int a_id) {
        return readReaderOrWriterByID(a_ta, a_id, true);
    }
    
    private final YapReader readReaderOrWriterByID(Transaction a_ta, int a_id, boolean useReader) {
        if (a_id == 0) {
            return null;
        }
        
        if(DTrace.enabled){
            DTrace.READ_ID.log(a_id);
        }
        
        try {
            Slot slot = a_ta.getSlotInformation(a_id);
            if (slot == null) {
                return null;
            }
            
            if (slot._address == 0) {
                return null;
            }
            
            if(DTrace.enabled){
                DTrace.READ_SLOT.logLength(slot._address, slot._length);
            }
            
            YapReader reader = null;
            if(useReader){
                reader = new YapReader(slot._length);
            }else{
                reader = getWriter(a_ta, slot._address, slot._length);
                ((YapWriter)reader).setID(a_id);
            }

            reader.readEncrypt(this, slot._address);
            return reader;
            
        } catch (Exception e) {
            
            // This is a tough catch-all block, but it does make sense:
            // A call for getById() could accidentally find something
            // that looks like a slot and try to use it.
            
            // TODO: For debug purposes analyse the caller stack and
            // differentiate here in debug mode.

            if (Debug.atHome) {
                System.out.println("YapFile.WriterByID failed for ID: " + a_id);
                e.printStackTrace();
            }
        }
        return null;        
        
    }

    void readThis() {
        YapWriter myreader = getWriter(i_systemTrans, 0, HEADER_LENGTH);
        myreader.read();

        byte firstFileByte = myreader.readByte();
        byte blockLen = 1;

        if (firstFileByte != YapConst.YAPBEGIN) {
            
            if(firstFileByte != YapConst.YAPFILEVERSION){
                Exceptions4.throwRuntimeException(17);
            }
            
            blockLen = myreader.readByte();
            
        }else{
	        if (myreader.readByte() != YapConst.YAPFILE) {
	            Exceptions4.throwRuntimeException(17);
	        }
        }
        
        blockSize(blockLen);
        
// Test code to force a big database file        
        
//        long len = fileLength();
//        long min = Integer.MAX_VALUE;
//        min *= (long)2;
//        if(len < min){
//            len = min;
//        }
//        i_writeAt = blocksFor(len);
        
        i_writeAt = blocksFor(fileLength());

        _configBlock = new YapConfigBlock(this);
        
        _configBlock.read(myreader.readInt());

        // configuration lock time skipped
        myreader.incrementOffset(YapConst.YAPID_LENGTH);

        i_classCollection.setID(myreader.readInt());
        i_classCollection.read(i_systemTrans);

        int freespaceID = myreader.readInt();
        
        _freespaceManager = FreespaceManager.createNew(this, _configBlock._freespaceSystem);
        _freespaceManager.read(freespaceID);
        
        if(Debug.freespace){
            _fmChecker = new FreespaceManagerRam(this);
            _fmChecker.read(freespaceID);
        }
        
        _freespaceManager.start(_configBlock._freespaceAddress);
        
        if(Debug.freespace){
            _fmChecker.start(0);
        }
        
        if(i_config.freespaceSystem() != 0  || _configBlock._freespaceSystem == FreespaceManager.FM_LEGACY_RAM){
            if(_freespaceManager.systemType() != i_config.freespaceSystem()){
                FreespaceManager newFM = FreespaceManager.createNew(this, i_config.freespaceSystem());
                int fmSlot = _configBlock.newFreespaceSlot(i_config.freespaceSystem());
                newFM.start(fmSlot);
                _freespaceManager.migrate(newFM);
                FreespaceManager oldFM = _freespaceManager;
                _freespaceManager = newFM;
                oldFM.freeSelf();
                _freespaceManager.beginCommit();
                _freespaceManager.endCommit();
                _configBlock.write();
            }
        }
        
        showInternalClasses(true);
        Object bootRecord = null;
        if (_configBlock._bootRecordID > 0) {
            bootRecord = getByID1(i_systemTrans, _configBlock._bootRecordID);
        }
        if (bootRecord instanceof PBootRecord) {
            _bootRecord = (PBootRecord) bootRecord;
            _bootRecord.checkActive();
            _bootRecord.i_stream = this;
            if (_bootRecord.initConfig(i_config)) {
                i_classCollection.reReadYapClass(getYapClass(
                    i_handlers.ICLASS_PBOOTRECORD, false));
                setInternal(i_systemTrans, _bootRecord, false);
            }
        } else {
            initBootRecord();
        }
        
        showInternalClasses(false);
        writeHeader(false);
        Transaction trans = _configBlock.getTransactionToCommit();
        if (trans != null) {
            if (!i_config.commitRecoveryDisabled()) {
                trans.writeOld();
            }
        }
    }

    public void releaseSemaphore(String name) {
        releaseSemaphore(checkTransaction(null), name);
    }

    void releaseSemaphore(Transaction ta, String name) {
        if (i_semaphores != null) {
            synchronized (i_semaphores) {
                if (i_semaphores != null && ta == i_semaphores.get(name)) {
                    i_semaphores.remove(name);
                    i_semaphores.notifyAll();
                }
            }
        }
    }

    void releaseSemaphores(Transaction ta) {
        if (i_semaphores != null) {
            synchronized (i_semaphores) {
                i_semaphores.forEachKeyForIdentity(new Visitor4() {
                    public void visit(Object a_object) {
                        i_semaphores.remove(a_object);
                    }
                }, ta);
                i_semaphores.notifyAll();
            }
        }
    }

    final void rollback1() {
        checkClosed();
        i_entryCounter++;
        getTransaction().rollback();
        i_entryCounter--;
    }

    final void setDirty(UseSystemTransaction a_object) {
        ((YapMeta) a_object).setStateDirty();
        ((YapMeta) a_object).cacheDirty(i_dirty);
    }

    public boolean setSemaphore(String name, int timeout) {
        return setSemaphore(checkTransaction(null), name, timeout);
    }

    boolean setSemaphore(Transaction ta, String name, int timeout) {
        if (name == null) {
            throw new NullPointerException();
        }
        if (i_semaphores == null) {
            synchronized (i_lock) {
                if (i_semaphores == null) {
                    i_semaphores = new Hashtable4(10);
                }
            }
        }
        synchronized (i_semaphores) {
            Object obj = i_semaphores.get(name);
            if (obj == null) {
                i_semaphores.put(name, ta);
                return true;
            }
            if (ta == obj) {
                return true;
            }
            long endtime = System.currentTimeMillis() + timeout;
            long waitTime = timeout;
            while (waitTime > 0) {
                try {
                    i_semaphores.wait(waitTime);
                } catch (Exception e) {
                    if (Debug.atHome) {
                        e.printStackTrace();
                    }
                }
                if (i_classCollection == null) {
                    return false;
                }

                obj = i_semaphores.get(name);

                if (obj == null) {
                    i_semaphores.put(name, ta);
                    return true;
                }

                waitTime = endtime - System.currentTimeMillis();
            }
            return false;
        }
    }

    void setServer(boolean flag) {
        i_isServer = flag;
    }

    public abstract void copy(int oldAddress, int oldAddressOffset, int newAddress, int newAddressOffset, int length);

    public abstract void syncFiles();

    public String toString() {
        if (Debug.prettyToStrings) {
            return super.toString();
        }
        return fileName();
    }

    final YapWriter updateObject(Transaction a_trans, YapMeta a_object) {
        int length = a_object.ownLength();
        int id = a_object.getID();
        int address = getSlot(length);
        
        a_trans.slotFreeOnRollbackCommitSetPointer(id, address, length);
        
        YapWriter writer = a_trans.i_stream.getWriter(a_trans, length);
        writer.useSlot(id, address, length);

        if (Deploy.debug) {
            writer.writeBegin(a_object.getIdentifier(), length);
        }
        return writer;
    }

    void write(boolean shuttingDown) {

        // This will also commit the System Transaction,
        // since it is the parent or the same object.
        i_trans.commit();

        if (shuttingDown) {
            writeHeader(shuttingDown);
        }
    }

    abstract boolean writeAccessTime() throws IOException;

    abstract void writeBytes(YapReader a_Bytes, int address, int addressOffset);

    final void writeDirty() {
        YapMeta dirty;
        Iterator4 i = i_dirty.iterator();
        while (i.hasNext()) {
            dirty = (YapMeta) i.next();
            dirty.write(i_systemTrans);
            dirty.notCachedDirty();
        }
        i_dirty.clear();
        writeBootRecord();
    }
    
    final void writeEmbedded(YapWriter a_parent, YapWriter a_child) {
        int length = a_child.getLength();
        int address = getSlot(length);
        a_child.getTransaction().slotFreeOnRollback(address, address, length);
        a_child.address(address);
        a_child.writeEncrypt();
        int offsetBackup = a_parent._offset;
        a_parent._offset = a_child.getID();
        a_parent.writeInt(address);
        a_parent._offset = offsetBackup;
    }

    void writeHeader(boolean shuttingDown) {
        
        int freespaceID = _freespaceManager.write(shuttingDown);
        
        if(shuttingDown){
            _freespaceManager = null;
        }
        
        if(Debug.freespace && Debug.freespaceChecker){
            freespaceID = _fmChecker.write(shuttingDown);
        }
        
        YapWriter writer = getWriter(i_systemTrans, 0, HEADER_LENGTH);
        writer.append(YapConst.YAPFILEVERSION);
        writer.append(blockSize());
        writer.writeInt(_configBlock._address);
        writer.writeInt(0);
        writer.writeInt(i_classCollection.getID());
        writer.writeInt(freespaceID);
        if (Debug.xbytes && Deploy.overwrite) {
            writer.setID(YapConst.IGNORE_ID);
        }
        writer.write();
        if(shuttingDown){
            ensureLastSlotWritten();
        }
        syncFiles();
    }

    final void writeNew(YapClass a_yapClass, YapWriter aWriter) {
        writeObject(null, aWriter, aWriter.getAddress());
        if (maintainsIndices()) {
            a_yapClass.addToIndex(this, aWriter.getTransaction(), aWriter
                .getID());
        }
    }

    final void writeObject(YapMeta a_object, YapReader a_writer, int address) {
        i_handlers.encrypt(a_writer);
        writeBytes(a_writer, address, 0);
    }

    void writeBootRecord() {
        _bootRecord.store(1);
    }

    // This is a reroute of writeBytes to write the free blocks
    // unchecked.

    public abstract void writeXBytes(int a_address, int a_length);

    YapWriter xBytes(int a_address, int a_length) {
        if (Debug.xbytes) {
            YapWriter bytes = getWriter(i_systemTrans, a_address, a_length);
            for (int i = 0; i < a_length; i++) {
                bytes.append(YapConst.XBYTE);
            }
            return bytes;
        } else {
            throw YapConst.virtualException();
        }
    }

    final void writeTransactionPointer(int a_address) {
        YapWriter bytes = new YapWriter(i_systemTrans,
            _configBlock._address, YapConst.YAPINT_LENGTH * 2);
        bytes.moveForward(YapConfigBlock.TRANSACTION_OFFSET);
        bytes.writeInt(a_address);
        bytes.writeInt(a_address);
        if (Debug.xbytes && Deploy.overwrite) {
            bytes.setID(YapConst.IGNORE_ID);
        }
        bytes.write();
    }

    final void writeUpdate(YapClass a_yapClass, YapWriter a_bytes) {
        Transaction trans = a_bytes.getTransaction();
        int id = a_bytes.getID();
        int length = a_bytes.getLength();
        int address = getSlot(length);
        a_bytes.address(address);
        trans.slotFreeOnRollbackSetPointer(id, address, length);
        i_handlers.encrypt(a_bytes);
        a_bytes.write();
    }
}