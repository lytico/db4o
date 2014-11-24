using System;
using System.Collections.Generic;

namespace Db4oUnit.Extensions.Dbmock
{
	public partial class MockClient : IDisposable {
		public virtual void Dispose() {
		}

	    public virtual IList<Extent> Query<Extent>(Predicate<Extent> match)
	    {
	        throw new System.NotImplementedException();
	    }

	    public virtual IList<Extent> Query<Extent>(Predicate<Extent> match, IComparer<Extent> comparer)
	    {
	        throw new System.NotImplementedException();
	    }

	    public virtual IList<Extent> Query<Extent>(Predicate<Extent> match, Comparison<Extent> comparison)
	    {
	        throw new System.NotImplementedException();
	    }

	    public virtual IList<ElementType> Query<ElementType>(Type extent)
	    {
	        throw new System.NotImplementedException();
	    }

	    public virtual IList<Extent> Query<Extent>()
	    {
	        throw new System.NotImplementedException();
	    }

	    public virtual IList<Extent> Query<Extent>(IComparer<Extent> comparer)
	    {
	        throw new System.NotImplementedException();
	    }
	}
}
