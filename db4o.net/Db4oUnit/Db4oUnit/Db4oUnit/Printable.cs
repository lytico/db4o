/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;

namespace Db4oUnit
{
	public abstract class Printable
	{
		public override string ToString()
		{
			StringWriter writer = new StringWriter();
			try
			{
				Print(writer);
			}
			catch (IOException)
			{
			}
			return writer.ToString();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public abstract void Print(TextWriter writer);
	}
}
