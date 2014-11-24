using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Spikes
{
	static class Configurations
	{
		public static IConfiguration IndexedFields()
		{
			var config = Db4oFactory.NewConfiguration();
			config.ObjectClass(typeof(Cow)).ObjectField("Code").Indexed(true);

			var cowEventClass = config.ObjectClass(typeof(CowEvent));
			cowEventClass.ObjectField("Cow").Indexed(true);
			cowEventClass.ObjectField("Date").Indexed(true);
			return config;
		}

		public static IConfiguration Faster()
		{
			var config = IndexedFields();
			
			config.WeakReferences(false);
			config.BTreeCacheHeight(3);
			config.BTreeNodeSize(120);

			config.FlushFileBuffers(false);

			config.Callbacks(false);
			config.CallConstructors(false);

			return config;
		}
	}
}
