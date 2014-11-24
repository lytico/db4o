/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
using System;
using System.Collections.Generic;
using Db4objects.Db4o.Tests.CLI1.Handlers;
using System.IO;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4objects.Db4o.Tests.CLI2.Handlers;

namespace Db4objects.Db4o.Tests.Common.Migration
{
#if !CF && !SILVERLIGHT
    class Db4oNETMigrationTestSuite : Db4oMigrationTestSuite
    {
        //override protected string[] Libraries()
        //{
        //    return new string[] { AssemblyPathFor("7.9") };
        //}

    	private string AssemblyPathFor(string version)
    	{
    		return WorkspaceServices.WorkspacePath("db4o.archives/net-2.0/" + version + "/Db4objects.Db4o.dll");
    	}

    	protected override Type[] TestCases()
        {
            if (!Directory.Exists(Db4oLibrarian.LibraryPath()))
            {
                TestPlatform.GetStdErr().WriteLine("DISABLED: " + GetType());
                return new Type[] { };
            }

            var list = new List<Type>();
            list.AddRange(base.TestCases());

            var netTypes = new[] {
                typeof(SimplestPossibleHandlerUpdateTestCase),
                typeof(GenericListVersionUpdateTestCase),
                typeof(GenericDictionaryVersionUpdateTestCase),
                typeof(DateTimeHandlerUpdateTestCase),
				typeof(DateTimeOffsetHandlerUpdateTestCase),
                typeof(IndexedDateTimeUpdateTestCase),
                typeof(DecimalHandlerUpdateTestCase),
				typeof(EnumHandlerUpdateTestCase),
                typeof(GUIDHandlerUpdateTestCase),
                typeof(GUIDHandlerUpdateIndexedOnCurrentVersionTestCase),
                typeof(GUIDHandlerUpdateIndexedOnPreviousVersionsTestCase),
                typeof(GUIDHandlerUpdateIndexedOnPreviousButNotOnCurrentVersionTestCase),
                typeof(HashtableUpdateTestCase),
                typeof(NestedStructHandlerUpdateTestCase),
                typeof(SByteHandlerUpdateTestCase),
                typeof(StructHandlerUpdateTestCase),
                typeof(UIntHandlerUpdateTestCase),
                typeof(ULongHandlerUpdateTestCase),
                typeof(UShortHandlerUpdateTestCase),
				typeof(HandlerVersionWhenSeekingToFieldTestCase),
            };

            list.AddRange(netTypes);
    		return list.ToArray();
        }
    }
#endif
}
