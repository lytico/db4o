/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Bench.Crud;
using Db4objects.Db4o.Bench.Logging;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Bench.Crud
{
	/// <summary>
	/// Very simple CRUD (Create, Read, Update, Delete) application to
	/// produce log files as an input for I/O-benchmarking.
	/// </summary>
	/// <remarks>
	/// Very simple CRUD (Create, Read, Update, Delete) application to
	/// produce log files as an input for I/O-benchmarking.
	/// </remarks>
	public class CrudApplication
	{
		private static readonly string DatabaseFile = "simplecrud.db4o";

		public virtual void Run(int itemCount)
		{
			DeleteDbFile();
			Create(itemCount, NewConfiguration(itemCount));
			Read(NewConfiguration(itemCount));
			Update(NewConfiguration(itemCount));
			Delete(NewConfiguration(itemCount));
			DeleteDbFile();
		}

		private void Create(int itemCount, IEmbeddedConfiguration config)
		{
			IObjectContainer oc = Open(config);
			for (int i = 0; i < itemCount; i++)
			{
				oc.Store(Item.NewItem(i));
				// preventing heap space problems by committing from time to time
				if (i % 100000 == 0)
				{
					oc.Commit();
				}
			}
			oc.Commit();
			oc.Close();
		}

		private void Read(IEmbeddedConfiguration config)
		{
			IObjectContainer oc = Open(config);
			IObjectSet objectSet = AllItems(oc);
			while (objectSet.HasNext())
			{
				Item item = (Item)objectSet.Next();
			}
			oc.Close();
		}

		private void Update(IEmbeddedConfiguration config)
		{
			IObjectContainer oc = Open(config);
			IObjectSet objectSet = AllItems(oc);
			while (objectSet.HasNext())
			{
				Item item = (Item)objectSet.Next();
				item.Change();
				oc.Store(item);
			}
			oc.Close();
		}

		private void Delete(IEmbeddedConfiguration config)
		{
			IObjectContainer oc = Open(config);
			IObjectSet objectSet = AllItems(oc);
			while (objectSet.HasNext())
			{
				oc.Delete(objectSet.Next());
				// adding commit results in more syncs in the log, 
				// which is necessary for meaningful statistics!
				oc.Commit();
			}
			oc.Close();
		}

		private IEmbeddedConfiguration NewConfiguration(int itemCount)
		{
			FileStorage rafAdapter = new FileStorage();
			IStorage ioAdapter = new LoggingStorage(rafAdapter, LogFileName(itemCount));
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = ioAdapter;
			return config;
		}

		private void DeleteDbFile()
		{
			new Sharpen.IO.File(DatabaseFile).Delete();
		}

		private IObjectSet AllItems(IObjectContainer oc)
		{
			return oc.Query(typeof(Item));
		}

		private IObjectContainer Open(IEmbeddedConfiguration config)
		{
			return Db4oEmbedded.OpenFile(config, DatabaseFile);
		}

		public static string LogFileName(int itemCount)
		{
			return "simplecrud_" + itemCount + ".log";
		}
	}
}
