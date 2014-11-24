/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
#if NET_4_0 && !SILVERLIGHT
using System.IO;
using System.Runtime.InteropServices;

namespace Db4objects.Db4o.Internal.CLI
{
	internal class CLR40 : CLIBase
	{
		[System.Security.SecuritySafeCritical]
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern int FlushFileBuffers(Microsoft.Win32.SafeHandles.SafeFileHandle fileHandle);

		[System.Security.SecuritySafeCritical]
		public override void Flush(FileStream stream)
		{
			stream.Flush(true); 
			FlushFileBuffers(stream.SafeFileHandle); // We still need to call FlushFileBuffer due to bug 
													 // http://connect.microsoft.com/VisualStudio/feedback/details/634385/filestream-flush-flushtodisk-true-call-does-not-flush-the-buffers-to-disk#details
		}
	}
}
#endif