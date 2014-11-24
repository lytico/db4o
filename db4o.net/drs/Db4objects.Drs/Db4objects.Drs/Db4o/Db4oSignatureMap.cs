/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Drs.Db4o
{
	internal class Db4oSignatureMap
	{
		private readonly IInternalObjectContainer _stream;

		private readonly Hashtable4 _identities;

		internal Db4oSignatureMap(IInternalObjectContainer stream)
		{
			_stream = stream;
			_identities = new Hashtable4();
		}

		internal virtual Db4oDatabase Produce(byte[] signature, long creationTime)
		{
			Db4oDatabase db = (Db4oDatabase)_identities.Get(signature);
			if (db != null)
			{
				return db;
			}
			db = new Db4oDatabase(signature, creationTime);
			db.Bind(_stream.Transaction);
			_identities.Put(signature, db);
			return db;
		}

		public virtual void Put(Db4oDatabase db)
		{
			Db4oDatabase existing = (Db4oDatabase)_identities.Get(db.GetSignature());
			if (existing == null)
			{
				_identities.Put(db.GetSignature(), db);
			}
		}
	}
}
