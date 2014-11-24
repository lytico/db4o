/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class CommittedCallbackRefreshTestCase : Db4oClientServerTestCase
	{
		private readonly object _lock = new object();

		private readonly int Count = 1;

		public class Item
		{
			public string _name;

			public CommittedCallbackRefreshTestCase.SubItem _subItem;

			public int _updates;

			public Item(string name, CommittedCallbackRefreshTestCase.SubItem subItem)
			{
				_name = name;
				_subItem = subItem;
			}

			public virtual void Update()
			{
				_updates++;
				_subItem._updates++;
			}

			public virtual void Check()
			{
				Assert.IsNotNull(_name);
				Assert.AreEqual(_name, _subItem._name);
				Assert.AreEqual(_updates, _subItem._updates);
			}
			// System.out.println(_updates);
		}

		public class SubItem
		{
			public string _name;

			public int _updates;

			public SubItem(string name)
			{
				_name = name;
			}
		}

		public static void Main(string[] arguments)
		{
			new CommittedCallbackRefreshTestCase().RunConcurrency();
		}

		protected override void Store()
		{
			for (int i = 0; i < Count; i++)
			{
				string name = "original" + i;
				Store(new CommittedCallbackRefreshTestCase.Item(name, new CommittedCallbackRefreshTestCase.SubItem
					(name)));
			}
		}

		public virtual void Conc(IExtObjectContainer oc, int seq)
		{
			EventRegistry(oc).Committed += new System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>
				(new _IEventListener4_74(oc).OnEvent);
			CommittedCallbackRefreshTestCase.Item[] items = new CommittedCallbackRefreshTestCase.Item
				[Count];
			IObjectSet objectSet = NewQuery(typeof(CommittedCallbackRefreshTestCase.Item)).Execute
				();
			int count = 0;
			while (objectSet.HasNext())
			{
				lock (_lock)
				{
					items[count] = (CommittedCallbackRefreshTestCase.Item)objectSet.Next();
					items[count].Check();
					count++;
				}
			}
			for (int i = 0; i < items.Length; i++)
			{
				lock (_lock)
				{
					items[i].Update();
					Store(items[i]._subItem);
					Store(items[i]);
				}
				Db().Commit();
			}
			Runtime4.Sleep(1000);
			for (int i = 0; i < items.Length; i++)
			{
				lock (_lock)
				{
					items[i].Check();
				}
			}
			Runtime4.Sleep(3000);
		}

		private sealed class _IEventListener4_74
		{
			public _IEventListener4_74(IExtObjectContainer oc)
			{
				this.oc = oc;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CommitEventArgs args)
			{
				if (oc.IsClosed())
				{
					return;
				}
				IObjectInfoCollection updated = ((CommitEventArgs)args).Updated;
				IEnumerator infos = updated.GetEnumerator();
				while (infos.MoveNext())
				{
					IObjectInfo info = (IObjectInfo)infos.Current;
					object obj = info.GetObject();
					oc.Refresh(obj, 2);
				}
			}

			private readonly IExtObjectContainer oc;
		}

		private IEventRegistry EventRegistry(IExtObjectContainer oc)
		{
			return EventRegistryFactory.ForObjectContainer(oc);
		}
	}
}
#endif // !SILVERLIGHT
