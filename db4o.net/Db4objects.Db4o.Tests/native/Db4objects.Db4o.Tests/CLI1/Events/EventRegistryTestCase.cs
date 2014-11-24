/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;
using Db4objects.Db4o.Events;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Events
{
	public class EventRegistryTestCase : AbstractDb4oTestCase
	{
		public class Item
		{	
		}
		
		class EventRecorder
		{
			readonly List<string> _events = new List<string>();

			public EventRecorder(IObjectContainer container)
			{
				IEventRegistry registry = EventRegistryFactory.ForObjectContainer(container);
				registry.Creating += OnCreating;
			}
			
			public string this[int index]
			{
				get { return _events[index];  }
			}

			void OnCreating(object sender, CancellableObjectEventArgs args)
			{
				_events.Add("Creating");
				Assert.IsFalse(args.IsCancelled);
				args.Cancel();
			}
		}
		
		public void TestCreating()
		{
			EventRecorder recorder = new EventRecorder(Db());

			Store(new Item());

			Assert.AreEqual(0, Db().QueryByExample(typeof(Item)).Count);
			Assert.AreEqual("Creating", recorder[0]);
		}
	}
}
