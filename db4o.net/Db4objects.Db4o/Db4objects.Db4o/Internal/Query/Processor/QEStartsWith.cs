/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public class QEStartsWith : QEStringCmp
	{
		/// <summary>for C/S messaging only</summary>
		public QEStartsWith()
		{
		}

		public QEStartsWith(bool caseSensitive_) : base(caseSensitive_)
		{
		}

		protected override bool CompareStrings(string candidate, string constraint)
		{
			return candidate.IndexOf(constraint) == 0;
		}
	}
}
