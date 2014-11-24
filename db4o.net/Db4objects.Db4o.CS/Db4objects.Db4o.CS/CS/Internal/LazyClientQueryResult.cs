/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public class LazyClientQueryResult : AbstractQueryResult
	{
		private const int SizeNotSet = -1;

		private readonly ClientObjectContainer _client;

		private readonly int _queryResultID;

		private int _size = SizeNotSet;

		private readonly LazyClientIdIterator _iterator;

		public LazyClientQueryResult(Transaction trans, ClientObjectContainer client, int
			 queryResultID) : base(trans)
		{
			_client = client;
			_queryResultID = queryResultID;
			_iterator = new LazyClientIdIterator(this);
		}

		public override object Get(int index)
		{
			lock (Lock())
			{
				return ActivatedObject(GetId(index));
			}
		}

		public override int GetId(int index)
		{
			return AskServer(Msg.ObjectsetGetId, index);
		}

		public override int IndexOf(int id)
		{
			return AskServer(Msg.ObjectsetIndexof, id);
		}

		private int AskServer(MsgD message, int param)
		{
			_client.Write(message.GetWriterForInts(_transaction, new int[] { _queryResultID, 
				param }));
			return ((MsgD)_client.ExpectedResponse(message)).ReadInt();
		}

		public override IIntIterator4 IterateIDs()
		{
			return _iterator;
		}

		public override IEnumerator GetEnumerator()
		{
			return Skip(ClientServerPlatform.CreateClientQueryResultIterator(this));
		}

		public override int Size()
		{
			if (_size == SizeNotSet)
			{
				_client.Write(Msg.ObjectsetSize.GetWriterForInt(_transaction, _queryResultID));
				_size = ((MsgD)_client.ExpectedResponse(Msg.ObjectsetSize)).ReadInt();
			}
			return _size;
		}

		~LazyClientQueryResult()
		{
			_client.Write(Msg.ObjectsetFinalized.GetWriterForInt(_transaction, _queryResultID
				));
		}

		public override void LoadFromIdReader(IEnumerator reader)
		{
			_iterator.LoadFromIdReader(reader);
		}

		public virtual void Reset()
		{
			_client.Write(Msg.ObjectsetReset.GetWriterForInt(_transaction, _queryResultID));
		}

		public virtual void FetchIDs(int batchSize)
		{
			_client.Write(Msg.ObjectsetFetch.GetWriterForInts(_transaction, new int[] { _queryResultID
				, batchSize, _client.PrefetchDepth() }));
			ByteArrayBuffer reader = _client.ExpectedBufferResponse(Msg.IdList);
			LoadFromIdReader(_client.IdIteratorFor(_transaction, reader));
		}
	}
}
