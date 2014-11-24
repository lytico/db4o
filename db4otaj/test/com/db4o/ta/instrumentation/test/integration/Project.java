/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.ta.instrumentation.test.integration;import java.util.*;class Project {		List _subProjects = new com.db4o.db4ounit.common.ta.collections.PagedList();		List _workLog = new com.db4o.db4ounit.common.ta.collections.PagedList();		String _name;		public Project(String name) {		_name = name;	}	public void logWorkDone(UnitOfWork work) {		_workLog.add(work);	}	public long totalTimeSpent() {				long total = 0;		for (Iterator iter = _workLog.iterator(); iter.hasNext();) {			UnitOfWork item = (UnitOfWork) iter.next();			total += item.timeSpent();		}		return total;	}
	
	public boolean sameName(Project project) {
		return _name.equals(project._name);
	}}