/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */
namespace Db4objects.Db4o.Tests.Common.Api
{
	public partial class TestWithTempFile
	{
		protected virtual string TempFile()
		{
			if (_tempFile == null)
			{
#if SILVERLIGHT
				_tempFile = "temp-file-" + System.DateTime.Now.Ticks;
#else
				 _tempFile = System.IO.Path.GetTempFileName();
#endif
			}
			return _tempFile;
		}
	}
}
