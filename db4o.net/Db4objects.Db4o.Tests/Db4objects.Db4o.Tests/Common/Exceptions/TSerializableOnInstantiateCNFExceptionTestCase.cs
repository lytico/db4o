/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class TSerializableOnInstantiateCNFExceptionTestCase : AbstractDb4oTestCase
		, IOptOutDefragSolo
	{
		public static void Main(string[] args)
		{
			new TSerializableOnInstantiateCNFExceptionTestCase().RunAll();
		}
	}
}
