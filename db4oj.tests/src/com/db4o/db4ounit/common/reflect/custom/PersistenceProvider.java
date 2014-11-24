package com.db4o.db4ounit.common.reflect.custom;

import com.db4o.foundation.*;

/**
 * Models an external storage model to which db4o have to be adapted to.
 *
 * This particular one is a tuple based persistence mechanism modeled after the GigaSpaces
 * persistence API.
 *
 * There are only two types of fields supported: string and int which are mapped
 * to java.lang.String and java.lang.Integer.
 */
public interface PersistenceProvider {

	void initContext(PersistenceContext context);

	void closeContext(PersistenceContext context);
	
	void purge(String url);

	void createEntryClass(PersistenceContext context,
			String className, String[] fieldNames, String[] fieldTypes);

	void createIndex(PersistenceContext context, String className, String fieldName);

	void dropIndex(PersistenceContext context, String className, String fieldName);

	void dropEntryClass(PersistenceContext context, String className);

	void insert(PersistenceContext context, PersistentEntry entry);

	void update(PersistenceContext context, PersistentEntry entry);

	int delete(PersistenceContext context, String className, Object uid);

	Iterator4 select(PersistenceContext context, PersistentEntryTemplate template);
}
