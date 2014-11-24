/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.Querying;
using Sharpen.Util;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class NonPrimitiveSortingTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new NonPrimitiveSortingTestCase().RunSolo();
		}

		private static readonly NonPrimitiveSortingTestCase.ReleaseNote rn1 = new NonPrimitiveSortingTestCase.ReleaseNote
			("foo", "no comments");

		private static readonly NonPrimitiveSortingTestCase.ReleaseNote rn2 = new NonPrimitiveSortingTestCase.ReleaseNote
			("bar", "next after the best ever");

		private static readonly NonPrimitiveSortingTestCase.ReleaseNote rn3 = new NonPrimitiveSortingTestCase.ReleaseNote
			("foo.bar", "the best ever");

		private static readonly IList reference = Arrays.AsList(new NonPrimitiveSortingTestCase.Holder
			[] { new NonPrimitiveSortingTestCase.Holder(new NonPrimitiveSortingTestCase.Version
			(1, 0, 0, rn1)), new NonPrimitiveSortingTestCase.Holder(new NonPrimitiveSortingTestCase.Version
			(3, 0, 0, rn2)), new NonPrimitiveSortingTestCase.Holder(new NonPrimitiveSortingTestCase.Version
			(2, 0, 0, rn3)), new NonPrimitiveSortingTestCase.Holder(new NonPrimitiveSortingTestCase.Version
			(1, 1, 0, rn1)), new NonPrimitiveSortingTestCase.Holder(new NonPrimitiveSortingTestCase.Version
			(1, 0, 1, rn2)), new NonPrimitiveSortingTestCase.Holder(new NonPrimitiveSortingTestCase.Version
			(1, 0, 0, rn2)), new NonPrimitiveSortingTestCase.Holder(new NonPrimitiveSortingTestCase.Version
			(2, 0, 1, rn3)), new NonPrimitiveSortingTestCase.Holder(new NonPrimitiveSortingTestCase.Version
			(2, 1, 0, rn1)), new NonPrimitiveSortingTestCase.Holder(new NonPrimitiveSortingTestCase.Version
			(2, 0, 2, rn2)) });

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentActivationSupport());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (IEnumerator holderIter = reference.GetEnumerator(); holderIter.MoveNext(); )
			{
				NonPrimitiveSortingTestCase.Holder holder = ((NonPrimitiveSortingTestCase.Holder)
					holderIter.Current);
				Db().Store(holder);
			}
		}

		public virtual void TestSorting()
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(NonPrimitiveSortingTestCase.Holder));
			query.Descend("version").OrderAscending();
			IList queryResult = query.Execute();
			IList referenceSorted = ReferencSorting();
			IEnumerator db4oIt = queryResult.GetEnumerator();
			IEnumerator referenceIt = referenceSorted.GetEnumerator();
			Assert.AreEqual(referenceSorted.Count, queryResult.Count);
			while (db4oIt.MoveNext() && referenceIt.MoveNext())
			{
				NonPrimitiveSortingTestCase.Holder db4o = ((NonPrimitiveSortingTestCase.Holder)db4oIt
					.Current);
				NonPrimitiveSortingTestCase.Holder reference = ((NonPrimitiveSortingTestCase.Holder
					)referenceIt.Current);
				NonPrimitiveSortingTestCase.Version expected = reference.GetVersion();
				Assert.AreEqual(db4o.GetVersion(), expected);
			}
		}

		private IList ReferencSorting()
		{
			ArrayList referencesorted = new ArrayList(reference);
			referencesorted.Sort(new _IComparer_67());
			return referencesorted;
		}

		private sealed class _IComparer_67 : IComparer
		{
			public _IComparer_67()
			{
			}

			public int Compare(object o1, object o2)
			{
				return ((NonPrimitiveSortingTestCase.Holder)o1).GetVersion().CompareTo(((NonPrimitiveSortingTestCase.Holder
					)o2).GetVersion());
			}
		}

		private class Holder
		{
			private readonly NonPrimitiveSortingTestCase.Version version;

			public Holder(NonPrimitiveSortingTestCase.Version version)
			{
				this.version = version;
			}

			public virtual NonPrimitiveSortingTestCase.Version GetVersion()
			{
				return version;
			}
		}

		private class ReleaseNote : IComparable
		{
			private string remarks;

			private string authorName;

			public ReleaseNote(string authorName, string remarks)
			{
				this.authorName = authorName;
				this.remarks = remarks;
			}

			public virtual int CompareTo(object other)
			{
				int cmp = Sharpen.Runtime.CompareOrdinal(authorName, ((NonPrimitiveSortingTestCase.ReleaseNote
					)other).authorName);
				if (cmp == 0)
				{
					cmp = Sharpen.Runtime.CompareOrdinal(remarks, ((NonPrimitiveSortingTestCase.ReleaseNote
						)other).remarks);
				}
				return cmp;
			}

			public override int GetHashCode()
			{
				return remarks.GetHashCode() ^ authorName.GetHashCode();
			}

			public override string ToString()
			{
				return "{" + GetType().Name + ": " + authorName + " / " + remarks + "}";
			}
		}

		private class Version : IComparable, IActivatable
		{
			private readonly int major;

			private readonly int minor;

			private readonly int bugFix;

			private NonPrimitiveSortingTestCase.ReleaseNote notes;

			[System.NonSerialized]
			private IActivator _activator;

			public Version(int major, int minor, int bugFix, NonPrimitiveSortingTestCase.ReleaseNote
				 notes)
			{
				this.major = major;
				this.minor = minor;
				this.bugFix = bugFix;
				this.notes = notes;
			}

			public virtual int CompareTo(object o)
			{
				Activate(ActivationPurpose.Read);
				int comparison = major.CompareTo(((NonPrimitiveSortingTestCase.Version)o).major);
				if (0 == comparison)
				{
					comparison = minor.CompareTo(((NonPrimitiveSortingTestCase.Version)o).minor);
				}
				if (0 == comparison)
				{
					comparison = bugFix.CompareTo(((NonPrimitiveSortingTestCase.Version)o).bugFix);
				}
				if (0 == comparison)
				{
					comparison = notes.CompareTo(((NonPrimitiveSortingTestCase.Version)o).notes);
				}
				return comparison;
			}

			public override bool Equals(object o)
			{
				Activate(ActivationPurpose.Read);
				if (this == o)
				{
					return true;
				}
				if (o == null || GetType() != o.GetType())
				{
					return false;
				}
				NonPrimitiveSortingTestCase.Version other = (NonPrimitiveSortingTestCase.Version)
					o;
				if (bugFix != other.bugFix)
				{
					return false;
				}
				if (major != other.major)
				{
					return false;
				}
				if (minor != other.minor)
				{
					return false;
				}
				if (notes.CompareTo(other.notes) != 0)
				{
					return false;
				}
				return true;
			}

			public override int GetHashCode()
			{
				Activate(ActivationPurpose.Read);
				int result = major;
				result = 31 * result + minor;
				result = 31 * result + bugFix;
				result = 31 * result + notes.GetHashCode();
				return result;
			}

			public override string ToString()
			{
				Activate(ActivationPurpose.Read);
				return typeof(NonPrimitiveSortingTestCase.Version).Name + " [" + major + "." + minor
					 + "." + bugFix + " - " + notes + "]";
			}

			public virtual void Bind(IActivator activator)
			{
				_activator = activator;
			}

			public virtual void Activate(ActivationPurpose purpose)
			{
				if (_activator != null)
				{
					_activator.Activate(purpose);
				}
			}
		}
	}
}
#endif // !SILVERLIGHT
