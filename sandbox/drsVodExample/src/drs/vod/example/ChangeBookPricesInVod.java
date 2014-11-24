/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example;

import java.util.*;

import javax.jdo.*;

import drs.vod.example.model.*;
import drs.vod.example.utils.*;

public class ChangeBookPricesInVod {
	
	public static void main(String[] args) {
		PersistenceManager pm = VodHelper.getPersistenceManager();
		pm.currentTransaction().begin();
		Collection result = (Collection) pm.newQuery(Book.class).execute();
		if(result.size() == 0){
			System.out.println("No books stored to VOD yet.");
		} else{
			Iterator i = result.iterator();
			while(i.hasNext()){
				Book book = (Book) i.next();
				book.price(book.price() + 0.42f);
				System.out.println("New price: " + book);
			}
		}
		pm.currentTransaction().commit();
		pm.close();
	}

}
