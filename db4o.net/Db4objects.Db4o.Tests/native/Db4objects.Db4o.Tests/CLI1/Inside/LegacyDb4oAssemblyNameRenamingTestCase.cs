/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

#if !SILVERLIGHT

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Api;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Inside
{
	public class LegacyDb4oAssemblyNameRenamingTestCase : TestWithTempFile
	{
		public void TestAssemblyNameEndingWithDb4o()
		{
			AssertNameRead("SomeType, exdb4o");
			AssertNameRead("SomeType, ex.db4o");
		}

		public void TestSimpleAssemblyName()
		{
			AssertNameRead("SomeType, some assembly");
		}

		public void TestLegacyDb4oAssemblyNames()
		{
			AssertNameRead("desktop.1, db4o-4.0-net1", "desktop.1, Db4objects.Db4o");
			AssertNameRead("cf1, db4o-4.0-compact1", "cf1, Db4objects.Db4o");
		}

		private void AssertNameRead(string name)
		{
			AssertNameRead(name, name);	
		}

		private void AssertNameRead(string originalName, string newName)
		{
			using (IObjectContainer db = Db4oEmbedded.OpenFile(TempFile()))
			{
				LocalObjectContainer localObjectContainer = (LocalObjectContainer) db;
				AssertNameRead(localObjectContainer, originalName, newName);
			}
		}

		private static void AssertNameRead(ObjectContainerBase localObjectContainer, string originalName, string newName)
		{
			byte[] originalBytes = localObjectContainer.StringIO().Write(originalName);
			byte[] updatedClassNameBytes = Platform4.UpdateClassName(originalBytes);
			Assert.AreEqual(newName, localObjectContainer.StringIO().Read(updatedClassNameBytes));
		}
	}
}

#endif