/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Foundation
{
	public interface ISocket4
	{
		/// <exception cref="System.IO.IOException"></exception>
		void Close();

		/// <exception cref="System.IO.IOException"></exception>
		void Flush();

		void SetSoTimeout(int timeout);

		bool IsConnected();

		/// <exception cref="System.IO.IOException"></exception>
		int Read(byte[] buffer, int offset, int count);

		/// <exception cref="System.IO.IOException"></exception>
		void Write(byte[] bytes, int offset, int count);

		/// <exception cref="System.IO.IOException"></exception>
		ISocket4 OpenParallelSocket();
	}
}
