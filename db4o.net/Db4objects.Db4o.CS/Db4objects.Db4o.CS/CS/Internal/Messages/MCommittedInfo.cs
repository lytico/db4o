/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Sharpen.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public class MCommittedInfo : MsgD, IClientSideMessage
	{
		public virtual MCommittedInfo Encode(CallbackObjectInfoCollections callbackInfo, 
			int dispatcherID)
		{
			ByteArrayOutputStream os = new ByteArrayOutputStream();
			PrimitiveCodec.WriteInt(os, dispatcherID);
			byte[] bytes = EncodeInfo(callbackInfo, os);
			MCommittedInfo committedInfo = (MCommittedInfo)GetWriterForLength(Transaction(), 
				bytes.Length + Const4.IntLength);
			committedInfo._payLoad.Append(bytes);
			return committedInfo;
		}

		private byte[] EncodeInfo(CallbackObjectInfoCollections callbackInfo, ByteArrayOutputStream
			 os)
		{
			EncodeObjectInfoCollection(os, callbackInfo.added, new MCommittedInfo.InternalIDEncoder
				(this));
			EncodeObjectInfoCollection(os, callbackInfo.deleted, new MCommittedInfo.FrozenObjectInfoEncoder
				(this));
			EncodeObjectInfoCollection(os, callbackInfo.updated, new MCommittedInfo.InternalIDEncoder
				(this));
			return os.ToByteArray();
		}

		private sealed class FrozenObjectInfoEncoder : MCommittedInfo.IObjectInfoEncoder
		{
			public void Encode(ByteArrayOutputStream os, IObjectInfo info)
			{
				PrimitiveCodec.WriteLong(os, info.GetInternalID());
				long sourceDatabaseId = ((FrozenObjectInfo)info).SourceDatabaseId(this._enclosing
					.Transaction());
				PrimitiveCodec.WriteLong(os, sourceDatabaseId);
				PrimitiveCodec.WriteLong(os, ((FrozenObjectInfo)info).UuidLongPart());
				PrimitiveCodec.WriteLong(os, info.GetCommitTimestamp());
			}

			public IObjectInfo Decode(ByteArrayInputStream @is)
			{
				long id = PrimitiveCodec.ReadLong(@is);
				if (id == -1)
				{
					return null;
				}
				long sourceDatabaseId = PrimitiveCodec.ReadLong(@is);
				Db4oDatabase sourceDatabase = null;
				if (sourceDatabaseId > 0)
				{
					sourceDatabase = (Db4oDatabase)this._enclosing.Container().GetByID(this._enclosing
						.Transaction(), sourceDatabaseId);
				}
				long uuidLongPart = PrimitiveCodec.ReadLong(@is);
				long version = PrimitiveCodec.ReadLong(@is);
				return new FrozenObjectInfo(null, id, sourceDatabase, uuidLongPart, version);
			}

			internal FrozenObjectInfoEncoder(MCommittedInfo _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly MCommittedInfo _enclosing;
		}

		private sealed class InternalIDEncoder : MCommittedInfo.IObjectInfoEncoder
		{
			public void Encode(ByteArrayOutputStream os, IObjectInfo info)
			{
				PrimitiveCodec.WriteLong(os, info.GetInternalID());
			}

			public IObjectInfo Decode(ByteArrayInputStream @is)
			{
				long id = PrimitiveCodec.ReadLong(@is);
				if (id == -1)
				{
					return null;
				}
				return new LazyObjectReference(this._enclosing.Transaction(), (int)id);
			}

			internal InternalIDEncoder(MCommittedInfo _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly MCommittedInfo _enclosing;
		}

		internal interface IObjectInfoEncoder
		{
			void Encode(ByteArrayOutputStream os, IObjectInfo info);

			IObjectInfo Decode(ByteArrayInputStream @is);
		}

		private void EncodeObjectInfoCollection(ByteArrayOutputStream os, IObjectInfoCollection
			 collection, MCommittedInfo.IObjectInfoEncoder encoder)
		{
			IEnumerator iter = collection.GetEnumerator();
			while (iter.MoveNext())
			{
				IObjectInfo obj = (IObjectInfo)iter.Current;
				encoder.Encode(os, obj);
			}
			PrimitiveCodec.WriteLong(os, -1);
		}

		public virtual CallbackObjectInfoCollections Decode(ByteArrayInputStream @is)
		{
			IObjectInfoCollection added = DecodeObjectInfoCollection(@is, new MCommittedInfo.InternalIDEncoder
				(this));
			IObjectInfoCollection deleted = DecodeObjectInfoCollection(@is, new MCommittedInfo.FrozenObjectInfoEncoder
				(this));
			IObjectInfoCollection updated = DecodeObjectInfoCollection(@is, new MCommittedInfo.InternalIDEncoder
				(this));
			return new CallbackObjectInfoCollections(added, updated, deleted);
		}

		private IObjectInfoCollection DecodeObjectInfoCollection(ByteArrayInputStream @is
			, MCommittedInfo.IObjectInfoEncoder encoder)
		{
			Collection4 collection = new Collection4();
			while (true)
			{
				IObjectInfo info = encoder.Decode(@is);
				if (null == info)
				{
					break;
				}
				collection.Add(info);
			}
			return new ObjectInfoCollectionImpl(collection);
		}

		public virtual bool ProcessAtClient()
		{
			ByteArrayInputStream @is = new ByteArrayInputStream(_payLoad._buffer);
			int dispatcherID = PrimitiveCodec.ReadInt(@is);
			CallbackObjectInfoCollections callbackInfos = Decode(@is);
			Container().ThreadPool().Start(ReflectPlatform.SimpleName(GetType()) + ": calling commit callbacks thread"
				, new _IRunnable_111(this, callbackInfos, dispatcherID));
			return true;
		}

		private sealed class _IRunnable_111 : IRunnable
		{
			public _IRunnable_111(MCommittedInfo _enclosing, CallbackObjectInfoCollections callbackInfos
				, int dispatcherID)
			{
				this._enclosing = _enclosing;
				this.callbackInfos = callbackInfos;
				this.dispatcherID = dispatcherID;
			}

			public void Run()
			{
				if (this._enclosing.Container().IsClosed())
				{
					return;
				}
				this._enclosing.Container().Callbacks().CommitOnCompleted(this._enclosing.Transaction
					(), callbackInfos, dispatcherID == ((ClientObjectContainer)this._enclosing.Container
					()).ServerSideID());
			}

			private readonly MCommittedInfo _enclosing;

			private readonly CallbackObjectInfoCollections callbackInfos;

			private readonly int dispatcherID;
		}

		/// <exception cref="System.IO.IOException"></exception>
		protected virtual void WriteByteArray(ByteArrayOutputStream os, byte[] signaturePart
			)
		{
			PrimitiveCodec.WriteLong(os, signaturePart.Length);
			os.Write(signaturePart);
		}
	}
}
