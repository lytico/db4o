/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Config.Attributes
{
	interface IDb4oAttribute
	{
		void Apply (object subject, ConfigurationIntrospector introspector);
	}
}
