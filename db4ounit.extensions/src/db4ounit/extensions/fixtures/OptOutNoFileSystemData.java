/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;

/**
 * Marker interface to denote that implementing test cases should be excluded
 * from running within a fixture that may not provide access to required data
 * on the file system. (This opt-out probably should be replaced by a less
 * file system dependent access mechanism to this data.)
 */
public interface OptOutNoFileSystemData extends OptOutFromTestFixture {

}
