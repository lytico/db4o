Db4oAdmin
=========

Db4o command line utility.

Execute Db4oAdmin.exe without any arguments to see the help information.

Enabling delegate based native queries for the Compact Framework
================================================================

Background Information
----------------------

The CompactFramework API is missing two important properties in the
System.Delegate type: Target and Method.

Without these two properties is not possible to discover which method and
object are behind a delegate reference making it impossible for db4o
to do any query analysis at runtime.

The Solution
------------

The instrumentation tool works by replacing invocations to:

    ObjectContainer.Query<Extent>(System.Predicate<Extent> match)
	
by:

    NativeQueryHandler.ExecuteQuery<Extent>(...)
			
inserting the appropriate stack adjustments instructions whenever
possible.

Db4oAdmin.Tests
=============================

The unit tests.