using System;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
	public abstract class GenericCollectionTestFactory : ILabeled
	{
		public static readonly string FieldName = "_coll";

		public abstract object NewItem<T>();
		public abstract Type ContainerType();
		public abstract string Label();
	}
}
