/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.ta.hierarchy;import java.util.*;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;

/**
 */@decaf.Ignore(decaf.Platform.JDK11)
class Project extends ActivatableImpl {		List _subProjects = new com.db4o.db4ounit.common.ta.collections.PagedList();		List _workLog = new com.db4o.db4ounit.common.ta.collections.PagedList();		String _name;		public Project(String name) {		_name = name;	}		public void logWorkDone(UnitOfWork work) {		// TA BEGIN		activate(ActivationPurpose.READ);		// TA END		_workLog.add(work);	}	public long totalTimeSpent() {				// TA BEGIN		activate(ActivationPurpose.READ);		// TA END				long total = 0;		Iterator i = _workLog.iterator();		while (i.hasNext()) {			UnitOfWork item = (UnitOfWork) i.next();			total += item.timeSpent();		}		return total;	}}