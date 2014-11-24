using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spikes
{
	public static class DateRangeExtensions
	{
		public static IEnumerable<DateTime> DaysUntil(this DateTime self, DateTime end)
		{
			DateTime current = self;
			while (current < end)
			{
				yield return current;
				current = current.AddDays(1);
			}
		}

	}
}
