/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.CS;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.CS
{
	/// <summary>required for CsSchemaUpdateTestCase</summary>
	public class CsSchemaMigrationSourceCode
	{
		public class Item
		{
			//update
			//assert
		}

		private static readonly string File = Runtime.GetProperty("java.io.tmpdir", ".") 
			+ Sharpen.IO.File.separator + "csmig.db4o";

		private const int Port = 4447;

		/// <exception cref="System.IO.IOException"></exception>
		public static void Main(string[] arguments)
		{
			new CsSchemaMigrationSourceCode().Run();
		}

		public virtual void Run()
		{
			//store
			IServerConfiguration conf = Db4oClientServer.NewServerConfiguration();
			IObjectServer server = Db4oClientServer.OpenServer(conf, File, Port);
			server.GrantAccess("db4o", "db4o");
			//store
			//update
			//assert
			server.Close();
		}

		//assert
		private void StoreItem()
		{
			IObjectContainer client = OpenClient();
			CsSchemaMigrationSourceCode.Item item = new CsSchemaMigrationSourceCode.Item();
			client.Store(item);
			client.Close();
		}

		//store
		private void UpdateItem()
		{
			IObjectContainer client = OpenClient();
			IQuery query = client.Query();
			query.Constrain(typeof(CsSchemaMigrationSourceCode.Item));
			CsSchemaMigrationSourceCode.Item item = (CsSchemaMigrationSourceCode.Item)query.Execute
				().Next();
			//update
			//assert
			client.Store(item);
			client.Close();
		}

		private IObjectContainer OpenClient()
		{
			return Db4oClientServer.OpenClient("localhost", Port, "db4o", "db4o");
		}

		private void AssertItem()
		{
			IObjectContainer client = OpenClient();
			IQuery query = client.Query();
			query.Constrain(typeof(CsSchemaMigrationSourceCode.Item));
			CsSchemaMigrationSourceCode.Item item = (CsSchemaMigrationSourceCode.Item)query.Execute
				().Next();
			Sharpen.Runtime.Out.WriteLine(item);
			client.Close();
		}
	}
}
#endif // !SILVERLIGHT
