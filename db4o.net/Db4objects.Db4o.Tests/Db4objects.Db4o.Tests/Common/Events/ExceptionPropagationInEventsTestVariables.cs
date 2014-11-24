/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class ExceptionPropagationInEventsTestVariables
	{
		internal static readonly FixtureVariable EventSelector = new FixtureVariable("event"
			);

		private sealed class _IProcedure4_14 : IProcedure4
		{
			public _IProcedure4_14()
			{
			}

			// 0
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Activated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
					(new _IEventListener4_16().OnEvent);
			}

			private sealed class _IEventListener4_16
			{
				public _IEventListener4_16()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
					)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_23 : IProcedure4
		{
			public _IProcedure4_23()
			{
			}

			// 1
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Activating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					(new _IEventListener4_25().OnEvent);
			}

			private sealed class _IEventListener4_25
			{
				public _IEventListener4_25()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
					 args)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_32 : IProcedure4
		{
			public _IProcedure4_32()
			{
			}

			// 2
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Deleted += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
					(new _IEventListener4_34().OnEvent);
			}

			private sealed class _IEventListener4_34
			{
				public _IEventListener4_34()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
					)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_42 : IProcedure4
		{
			public _IProcedure4_42()
			{
			}

			// 3
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Deleting += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					(new _IEventListener4_44().OnEvent);
			}

			private sealed class _IEventListener4_44
			{
				public _IEventListener4_44()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
					 args)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_51 : IProcedure4
		{
			public _IProcedure4_51()
			{
			}

			// 4
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Committing += new System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>
					(new _IEventListener4_53().OnEvent);
			}

			private sealed class _IEventListener4_53
			{
				public _IEventListener4_53()
				{
					this._firstTime = true;
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.CommitEventArgs args)
				{
					if (this._firstTime)
					{
						this._firstTime = false;
						throw new NotImplementedException();
					}
				}

				private bool _firstTime;
			}
		}

		private sealed class _IProcedure4_65 : IProcedure4
		{
			public _IProcedure4_65()
			{
			}

			// 5
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Committed += new System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>
					(new _IEventListener4_67().OnEvent);
			}

			private sealed class _IEventListener4_67
			{
				public _IEventListener4_67()
				{
					this._firstTime = true;
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.CommitEventArgs args)
				{
					if (this._firstTime)
					{
						this._firstTime = false;
						throw new NotImplementedException();
					}
				}

				private bool _firstTime;
			}
		}

		private sealed class _IProcedure4_79 : IProcedure4
		{
			public _IProcedure4_79()
			{
			}

			// 6
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Creating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					(new _IEventListener4_81().OnEvent);
			}

			private sealed class _IEventListener4_81
			{
				public _IEventListener4_81()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
					 args)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_89 : IProcedure4
		{
			public _IProcedure4_89()
			{
			}

			// 7
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Created += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
					(new _IEventListener4_91().OnEvent);
			}

			private sealed class _IEventListener4_91
			{
				public _IEventListener4_91()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
					)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_98 : IProcedure4
		{
			public _IProcedure4_98()
			{
			}

			// 8
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Instantiated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
					(new _IEventListener4_100().OnEvent);
			}

			private sealed class _IEventListener4_100
			{
				public _IEventListener4_100()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
					)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_107 : IProcedure4
		{
			public _IProcedure4_107()
			{
			}

			// 9
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Updating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					(new _IEventListener4_109().OnEvent);
			}

			private sealed class _IEventListener4_109
			{
				public _IEventListener4_109()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
					 args)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_116 : IProcedure4
		{
			public _IProcedure4_116()
			{
			}

			// 10
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).Updated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
					(new _IEventListener4_118().OnEvent);
			}

			private sealed class _IEventListener4_118
			{
				public _IEventListener4_118()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
					)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_126 : IProcedure4
		{
			public _IProcedure4_126()
			{
			}

			// 11
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).QueryStarted += new System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>
					(new _IEventListener4_128().OnEvent);
			}

			private sealed class _IEventListener4_128
			{
				public _IEventListener4_128()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.QueryEventArgs args)
				{
					throw new NotImplementedException();
				}
			}
		}

		private sealed class _IProcedure4_135 : IProcedure4
		{
			public _IProcedure4_135()
			{
			}

			// 12
			public void Apply(object eventRegistry)
			{
				((IEventRegistry)eventRegistry).QueryFinished += new System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>
					(new _IEventListener4_137().OnEvent);
			}

			private sealed class _IEventListener4_137
			{
				public _IEventListener4_137()
				{
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.QueryEventArgs args)
				{
					throw new NotImplementedException();
				}
			}
		}

		internal static readonly IFixtureProvider EventProvider = new SimpleFixtureProvider
			(EventSelector, new object[] { new EventInfo("query", new _IProcedure4_14()), new 
			EventInfo("query", new _IProcedure4_23()), new EventInfo("delete", false, new _IProcedure4_32
			()), new EventInfo("delete", false, new _IProcedure4_42()), new EventInfo("insert"
			, false, new _IProcedure4_51()), new EventInfo("insert", false, new _IProcedure4_65
			()), new EventInfo("insert", new _IProcedure4_79()), new EventInfo("insert", new 
			_IProcedure4_89()), new EventInfo("query", new _IProcedure4_98()), new EventInfo
			("update", new _IProcedure4_107()), new EventInfo("update", new _IProcedure4_116
			()), new EventInfo("query", new _IProcedure4_126()), new EventInfo("query", new 
			_IProcedure4_135()) });
	}
}
