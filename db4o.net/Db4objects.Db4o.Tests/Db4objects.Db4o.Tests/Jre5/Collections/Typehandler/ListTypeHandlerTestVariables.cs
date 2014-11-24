/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public sealed class ListTypeHandlerTestVariables
	{
		public static readonly FixtureVariable ListImplementation = new FixtureVariable("list"
			);

		public static readonly FixtureVariable ElementsSpec = new FixtureVariable("elements"
			);

		public static readonly FixtureVariable ListTypehander = new FixtureVariable("typehandler"
			);

		public static readonly IFixtureProvider ListFixtureProvider = new SimpleFixtureProvider
			(ListImplementation, new object[] { new ListTypeHandlerTestVariables.ArrayListItemFactory
			(), new ListTypeHandlerTestVariables.LinkedListItemFactory(), new ListTypeHandlerTestVariables.ListItemFactory
			(), new ListTypeHandlerTestVariables.NamedArrayListItemFactory() });

		public static readonly IFixtureProvider TypehandlerFixtureProvider = new SimpleFixtureProvider
			(ListTypehander, new object[] { null });

		public static readonly ListTypeHandlerTestElementsSpec StringElementsSpec = new ListTypeHandlerTestElementsSpec
			(new object[] { "zero", "one" }, "two", "zzz");

		public static readonly ListTypeHandlerTestElementsSpec IntElementsSpec = new ListTypeHandlerTestElementsSpec
			(new object[] { 0, 1 }, 2, int.MaxValue);

		public static readonly ListTypeHandlerTestElementsSpec ObjectElementsSpec = new ListTypeHandlerTestElementsSpec
			(new object[] { new ListTypeHandlerTestVariables.ReferenceElement(0), new ListTypeHandlerTestVariables.ReferenceElement
			(1) }, new ListTypeHandlerTestVariables.ReferenceElement(2), null);

		private ListTypeHandlerTestVariables()
		{
		}

		public class ReferenceElement
		{
			public int _id;

			public ReferenceElement(int id)
			{
				_id = id;
			}

			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null || GetType() != obj.GetType())
				{
					return false;
				}
				ListTypeHandlerTestVariables.ReferenceElement other = (ListTypeHandlerTestVariables.ReferenceElement
					)obj;
				return _id == other._id;
			}

			public override int GetHashCode()
			{
				return _id;
			}

			public override string ToString()
			{
				return "FCE#" + _id;
			}
		}

		private class ArrayListItemFactory : AbstractListItemFactory, ILabeled
		{
			private class Item
			{
				public ArrayList _list = new ArrayList();
			}

			public override object NewItem()
			{
				return new ListTypeHandlerTestVariables.ArrayListItemFactory.Item();
			}

			public override Type ItemClass()
			{
				return typeof(ListTypeHandlerTestVariables.ArrayListItemFactory.Item);
			}

			public override Type ContainerClass()
			{
				return typeof(ArrayList);
			}

			public virtual string Label()
			{
				return "ArrayList";
			}
		}

		private class LinkedListItemFactory : AbstractListItemFactory, ILabeled
		{
			private class Item
			{
				public ArrayList _list = new ArrayList();
			}

			public override object NewItem()
			{
				return new ListTypeHandlerTestVariables.LinkedListItemFactory.Item();
			}

			public override Type ItemClass()
			{
				return typeof(ListTypeHandlerTestVariables.LinkedListItemFactory.Item);
			}

			public override Type ContainerClass()
			{
				return typeof(ArrayList);
			}

			public virtual string Label()
			{
				return "LinkedList";
			}
		}

		private class ListItemFactory : AbstractListItemFactory, ILabeled
		{
			private class Item
			{
				public IList _list = new ArrayList();
			}

			public override object NewItem()
			{
				return new ListTypeHandlerTestVariables.ListItemFactory.Item();
			}

			public override Type ItemClass()
			{
				return typeof(ListTypeHandlerTestVariables.ListItemFactory.Item);
			}

			public override Type ContainerClass()
			{
				return typeof(ArrayList);
			}

			public virtual string Label()
			{
				return "[Linked]List";
			}
		}

		private class NamedArrayListItemFactory : AbstractListItemFactory, ILabeled
		{
			private class Item
			{
				public ArrayList _list = new NamedArrayList();
			}

			public override object NewItem()
			{
				return new ListTypeHandlerTestVariables.NamedArrayListItemFactory.Item();
			}

			public override Type ItemClass()
			{
				return typeof(ListTypeHandlerTestVariables.NamedArrayListItemFactory.Item);
			}

			public override Type ContainerClass()
			{
				return typeof(NamedArrayList);
			}

			public virtual string Label()
			{
				return "NamedArrayList";
			}
		}
	}
}
