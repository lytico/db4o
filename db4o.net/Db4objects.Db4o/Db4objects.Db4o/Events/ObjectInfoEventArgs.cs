/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Events
{
	public class ObjectInfoEventArgs : ObjectEventArgs
	{
		private readonly IObjectInfo _info;

		public ObjectInfoEventArgs(Transaction transaction, IObjectInfo info) : base(transaction
			)
		{
			_info = info;
		}

		public override object Object
		{
			get
			{
				return _info.GetObject();
			}
		}

		public virtual IObjectInfo Info
		{
			get
			{
				return _info;
			}
		}
	}
}
