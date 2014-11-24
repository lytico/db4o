/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4oUnit.Fixtures
{
	public class Contextful
	{
		protected readonly FixtureContext _context;

		public Contextful()
		{
			_context = CurrentContext();
		}

		protected virtual object Run(IClosure4 closure4)
		{
			return CombinedContext().Run(closure4);
		}

		protected virtual void Run(IRunnable runnable)
		{
			CombinedContext().Run(runnable);
		}

		private FixtureContext CombinedContext()
		{
			return CurrentContext().Combine(_context);
		}

		private FixtureContext CurrentContext()
		{
			return FixtureContext.Current;
		}
	}
}
