/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package db4ounit.extensions.fixtures;

/**
 * Opts out from fixtures that require customized ways of creating/opening ObjectContainer
 * instances and won't work with test cases explicitly invoking the common factory methods.
 */
public interface OptOutCustomContainerInstantiation extends OptOutFromTestFixture {
}
