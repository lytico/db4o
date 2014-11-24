/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package db4ounit.extensions.dbmock;

import java.util.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.qlin.*;
import com.db4o.query.*;

/**
 * @sharpen.partial
 */
public class MockEmbedded implements EmbeddedObjectContainer {

	public void backup(String path) throws Db4oIOException, DatabaseClosedException, NotSupportedException {
		throw new NotImplementedException();
	}

	public ObjectContainer openSession() {
		throw new NotImplementedException();
	}

	public void activate(Object obj, int depth) throws Db4oIOException, DatabaseClosedException {
		throw new NotImplementedException();
	}

	public boolean close() throws Db4oIOException {
		throw new NotImplementedException();
	}

	public void commit() throws Db4oIOException, DatabaseClosedException, DatabaseReadOnlyException {
		throw new NotImplementedException();
	}

	public void deactivate(Object obj, int depth) throws DatabaseClosedException {
		throw new NotImplementedException();
	}

	public void delete(Object obj) throws Db4oIOException, DatabaseClosedException, DatabaseReadOnlyException {
		throw new NotImplementedException();
	}

	public ExtObjectContainer ext() {
		throw new NotImplementedException();
	}

	public <T> ObjectSet<T> get(Object template) throws Db4oIOException, DatabaseClosedException {
		throw new NotImplementedException();
	}

	public Query query() throws DatabaseClosedException {
		throw new NotImplementedException();
	}

	public <TargetType> ObjectSet<TargetType> query(Class<TargetType> clazz) throws Db4oIOException, DatabaseClosedException {
		throw new NotImplementedException();
	}

	public <TargetType> ObjectSet<TargetType> query(Predicate<TargetType> predicate) throws Db4oIOException, DatabaseClosedException {
		throw new NotImplementedException();
	}

	public <TargetType> ObjectSet<TargetType> query(Predicate<TargetType> predicate, QueryComparator<TargetType> comparator) throws Db4oIOException, DatabaseClosedException {
		throw new NotImplementedException();
	}

	public <TargetType> ObjectSet<TargetType> query(Predicate<TargetType> predicate, Comparator<TargetType> comparator) throws Db4oIOException, DatabaseClosedException {
		throw new NotImplementedException();
	}

	public <T> ObjectSet<T> queryByExample(Object template) throws Db4oIOException, DatabaseClosedException {
		throw new NotImplementedException();
	}

	public void rollback() throws Db4oIOException, DatabaseClosedException, DatabaseReadOnlyException {
		throw new NotImplementedException();
	}

	public void store(Object obj) throws DatabaseClosedException, DatabaseReadOnlyException {
		throw new NotImplementedException();
	}
	
	public <T> QLin<T> from(Class<T> clazz) {
		throw new NotImplementedException();
	}

}
