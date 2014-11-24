/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public interface IQueue4
	{
		void Add(object obj);

		object Next();

		bool HasNext();

		/// <summary>Returns the next object in the queue that matches the specified condition.
		/// 	</summary>
		/// <remarks>
		/// Returns the next object in the queue that matches the specified condition.
		/// The operation is always NON-BLOCKING.
		/// </remarks>
		/// <param name="condition">the object must satisfy to be returned</param>
		/// <returns>the object satisfying the condition or null if none does</returns>
		object NextMatching(IPredicate4 condition);

		IEnumerator Iterator();
	}
}
