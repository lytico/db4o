/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o
{
	/// <exclude></exclude>
	public interface IBlobTransport
	{
		/// <exception cref="System.IO.IOException"></exception>
		void WriteBlobTo(Transaction trans, BlobImpl blob);

		/// <exception cref="System.IO.IOException"></exception>
		void ReadBlobFrom(Transaction trans, BlobImpl blob);

		void DeleteBlobFile(Transaction trans, BlobImpl blob);
	}
}
#endif // !SILVERLIGHT
