/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */
namespace Db4objects.Db4o.Foundation
{
	class Closures4
	{
		public delegate object Closure();

		public static IClosure4 ForDelegate(Closure @delegate)
		{
			return new Closure4OverDelegate(@delegate);
		}

		internal class Closure4OverDelegate : IClosure4
		{
			private Closure _delegate;

			public Closure4OverDelegate(Closure @delegate)
			{
				_delegate = @delegate;
			}

			public object Run()
			{
				return _delegate();
			}
		}
	}
}
