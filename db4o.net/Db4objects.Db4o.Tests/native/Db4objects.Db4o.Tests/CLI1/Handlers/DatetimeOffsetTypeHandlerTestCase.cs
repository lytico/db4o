/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System;
using Db4objects.Db4o.Query;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	public class DatetimeOffsetTypeHandlerTestCase : ValueTypeHandlerTestCaseBase<DateTimeOffset>
	{
		private static readonly ValueTypeHolder[] Objects = new ValueTypeHolder[]
		                                           	{
		                                           		new ValueTypeHolder(DateTimeOffset.MinValue, new ValueTypeHolder(DateTimeOffset.Now.AddDays(1))), 	
														new ValueTypeHolder(DateTimeOffset.Now, new ValueTypeHolder(DateTimeOffset.Now.AddDays(2))), 	
														new ValueTypeHolder(DateTimeOffset.MaxValue, new ValueTypeHolder(DateTimeOffset.Now.AddDays(3))), 	
													};

		protected override ValueTypeHolder[] ObjectsToStore()
		{
			return Objects;
		}

		protected override ValueTypeHolder[] ObjectsToOperateOn()
		{
			return new ValueTypeHolder[]
			       	{
						Objects[0],
						Objects[1]
					};
		}

		protected override DateTimeOffset UpdateValueFor(ValueTypeHolder holder)
		{
			holder.Value = holder.Value.AddYears(2009);
			return holder.Value;
		}

		public void TestGreater()
		{
			DateTimeOffset constraint = Objects[2].Value;
			IQuery query = NewDescendingQuery(	delegate(IQuery descendingQuery)
			                                  	{
			                                  		descendingQuery.Constrain(constraint).Greater();
			                                  	});

			IteratorAssert.AreEqual(
				Array.FindAll(Objects, delegate(ValueTypeHolder candidate) { return candidate.Value > constraint; }),
				query.Execute().GetEnumerator());
		}
	}
}

#endif