/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	public class LinkedArrayTestCase : AbstractDb4oTestCase, IOptOutTA
	{
		internal static int TestedDepth = 7;

		public static void Main(string[] args)
		{
			new LinkedArrayTestCase().RunAll();
		}

		private Db4oUUID _linkedArraysUUID;

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateUUIDs(ConfigScope.Globally);
			config.Add(new TransparentActivationSupport());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			LinkedArrays linkedArrays = LinkedArrays.NewLinkedArrayRoot(TestedDepth);
			Store(linkedArrays);
			_linkedArraysUUID = Db().GetObjectInfo(linkedArrays).GetUUID();
		}

		public virtual void TestTheTest()
		{
			for (int depth = 1; depth < TestedDepth; depth++)
			{
				LinkedArrays linkedArrays = LinkedArrays.NewLinkedArrays(depth);
				linkedArrays.AssertActivationDepth(depth - 1, false);
			}
		}

		public virtual void TestActivateFixedDepth()
		{
			LinkedArrays linkedArrays = Root();
			for (int depth = 0; depth < TestedDepth; depth++)
			{
				Db().Activate(linkedArrays, depth);
				linkedArrays.AssertActivationDepth(depth, false);
				Db().Deactivate(linkedArrays, int.MaxValue);
			}
		}

		public virtual void TestActivatingActive()
		{
			LinkedArrays linkedArrays = Root();
			for (int secondActivationDepth = 2; secondActivationDepth < TestedDepth; secondActivationDepth
				++)
			{
				for (int firstActivationDepth = 1; firstActivationDepth < secondActivationDepth; 
					firstActivationDepth++)
				{
					Db().Activate(linkedArrays, firstActivationDepth);
					Db().Activate(linkedArrays, secondActivationDepth);
					linkedArrays.AssertActivationDepth(secondActivationDepth, false);
					Db().Deactivate(linkedArrays, int.MaxValue);
				}
			}
		}

		public virtual void TestActivateDefaultMode()
		{
			LinkedArrays linkedArrays = Root();
			Db().Activate(linkedArrays);
			linkedArrays.AssertActivationDepth(TestedDepth - 1, true);
		}

		public virtual void TestPeekPersisted()
		{
			LinkedArrays linkedArrays = Root();
			for (int depth = 0; depth < TestedDepth; depth++)
			{
				LinkedArrays peeked = (LinkedArrays)((LinkedArrays)Db().PeekPersisted(linkedArrays
					, depth, true));
				peeked.AssertActivationDepth(depth, false);
			}
		}

		public virtual void TestTransparentActivationQuery()
		{
			LinkedArrays linkedArray = QueryForRoot();
			linkedArray.AssertActivationDepth(TestedDepth - 1, true);
		}

		public virtual void TestTransparentActivationTraversal()
		{
			LinkedArrays root = QueryForRoot();
			LinkedArrays.ActivatableItem activatableItem = root._activatableItemArray[0];
			activatableItem.Activate(ActivationPurpose.Read);
			LinkedArrays descendant = activatableItem._linkedArrays;
			descendant.AssertActivationDepth(TestedDepth - 3, true);
			Db().Deactivate(activatableItem, 1);
			activatableItem.Activate(ActivationPurpose.Read);
			descendant.AssertActivationDepth(TestedDepth - 3, true);
		}

		private LinkedArrays QueryForRoot()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(LinkedArrays));
			q.Descend("_isRoot").Constrain(true);
			return (LinkedArrays)q.Execute().Next();
		}

		private LinkedArrays Root()
		{
			return (LinkedArrays)Db().GetByUUID(_linkedArraysUUID);
		}
	}
}
