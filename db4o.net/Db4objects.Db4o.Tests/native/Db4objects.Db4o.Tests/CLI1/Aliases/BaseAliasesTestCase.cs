/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using System;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Aliases
{
    public class BaseAliasesTestCase : AbstractDb4oTestCase
    {
        protected void AssertAliasedData(IObjectContainer container)
        {
            AssertAliasedData(container.QueryByExample(GetAliasedDataType()));

            AssertAliasedData(QueryAliasedData(container));
        }

        protected IObjectSet QueryAliasedData(IObjectContainer container)
        {
            IQuery query = container.Query();
            query.Constrain(GetAliasedDataType());
            return query.Execute();
        }

        protected void AssertAliasedData(IObjectSet os)
        {
            AssertAliasedData(os, "Homer Simpson", "John Cleese");
        }

        protected void AssertAliasedData(IObjectSet os, params string[] expectedNames)
        {
            Assert.AreEqual(expectedNames.Length, os.Count);
            foreach (string name in expectedNames)
            {
                AssertContains(os, CreateAliasedData(name));
            }
        }

        protected virtual Type GetAliasedDataType()
        {
            return typeof(Person2);
        }

        protected object CreateAliasedData(string name)
        {
            return GetAliasedDataType()
                .GetConstructor(new Type[] { typeof(string) })
                    .Invoke(new object[] { name });
        }

        public static void AssertContains(IObjectSet actual, object expected)
        {
            actual.Reset();
            while (actual.HasNext())
            {
                object next = actual.Next();
                if (CFHelper.AreEqual(next, expected)) return;
            }
            Assert.Fail("Expected item: " + expected);
        }

    }
}
