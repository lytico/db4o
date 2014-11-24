/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;

/**
 * 
 * @exclude
 */
public class Index4 {

    public final Indexable4     _handler;

    static private int _version;

    public final MetaIndex    _metaIndex;

    private IndexTransaction _globalIndexTransaction;

    private Collection4        _indexTransactions;

    private IxFileRangeReader  _fileRangeReader;
    
    final boolean                   _nullHandling;  

    public Index4(Transaction systemTrans, Indexable4 handler, MetaIndex metaIndex, boolean nullHandling) {
        _metaIndex = metaIndex;
        _handler = handler;
        _globalIndexTransaction = new IndexTransaction(systemTrans, this);
        _nullHandling = nullHandling;
        createGlobalFileRange();
    }

    public IndexTransaction dirtyIndexTransaction(Transaction a_trans) {
        IndexTransaction ift = new IndexTransaction(a_trans, this);
        if (_indexTransactions == null) {
            _indexTransactions = new Collection4();
        } else {
            IndexTransaction iftExisting = (IndexTransaction) _indexTransactions.get(ift);
            if (iftExisting != null) {
                return iftExisting;
            }
        }
        a_trans.addDirtyFieldIndex(ift);
        ift.setRoot(Tree.deepClone(_globalIndexTransaction.getRoot(), ift));
        ift.i_version = ++_version;
        _indexTransactions.add(ift);
        return ift;
    }
    
    public IndexTransaction globalIndexTransaction(){
        return _globalIndexTransaction;
    }

    public IndexTransaction indexTransactionFor(Transaction a_trans) {
        if (_indexTransactions != null) {
            IndexTransaction ift = new IndexTransaction(a_trans, this);
            ift = (IndexTransaction) _indexTransactions.get(ift);
            if (ift != null) {
                return ift;
            }
        }
        return _globalIndexTransaction;
    }
    
// Debug index tree depth    
    
//    void debug(IxFieldTransaction a_ft){
//        System.out.println("+++  IxField commit debug begin");
//        Tree t1 = i_globalIndex.getRoot();
//        if (t1 != null){
//            System.out.println("i_globalIndex");
//            t1.debugDepth();
//        }
//        if(a_ft != null){
//            Tree t2 = a_ft.getRoot();
//            if (t2 != null){
//                System.out.println("a_ft");
//                t2.debugDepth();
//            }
//        }
//        System.out.println("---  IxField commit debug complete");
//    }
    
    private int[] freeForMetaIndex(){
        return new int[] {
            _metaIndex.indexAddress,
            _metaIndex.indexLength,
            _metaIndex.patchAddress,
            _metaIndex.patchLength
        };
    }
    
    private void doFree(int[] addressLength){
        YapFile yf = file();
        for(int i = 0; i < addressLength.length; i += 2){
            yf.free(addressLength[i], addressLength[i + 1]);
        }
    }
                
    
    /**
     * solving a hen-egg problem: commit itself works with freespace 
     * so we have to do this all sequentially in the right way, working
     * with with both indexes at the same time.
     */
    public void commitFreeSpace(Index4 other){
        
        int entries = countEntries();
        
        // For the two freespace indexes themselves, we call
        // the freespace system and we store two meta indexes. Potential effects:
        // 2 x getSlot   -2   to   0     
        // 4 x free      -4   to   + 4
        // 
        
        // Therefore we have to raise the value by 4, to make 
        // sure that there are enough.
        
        
        int length = (entries + 4) * lengthPerEntry();
        
        int mySlot = getSlot(length);
        int otherSlot = getSlot(length);
        doFree(freeForMetaIndex());
        doFree(other.freeForMetaIndex());
        
        entries = writeToNewSlot(mySlot, length);
        metaIndexSetMembers(entries, length, mySlot);
        createGlobalFileRange();
        
        int otherEntries = other.writeToNewSlot(otherSlot, length);
        
        if(Deploy.debug){
            if(entries != otherEntries){
                throw new RuntimeException("Should never happen");
            }
        }
        
        other.metaIndexSetMembers(entries, length, otherSlot);
        other.createGlobalFileRange();
    }
    
    private int lengthPerEntry(){
        return _handler.linkLength() + YapConst.YAPINT_LENGTH;
    }
    
    private void free(){
        file().free(_metaIndex.indexAddress, _metaIndex.indexLength);
        file().free(_metaIndex.patchAddress, _metaIndex.indexLength);
    }
    
