/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	public class TANArrayItem : ActivatableImpl
	{
		public int[][] value;

		public object obj;

		public LinkedList[][] lists;

		public object listsObject;

		public TANArrayItem()
		{
		}

		public virtual int[][] Value()
		{
			Activate(ActivationPurpose.Read);
			return value;
		}

		public virtual object Object()
		{
			Activate(ActivationPurpose.Read);
			return obj;
		}

		public virtual LinkedList[][] Lists()
		{
			Activate(ActivationPurpose.Read);
			return lists;
		}

		public virtual object ListsObject()
		{
			Activate(ActivationPurpose.Read);
			return listsObject;
		}
	}
}
