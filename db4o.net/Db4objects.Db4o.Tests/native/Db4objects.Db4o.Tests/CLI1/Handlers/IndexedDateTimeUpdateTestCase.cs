using System;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    class IndexedDateTimeUpdateTestCase : HandlerUpdateTestCaseBase
    {

        public class Item
        {
            public DateTime _dateTime;

            public Item(DateTime dateTime)
            {
                _dateTime = dateTime;
            }
        }

        protected override void AssertQueries(IExtObjectContainer objectContainer)
        {
            AssertDateTimeQuery(objectContainer, DateTime.MinValue);
            AssertDateTimeQuery(objectContainer, DateTime.MaxValue);
        }

        private void AssertDateTimeQuery(IExtObjectContainer objectContainer, DateTime value)
        {
            IQuery query = objectContainer.Query();
            query.Constrain(typeof (Item));
            query.Descend("_dateTime").Constrain(value);
            Assert.AreEqual(1, query.Execute().Count);
        }


        protected override string TypeName()
        {
            return "indexedDate";
        }

        protected override object[] CreateValues()
        {
            object[] items = new object[]{
             new Item(DateTime.MinValue),
             new Item(DateTime.MaxValue),
            };
            return items;
        }

        protected override object CreateArrays()
        {
            return null;
        }

        protected override void ConfigureForStore(IConfiguration config)
        {
            config.ObjectClass(typeof(Item)).ObjectField("_dateTime").Indexed(true);
        }


        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {

        }

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
            
        }
    }
}
