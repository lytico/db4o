/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;
using Db4objects.Db4o.Collections;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent.List
{
	partial class ActivatableListTestCase
	{
		private List<ICollectionElement> NewPopulatedPlainList()
		{
			return (List<ICollectionElement>)NewPopulatedPlainCollection();
		}

		private ActivatableList<ICollectionElement> SingleActivatableCollection()
		{
			return (ActivatableList<ICollectionElement>)SingleCollection();
		}

		private List<ICollectionElement> ToBeAdded()
		{
			return NewPopulatedPlainList();
		}

		protected override IList<ICollectionElement> SingleCollection()
		{
			CollectionHolder<IList<ICollectionElement>> holder = (CollectionHolder<IList<ICollectionElement>>)RetrieveOnlyInstance(typeof(CollectionHolder<IList<ICollectionElement>>));
			return holder.Collection;
		}

		protected override IList<ICollectionElement> NewPlainCollection()
		{
			return new List<ICollectionElement>();
		}

		protected override IList<ICollectionElement> NewActivatableCollection(IList<ICollectionElement> template)
		{
			return new ActivatableList<ICollectionElement>(template);
		}

		protected override IList<ICollectionElement> NewActivatableCollection()
		{
			return new ActivatableList<ICollectionElement>();
		}

		protected override ICollectionElement NewActivatableElement(string value)
		{
			return new ActivatableElement(value);
		}

		protected ICollectionElement NewElement(int index)
		{
			return new Element(Names[index]);
		}

		protected override ICollectionElement NewElement(string value)
		{
			return new Element(value);
		}
	}

	public class SimpleComparer : IComparer<ICollectionElement>
	{
		public static SimpleComparer Instance = new SimpleComparer();

		public int Compare(ICollectionElement x, ICollectionElement y)
		{
			return x.Name.CompareTo(y.Name);
		}
	}
}
