/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Foundation
{
	public abstract class LinePrinter
	{
		private sealed class _LinePrinter_9 : LinePrinter
		{
			public _LinePrinter_9()
			{
			}

			public override void Println(string str)
			{
			}
		}

		public static readonly LinePrinter NullPrinter = new _LinePrinter_9();

		// do nothing
		public abstract void Println(string str);

		public static LinePrinter ForPrintStream(TextWriter ps)
		{
			return new _LinePrinter_20(ps);
		}

		private sealed class _LinePrinter_20 : LinePrinter
		{
			public _LinePrinter_20(TextWriter ps)
			{
				this.ps = ps;
			}

			public override void Println(string str)
			{
				ps.WriteLine(str);
			}

			private readonly TextWriter ps;
		}
	}
}
