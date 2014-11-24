/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class ObjectContainerEnvironmentTestCase : AbstractDb4oTestCase
	{
		public virtual void TestMyObjectContainer()
		{
			Container().WithEnvironment(new _IRunnable_12(this));
		}

		private sealed class _IRunnable_12 : IRunnable
		{
			public _IRunnable_12(ObjectContainerEnvironmentTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				Assert.AreSame(this._enclosing.Container(), ((IObjectContainer)Environments.My(typeof(
					IObjectContainer))));
			}

			private readonly ObjectContainerEnvironmentTestCase _enclosing;
		}
	}
}
