/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class AspectVersionContextImpl : IAspectVersionContext
	{
		private readonly int _declaredAspectCount;

		private AspectVersionContextImpl(int count)
		{
			_declaredAspectCount = count;
		}

		public virtual int DeclaredAspectCount()
		{
			return _declaredAspectCount;
		}

		public virtual void DeclaredAspectCount(int count)
		{
			throw new InvalidOperationException();
		}

		public static readonly Db4objects.Db4o.Internal.Marshall.AspectVersionContextImpl
			 AlwaysEnabled = new Db4objects.Db4o.Internal.Marshall.AspectVersionContextImpl(
			int.MaxValue);

		public static readonly Db4objects.Db4o.Internal.Marshall.AspectVersionContextImpl
			 CheckAlwaysEnabled = new Db4objects.Db4o.Internal.Marshall.AspectVersionContextImpl
			(int.MaxValue - 1);

		public static IAspectVersionContext ForSize(int count)
		{
			return new Db4objects.Db4o.Internal.Marshall.AspectVersionContextImpl(count);
		}
	}
}
