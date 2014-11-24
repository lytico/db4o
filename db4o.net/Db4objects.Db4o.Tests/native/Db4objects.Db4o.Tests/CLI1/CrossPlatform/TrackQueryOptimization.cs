/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */
using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	internal class TrackQueryOptimization : IDiagnosticListener
	{
		private bool _optimized = true;

		public void OnDiagnostic(IDiagnostic d)
		{
			if ((d is NativeQueryNotOptimized) || (d is NativeQueryOptimizerNotLoaded))
			{
				_optimized = false;
			}
		}

		public bool Optimized
		{
			get
			{
				bool temp = _optimized;
				_optimized = true;

				return temp;
			}
		}
	}
}
