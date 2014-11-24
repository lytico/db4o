/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public class QEIdentity : QEEqual
	{
		private int i_objectID;

		public override bool Identity()
		{
			return true;
		}

		internal override bool Evaluate(QConObject a_constraint, IInternalCandidate a_candidate
			, object a_value)
		{
			if (i_objectID == 0)
			{
				i_objectID = a_constraint.GetObjectID();
			}
			return a_candidate.Id() == i_objectID;
		}
	}
}
