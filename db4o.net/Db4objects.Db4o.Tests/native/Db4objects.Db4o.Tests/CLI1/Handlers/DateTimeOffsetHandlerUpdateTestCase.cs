/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
#if !CF && !SILVERLIGHT
using System;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	internal class DateTimeOffsetHandlerUpdateTestCase : ValueTypeHandlerUpdateTestCaseBase<DateTimeOffset>
	{
		protected override DateTimeOffset[] GetData()
		{
			return new DateTimeOffset[]
			       	{
						DateTimeOffset.MinValue,
						new DateTimeOffset(2009, 12, 11,17,03,35,980, TimeSpan.FromHours(5)),
						DateTimeOffset.MaxValue
					};
		}
	}
}
#endif