/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	public abstract class ShutdownMode
	{
		public class NormalMode : ShutdownMode
		{
			internal NormalMode()
			{
			}

			public override bool IsFatal()
			{
				return false;
			}
		}

		public class FatalMode : ShutdownMode
		{
			private Exception _exc;

			internal FatalMode(Exception exc)
			{
				_exc = exc;
			}

			public virtual Exception Exc()
			{
				return _exc;
			}

			public override bool IsFatal()
			{
				return true;
			}
		}

		public static readonly ShutdownMode Normal = new ShutdownMode.NormalMode();

		public static ShutdownMode Fatal(Exception exc)
		{
			return new ShutdownMode.FatalMode(exc);
		}

		public abstract bool IsFatal();

		private ShutdownMode()
		{
		}
	}
}
