using System;
using System.IO;

namespace WixBuilder.Tests
{
	class TestUtils
	{
		public static string UniqueTempFolder()
		{
			return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		}
	}
}
