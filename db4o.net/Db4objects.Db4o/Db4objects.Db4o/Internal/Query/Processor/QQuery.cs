/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>QQuery is the users hook on our graph.</summary>
	/// <remarks>
	/// QQuery is the users hook on our graph.
	/// A QQuery is defined by it's constraints.
	/// </remarks>
	/// <exclude></exclude>
	public class QQuery : QQueryBase, IQuery
	{
		public QQuery()
		{
		}

		public QQuery(Transaction a_trans, Db4objects.Db4o.Internal.Query.Processor.QQuery
			 a_parent, string a_field) : base(a_trans, a_parent, a_field)
		{
		}
		// C/S only
	}
}
