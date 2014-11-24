/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Activation
{
	public sealed class ActivationMode
	{
		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Activate
			 = new Db4objects.Db4o.Internal.Activation.ActivationMode();

		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Deactivate
			 = new Db4objects.Db4o.Internal.Activation.ActivationMode();

		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Peek = 
			new Db4objects.Db4o.Internal.Activation.ActivationMode();

		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Prefetch
			 = new Db4objects.Db4o.Internal.Activation.ActivationMode();

		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Refresh
			 = new Db4objects.Db4o.Internal.Activation.ActivationMode();

		private ActivationMode()
		{
		}

		public override string ToString()
		{
			if (IsActivate())
			{
				return "ACTIVATE";
			}
			if (IsDeactivate())
			{
				return "DEACTIVATE";
			}
			if (IsPrefetch())
			{
				return "PREFETCH";
			}
			if (IsRefresh())
			{
				return "REFRESH";
			}
			return "PEEK";
		}

		public bool IsDeactivate()
		{
			return this == Deactivate;
		}

		public bool IsActivate()
		{
			return this == Activate;
		}

		public bool IsPeek()
		{
			return this == Peek;
		}

		public bool IsPrefetch()
		{
			return this == Prefetch;
		}

		public bool IsRefresh()
		{
			return this == Refresh;
		}
	}
}
