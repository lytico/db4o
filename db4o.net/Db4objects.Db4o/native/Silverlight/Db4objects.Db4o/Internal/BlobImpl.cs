/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

#if SILVERLIGHT

namespace Db4objects.Db4o.Internal
{
	public class BlobImpl : IDb4oTypeImpl
	{
		public void SetTrans(Transaction a_trans)
		{
			throw new System.NotImplementedException();
		}

		public bool CanBind()
		{
			throw new System.NotImplementedException();
		}

		public object CreateDefault(Transaction trans)
		{
			throw new System.NotImplementedException();
		}

		public bool HasClassIndex()
		{
			throw new System.NotImplementedException();
		}

		public void SetObjectReference(ObjectReference @ref)
		{
			throw new System.NotImplementedException();
		}

		public object StoredTo(Transaction trans)
		{
			throw new System.NotImplementedException();
		}

		public void PreDeactivate()
		{
			throw new System.NotImplementedException();
		}
	}
}

#endif