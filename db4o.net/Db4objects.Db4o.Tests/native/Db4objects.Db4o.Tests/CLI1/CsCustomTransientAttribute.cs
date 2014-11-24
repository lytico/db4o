/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class CsCustomTransientAttribute : AbstractDb4oTestCase
    {
        [CustomTransient]
        public String myTransient;

		public String myPersistent;

        protected override void Configure(Db4objects.Db4o.Config.IConfiguration config)
        {
            base.Configure(config);
            config.MarkTransient(typeof(CustomTransientAttribute).FullName);
        }

        override protected void Store()
        {
            CsCustomTransientAttribute item = new CsCustomTransientAttribute();
            item.myTransient = "trans";
            item.myPersistent = "pers";
            Store(item);
        }

        public void Test()
        {
            CsCustomTransientAttribute instance = (CsCustomTransientAttribute)RetrieveOnlyInstance(GetType());
            Assert.IsNull(instance.myTransient);
            Assert.AreEqual("pers", instance.myPersistent);
        }
    }

    public class CustomTransientAttribute : Attribute
    {
        public CustomTransientAttribute()
        {
        }
    }

}
