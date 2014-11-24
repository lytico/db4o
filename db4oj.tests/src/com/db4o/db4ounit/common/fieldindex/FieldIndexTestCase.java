/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.fieldindex;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class FieldIndexTestCase extends FieldIndexTestCaseBase {
	
	private static final int[] FOOS = new int[]{3,7,9,4};
    
    public static void main(String[] arguments) {
        new FieldIndexTestCase().runSolo();
    }
    
    protected void configure(Configuration config) {
    	super.configure(config);
    }
    
	protected void store() {
		storeItems(FOOS);
	}
    
    public void testTraverseValues(){
        StoredField field = storedField();
        ExpectingVisitor expectingVisitor = new ExpectingVisitor(IntArrays4.toObjectArray(FOOS));
        field.traverseValues(expectingVisitor);
        expectingVisitor.assertExpectations();
    }
    
    public void testAllThere() throws Exception{
        for (int i = 0; i < FOOS.length; i++) {
            Query q = createQuery(FOOS[i]);
            ObjectSet objectSet = q.execute();
            Assert.areEqual(1, objectSet.size());
            FieldIndexItem fii = (FieldIndexItem) objectSet.next();
            Assert.areEqual(FOOS[i], fii.foo);
        }
    }

	public void testAccessingBTree() throws Exception{
        BTree bTree = storedField().getIndex(trans());
        Assert.isNotNull(bTree);
        expectKeysSearch(bTree, FOOS);
    }

    private void expectKeysSearch(BTree btree, int[] values) {
        int lastValue = Integer.MIN_VALUE;
        for (int i = 0; i < values.length; i++) {
            if(values[i] != lastValue){
                final ExpectingVisitor expectingVisitor = ExpectingVisitor.createExpectingVisitor(values[i], IntArrays4.occurences(values, values[i]));
                BTreeRange range = fieldIndexKeySearch(trans(), btree, new Integer(values[i]));
                BTreeAssert.traverseKeys(range, new Visitor4() {
                    public void visit(Object obj) {
                        FieldIndexKey fik = (FieldIndexKey)obj;
                        expectingVisitor.visit(fik.value());
                    }
                });
                expectingVisitor.assertExpectations();
                lastValue = values[i];
            }
        }
    }
    
    private FieldIndexKey fieldIndexKey(int integerPart, Object composite){
        return new FieldIndexKeyImpl(integerPart, composite);
    }
    
    private BTreeRange fieldIndexKeySearch(Transaction trans, BTree btree, Object key) {
        // SearchTarget should not make a difference, HIGHEST is faster
        BTreeNodeSearchResult start = btree.searchLeafByObject(trans, fieldIndexKey(0, key), SearchTarget.LOWEST);
        BTreeNodeSearchResult end = btree.searchLeafByObject(trans, fieldIndexKey(Integer.MAX_VALUE, key), SearchTarget.LOWEST);
        return start.createIncludingRange(end);
    }
    
    private FieldMetadata storedField() {
        return classMetadataFor(FieldIndexItem.class).fieldMetadataForName("foo");
    }
    

}
