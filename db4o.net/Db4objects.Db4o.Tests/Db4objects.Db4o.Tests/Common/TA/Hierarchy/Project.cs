/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Collections;
using Db4objects.Db4o.Tests.Common.TA.Hierarchy;

namespace Db4objects.Db4o.Tests.Common.TA.Hierarchy
{
	internal class Project : ActivatableImpl
	{
		internal IList _subProjects = new PagedList();

		internal IList _workLog = new PagedList();

		internal string _name;

		public Project(string name)
		{
			_name = name;
		}

		public virtual void LogWorkDone(UnitOfWork work)
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			_workLog.Add(work);
		}

		public virtual long TotalTimeSpent()
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			long total = 0;
			IEnumerator i = _workLog.GetEnumerator();
			while (i.MoveNext())
			{
				UnitOfWork item = (UnitOfWork)i.Current;
				total += item.TimeSpent();
			}
			return total;
		}
	}
}
