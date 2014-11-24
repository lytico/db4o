/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;

import db4ounit.extensions.*;

/**
 * 'Abstract' marker interface to denote that implementing test cases should be
 * excluded from running against specific fixtures. Concrete marker interfaces
 * for specific fixtures should extend OptOutFromTestFixture.
 */
public interface OptOutFromTestFixture extends Db4oTestCase {

}
