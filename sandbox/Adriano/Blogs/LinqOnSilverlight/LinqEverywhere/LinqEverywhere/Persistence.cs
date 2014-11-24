using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace LinqEverywhere
{
	public class Persistence
	{
		private static IEmbeddedObjectContainer container;

		public static IObjectContainer Database
		{
			get
			{
				return InternalGetDatabase();
			}
		}

		public static void Close()
		{
			if (container != null)
			{
				container.Close();
			}
		}

		private static IObjectContainer InternalGetDatabase()
		{
			if (container == null)
			{
				container = Db4oEmbedded.OpenFile(NewConfiguration(), "LINQOnSilverlightDemostration.odb");
			}

			return container;
		}

		private static IEmbeddedConfiguration NewConfiguration()
		{
			var config = Db4oEmbedded.NewConfiguration();
			config.AddConfigurationItem(new SilverlightSupport());
			
			return config;
		}

		public static void Store<T>(T obj)
		{
			Database.Store(obj);
			Database.Commit();
		}
	}
}
