using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;

namespace Timestamps
{
	namespace Framework
	{
		/// <summary>
		/// Interface for objects that want automatic timestamp support.
		/// </summary>
		public interface ITimestampable
		{
			DateTime Created { get; set; }
			DateTime Updated { get; set; }
		}

		/// <summary>
		/// Automatically keeps ITimestampable supporting objects uptodate.
		/// </summary>
		public class TimestampSupport : IConfigurationItem
		{
			public void Apply(IInternalObjectContainer container)
			{
				IEventRegistry registry = EventRegistryFactory.ForObjectContainer(container);
				registry.Creating += new CancellableObjectEventHandler(registry_Creating);
				registry.Updating += new CancellableObjectEventHandler(registry_Updating);
			}

			void registry_Creating(object sender, CancellableObjectEventArgs args)
			{
				ITimestampable timestampable = args.Object as ITimestampable;
				if (timestampable == null) return;

				timestampable.Created = DateTime.Now;
			}

			void registry_Updating(object sender, CancellableObjectEventArgs args)
			{
				ITimestampable timestampable = args.Object as ITimestampable;
				if (timestampable == null) return;

				timestampable.Updated = DateTime.Now;
			}

			public void Prepare(IConfiguration configuration)
			{
				// nothing to do here
			}

		}
	}

	public class Foo : Timestamps.Framework.ITimestampable
	{
		private DateTime _created;
		private DateTime _updated;

		public DateTime Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public DateTime Updated
		{
			get { return _updated; }
			set { _updated = value; }
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			using (IObjectContainer container = Db4oFactory.OpenFile(TimestampConfig(), "timestamps.odb"))
			{
				Foo foo = new Foo();
				container.Store(foo);
				container.Commit();

				DumpTimestamps(foo);
				
				container.Store(foo);
				container.Commit();

				DumpTimestamps(foo);

			}
		}

		private static IConfiguration TimestampConfig()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.Add(new Timestamps.Framework.TimestampSupport());
			return config;
		}

		private static void DumpTimestamps(Timestamps.Framework.ITimestampable foo)
		{
			Console.WriteLine("Created: {0}", foo.Created);
			Console.WriteLine("Updated: {0}", foo.Updated);
		}
	}
}
