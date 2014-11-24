/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

import java.io.*;

import com.db4o.cs.internal.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class MCommittedInfo extends MsgD implements ClientSideMessage {

	public MCommittedInfo encode(CallbackObjectInfoCollections callbackInfo, int dispatcherID) {
		ByteArrayOutputStream os = new ByteArrayOutputStream();
		PrimitiveCodec.writeInt(os, dispatcherID);
		byte[] bytes = encodeInfo(callbackInfo, os);
		MCommittedInfo committedInfo = (MCommittedInfo) getWriterForLength(transaction(), bytes.length + Const4.INT_LENGTH);
		committedInfo._payLoad.append(bytes);
		return committedInfo;
	}

	private byte[] encodeInfo(CallbackObjectInfoCollections callbackInfo, ByteArrayOutputStream os) {
		encodeObjectInfoCollection(os, callbackInfo.added, new InternalIDEncoder());
		encodeObjectInfoCollection(os, callbackInfo.deleted, new FrozenObjectInfoEncoder());
		encodeObjectInfoCollection(os, callbackInfo.updated, new InternalIDEncoder());
		
		return os.toByteArray();
	}
	
	private final class FrozenObjectInfoEncoder implements ObjectInfoEncoder {
		public void encode(ByteArrayOutputStream os, ObjectInfo info) {
			PrimitiveCodec.writeLong(os, info.getInternalID());
	        long sourceDatabaseId = ((FrozenObjectInfo)info).sourceDatabaseId(transaction());
	        PrimitiveCodec.writeLong(os, sourceDatabaseId);
	        PrimitiveCodec.writeLong(os, ((FrozenObjectInfo)info).uuidLongPart());
	        PrimitiveCodec.writeLong(os, info.getCommitTimestamp());
		}

		public ObjectInfo decode(ByteArrayInputStream is) {
			long id = PrimitiveCodec.readLong(is);
			if (id == -1) {
				return null;
			}
			long sourceDatabaseId = PrimitiveCodec.readLong(is);
			Db4oDatabase sourceDatabase = null;
			if (sourceDatabaseId > 0 ){
			    sourceDatabase  = (Db4oDatabase) container().getByID(transaction(), sourceDatabaseId);
			}
			long uuidLongPart = PrimitiveCodec.readLong(is);
			long version = PrimitiveCodec.readLong(is);
			return new FrozenObjectInfo(null, id, sourceDatabase, uuidLongPart, version);
		}
	}

	private final class InternalIDEncoder implements ObjectInfoEncoder {
		public void encode(ByteArrayOutputStream os, ObjectInfo info) {
			PrimitiveCodec.writeLong(os, info.getInternalID());
		}

		public ObjectInfo decode(ByteArrayInputStream is) {
			long id = PrimitiveCodec.readLong(is);
			if (id == -1) {
				return null;
			}
			return new LazyObjectReference(transaction(), (int) id);
		}
	}

	interface ObjectInfoEncoder {
		void encode(ByteArrayOutputStream os, ObjectInfo info);
		ObjectInfo decode(ByteArrayInputStream is);
	}
	
	private void encodeObjectInfoCollection(ByteArrayOutputStream os,
			ObjectInfoCollection collection, final ObjectInfoEncoder encoder) {
		Iterator4 iter = collection.iterator();
		while (iter.moveNext()) {
			ObjectInfo obj = (ObjectInfo) iter.current();
			encoder.encode(os, obj);
		}
		PrimitiveCodec.writeLong(os, -1);
	}
	
	public CallbackObjectInfoCollections decode(ByteArrayInputStream is) {		
		final ObjectInfoCollection added = decodeObjectInfoCollection(is, new InternalIDEncoder());
		final ObjectInfoCollection deleted = decodeObjectInfoCollection(is, new FrozenObjectInfoEncoder());
		final ObjectInfoCollection updated = decodeObjectInfoCollection(is, new InternalIDEncoder());
		return new CallbackObjectInfoCollections(added, updated, deleted);
	}

	private ObjectInfoCollection decodeObjectInfoCollection(ByteArrayInputStream is, ObjectInfoEncoder encoder){
		final Collection4 collection = new Collection4();
		while (true) {
			ObjectInfo info = encoder.decode(is);
			if (null == info) {
				break;
			}
			collection.add(info);
		}
		return new ObjectInfoCollectionImpl(collection);
	}

	public boolean processAtClient() {
		ByteArrayInputStream is = new ByteArrayInputStream(_payLoad._buffer);
		final int dispatcherID = PrimitiveCodec.readInt(is);
		final CallbackObjectInfoCollections callbackInfos = decode(is);
		container().threadPool().start(ReflectPlatform.simpleName(getClass())+": calling commit callbacks thread", new Runnable() {
			public void run() {
				if(container().isClosed()){
					return;
				}
				container().callbacks().commitOnCompleted(transaction(), callbackInfos, dispatcherID == ((ClientObjectContainer)container()).serverSideID());
			}
		});
		return true;
	}
	
	protected void writeByteArray(ByteArrayOutputStream os, byte[] signaturePart) throws IOException {
		PrimitiveCodec.writeLong(os, signaturePart.length);
		os.write(signaturePart);
	}
}
