/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example;

import java.util.*;

import javax.jdo.*;

import com.db4o.drs.versant.metadata.*;

import drs.vod.example.model.*;
import drs.vod.example.utils.*;

public class DeleteAllFromVod {
	
	public static void main(String[] args) {
		PersistenceManager pm = VodHelper.getPersistenceManager();
		pm.currentTransaction().begin();
		deleteAll(pm, Book.class);
		deleteAll(pm, Customer.class);
		deleteAll(pm, Order.class);
		
		// In a real life scenario please take care with deleting ObjectInfo instances.
		// They hold all information required for replication.
		deleteAll(pm, ObjectInfo.class);
		
		pm.currentTransaction().commit();
		pm.close();
	}
	
	private static void deleteAll(PersistenceManager pm, Class clazz) {
		System.out.println("Deleting from VOD, class: " + clazz.getName());
		Collection collection = (Collection) pm.newQuery(clazz).execute();
		for (Object object : collection) {
			pm.deletePersistent(object);
		}
	}

}
