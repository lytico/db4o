/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class StoreExceptionBubblesUpTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new StoreExceptionBubblesUpTestCase().RunNetworking();
		}

		public sealed class ItemTranslator : IObjectTranslator
		{
			public void OnActivate(IObjectContainer container, object applicationObject, object
				 storedObject)
			{
			}

			public object OnStore(IObjectContainer container, object applicationObject)
			{
				throw new ItemException();
			}

			public Type StoredClass()
			{
				return typeof(Item);
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(Item)).Translate(new StoreExceptionBubblesUpTestCase.ItemTranslator
				());
		}

		public virtual void Test()
		{
			ICodeBlock exception = new _ICodeBlock_41(this);
			Assert.Expect(typeof(ReflectException), exception);
		}

		private sealed class _ICodeBlock_41 : ICodeBlock
		{
			public _ICodeBlock_41(StoreExceptionBubblesUpTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Store(new Item());
			}

			private readonly StoreExceptionBubblesUpTestCase _enclosing;
		}
	}
}
