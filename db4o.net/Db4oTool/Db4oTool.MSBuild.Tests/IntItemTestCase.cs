using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4oTool.MSBuild.Tests
{
    class IntItemTestCase : AbstractDb4oTestCase
    {
        protected override void Configure(IConfiguration config)
        {
            config.Add(new TransparentActivationSupport());
        }

        private const int DEPTH = 10;

        protected override void Store()
        {
            IntItem item = IntItem.NewIntItem(DEPTH);
            item._isRoot = true;
            Store(item);
        }

        public void Test()
        {
            IntItem item = RetrieveRoot();
            Assert.IsNotNull(item);

            for (int i = 0; i < DEPTH - 1; i++)
            {
                Assert.AreEqual(0, GetField(item, "_intValue"));
                Assert.IsNull(GetField(item, "_next"));
                Assert.AreEqual(DEPTH - i, item.GetIntValue());
                Assert.IsNotNull(item.Next());
                Assert.AreEqual(DEPTH - i, GetField(item, "_intValue"));
                Assert.IsNotNull(GetField(item, "_next"));
                item = item.Next();
            }

            Assert.AreEqual(0, GetField(item, "_intValue"));
            Assert.IsNull(GetField(item, "_next"));
            Assert.AreEqual(1, item.GetIntValue());
            Assert.IsNull(GetField(item, "_next"));
        }

        private IntItem RetrieveRoot()
        {
            IQuery query = Db().Query();
            query.Constrain(typeof(IntItem));
            query.Descend("_isRoot").Constrain(true);
            IObjectSet result = query.Execute();
            return result.HasNext() ? (IntItem)result.Next() : null;
        }

        private object GetField(object obj, string fieldName)
        {
            IReflectClass clazz = Reflector().ForObject(obj);
            IReflectField field = clazz.GetDeclaredField(fieldName);
            return field.Get(obj);
        }
    }
}
