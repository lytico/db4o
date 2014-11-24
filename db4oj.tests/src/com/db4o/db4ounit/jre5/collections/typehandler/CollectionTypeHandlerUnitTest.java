package com.db4o.db4ounit.jre5.collections.typehandler;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public abstract class CollectionTypeHandlerUnitTest extends TypeHandlerTestUnitBase {

	protected abstract void assertCompareItems(Object element, boolean successful);

	protected void assertQuery(boolean successful, Object element, boolean withContains) {
		Query q = newQuery(itemFactory().itemClass());
		Constraint constraint = q.descend(itemFactory().fieldName()).constrain(element);
		if(withContains) {
			constraint.contains();
		}
		assertQueryResult(q, successful);
	}
	
	public void testRetrieveInstance() {
	    Object item = retrieveItemInstance();
	    assertContent(item);
	}

    protected Object retrieveItemInstance() {
        Class itemClass = itemFactory().itemClass();
	    Object item = retrieveOnlyInstance(itemClass);
        return item;
    }
	
	public void testDefragRetrieveInstance() throws Exception {
		defragment();
	    Object item = retrieveItemInstance();
	    assertContent(item);
	}

	public void testSuccessfulQuery() throws Exception {
		assertQuery(true, elements()[0], false);
	}

	public void testFailingQuery() throws Exception {
		assertQuery(false, notContained(), false);
	}

	public void testSuccessfulContainsQuery() throws Exception {
		assertQuery(true, elements()[0], true);
	}

	public void testFailingContainsQuery() throws Exception {
		assertQuery(false, notContained(), true);
	}

	public void testCompareItems() throws Exception {
		assertCompareItems(elements()[0], true);
	}

	public void testFailingCompareItems() throws Exception {
		assertCompareItems(notContained(), false);
	}

	public void testDeletion() throws Exception {
	    assertReferenceTypeElementCount(elements().length);
	    Object item = retrieveOnlyInstance(itemFactory().itemClass());
	    db().delete(item);
	    db().purge();
	    Db4oAssert.persistedCount(0, itemFactory().itemClass());
	    assertReferenceTypeElementCount(0);
	}

	public void testJoin() {
		Query q = newQuery(itemFactory().itemClass());
		q.descend(itemFactory().fieldName()).constrain(elements()[0])
			.and(q.descend(itemFactory().fieldName()).constrain(elements()[1]));
		assertQueryResult(q, true);
	}

	public void testSubQuery() {
		Query q = newQuery(itemFactory().itemClass());
		Query qq = q.descend(itemFactory().fieldName());
		qq.constrain(elements()[0]);
		ObjectSet set = qq.execute();
    	Assert.areEqual(1, set.size());
    	assertPlainContent(set.next());
	}
	
	protected void assertReferenceTypeElementCount(int expected) {
		if(!isReferenceElement(elementClass())) {
			return;
		}
		Db4oAssert.persistedCount(expected, elementClass());
	}

	private boolean isReferenceElement(Class elementClass) {
		return ListTypeHandlerTestVariables.ReferenceElement.class == elementClass;
	}

}
