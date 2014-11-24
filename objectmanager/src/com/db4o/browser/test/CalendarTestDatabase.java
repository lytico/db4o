package com.db4o.browser.test;

import java.io.File;
import java.net.MalformedURLException;
import java.util.Calendar;
import java.util.GregorianCalendar;

import com.db4o.Db4o;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;

public class CalendarTestDatabase {
	private static final String FILENAME = "calendar.yap";

	public static void main(String[] args) throws MalformedURLException {
//		Db4o.configure().exceptionsOnNotStorable(true);
		new File(FILENAME).delete();
		ObjectContainer db=Db4o.openFile(FILENAME);
		db.set(new GregorianCalendar());
		db.close();
		
//		Db4o.configure().objectClass(Locale.class).callConstructor(true);
		db=Db4o.openFile(FILENAME);
		ObjectSet result=db.get(Calendar.class);
		while(result.hasNext()) {
			System.out.println(result.next());
		}
		db.close();
	}
}
