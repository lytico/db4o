package com.db4o.db4ounit.common.soda.ordered;

import com.db4o.db4ounit.common.soda.util.*;
import com.db4o.query.*;

import db4ounit.extensions.fixtures.*;

/**
 * Tests for COR-1007
 */
public class STOrderingTestCase extends SodaBaseTestCase implements OptOutMultiSession {

    public static void main(String[] args) {
        new STOrderingTestCase().runSolo();
    }

    public Object[] createData() {
        return new Object[] { new OrderTestSubject("Alexandr", 30, 5), // 0
                new OrderTestSubject("Cris", 30, 5), // 1
                new OrderTestSubject("Boris", 30, 5), // 2
                new OrderTestSubject("Helen", 25, 5), // 3
                new OrderTestSubject("Zeus", 25, 3), // 4
                new OrderTestSubject("Alexsandra", 25, 3), // 5
                new OrderTestSubject("Liza", 25, 4), // 6
                new OrderTestSubject("Bred", 25, 3), // 7
                new OrderTestSubject("Liza", 25, 3), // 8
                new OrderTestSubject("Gregory", 25, 4), }; // 9
    }

    public void testFirstAndSecondFieldsAreIrrelevant() {
        Query q = newQuery();
        q.constrain(OrderTestSubject.class);
        q.descend("_seniority").orderAscending();
        q.descend("_age").orderAscending();
        q.descend("_name").orderAscending();

        expectOrdered(q, new int[] { 5, 7, 8, 4, 9, 6, 3, 0, 2, 1 });
    }

    public void testSecondAndThirdFieldsAreIrrelevant() {
        Query q = newQuery();
        q.constrain(OrderTestSubject.class);
        q.descend("_age").orderAscending();
        q.descend("_name").orderAscending();
        q.descend("_seniority").orderAscending();

        expectOrdered(q, new int[] { 5, 7, 9, 3, 8, 6, 4, 0, 2, 1 });
    }

    public void testOrderByNameAndAgeAscending() {
        Query q = newQuery();
        q.constrain(OrderTestSubject.class);

        q.descend("_age").orderAscending();
        q.descend("_name").orderAscending();

        expectOrdered(q, new int[] { 5, 7, 9, 3, 6, 8, 4, 0, 2, 1 });
    }

    public void testAscendingOrderWithOutAge() {
        Query q = newQuery();
        q.constrain(OrderTestSubject.class);
        q.descend("_seniority").orderAscending();
        q.descend("_name").orderAscending();

        expectOrdered(q, new int[] { 5, 7, 8, 4, 9, 6, 0, 2, 1, 3 });
    }
}
