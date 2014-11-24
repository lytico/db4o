/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class QueryingReadContext : AbstractReadContext, IHandlerVersionContext, IAspectVersionContext
		, IObjectIdContext
	{
		private readonly QCandidates _candidates;

		private readonly int _collectionID;

		private readonly int _handlerVersion;

		private IdObjectCollector _collector;

		private int _declaredAspectCount;

		private int _id;

		private QueryingReadContext(Transaction transaction, QCandidates candidates, int 
			handlerVersion, IReadBuffer buffer, int collectionID, IdObjectCollector collector
			) : base(transaction, buffer)
		{
			_candidates = candidates;
			_activationDepth = new LegacyActivationDepth(0);
			_collectionID = collectionID;
			_handlerVersion = handlerVersion;
			_collector = collector;
		}

		public QueryingReadContext(Transaction transaction, QCandidates candidates, int handlerVersion
			, IReadBuffer buffer, int collectionID) : this(transaction, candidates, handlerVersion
			, buffer, collectionID, new IdObjectCollector())
		{
		}

		public QueryingReadContext(Transaction transaction, int handlerVersion, IReadBuffer
			 buffer, int id) : this(transaction, null, handlerVersion, buffer, 0)
		{
			_id = id;
		}

		public QueryingReadContext(Transaction transaction, int handlerVersion, IReadBuffer
			 buffer, int collectionID, IdObjectCollector collector) : this(transaction, null
			, handlerVersion, buffer, collectionID, collector)
		{
		}

		public virtual int CollectionID()
		{
			return _collectionID;
		}

		public virtual QCandidates Candidates()
		{
			return _candidates;
		}

		public override int HandlerVersion()
		{
			return _handlerVersion;
		}

		public virtual void AddId(int id)
		{
			_collector.AddId(id);
		}

		public virtual TreeInt Ids()
		{
			return _collector.Ids();
		}

		public virtual void Add(object obj)
		{
			int id = GetID(obj);
			if (id > 0)
			{
				AddId(id);
				return;
			}
			AddObjectWithoutId(obj);
		}

		private int GetID(object obj)
		{
			return Container().GetID(Transaction(), obj);
		}

		public virtual void ReadId(ITypeHandler4 handler)
		{
			ObjectID objectID = ObjectID.NotPossible;
			try
			{
				int offset = Offset();
				if (handler is IReadsObjectIds)
				{
					objectID = ((IReadsObjectIds)handler).ReadObjectID(this);
				}
				if (objectID.IsValid())
				{
					AddId(objectID._id);
					return;
				}
				if (objectID == ObjectID.NotPossible)
				{
					Seek(offset);
					// FIXME: there's no point in activating the object
					// just find its id
					// type handlers know how to do it
					object obj = Read(handler);
					if (obj != null)
					{
						int id = (int)GetID(obj);
						if (id > 0)
						{
							AddId(id);
						}
						else
						{
							AddObjectWithoutId(obj);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// FIXME: Catchall
		private void AddObjectWithoutId(object obj)
		{
			_collector.Add(obj);
		}

		public virtual void SkipId(ITypeHandler4 handler)
		{
			if (handler is IReadsObjectIds)
			{
				((IReadsObjectIds)handler).ReadObjectID(this);
				return;
			}
			// TODO: Optimize for just doing a seek here.
			Read(handler);
		}

		public virtual IEnumerator ObjectsWithoutId()
		{
			return _collector.Objects();
		}

		public virtual int DeclaredAspectCount()
		{
			return _declaredAspectCount;
		}

		public virtual void DeclaredAspectCount(int count)
		{
			_declaredAspectCount = count;
		}

		public virtual IdObjectCollector Collector()
		{
			return _collector;
		}

		public virtual int ObjectId()
		{
			return _id;
		}
	}
}
