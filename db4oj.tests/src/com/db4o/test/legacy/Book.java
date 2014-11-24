/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.legacy;

import com.db4o.*;
import com.db4o.db4ounit.common.querying.MultiFieldIndexQueryTestCase.*;
import com.db4o.query.*;
import com.db4o.test.*;


public class Book {
    
    public Person[] authors;
    public String title;

    public Book(){}

    public Book(String title, Person[] authors){
        this.title = title;
        this.authors = authors;
    }

    public void store(){
        Person aaron = new Person("Aaron", "OneOK");
        Person bill = new Person("Bill", "TwoOK");
        Person chris = new Person("Chris", "ThreeOK");
        Person dave = new Person("Dave", "FourOK");
        Person neil = new Person("Neil", "Notwanted");
        Person nat = new Person("Nat", "Neverwanted");
        Test.store(new Book("Persistence possibilities", new Person[] { aaron, bill,chris}));
        Test.store(new Book("Persistence using S.O.D.A.", new Person[]{aaron}));
        Test.store(new Book("Persistence using JDO", new Person[]{bill, dave}));
        Test.store(new Book("Don't want to find Phil", new Person[]{aaron, bill, neil}));
        Test.store(new Book("Persistence by Jeff", new Person[]{nat}));
    }

    public void test(){
        Query qBooks = Test.query();
        qBooks.constrain(Book.class);
        qBooks.descend("title").constrain("Persistence").like();
        Query qAuthors = qBooks.descend("authors");
        Query qFirstName = qAuthors.descend("firstName");
        Query qLastName = qAuthors.descend("lastName");
        Constraint cAaron =
            qFirstName.constrain("Aaron").and(
            qLastName.constrain("OneOK")
            );
        Constraint cBill =
            qFirstName.constrain("Bill").and(
            qLastName.constrain("TwoOK")
            );
        cAaron.or(cBill);
        ObjectSet results = qAuthors.execute();
        Test.ensure(results.size() == 4);
        while(results.hasNext()){
            Person person = (Person)results.next();
            Test.ensure(person.lastName.endsWith("OK"));
        }
    }


    public String toString(){
        String ret = title;
        if(authors != null){
            for (int i = 0; i < authors.length; i++) {
                ret += "\n  " + authors[i].toString(); 
            }
        }
        return ret;
    }
}
