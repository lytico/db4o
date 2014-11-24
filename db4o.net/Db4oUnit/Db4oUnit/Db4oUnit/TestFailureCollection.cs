/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using System.IO;
using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit
{
	public class TestFailureCollection : Printable, IEnumerable
	{
		private readonly Collection4 _failures = new Collection4();

		public virtual IEnumerator GetEnumerator()
		{
			return _failures.GetEnumerator();
		}

		public virtual int Count
		{
			get
			{
				return _failures.Size();
			}
		}

		public virtual void Add(TestFailure failure)
		{
			_failures.Add(failure);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public override void Print(TextWriter writer)
		{
			PrintSummary(writer);
			PrintDetails(writer);
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void PrintSummary(TextWriter writer)
		{
			int index = 1;
			IEnumerator e = GetEnumerator();
			while (e.MoveNext())
			{
				writer.Write(index.ToString());
				writer.Write(") ");
				writer.Write(((TestFailure)e.Current).TestLabel);
				writer.Write(TestPlatform.NewLine);
				++index;
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void PrintDetails(TextWriter writer)
		{
			int index = 1;
			IEnumerator e = GetEnumerator();
			while (e.MoveNext())
			{
				writer.Write(TestPlatform.NewLine);
				writer.Write(index.ToString());
				writer.Write(") ");
				((Printable)e.Current).Print(writer);
				writer.Write(TestPlatform.NewLine);
				++index;
			}
		}
	}
}
