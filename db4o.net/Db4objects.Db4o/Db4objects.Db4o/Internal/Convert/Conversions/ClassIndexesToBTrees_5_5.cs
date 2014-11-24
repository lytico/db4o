/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Convert;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	/// <exclude></exclude>
	public class ClassIndexesToBTrees_5_5 : Conversion
	{
		public const int Version = 5;

		public virtual void Convert(LocalObjectContainer container, int classIndexId, BTree
			 bTree)
		{
			Transaction trans = container.SystemTransaction();
			ByteArrayBuffer reader = container.ReadBufferById(trans, classIndexId);
			if (reader == null)
			{
				return;
			}
			int entries = reader.ReadInt();
			for (int i = 0; i < entries; i++)
			{
				bTree.Add(trans, reader.ReadInt());
			}
		}

		public override void Convert(ConversionStage.SystemUpStage stage)
		{
			// calling #storedClasses forces reading all classes
			// That's good enough to load them all and to call the
			// above convert method.
			stage.File().StoredClasses();
		}
	}
}
