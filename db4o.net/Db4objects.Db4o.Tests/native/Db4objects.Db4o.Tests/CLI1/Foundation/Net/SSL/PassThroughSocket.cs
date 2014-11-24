/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System.IO;
using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.Tests.CLI1.Foundation.Net.SSL
{
	internal class PassThroughSocket : Socket4Decorator
	{
		public PassThroughSocket(ISocket4 socket) : base(socket)
		{
		}

		public override void Write(byte[] bytes, int offset, int count)
		{
			AppendBytes(bytes, offset, count);
			base.Write(bytes, offset, count);
		}

		private void AppendBytes(byte[] bytes, int offset, int count)
		{
			_written.Write(bytes, offset, count);
		}

		public byte[] Written
		{
			get
			{
				_written.Seek(0, SeekOrigin.Begin);

				byte[] data = new byte[_written.Length];
				int read = _written.Read(data, 0, data.Length);
				return data;
			}
		}

		private readonly MemoryStream _written = new MemoryStream();
	}
}
#endif