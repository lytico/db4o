/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package drs.vod.example;

import javax.jdo.*;


import drs.vod.example.model.*;
import drs.vod.example.utils.*;

public class Store2NewBooksToVod {
	
	public static void main(String[] args) {
		
		VodHelper.ensureVodDatabaseExists();
		
		PersistenceManager pm = VodHelper.getPersistenceManager();
		pm.currentTransaction().begin();
		
		Book cSharpInDepth = new Book("C# in depth", 29.28f);
		pm.makePersistent(cSharpInDepth);
		Book programmingInScala = new Book("Programming in Scala", 31.49f);
		pm.makePersistent(programmingInScala);
		
		System.out.println("Stored Books to VOD:");
		System.out.println(cSharpInDepth);
		System.out.println(programmingInScala);
		
		pm.currentTransaction().commit();
		pm.close();

	}

}
