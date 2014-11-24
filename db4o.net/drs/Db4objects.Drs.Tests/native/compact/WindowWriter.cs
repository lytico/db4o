/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */
#if CF
namespace Db4objects.Db4o.Tests
{
	class WindowWriter : System.IO.TextWriter 
	{
		public override System.Text.Encoding Encoding
		{
			get
			{
				return System.Text.Encoding.UTF8;
			}
		}

		public override void Write(string s)
		{
			Console.WriteLine(s);
		}

		public override void Write(object o)
		{
			Console.WriteLine(o);
		}

		public override void WriteLine(string s)
		{
			Console.WriteLine(s);
		}

		public override void WriteLine(object o)
		{
			Console.WriteLine(o);
		}
	}
}

#endif