package omplus.debug;

import java.io.*;

import com.db4o.*;

public class OMEDebugItem {

	private int _id;
	
	public OMEDebugItem(int id) {
		_id = id;
	}
	
	@Override
	public String toString() {
		return "OMEDebugItem #" + _id;
	}
	
	public static final String FILENAME = "ometest.db4o";

	public static void createDebugDatabase() {
		new File(FILENAME).delete();
		ObjectContainer db = Db4o.openFile(Db4o.newConfiguration(), FILENAME);
		db.store(new OMEDebugItem(42));
		db.close();
	}

}
