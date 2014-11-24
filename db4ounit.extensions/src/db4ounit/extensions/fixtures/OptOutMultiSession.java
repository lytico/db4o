/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;


/**
 * Marker interface to denote that implementing test cases should be excluded
 * from running both with the embedded and networking Client/Server fixture.
 */
public interface OptOutMultiSession extends OptOutFromTestFixture {
}
