/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.inside.*;

class Sessions extends Collection4{
	
	
	void forEach(Visitor4 visitor){
		synchronized(Db4o.lock){
			Iterator4 i = iterator();
			while(i.hasNext()){
				visitor.visit(i.next());
			}
		}
	}

	ObjectContainer open(String databaseFileName) {
		
		synchronized(Db4o.lock){
			ObjectContainer oc = null;
			Session newSession = new Session(databaseFileName);
	
			Session oldSession = (Session) get(newSession);
			if (oldSession != null) {
				oc = oldSession.subSequentOpen();
				if (oc == null) {
					remove(oldSession);
				}
				return oc;
			}
			
			if (Deploy.debug) {
				System.out.println("db4o Debug is ON");
				if (!Deploy.flush) {
					System.out.println("Debug option set NOT to flush file.");
				}
				try{
				    oc = new YapRandomAccessFile(newSession);
				}catch(Exception e){
				    e.printStackTrace();
				}
			} else {
				try {
					oc = new YapRandomAccessFile(newSession);
				} catch (LongJumpOutException e) {
					throw e;
				} catch (DatabaseFileLockedException e) {
					throw e;
				} catch (ObjectNotStorableException e) {
					throw e;
				} catch (UserException eu) {
					Exceptions4.throwRuntimeException(eu.errCode, eu.errMsg);
				} catch (Throwable t) {
					Messages.logErr(Db4o.i_config, 4, databaseFileName, t);
					return null;
				}
			}
			if (oc != null) {
				newSession.i_stream = (YapStream) oc;
				add(newSession);
				Platform4.postOpen(oc);
				Messages.logMsg(Db4o.i_config, 5, databaseFileName);
			}
			return oc;
		}
	}
	
	public Object remove(Object obj){
		synchronized(Db4o.lock){
			return super.remove(obj);
		}
	}
	
}
