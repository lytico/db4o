/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class ObjectHeaderAttributes
	{
		private readonly int _fieldCount;

		private readonly BitMap4 _nullBitMap;

		public ObjectHeaderAttributes(ByteArrayBuffer reader)
		{
			_fieldCount = reader.ReadInt();
			_nullBitMap = reader.ReadBitMap(_fieldCount);
		}

		public virtual bool IsNull(int fieldIndex)
		{
			return _nullBitMap.IsTrue(fieldIndex);
		}
	}
}
