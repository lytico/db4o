using System;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	class DatePlatform
	{
		public static readonly long MinDate = DateTime.MinValue.Ticks;

		public static readonly long MaxDate = DateTime.MaxValue.Ticks;
	}
}
