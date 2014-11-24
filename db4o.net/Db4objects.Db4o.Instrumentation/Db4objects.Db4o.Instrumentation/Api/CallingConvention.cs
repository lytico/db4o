/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Instrumentation.Api
{
	public sealed class CallingConvention
	{
		public static readonly Db4objects.Db4o.Instrumentation.Api.CallingConvention Static
			 = new Db4objects.Db4o.Instrumentation.Api.CallingConvention();

		public static readonly Db4objects.Db4o.Instrumentation.Api.CallingConvention Virtual
			 = new Db4objects.Db4o.Instrumentation.Api.CallingConvention();

		public static readonly Db4objects.Db4o.Instrumentation.Api.CallingConvention Interface
			 = new Db4objects.Db4o.Instrumentation.Api.CallingConvention();

		private CallingConvention()
		{
		}
	}
}
