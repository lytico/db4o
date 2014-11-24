/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.header;

import com.db4o.config.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ConfigurationSettingsTestCase extends AbstractDb4oTestCase
		implements OptOutMultiSession {

	/**
	 * @deprecated
	 */
	public void testChangingUuidSettings() throws Exception {

		fixture().config().generateUUIDs(ConfigScope.GLOBALLY);

		reopen();

		Assert.areEqual(ConfigScope.GLOBALLY, generateUUIDs());

		db().configure().generateUUIDs(ConfigScope.DISABLED);

		Assert.areEqual(ConfigScope.DISABLED, generateUUIDs());

		fixture().config().generateUUIDs(ConfigScope.GLOBALLY);

		reopen();

		Assert.areEqual(ConfigScope.GLOBALLY, generateUUIDs());

	}

	private ConfigScope generateUUIDs() {
		return ((LocalObjectContainer) db()).config().generateUUIDs();
	}
}
