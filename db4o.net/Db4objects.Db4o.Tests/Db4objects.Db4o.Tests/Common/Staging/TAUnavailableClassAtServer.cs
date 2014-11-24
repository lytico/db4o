/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class TAUnavailableClassAtServer : AbstractDb4oTestCase, ICustomClientServerConfiguration
		, IOptOutAllButNetworkingCS
	{
		public static void Main(string[] args)
		{
			new TAUnavailableClassAtServer().RunNetworking();
		}

		public class ParentWithMultipleChilds
		{
			private TAUnavailableClassAtServer.Child[] _children = new TAUnavailableClassAtServer.Child
				[0];

			public ParentWithMultipleChilds(TAUnavailableClassAtServer _enclosing, TAUnavailableClassAtServer.Child
				[] children)
			{
				this._enclosing = _enclosing;
				this._children = children;
			}

			public virtual TAUnavailableClassAtServer.Child[] Children()
			{
				return this._children;
			}

			public virtual void Children(TAUnavailableClassAtServer.Child[] children)
			{
				this._children = children;
			}

			private readonly TAUnavailableClassAtServer _enclosing;
		}

		public class ParentWithSingleChild
		{
			private TAUnavailableClassAtServer.Child _child;

			public ParentWithSingleChild(TAUnavailableClassAtServer _enclosing, TAUnavailableClassAtServer.Child
				 child)
			{
				this._enclosing = _enclosing;
				this._child = child;
			}

			public virtual TAUnavailableClassAtServer.Child Child()
			{
				return this._child;
			}

			public virtual void Child(TAUnavailableClassAtServer.Child child)
			{
				this._child = child;
			}

			private readonly TAUnavailableClassAtServer _enclosing;
		}

		public class Child : ActivatableBase
		{
			private int _value;

			public Child(TAUnavailableClassAtServer _enclosing, int value)
			{
				this._enclosing = _enclosing;
				this._value = value;
			}

			public virtual int Value()
			{
				this.ActivateForRead();
				return this._value;
			}

			public virtual void Value(int value)
			{
				this.ActivateForWrite();
				this._value = value;
			}

			private readonly TAUnavailableClassAtServer _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureServer(IConfiguration config)
		{
			config.ReflectWith(new ExcludingReflector(new Type[] { typeof(TAUnavailableClassAtServer.Child
				), typeof(TAUnavailableClassAtServer.ParentWithMultipleChilds), typeof(TAUnavailableClassAtServer.ParentWithSingleChild
				) }));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureClient(IConfiguration config)
		{
			config.Add(new TransparentActivationSupport());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new TAUnavailableClassAtServer.ParentWithMultipleChilds(this, new TAUnavailableClassAtServer.Child
				[] { new TAUnavailableClassAtServer.Child(this, 42) }));
			Store(new TAUnavailableClassAtServer.ParentWithSingleChild(this, new TAUnavailableClassAtServer.Child
				(this, 43)));
		}

		public virtual void TestChildArray()
		{
			IExtObjectContainer client1 = OpenNewSession();
			IQuery query = client1.Query();
			query.Constrain(typeof(TAUnavailableClassAtServer.ParentWithMultipleChilds));
			IObjectSet result = query.Execute();
			Assert.IsTrue(result.HasNext());
			TAUnavailableClassAtServer.ParentWithMultipleChilds parent = (TAUnavailableClassAtServer.ParentWithMultipleChilds
				)result.Next();
			Assert.IsNotNull(parent.Children());
			client1.Close();
		}

		public virtual void TestSingleChild()
		{
			IExtObjectContainer client1 = OpenNewSession();
			IQuery query = client1.Query();
			query.Constrain(typeof(TAUnavailableClassAtServer.ParentWithSingleChild));
			IObjectSet result = query.Execute();
			Assert.IsTrue(result.HasNext());
			TAUnavailableClassAtServer.ParentWithSingleChild parent = (TAUnavailableClassAtServer.ParentWithSingleChild
				)result.Next();
			Assert.AreEqual(43, parent.Child().Value());
			client1.Close();
		}
	}
}
