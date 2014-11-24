/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Freespace;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Freespace
{
	/// <exclude></exclude>
	public class FreespaceManagerMigrationTestCase : FormatMigrationTestCaseBase
	{
		internal int[][] IntArrayData = new int[][] { new int[] { 1, 2 }, new int[] { 3, 
			4 } };

		internal string[][] StringArrayData = new string[][] { new string[] { "a", "b" }, 
			new string[] { "c", "d" } };

		public class StClass
		{
			public int id;

			public ArrayList vect;

			public virtual ArrayList GetVect()
			{
				return vect;
			}

			public virtual void SetVect(ArrayList vect)
			{
				this.vect = vect;
			}

			public virtual int GetId()
			{
				return id;
			}

			public virtual void SetId(int id)
			{
				this.id = id;
			}
		}

		protected override void ConfigureForStore(IConfiguration config)
		{
			CommonConfigure(config);
			config.Freespace().UseIndexSystem();
		}

		protected override bool IsApplicableForDb4oVersion()
		{
			return Db4oMajorVersion() >= 5;
		}

		protected override void ConfigureForTest(IConfiguration config)
		{
			CommonConfigure(config);
			config.Freespace().UseBTreeSystem();
		}

		protected override void DeconfigureForStore(IConfiguration config)
		{
			if (!IsApplicableForDb4oVersion())
			{
				return;
			}
			config.Freespace().UseRamSystem();
		}

		protected override void DeconfigureForTest(IConfiguration config)
		{
			if (!IsApplicableForDb4oVersion())
			{
				return;
			}
			config.Freespace().UseRamSystem();
		}

		private void CommonConfigure(IConfiguration config)
		{
			// config.blockSize(8);
			config.ObjectClass(typeof(FreespaceManagerMigrationTestCase.StClass)).CascadeOnActivate
				(true);
			config.ObjectClass(typeof(FreespaceManagerMigrationTestCase.StClass)).CascadeOnUpdate
				(true);
			config.ObjectClass(typeof(FreespaceManagerMigrationTestCase.StClass)).CascadeOnDelete
				(true);
			config.ObjectClass(typeof(FreespaceManagerMigrationTestCase.StClass)).MinimumActivationDepth
				(5);
			config.ObjectClass(typeof(FreespaceManagerMigrationTestCase.StClass)).UpdateDepth
				(10);
		}

		protected override void AssertObjectsAreReadable(IExtObjectContainer objectContainer
			)
		{
			IObjectSet objectSet = objectContainer.Query(typeof(FreespaceManagerMigrationTestCase.StClass
				));
			for (int i = 0; i < 2; i++)
			{
				FreespaceManagerMigrationTestCase.StClass cls = (FreespaceManagerMigrationTestCase.StClass
					)objectSet.Next();
				ArrayList v = cls.GetVect();
				int[][] intArray = (int[][])v[0];
				ArrayAssert.AreEqual(IntArrayData[0], intArray[0]);
				ArrayAssert.AreEqual(IntArrayData[1], intArray[1]);
				string[][] stringArray = (string[][])v[1];
				ArrayAssert.AreEqual(StringArrayData[0], stringArray[0]);
				ArrayAssert.AreEqual(StringArrayData[1], stringArray[1]);
				objectContainer.Delete(cls);
			}
		}

		protected override string FileNamePrefix()
		{
			return "freespace";
		}

		protected override void Store(IObjectContainerAdapter objectContainer)
		{
			for (int i = 0; i < 10; i++)
			{
				FreespaceManagerMigrationTestCase.StClass cls = new FreespaceManagerMigrationTestCase.StClass
					();
				ArrayList v = new ArrayList(10);
				v.Add(IntArrayData);
				v.Add(StringArrayData);
				cls.SetId(i);
				cls.SetVect(v);
				objectContainer.Store(cls);
			}
		}
	}
}
