/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oUnit.Extensions.Fixtures
{
	public class NonStandardBlockSizeFixture : Db4oSolo
	{
		protected override IObjectContainer CreateDatabase(IConfiguration config)
		{
			config.BlockSize(7);
			return base.CreateDatabase(config);
		}

		public override bool Accept(Type clazz)
		{
			return base.Accept(clazz) && !typeof(IOptOutNonStandardBlockSize).IsAssignableFrom
				(clazz);
		}

		public override string Label()
		{
			return "BlockSize-" + base.Label();
		}
	}
}
