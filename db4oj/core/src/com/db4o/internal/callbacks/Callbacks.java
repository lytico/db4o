/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.callbacks;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.query.*;

public interface Callbacks {

	boolean objectCanNew(Transaction transaction, Object obj);
	boolean objectCanActivate(Transaction transaction, Object obj);
	boolean objectCanUpdate(Transaction transaction, ObjectInfo objectInfo);
	boolean objectCanDelete(Transaction transaction, ObjectInfo objectInfo);
	boolean objectCanDeactivate(Transaction transaction, ObjectInfo objectInfo);

	void objectOnActivate(Transaction transaction, ObjectInfo obj);
	void objectOnNew(Transaction transaction, ObjectInfo obj);
	void objectOnUpdate(Transaction transaction, ObjectInfo obj);
	void objectOnDelete(Transaction transaction, ObjectInfo obj);
	void objectOnDeactivate(Transaction transaction, ObjectInfo obj);
	void objectOnInstantiate(Transaction transaction, ObjectInfo obj);

	void queryOnStarted(Transaction transaction, Query query);
	void queryOnFinished(Transaction transaction, Query query);
	
	boolean caresAboutCommitting();
	boolean caresAboutCommitted();
	
	void classOnRegistered(ClassMetadata clazz);
	
	void commitOnStarted(Transaction transaction, CallbackObjectInfoCollections objectInfoCollections);
	void commitOnCompleted(Transaction transaction, CallbackObjectInfoCollections objectInfoCollections, boolean isOwnCommit);

    boolean caresAboutDeleting();
    boolean caresAboutDeleted();
    
    void openOnFinished(ObjectContainer container);
    void closeOnStarted(ObjectContainer container);
}
