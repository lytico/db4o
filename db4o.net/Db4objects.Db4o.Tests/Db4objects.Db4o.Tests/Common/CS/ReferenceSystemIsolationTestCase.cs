/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ReferenceSystemIsolationTestCase : EmbeddedAndNetworkingClientTestCaseBase
	{
		[System.Serializable]
		public sealed class IncludeAllEvaluation : IEvaluation
		{
			public void Evaluate(ICandidate candidate)
			{
				candidate.Include(true);
			}
		}

		public class Item
		{
		}

		public virtual void Test()
		{
			ReferenceSystemIsolationTestCase.Item item = new ReferenceSystemIsolationTestCase.Item
				();
			NetworkingClient().Store(item);
			int id = (int)NetworkingClient().GetID(item);
			IQuery query = NetworkingClient().Query();
			query.Constrain(typeof(ReferenceSystemIsolationTestCase.Item));
			query.Constrain(new ReferenceSystemIsolationTestCase.IncludeAllEvaluation());
			query.Execute();
			Assert.IsNull(EmbeddedClient().Transaction.ReferenceForId(id));
		}
	}
}
#endif // !SILVERLIGHT
