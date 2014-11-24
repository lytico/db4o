/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	internal class QEBitmap
	{
		public static Db4objects.Db4o.Internal.Fieldindex.QEBitmap ForQE(QE qe)
		{
			bool[] bitmap = new bool[4];
			qe.IndexBitMap(bitmap);
			return new Db4objects.Db4o.Internal.Fieldindex.QEBitmap(bitmap);
		}

		private QEBitmap(bool[] bitmap)
		{
			_bitmap = bitmap;
		}

		private bool[] _bitmap;

		public virtual bool TakeGreater()
		{
			return _bitmap[QE.Greater];
		}

		public virtual bool TakeEqual()
		{
			return _bitmap[QE.Equal];
		}

		public virtual bool TakeSmaller()
		{
			return _bitmap[QE.Smaller];
		}
	}
}
