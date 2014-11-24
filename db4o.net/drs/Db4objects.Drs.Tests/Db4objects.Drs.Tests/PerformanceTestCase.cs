/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class PerformanceTestCase : DrsTestCase
	{
		private static int ListHolderCount = 10;

		private static int ObjectCount = 100;

		private static int TotalObjectCount = ListHolderCount + (ListHolderCount * ObjectCount
			 * 2);

		public virtual void Test()
		{
			Sharpen.Runtime.Out.WriteLine("**** Simple Replication Performance Test ****");
			long duration = StopWatch.Time(new _IBlock4_24(this));
			Sharpen.Runtime.Out.WriteLine("**** Total time taken " + duration + "ms ****");
		}

		private sealed class _IBlock4_24 : IBlock4
		{
			public _IBlock4_24(PerformanceTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				Sharpen.Runtime.Out.WriteLine("Storing in " + this._enclosing.A().Provider().GetName
					());
				this._enclosing.StoreInA();
				Sharpen.Runtime.Out.WriteLine("Replicating " + this._enclosing.A().Provider().GetName
					() + " to " + this._enclosing.B().Provider().GetName());
				this._enclosing.Replicate(this._enclosing.A().Provider(), this._enclosing.B().Provider
					());
				Sharpen.Runtime.Out.WriteLine("Modifying in " + this._enclosing.B().Provider().GetName
					());
				this._enclosing.ModifyInB();
				Sharpen.Runtime.Out.WriteLine("Replicating " + this._enclosing.B().Provider().GetName
					() + " to " + this._enclosing.A().Provider().GetName());
				this._enclosing.Replicate(this._enclosing.B().Provider(), this._enclosing.A().Provider
					());
			}

			private readonly PerformanceTestCase _enclosing;
		}

		private void StoreInA()
		{
			long duration = StopWatch.Time(new _IBlock4_40(this));
			Sharpen.Runtime.Out.WriteLine("Time to store " + TotalObjectCount + " objects: " 
				+ duration + "ms");
		}

		private sealed class _IBlock4_40 : IBlock4
		{
			public _IBlock4_40(PerformanceTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				for (int i = 0; i < PerformanceTestCase.ListHolderCount; i++)
				{
					SimpleListHolder listHolder = new SimpleListHolder("CreatedHolder");
					for (int j = 0; j < PerformanceTestCase.ObjectCount; j++)
					{
						SimpleItem child = new SimpleItem("CreatedChild");
						SimpleItem parent = new SimpleItem(listHolder, child, "CreatedParent");
						this._enclosing.A().Provider().StoreNew(parent);
						listHolder.Add(parent);
					}
					this._enclosing.A().Provider().StoreNew(listHolder);
					this._enclosing.A().Provider().Commit();
				}
				this._enclosing.A().Provider().Commit();
			}

			private readonly PerformanceTestCase _enclosing;
		}

		private void Replicate(ITestableReplicationProviderInside from, ITestableReplicationProviderInside
			 to)
		{
			long duration = StopWatch.Time(new _IBlock4_60(this, from, to));
			Sharpen.Runtime.Out.WriteLine("Time to replicate " + TotalObjectCount + " objects: "
				 + duration + "ms");
		}

		private sealed class _IBlock4_60 : IBlock4
		{
			public _IBlock4_60(PerformanceTestCase _enclosing, ITestableReplicationProviderInside
				 from, ITestableReplicationProviderInside to)
			{
				this._enclosing = _enclosing;
				this.from = from;
				this.to = to;
			}

			public void Run()
			{
				this._enclosing.ReplicateAll(from, to);
			}

			private readonly PerformanceTestCase _enclosing;

			private readonly ITestableReplicationProviderInside from;

			private readonly ITestableReplicationProviderInside to;
		}

		private void ModifyInB()
		{
			long duration = StopWatch.Time(new _IBlock4_69(this));
			Sharpen.Runtime.Out.WriteLine("Time to update " + TotalObjectCount + " objects: "
				 + duration + "ms");
		}

		private sealed class _IBlock4_69 : IBlock4
		{
			public _IBlock4_69(PerformanceTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				IObjectSet storedObjects = this._enclosing.B().Provider().GetStoredObjects(typeof(
					SimpleListHolder));
				while (storedObjects.HasNext())
				{
					SimpleListHolder listHolder = (SimpleListHolder)storedObjects.Next();
					listHolder.SetName("modifiedHolder");
					IEnumerator i = listHolder.GetList().GetEnumerator();
					while (i.MoveNext())
					{
						SimpleItem parent = (SimpleItem)i.Current;
						parent.SetValue("ModifiedParent");
						this._enclosing.B().Provider().Update(parent);
						SimpleItem child = parent.GetChild();
						child.SetValue("ModifiedChild");
						this._enclosing.B().Provider().Update(child);
						this._enclosing.B().Provider().Commit();
					}
					this._enclosing.B().Provider().Update(listHolder);
					this._enclosing.B().Provider().Update(listHolder.GetList());
				}
			}

			private readonly PerformanceTestCase _enclosing;
		}
	}
}
