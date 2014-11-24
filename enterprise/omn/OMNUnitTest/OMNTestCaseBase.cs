using System.IO;
using Db4objects.Db4o;
using NUnit.Framework;
using OManager.BusinessLayer.Login;
using OManager.DataLayer.Connection;

namespace OMNUnitTest
{
	public abstract class OMNTestCaseBase
	{
		[SetUp]
		public void Setup()
		{
			GenerateDatabase();
			SetupTest();
		}

		[TearDown]
		public void TearDown()
		{
			TearDownTest();
			IObjectContainer client = Db4oClient.Client;
			if (null != client)
			{
				string connection = Db4oClient.CurrentConnParams .Connection;
				Db4oClient.CloseConnection();
				File.Delete(connection);
			}
		}

		protected virtual void TearDownTest()
		{
		}

		protected virtual void SetupTest()
		{
		}
		private void GenerateDatabase()
		{
			string databaseFile = Path.GetTempFileName();
			Db4oClient.CurrentConnParams  = new ConnParams(databaseFile,false);
			Store();
		}
		
		protected void Reopen()
		{
			string connection = Db4oClient.CurrentConnParams.Connection;
			Db4oClient.CloseConnection();
            Db4oClient.CurrentConnParams = new ConnParams(connection, false);
		}
		
		protected static void Store(object obj)
		{
			Db.Store(obj);
		}

		protected static void Store(object obj, int depth)
		{
			Db.Ext().Store(obj, depth);
		}

		protected static IObjectContainer Db
		{
			get
			{
				return Db4oClient.Client;
			}
		}

		protected abstract void Store();
	}
}
