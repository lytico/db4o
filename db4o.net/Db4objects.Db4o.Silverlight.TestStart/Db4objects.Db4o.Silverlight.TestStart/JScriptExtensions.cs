namespace Db4objects.Db4o.Silverlight.TestStart
{
	public static class JScriptExtensions
	{
		public static string ToJScriptString(this object value)
		{
			return value.ToString()
						.Replace("\"", @"\\""")
						.Replace("\r", @"\r")
						.Replace("\n", @"\n");
		}
	}
}
