package com.db4o.objectmanager;

import com.db4o.objectManager.v2.demo.DemoDbTask;
import com.db4o.objectManager.v2.importExport.GenericObjectConverter;
import com.db4o.objectManager.v2.importExport.DateConverter;
import com.db4o.ObjectContainer;
import com.db4o.Db4o;
import com.db4o.ObjectSet;
import com.db4o.query.Query;
import com.thoughtworks.xstream.XStream;
import demo.objectmanager.model.DemoPopulator;
import demo.objectmanager.model.Contact;

import java.io.*;
import java.util.List;

/**
 * 
 *
 * User: treeder
 * Date: Mar 14, 2007
 * Time: 8:21:14 PM
 */
public class ExportTest {
	public static void main(String[] args) {

		String file = createDemoDb();

		ObjectContainer db = Db4o.openFile(file);
		String toLoad = "com.acme.Car";//Contact.class.getName();
		ObjectSet result = queryFor(db, toLoad);
		DumpObject dump = addToDump(result);
		db.close();
		int size = dump.size();
		XStream xstream = getXstream();

		File f = getExportFile();
		try {
			writeXml(f, xstream, dump);
			// now the other way
			DumpObject in = readXml(f, xstream);
			System.out.println("outsize: " + size + " - insize: " + in.size());
			// todo: Assert sizes are equal
			// now try load into db4o
			db = getLoadDb(file);
			writeToDb(in, db);
			result = queryFor(db, toLoad);
			System.out.println("result.size: " + result.size());
			db.close();
		} catch(IOException e) {
			e.printStackTrace();
		}
	}

	private static XStream getXstream() {
		XStream xstream = new XStream();
		xstream.registerConverter(new GenericObjectConverter());
		xstream.registerConverter(new DateConverter());
//		xstream.alias("person", Person.class);
		return xstream;
	}

	private static String createDemoDb() {
		DemoDbTask task = new DemoDbTask();
		task.run();
		DemoPopulator populator = task.getPopulator();
		String file = populator.getFileName();
		return file;
	}

	private static File getExportFile() {
		File f = new File("db-export.xml");
		if(f.exists()) f.delete();
		return f;
	}

	private static ObjectContainer getLoadDb(String file) {
		File f = new File(file + ".load");
		if(f.exists()) f.delete();
		ObjectContainer db;
		db = Db4o.openFile(f.getAbsolutePath());
		return db;
	}

	private static ObjectSet queryFor(ObjectContainer db, String aClass) {
		Query q = db.query();
		if(aClass != null) q.constrain(db.ext().reflector().forName(aClass));
		ObjectSet result = q.execute();
		return result;
	}

	private static void writeToDb(DumpObject in, ObjectContainer db) {
		List obs = in.getObjects();
		for(int i = 0; i < obs.size(); i++) {
			Object o = obs.get(i);
			db.set(o);
		}
		db.commit();
	}

	private static DumpObject readXml(File f, XStream xstream) throws IOException {
		DumpObject ret = new DumpObject();
		BufferedInputStream in = new BufferedInputStream(new FileInputStream(f));
		xstream.fromXML(in, ret);
		in.close();
		return ret;
	}

	private static void writeXml(File f, XStream xstream, DumpObject dump) throws IOException {
		BufferedOutputStream out = new BufferedOutputStream(new FileOutputStream(f));
		xstream.toXML(dump, out);
		out.close();
	}

	private static DumpObject addToDump(ObjectSet result) {
		DumpObject dump = new DumpObject();
		while(result.hasNext()){
			Object ob = result.next();
			dump.add(ob);
		}
		return dump;
	}
}
