/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public abstract class QEAbstract : QE
	{
		internal override QE Add(QE evaluator)
		{
			QE qe = new QEMulti();
			qe.Add(this);
			qe.Add(evaluator);
			return qe;
		}

		internal override bool IsDefault()
		{
			return false;
		}
	}
}
