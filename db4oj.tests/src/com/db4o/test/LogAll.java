/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.tools.*;

public class LogAll
{
	public static void main(String[] args)
	{
		run(Regression.FILE);
	}
	
	
	public static void run(String fileName){
		System.out.println("/** Logging database file: '" + fileName + "' **/");
		ObjectContainer con = Db4o.openFile(fileName);
		ObjectSet set = con.queryByExample(null);
		while(set.hasNext()){
			Logger.log(con, set.next());
		}
		con.close();
	}
}
