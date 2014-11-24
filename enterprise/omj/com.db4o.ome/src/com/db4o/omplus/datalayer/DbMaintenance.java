package com.db4o.omplus.datalayer;

import com.db4o.ObjectContainer;
import com.db4o.ext.DatabaseClosedException;
import com.db4o.ext.Db4oIOException;
import com.db4o.foundation.NotSupportedException;
import com.db4o.omplus.Activator;

public class DbMaintenance {
	
	private ObjectContainer oc;
	
	public boolean isDBOpened(){
		if(getObjectContainer() == null)
			return false;
		return true;
	}
	
	public void backup(String path) throws Exception {
		oc = getObjectContainer();
		if(oc != null ){
			try{
				oc.ext().backup(path);
			}
			catch( Db4oIOException ex){
				throw new Db4oIOException(" Operation Failed as IO Exception occurred");
			}
			catch( DatabaseClosedException ex){
				throw new RuntimeException(" Operation Failed as database is closed");
			}
			catch( NotSupportedException ex){
				throw new NotSupportedException(" Operation Failed as backup" +
						" is not supported");
			}
		}
	}

	public boolean isClient() {
		return Activator.getDefault().dbModel().db().isClient();
	}

	private ObjectContainer getObjectContainer(){
		IDbInterface dbinterface = Activator.getDefault().dbModel().db();
		oc = dbinterface.getDB();
		return oc;
	}
		
}
