/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Fixtures;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public abstract class TopLevelOperation : ILabeled
	{
		private readonly string _label;

		public TopLevelOperation(string label)
		{
			_label = label;
		}

		public abstract void Apply(DatabaseContext context);

		public virtual string Label()
		{
			return _label;
		}
	}
}
