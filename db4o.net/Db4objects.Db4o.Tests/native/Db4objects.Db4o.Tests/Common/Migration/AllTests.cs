/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Migration
{
    class AllTests : Db4oTestSuite
    {
        protected override Type[] TestCases()
        {
            return new Type[] {
#if !CF && !SILVERLIGHT
                typeof(Db4oNETMigrationTestSuite),
#endif
            };
        }
    }
}
