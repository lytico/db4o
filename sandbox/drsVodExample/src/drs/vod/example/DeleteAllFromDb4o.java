/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example;

import java.io.*;

public class DeleteAllFromDb4o {
	
	public static void main(String[] args) {
		System.out.println("Deleting db4o database file: dRSVodExample.db4o");
		new File("dRSVodExample.db4o").delete();
	}

}
