/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Foundation
{
	public interface IServerSocket4
	{
		void SetSoTimeout(int timeout);

		int GetLocalPort();

		/// <exception cref="System.IO.IOException"></exception>
		ISocket4 Accept();

		/// <exception cref="System.IO.IOException"></exception>
		void Close();
	}
}
