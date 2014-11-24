/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;

/**
 * Marker interface to denote that implementing test cases using JavaServices
 * should be excluded from fixtures that run in an environment where the classpath
 * is not captured in the java.class.path system property.
 */
public interface OptOutNoInheritedClassPath extends OptOutFromTestFixture {
}
