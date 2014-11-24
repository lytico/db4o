/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs;

namespace Db4objects.Drs.Inside
{
	/// <summary>A default implementation of ConflictResolver.</summary>
	/// <remarks>
	/// A default implementation of ConflictResolver. In case of a conflict,
	/// if the object is known to only one database the object is copied
	/// to the other database. If the object is known in both databases
	/// a
	/// <see cref="Db4objects.Drs.ReplicationConflictException">Db4objects.Drs.ReplicationConflictException
	/// 	</see>
	/// is thrown.
	/// </remarks>
	/// <version>1.1</version>
	/// <since>dRS 1.0</since>
	public class DefaultReplicationEventListener : IReplicationEventListener
	{
		public virtual void OnReplicate(IReplicationEvent e)
		{
			if (e.IsConflict())
			{
				if (!e.StateInProviderA().IsKnown())
				{
					e.OverrideWith(e.StateInProviderB());
				}
				else
				{
					if (!e.StateInProviderB().IsKnown())
					{
						e.OverrideWith(e.StateInProviderA());
					}
				}
			}
		}
	}
}
