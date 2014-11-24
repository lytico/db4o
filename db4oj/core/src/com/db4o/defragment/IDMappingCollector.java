/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.defragment;

import com.db4o.foundation.*;
import com.db4o.internal.*;

public class IDMappingCollector {
	
	private final static int ID_BATCH_SIZE=4096;

	private TreeInt _ids;
	
	void createIDMapping(DefragmentServicesImpl context, int objectID, boolean isClassID) {
		if(batchFull()) {
			flush(context);
		}
		_ids=TreeInt.add(_ids,(isClassID ? -objectID : objectID));
	}

	private boolean batchFull() {
		return _ids!=null&&_ids.size()==ID_BATCH_SIZE;
	}

	public void flush(DefragmentServicesImpl context) {
		if(_ids==null) {
			return;
		}
		Iterator4 idIter=new TreeKeyIterator(_ids);
		while(idIter.moveNext()) {
			int objectID=((Integer)idIter.current()).intValue();
			boolean isClassID=false;
			if(objectID<0) {
				objectID=-objectID;
				isClassID=true;
			}
			
			if(DefragmentConfig.DEBUG){
				int mappedID = context.mappedID(objectID, -1);
				// seen object ids don't come by here anymore - any other candidates?
				if(mappedID>=0) {
					throw new IllegalStateException();
				}
			}
			context.mapIDs(objectID,context.targetNewId(), isClassID);
		}
		context.mapping().commit();
		_ids=null;
	}
}