    private void metaIndexStore(int entries, int length, int address){
        Transaction transact = trans();
        metaIndexSetMembers(entries, length, address);
        transact.i_stream.setInternal(transact, _metaIndex, 1, false);
    }
    
    private void metaIndexSetMembers(int entries, int length, int address){
        _metaIndex.indexEntries = entries;
        _metaIndex.indexLength = length;
        _metaIndex.indexAddress = address;
        _metaIndex.patchEntries = 0;
        _metaIndex.patchAddress = 0;
        _metaIndex.patchLength = 0;
    }
    
    private int writeToNewSlot(int slot, int length ){
        Tree root = getRoot();
        final YapWriter writer = new YapWriter(trans(),slot, lengthPerEntry());
        final int[] entries = new int[] {0};
        if (root != null) {
            root.traverse(new Visitor4() {
                public void visit(Object a_object) {
                    entries[0] += ((IxTree) a_object).write(_handler, writer); 
                }
            });
        }
        return entries[0];
    }
    

    void commit(IndexTransaction ixTrans) {
        
        _indexTransactions.remove(ixTrans);
        _globalIndexTransaction.merge(ixTrans);


        // TODO: Use more intelligent heuristic here to
        // calculate when to flush the global index
        
        // int leaves = _globalIndexTransaction.countLeaves();
        // boolean createNewFileRange = leaves > MAX_LEAVES;
        
        
        boolean createNewFileRange = true;

        if (createNewFileRange) {
            
            int entries = countEntries();
            int length = countEntries() * lengthPerEntry();
            int slot = getSlot(length);
            
            int[] free = freeForMetaIndex();
            
            metaIndexStore(entries, length, slot);
            
            writeToNewSlot(slot, length);
            
            IxFileRange newFileRange = createGlobalFileRange();

            if(_indexTransactions != null){
                Iterator4 i = _indexTransactions.iterator();
                while (i.hasNext()) {
                    final IndexTransaction ft = (IndexTransaction) i.next();
                    Tree clonedTree = newFileRange;
                    if (clonedTree != null) {
                        clonedTree = clonedTree.deepClone(ft);
                    }
                    final Tree[] tree = { clonedTree};
                    ft.getRoot().traverseFromLeaves((new Visitor4() {
                        
                        public void visit(Object a_object) {
                            IxTree ixTree = (IxTree) a_object;
                            if (ixTree._version == ft.i_version) {
                                if (!(ixTree instanceof IxFileRange)) {
                                    ixTree.beginMerge();
                                    tree[0] = Tree.add(tree[0], ixTree);
                                }
                            }
                        }
                    }));
                    ft.setRoot(tree[0]);
                }
            }
            
            doFree(free);

        } else {
            Iterator4 i = _indexTransactions.iterator();
            while (i.hasNext()) {
                ((IndexTransaction) i.next()).merge(ixTrans);
            }
        }
    }

    private IxFileRange createGlobalFileRange() {
        IxFileRange fr = null;
        if (_metaIndex.indexEntries > 0) {
            fr = new IxFileRange(_globalIndexTransaction,
                    _metaIndex.indexAddress, 0, _metaIndex.indexEntries);
        }
        _globalIndexTransaction.setRoot(fr);
        return fr;
    }

    void rollback(IndexTransaction a_ft) {
        _indexTransactions.remove(a_ft);
    }

    IxFileRangeReader fileRangeReader() {
        if (_fileRangeReader == null) {
            _fileRangeReader = new IxFileRangeReader(_handler);
        }
        return _fileRangeReader;
    }

    public String toString() {
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
        StringBuffer sb = new StringBuffer();
        sb.append("IxField  " + System.identityHashCode(this));
        if (_globalIndexTransaction != null) {
            sb.append("\n  Global \n   ");
            sb.append(_globalIndexTransaction.toString());
        } else {
            sb.append("\n  no global index \n   ");
        }
        if (_indexTransactions != null) {
            Iterator4 i = _indexTransactions.iterator();
            while (i.hasNext()) {
                sb.append("\n");
                sb.append(i.next().toString());
            }
        }
        return sb.toString();
    }
    
    private Transaction trans(){
        return _globalIndexTransaction.i_trans;
    }
    
    private YapFile file(){
        return trans().i_file;
    }
    
    private int getSlot(int length){
        return file().getSlot(length);
    }
    
    private Tree getRoot(){
        return _globalIndexTransaction.getRoot();
    }
    
    private int countEntries(){
        Tree root = getRoot();
        return root == null ? 0 : root.size();
    }
    

}
