/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.TA
{
	/// <exclude></exclude>
	public class LinkedList
	{
		public static Db4objects.Db4o.Tests.Common.TA.LinkedList NewList(int depth)
		{
			if (depth == 0)
			{
				return null;
			}
			Db4objects.Db4o.Tests.Common.TA.LinkedList head = new Db4objects.Db4o.Tests.Common.TA.LinkedList
				(depth);
			head.next = NewList(depth - 1);
			return head;
		}

		public Db4objects.Db4o.Tests.Common.TA.LinkedList next;

		public int value;

		public LinkedList(int v)
		{
			value = v;
		}

		public virtual Db4objects.Db4o.Tests.Common.TA.LinkedList NextN(int depth)
		{
			Db4objects.Db4o.Tests.Common.TA.LinkedList node = this;
			for (int i = 0; i < depth; ++i)
			{
				node = node.next;
			}
			return node;
		}

		public override bool Equals(object other)
		{
			Db4objects.Db4o.Tests.Common.TA.LinkedList otherList = (Db4objects.Db4o.Tests.Common.TA.LinkedList
				)other;
			if (value != otherList.value)
			{
				return false;
			}
			if (next == otherList.next)
			{
				return true;
			}
			if (otherList.next == null)
			{
				return false;
			}
			return next.Equals(otherList.next);
		}
	}
}
