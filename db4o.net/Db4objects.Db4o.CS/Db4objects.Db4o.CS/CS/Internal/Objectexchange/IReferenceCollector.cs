/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public interface IReferenceCollector
	{
		IEnumerator ReferencesFrom(int id);
	}
}
