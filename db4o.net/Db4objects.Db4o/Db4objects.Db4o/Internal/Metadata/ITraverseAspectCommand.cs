/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Metadata
{
	/// <exclude></exclude>
	public interface ITraverseAspectCommand
	{
		int DeclaredAspectCount(ClassMetadata classMetadata);

		bool Cancelled();

		void ProcessAspectOnMissingClass(ClassAspect aspect, int currentSlot);

		void ProcessAspect(ClassAspect aspect, int currentSlot);
	}
}
