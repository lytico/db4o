/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2009  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using System.Collections.Generic;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Inside;
using Db4objects.Db4o;
using Db4oUnit;

namespace Db4objects.Drs.Tests.Regression
{
    abstract class GenericCollectionTestCaseBase : DrsTestCase
    {
        public void Test()
        {
            StoreToProviderA();
            ReplicateAllToProviderB();
            EnsureContent(B().Provider());
        }

        private void StoreToProviderA()
        {
            ITestableReplicationProviderInside providerA = A().Provider();
            providerA.StoreNew(CreateItem());
            providerA.Commit();
        }
        
        private void ReplicateAllToProviderB()
        {
            ReplicateAll(A().Provider(), B().Provider());
        }

        public object QueryItem(ITestableReplicationProviderInside provider, Type type)
        {
            IObjectSet result = provider.GetStoredObjects(type);
            Assert.AreEqual(1, result.Count);
            return result.Next();
        }

        public abstract object CreateItem();

        public abstract void EnsureContent(ITestableReplicationProviderInside provider);
    }
}
