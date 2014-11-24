/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public abstract class QEStringCmp : QEAbstract
	{
		private bool caseSensitive;

		/// <summary>for C/S messaging only</summary>
		public QEStringCmp()
		{
		}

		public QEStringCmp(bool caseSensitive_)
		{
			caseSensitive = caseSensitive_;
		}

		internal override bool Evaluate(QConObject constraint, IInternalCandidate candidate
			, object obj)
		{
			if (obj != null)
			{
				if (obj is ByteArrayBuffer)
				{
					obj = StringHandler.ReadString(candidate.Transaction().Context(), (ByteArrayBuffer
						)obj);
				}
				string candidateStringValue = obj.ToString();
				string stringConstraint = constraint.GetObject().ToString();
				if (!caseSensitive)
				{
					candidateStringValue = candidateStringValue.ToLower();
					stringConstraint = stringConstraint.ToLower();
				}
				return CompareStrings(candidateStringValue, stringConstraint);
			}
			return constraint.GetObject() == null;
		}

		public override bool SupportsIndex()
		{
			return false;
		}

		protected abstract bool CompareStrings(string candidate, string constraint);
	}
}
