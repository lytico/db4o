/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public partial class Pre7_1ObjectContainerAdapter : AbstractObjectContainerAdapter
	{
		public override void Store(object obj)
		{
			StoreObject(obj);
		}

		public override void Store(object obj, int depth)
		{
			StoreObject(obj, depth);
		}

		private void StoreObject(object obj)
		{
			try
			{
				StoreInternal(ResolveSetMethod(), new object[] { obj });
			}
			catch (Exception e)
			{
				Assert.Fail("Call to set method failed.", e);
			}
		}

		private void StoreObject(object obj, int depth)
		{
			try
			{
				StoreInternal(ResolveSetWithDepthMethod(), new object[] { obj, depth });
			}
			catch (Exception e)
			{
				Assert.Fail("Call to set method failed.", e);
			}
		}

		public virtual void StoreInternal(MethodInfo method, object[] args)
		{
			try
			{
				method.Invoke(db, args);
			}
			catch (Exception e)
			{
				Assert.Fail(e.ToString());
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}

		/// <exception cref="System.Exception"></exception>
		private MethodInfo ResolveSetWithDepthMethod()
		{
			if (setWithDepthMethod != null)
			{
				return setWithDepthMethod;
			}
			setWithDepthMethod = db.GetType().GetMethod(SetMethodName(), new Type[] { typeof(
				object), typeof(int) });
			return setWithDepthMethod;
		}

		/// <exception cref="System.Exception"></exception>
		private MethodInfo ResolveSetMethod()
		{
			if (setMethod != null)
			{
				return setMethod;
			}
			setMethod = db.GetType().GetMethod(SetMethodName(), new Type[] { typeof(object) }
				);
			return setMethod;
		}

		private MethodInfo setWithDepthMethod = null;

		private MethodInfo setMethod = null;
	}
}
