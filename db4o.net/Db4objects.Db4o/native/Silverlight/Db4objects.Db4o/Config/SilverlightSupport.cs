/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
#if SILVERLIGHT
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// Configures the database to be used in a Silverlight application.
	/// </summary>
	/// <remarks>
	/// This configuration item basically configures db4o to use Silverlight isolatad storage. 
	/// If your Silverlight application may run "out of browser" you may want to not add this.
	/// </remarks>
	public class SilverlightSupport : IEmbeddedConfigurationItem
	{
		public void Prepare(IEmbeddedConfiguration configuration)
		{
			configuration.File.Storage = new IsolatedStorageStorage();
		}

		public void Apply(IEmbeddedObjectContainer db)
		{
		}
	}
}
#endif