/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class StoredClassExceptionBubblesUpTestCase : AbstractDb4oTestCase, IOptOutMultiSession
		, IOptOutDefragSolo
	{
		public static void Main(string[] args)
		{
			new StoredClassExceptionBubblesUpTestCase().RunSolo();
		}

		public sealed class ItemTranslator : IObjectTranslator
		{
			public void OnActivate(IObjectContainer container, object applicationObject, object
				 storedObject)
			{
			}

			public object OnStore(IObjectContainer container, object applicationObject)
			{
				return null;
			}

			public Type StoredClass()
			{
				throw new ItemException();
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(Item)).Translate(new StoredClassExceptionBubblesUpTestCase.ItemTranslator
				());
		}

		public virtual void Test()
		{
			Assert.Expect(typeof(ReflectException), typeof(ItemException), new _ICodeBlock_43
				(this));
		}

		private sealed class _ICodeBlock_43 : ICodeBlock
		{
			public _ICodeBlock_43(StoredClassExceptionBubblesUpTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Store(new Item());
			}

			private readonly StoredClassExceptionBubblesUpTestCase _enclosing;
		}
	}
}
