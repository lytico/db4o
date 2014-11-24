/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.cobra.qlin;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.foundation.*;
import com.db4o.drs.versant.*;
import com.db4o.qlin.*;
import com.versant.odbms.query.*;

public class CLinRoot<T> extends CLinCobraNode<T> {
	
	private final VodCobraFacade _cobra;
	
	private final Class<T> _clazz;
	
	private DatastoreQuery _query;
	
	private int _limit = -1;

	public CLinRoot(VodCobraFacade cobra, Class<T> clazz) {
		_cobra = cobra;
		_clazz = clazz;
		_query = new DatastoreQuery(clazz.getName());
		QLinSupport.context(clazz);
	}
	
	public DatastoreQuery query(){
		return _query;
	}

	public ObjectSet<T> select() {
		Object[] loids = _cobra.executeQuery(_query);
		return new ObjectSetCollectionFacade(
				(loids.length == 0) ? 
				new ArrayList<T>() : 
				_cobra.readObjects(_clazz, loids, _limit));
	}
	
	public QLin<T> limit(int size){
		if(size < 1){
			throw new QLinException("Limit must be greater that 0");
		}
		_limit = size;
		return this;
	}
	
	@Override
	protected CLinRoot<T> root() {
		return this;
	}


}
