/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
#if !NET_4_0 && NET_3_5 && !SILVERLIGHT && !CF
using System.IO;
using System.Runtime.InteropServices;

namespace Db4objects.Db4o.Internal.CLI
{
	internal class CLR35 : CLIBase
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern int FlushFileBuffers(Microsoft.Win32.SafeHandles.SafeFileHandle fileHandle);

		public override void Flush(FileStream stream)
		{
			stream.Flush(); 
			FlushFileBuffers(stream.SafeFileHandle); 
		}
	}
}
#endif