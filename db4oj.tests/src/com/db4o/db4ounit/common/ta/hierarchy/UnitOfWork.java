/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.ta.hierarchy;import java.util.*;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;
/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
class UnitOfWork extends ActivatableImpl {
		Date _started;	Date _finished;	String _name;
	public UnitOfWork(String name, Date started, Date finished) {		_name = name;		_started = started;		_finished = finished;	}	public String getName() {		// TA BEGIN		activate(ActivationPurpose.READ);		// TA END		return _name;	}		public long timeSpent() {		// TA BEGIN		activate(ActivationPurpose.READ);		// TA END		return _finished.getTime() - _started.getTime();	}}