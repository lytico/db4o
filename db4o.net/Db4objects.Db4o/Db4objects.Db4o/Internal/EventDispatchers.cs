/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public sealed class EventDispatchers
	{
		private sealed class _IEventDispatcher_11 : IEventDispatcher
		{
			public _IEventDispatcher_11()
			{
			}

			public bool Dispatch(Transaction trans, object obj, int eventID)
			{
				return true;
			}

			public bool HasEventRegistered(int eventID)
			{
				return false;
			}
		}

		public static readonly IEventDispatcher NullDispatcher = new _IEventDispatcher_11
			();

		private static readonly string[] events = new string[] { "objectCanDelete", "objectOnDelete"
			, "objectOnActivate", "objectOnDeactivate", "objectOnNew", "objectOnUpdate", "objectCanActivate"
			, "objectCanDeactivate", "objectCanNew", "objectCanUpdate" };

		internal const int CanDelete = 0;

		internal const int Delete = 1;

		internal const int Activate = 2;

		internal const int Deactivate = 3;

		internal const int New = 4;

		public const int Update = 5;

		internal const int CanActivate = 6;

		internal const int CanDeactivate = 7;

		internal const int CanNew = 8;

		internal const int CanUpdate = 9;

		internal const int DeleteCount = 2;

		internal const int Count = 10;

		private class EventDispatcherImpl : IEventDispatcher
		{
			private readonly IReflectMethod[] methods;

			public EventDispatcherImpl(IReflectMethod[] methods_)
			{
				methods = methods_;
			}

			public virtual bool HasEventRegistered(int eventID)
			{
				return methods[eventID] != null;
			}

			public virtual bool Dispatch(Transaction trans, object obj, int eventID)
			{
				if (methods[eventID] == null)
				{
					return true;
				}
				object[] parameters = new object[] { trans.ObjectContainer() };
				ObjectContainerBase container = trans.Container();
				int stackDepth = container.StackDepth();
				int topLevelCallId = container.TopLevelCallId();
				container.StackDepth(0);
				try
				{
					object res = methods[eventID].Invoke(obj, parameters);
					if (res is bool)
					{
						return ((bool)res);
					}
				}
				finally
				{
					container.StackDepth(stackDepth);
					container.TopLevelCallId(topLevelCallId);
				}
				return true;
			}
		}

		public static IEventDispatcher ForClass(ObjectContainerBase container, IReflectClass
			 classReflector)
		{
			if (container == null || classReflector == null)
			{
				throw new ArgumentNullException();
			}
			if (!container.DispatchsEvents())
			{
				return NullDispatcher;
			}
			int count = EventCountFor(container);
			if (count == 0)
			{
				return NullDispatcher;
			}
			IReflectMethod[] handlers = EventHandlerTableFor(container, classReflector);
			return HasEventHandler(handlers) ? new EventDispatchers.EventDispatcherImpl(handlers
				) : NullDispatcher;
		}

		private static IReflectMethod[] EventHandlerTableFor(ObjectContainerBase container
			, IReflectClass classReflector)
		{
			IReflectClass[] parameterClasses = new IReflectClass[] { container._handlers.IclassObjectcontainer
				 };
			IReflectMethod[] methods = new IReflectMethod[Count];
			for (int i = Count - 1; i >= 0; i--)
			{
				IReflectMethod method = classReflector.GetMethod(events[i], parameterClasses);
				if (null == method)
				{
					method = classReflector.GetMethod(ToPascalCase(events[i]), parameterClasses);
				}
				if (method != null)
				{
					methods[i] = method;
				}
			}
			return methods;
		}

		private static bool HasEventHandler(IReflectMethod[] methods)
		{
			return Iterators.Any(Iterators.Iterate(methods), new _IPredicate4_118());
		}

		private sealed class _IPredicate4_118 : IPredicate4
		{
			public _IPredicate4_118()
			{
			}

			public bool Match(object candidate)
			{
				return candidate != null;
			}
		}

		private static int EventCountFor(ObjectContainerBase container)
		{
			CallBackMode callbackMode = container.ConfigImpl.CallbackMode();
			if (callbackMode == CallBackMode.All)
			{
				return Count;
			}
			if (callbackMode == CallBackMode.DeleteOnly)
			{
				return DeleteCount;
			}
			return 0;
		}

		private static string ToPascalCase(string name)
		{
			return Sharpen.Runtime.Substring(name, 0, 1).ToUpper() + Sharpen.Runtime.Substring
				(name, 1);
		}
	}
}
