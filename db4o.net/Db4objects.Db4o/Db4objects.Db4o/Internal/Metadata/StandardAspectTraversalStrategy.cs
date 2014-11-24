/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Metadata;

namespace Db4objects.Db4o.Internal.Metadata
{
	/// <exclude></exclude>
	public class StandardAspectTraversalStrategy : IAspectTraversalStrategy
	{
		private readonly ClassMetadata _classMetadata;

		public StandardAspectTraversalStrategy(ClassMetadata classMetadata)
		{
			_classMetadata = classMetadata;
		}

		public virtual void TraverseAllAspects(ITraverseAspectCommand command)
		{
			ClassMetadata classMetadata = _classMetadata;
			int currentSlot = 0;
			while (classMetadata != null)
			{
				int aspectCount = command.DeclaredAspectCount(classMetadata);
				for (int i = 0; i < aspectCount && !command.Cancelled(); i++)
				{
					command.ProcessAspect(classMetadata._aspects[i], currentSlot);
					currentSlot++;
				}
				if (command.Cancelled())
				{
					return;
				}
				classMetadata = classMetadata._ancestor;
			}
		}
	}
}
