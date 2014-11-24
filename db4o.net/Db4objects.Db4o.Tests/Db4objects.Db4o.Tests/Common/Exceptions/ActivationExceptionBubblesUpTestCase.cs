/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class ActivationExceptionBubblesUpTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new ActivationExceptionBubblesUpTestCase().RunAll();
		}

		[System.Serializable]
		public class AcceptAllEvaluation : IEvaluation
		{
			public virtual void Evaluate(ICandidate candidate)
			{
				candidate.Include(true);
			}
		}

		public sealed class ItemTranslator : IObjectTranslator
		{
			public void OnActivate(IObjectContainer container, object applicationObject, object
				 storedObject)
			{
				throw new ItemException();
			}

			public object OnStore(IObjectContainer container, object applicationObject)
			{
				return applicationObject;
			}

			public Type StoredClass()
			{
				return typeof(Item);
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(Item)).Translate(new ActivationExceptionBubblesUpTestCase.ItemTranslator
				());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new Item());
		}

		public virtual void Test()
		{
			Assert.Expect(typeof(ReflectException), typeof(ItemException), new _ICodeBlock_54
				(this));
		}

		private sealed class _ICodeBlock_54 : ICodeBlock
		{
			public _ICodeBlock_54(ActivationExceptionBubblesUpTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				IQuery q = this._enclosing.Db().Query();
				q.Constrain(typeof(Item));
				q.Constrain(new ActivationExceptionBubblesUpTestCase.AcceptAllEvaluation());
				q.Execute().Next();
			}

			private readonly ActivationExceptionBubblesUpTestCase _enclosing;
		}
	}
}
