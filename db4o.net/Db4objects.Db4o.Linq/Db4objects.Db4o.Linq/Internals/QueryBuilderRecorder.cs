/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq.Internals
{
	internal interface IQueryBuilderRecord
	{
		void Playback(IQuery query);
		void Playback(QueryBuilderContext context);
	}

	internal class NullQueryBuilderRecord : IQueryBuilderRecord
	{
		public static readonly NullQueryBuilderRecord Instance = new NullQueryBuilderRecord();

		private NullQueryBuilderRecord()
		{
		}

		public void Playback(IQuery query)
		{
		}

		public void Playback(QueryBuilderContext context)
		{
		}
	}

	internal abstract class QueryBuilderRecordImpl : IQueryBuilderRecord
	{
		public void Playback(IQuery query)
		{
			Playback(new QueryBuilderContext(query));
		}

		public abstract void Playback(QueryBuilderContext context);
	}

	internal class CompositeQueryBuilderRecord : QueryBuilderRecordImpl
	{
		private readonly IQueryBuilderRecord _first;
		private readonly IQueryBuilderRecord _second;

		public CompositeQueryBuilderRecord(IQueryBuilderRecord first, IQueryBuilderRecord second)
		{
			_first = first;
			_second = second;
		}

		override public void Playback(QueryBuilderContext context)
		{
			context.SaveQuery();
			_first.Playback(context);
			context.RestoreQuery();

			_second.Playback(context);
		}
	}

	internal class ChainedQueryBuilderRecord : QueryBuilderRecordImpl
	{
		private readonly Action<QueryBuilderContext> _action;
		private readonly IQueryBuilderRecord _next;

		public ChainedQueryBuilderRecord(IQueryBuilderRecord next, Action<QueryBuilderContext> action)
		{
			_next = next;
			_action = action;
		}

		override public void Playback(QueryBuilderContext context)
		{
			_next.Playback(context);
			_action(context);
		}
	}

	internal class QueryBuilderRecorder
	{
		private IQueryBuilderRecord _last = NullQueryBuilderRecord.Instance;

		public QueryBuilderRecorder()
		{
		}

		public QueryBuilderRecorder(IQueryBuilderRecord record)
		{
			_last = record;
		}

		public IQueryBuilderRecord Record
		{
			get { return _last; }
		}

		public void Add(Action<QueryBuilderContext> action)
		{
			_last = new ChainedQueryBuilderRecord(_last, action);
		}
	}
}
