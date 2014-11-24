/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public class FileHeaderVariablePart3 : FileHeaderVariablePart2
	{
		public FileHeaderVariablePart3(LocalObjectContainer container) : base(container)
		{
		}

		public override int OwnLength()
		{
			return base.OwnLength() + Const4.IntLength * 2;
		}

		protected override void ReadBuffer(ByteArrayBuffer buffer, bool versionsAreConsistent
			)
		{
			base.ReadBuffer(buffer, versionsAreConsistent);
			SystemData systemData = SystemData();
			systemData.IdToTimestampIndexId(buffer.ReadInt());
			systemData.TimestampToIdIndexId(buffer.ReadInt());
		}

		protected override void WriteBuffer(ByteArrayBuffer buffer, bool shuttingDown)
		{
			base.WriteBuffer(buffer, shuttingDown);
			SystemData systemData = SystemData();
			buffer.WriteInt(systemData.IdToTimestampIndexId());
			buffer.WriteInt(systemData.TimestampToIdIndexId());
		}
	}
}
