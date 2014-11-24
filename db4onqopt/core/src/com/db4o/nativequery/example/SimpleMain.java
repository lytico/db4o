/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.example;

import java.io.*;
import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.internal.*;
import com.db4o.internal.query.*;
import com.db4o.query.*;


public class SimpleMain {
	private static final String FILENAME = "simple.db4o";

	public static void main(String[] args) {
		System.setProperty("db4o.dynamicnq","true");
		new File(FILENAME).delete();
		final Configuration config = Db4o.newConfiguration();
		ObjectClass classConfig=config.objectClass(Student.class);
		classConfig.objectField("name").indexed(true);
		classConfig.objectField("age").indexed(true);
		ObjectContainer db=Db4o.openFile(config, FILENAME);
		try {
			Student mumon = new Student(100,"Mumon",1.50f);
			Student tortoise = new Student(101,"Tortoise",0.85f,mumon);
			Student achilles = new Student(30,"Achilles",1.80f,tortoise);
			db.store(mumon);
			db.store(tortoise);
			db.store(achilles);
//			for(int i=0;i<100000;i++) {
//				db.set(new Student(1,"noone",achilles));
//			}
			db.commit();
			db.close();
			
			db=Db4o.openFile(config, FILENAME);
			final String protoName="Achilles";
			Predicate<Student> filter=new Predicate<Student>() {
				private int protoAge=203;
				
				public boolean match(Student candidate) {
					return candidate.tortue!=null&&candidate.getTortue().getAge()>=protoAge/2
							||candidate.getName().equals(protoName)
							||candidate.getSize()<1;
				}
			};
			((InternalObjectContainer)db).getNativeQueryHandler().addListener(new Db4oQueryExecutionListener() {
				public void notifyQueryExecuted(NQOptimizationInfo info) {
					System.err.println(info.message());
				}
			});
			long startTime=System.currentTimeMillis();
			List filtered=db.query(filter);
			for (Iterator resultIter = filtered.iterator(); resultIter.hasNext();) {
				Student student = (Student) resultIter.next();
				System.out.println(student);
			}
			System.out.println("Took "+(System.currentTimeMillis()-startTime)+" ms");
		}
		finally {
			db.close();
		}
	}
}
