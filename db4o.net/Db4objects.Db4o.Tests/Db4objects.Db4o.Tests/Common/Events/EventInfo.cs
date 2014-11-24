/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Events
{
	internal class EventInfo
	{
		public EventInfo(string eventFirerName, IProcedure4 eventListenerSetter) : this(eventFirerName
			, true, eventListenerSetter)
		{
		}

		public EventInfo(string eventFirerName, bool isClientServerEvent, IProcedure4 eventListenerSetter
			)
		{
			_listenerSetter = eventListenerSetter;
			_eventFirerName = eventFirerName;
			_isClientServerEvent = isClientServerEvent;
		}

		public virtual IProcedure4 ListenerSetter()
		{
			return _listenerSetter;
		}

		public virtual string EventFirerName()
		{
			return _eventFirerName;
		}

		public virtual bool IsClientServerEvent()
		{
			return _isClientServerEvent;
		}

		private readonly IProcedure4 _listenerSetter;

		private readonly string _eventFirerName;

		private readonly bool _isClientServerEvent;
	}
}
