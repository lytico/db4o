/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	public class GuidTypeHandlerTestCase : ValueTypeHandlerTestCaseBase<Guid>
	{
		protected override ValueTypeHolder[] ObjectsToStore()
		{
			return Objects;
		}

		protected override ValueTypeHolder[] ObjectsToOperateOn()
		{
			return new[] { Objects[0], Objects[1] };
		}

		protected override Guid UpdateValueFor(ValueTypeHolder holder)
		{
			holder.Value = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0xA, 0xB);
			return holder.Value;
		}

		private static Guid NewGuidFor(int i)
		{
			return new Guid(126 + i, 0, 0, 0, 0, 0, 0, 0, 0, 0, (byte)i);
		}

		private ValueTypeHolder[] Objects = new[]
			                            	{
			                            		new ValueTypeHolder(NewGuidFor(1), new ValueTypeHolder(NewGuidFor(10))), 
			                            		new ValueTypeHolder(NewGuidFor(2), new ValueTypeHolder(NewGuidFor(20))), 
			                            		new ValueTypeHolder(NewGuidFor(3), new ValueTypeHolder(NewGuidFor(30))), 
			                            		new ValueTypeHolder(NewGuidFor(4), new ValueTypeHolder(NewGuidFor(40))), 
			                            	};
	}
}
