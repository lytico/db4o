/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Tests.Common.References;
using Sharpen;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.References
{
	public class WeakReferenceCollectionTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
		}

		//COR-1839
		#if !SILVERLIGHT
		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			if (!Platform4.HasWeakReferences())
			{
				return;
			}
			WeakReferenceCollectionTestCase.Item item = new WeakReferenceCollectionTestCase.Item
				();
			Store(item);
			Commit();
			ByRef reference = new ByRef();
			ReferenceSystem().TraverseReferences(new _IVisitor4_30(reference));
			Assert.IsNotNull(((ObjectReference)reference.value));
			item = null;
			long timeout = 10000;
			long startTime = Runtime.CurrentTimeMillis();
			while (true)
			{
				long currentTime = Runtime.CurrentTimeMillis();
				if (currentTime - startTime >= timeout)
				{
					Assert.Fail("Timeout waiting for WeakReference collection.");
				}
				Runtime.Gc();
				Runtime.RunFinalization();
				Thread.Sleep(1);
				if (((ObjectReference)reference.value).GetObject() == null)
				{
					break;
				}
			}
			startTime = Runtime.CurrentTimeMillis();
			while (true)
			{
				long currentTime = Runtime.CurrentTimeMillis();
				if (currentTime - startTime >= timeout)
				{
					Assert.Fail("Timeout waiting for removal of ObjectReference from ReferenceSystem."
						);
				}
				BooleanByRef found = new BooleanByRef();
				ReferenceSystem().TraverseReferences(new _IVisitor4_63(reference, found));
				if (!found.value)
				{
					return;
				}
				Thread.Sleep(10);
			}
		}
		#endif // !SILVERLIGHT

		private sealed class _IVisitor4_30 : IVisitor4
		{
			public _IVisitor4_30(ByRef reference)
			{
				this.reference = reference;
			}

			public void Visit(object @ref)
			{
				if (((ObjectReference)@ref).GetObject() is WeakReferenceCollectionTestCase.Item)
				{
					reference.value = ((ObjectReference)@ref);
				}
			}

			private readonly ByRef reference;
		}

		private sealed class _IVisitor4_63 : IVisitor4
		{
			public _IVisitor4_63(ByRef reference, BooleanByRef found)
			{
				this.reference = reference;
				this.found = found;
			}

			public void Visit(object @ref)
			{
				if (((ObjectReference)@ref) == ((ObjectReference)reference.value))
				{
					found.value = true;
				}
			}

			private readonly ByRef reference;

			private readonly BooleanByRef found;
		}

		private IReferenceSystem ReferenceSystem()
		{
			return Trans().ReferenceSystem();
		}
	}
}
