/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections;

import java.util.*;

import com.db4o.collections.*;
import com.db4o.db4ounit.common.ta.*;
import com.db4o.ext.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ArrayList4TATestCaseBase extends TransparentActivationTestCaseBase {
	
	@Override
	protected void store() throws Exception {
		List<Integer> list = new ArrayList4<Integer>();
		ArrayList4Asserter.createList(list);
		store(list);
	}
	
	protected ArrayList4<Integer> retrieveAndAssertNullArrayList4() throws Exception{
		return CollectionsUtil.retrieveAndAssertNullArrayList4(db(), reflector());
	}
	
	protected ArrayList4<Integer> retrieveAndAssertNullArrayList4(ExtObjectContainer oc) throws Exception{
		return CollectionsUtil.retrieveAndAssertNullArrayList4(oc, reflector());
	}

}
