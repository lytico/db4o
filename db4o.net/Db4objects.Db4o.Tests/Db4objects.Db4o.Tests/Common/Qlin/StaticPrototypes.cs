/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Tests.Common.Qlin;

namespace Db4objects.Db4o.Tests.Common.Qlin
{
	public class StaticPrototypes
	{
		internal static BasicQLinTestCase.Cat cat = ((BasicQLinTestCase.Cat)QLinSupport.Prototype
			(typeof(BasicQLinTestCase.Cat)));

		internal static BasicQLinTestCase.Dog dog = ((BasicQLinTestCase.Dog)QLinSupport.Prototype
			(typeof(BasicQLinTestCase.Dog)));
	}
}
#endif // !SILVERLIGHT
