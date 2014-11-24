/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Foundation
{
	public interface IDrsUUID
	{
		long GetLongPart();

		byte[] GetSignaturePart();
	}
}
