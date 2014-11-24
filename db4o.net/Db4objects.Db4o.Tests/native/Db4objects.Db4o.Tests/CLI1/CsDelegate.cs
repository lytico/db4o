/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class OnActivateEventStrategy
    {
        public event EventHandler Crash;

        public void ObjectOnActivate(IObjectContainer container)
        {
            Assert.IsNull(Crash);
            Crash += new EventHandler(Boom);
        }

        public void RaiseCrash()
        {
            if (null != Crash)
            {
                Crash(this, EventArgs.Empty);
            }
        }

        static string Message = null;

        public static void Prepare()
        {
            Message = null;
        }

        public static void Check()
        {
            Assert.AreEqual("Boom!!!!", OnActivateEventStrategy.Message);
        }

        static void Boom(object sender, EventArgs args)
        {
            Message = "Boom!!!!";
        }
    }

    public class CsDelegate : AbstractDb4oTestCase
    {
        public event EventHandler Bang;

        public object UntypedDelegate;

		public static string Message = null;

        public void RaiseBang()
        {
            Bang(this, EventArgs.Empty);
        }

        override protected void Store()
        {
            CsDelegate item = new CsDelegate();
            item.Bang += new EventHandler(OnBang);
            item.UntypedDelegate = new EventHandler(OnBang);
            Store(item);
        }

        public void TestFieldsAreNotStored()
        {
            CsDelegate instance = (CsDelegate)RetrieveOnlyInstance(GetType());
            // delegate fields are simply not stored
            Assert.AreEqual(null, instance.Bang);
            Assert.AreEqual(null, instance.UntypedDelegate);
        }

        public void TestOnActivateEventStrategy()
        {
            DeleteAllInstances(typeof(OnActivateEventStrategy));
            Store(new OnActivateEventStrategy());
            Fixture().Reopen(this);

            OnActivateEventStrategy.Prepare();
			OnActivateEventStrategy obj = (OnActivateEventStrategy)Db().QueryByExample(typeof(OnActivateEventStrategy)).Next();
            obj.RaiseCrash();
            OnActivateEventStrategy.Check();
        }

        private void DeleteAllInstances(Type type)
        {
            foreach (object item in Db().Query(type))
            {
                Db().Delete(item);
            }
        }

        static void OnBang(object sender, EventArgs args)
        {
            CsDelegate.Message = "Bang!!!!";
        }
    }
}
