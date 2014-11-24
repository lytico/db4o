/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Foundation.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Typehandlers
{
	public class GenericCollectionTypeHandler : IReferenceTypeHandler, ICascadingTypeHandler, IVariableLengthTypeHandler, IQueryableTypeHandler
	{
		public virtual IPreparedComparison PrepareComparison(IContext context, object obj)
		{
			return null;
		}

		public virtual void Write(IWriteContext context, object obj)
		{
            ICollectionInitializer initializer = CollectionInitializer.For(obj);
            IEnumerable enumerable = (IEnumerable)obj;
			ClassMetadata elementType = DetectElementTypeErasingNullables(Container(context), enumerable);
			WriteElementTypeHandlerId(context, elementType);
			WriteElementCount(context, initializer);
			WriteElements(context, enumerable, elementType.TypeHandler());
		}

		public virtual void Activate(IReferenceActivationContext context)
		{
			object collection = context.PersistentObject();
			ICollectionInitializer initializer = CollectionInitializer.For(collection);
			initializer.Clear();

			ReadElements(context, initializer, ReadElementTypeHandler(context, context));

			initializer.FinishAdding();
		}

		public virtual void Delete(IDeleteContext context)
		{
			if (!context.CascadeDelete()) return;

			ITypeHandler4 handler = ReadElementTypeHandler(context, context);
			int elementCount = context.ReadInt();
			for (int i = elementCount; i > 0; i--)
			{
				handler.Delete(context);
			}
		}

		public virtual void Defragment(IDefragmentContext context)
		{
			DefragmentElementHandlerId(context);
			ITypeHandler4 handler = ReadElementTypeHandler(context, context);
			int elementCount = context.ReadInt();
			for (int i = 0; i < elementCount; i++)
			{
				context.Defragment(handler);
			}
		}

		public void CascadeActivation(IActivationContext context)
		{
            IEnumerable collection = ((IEnumerable)context.TargetObject());

			// TODO: detect the element type
			// and return immediately when it's a primitive

			foreach (object item in collection)
			{
				context.CascadeActivationToChild(item);
			}
		}

		public virtual ITypeHandler4 ReadCandidateHandler(QueryingReadContext context)
		{
			return this;
		}

		public virtual void CollectIDs(QueryingReadContext context)
		{
			ITypeHandler4 elementHandler = ReadElementTypeHandler(context, context);
			int elementCount = context.ReadInt();
			for (int i = 0; i < elementCount; i++)
			{
				context.ReadId(elementHandler);
			}
		}

		private static void DefragmentElementHandlerId(IDefragmentContext context)
		{
			int offset = context.Offset();
			context.CopyID();
			context.Seek(offset);
		}

		private static ITypeHandler4 OpenTypeHandlerFrom(IContext context)
		{
			return context.Transaction().Container().Handlers.OpenTypeHandler();
		}

		private static void ReadElements(IReadContext context, ICollectionInitializer initializer, ITypeHandler4 elementHandler)
		{
			int elementCount = context.ReadInt();
			for (int i = 0; i < elementCount; i++)
			{
				initializer.Add(context.ReadObject(elementHandler));
			}
		}

		private static void WriteElementTypeHandlerId(IWriteContext context, ClassMetadata type)
		{
			context.WriteInt(type.GetID());
		}

        private static void WriteElementCount(IWriteBuffer context, ICollectionInitializer initializer)
		{
            context.WriteInt(initializer.Count());
		}

		private static void WriteElements(IWriteContext context, IEnumerable enumerable, ITypeHandler4 elementHandler)
		{
			IEnumerator elements = enumerable.GetEnumerator();
			while (elements.MoveNext())
			{
				context.WriteObject(elementHandler, elements.Current);
			}
		}

		private static ObjectContainerBase Container(IContext context)
		{
			return ((IInternalObjectContainer)context.ObjectContainer()).Container;
		}

		private static ITypeHandler4 ReadElementTypeHandler(IReadBuffer buffer, IContext context)
		{
			int elementTypeId = buffer.ReadInt();
			if (elementTypeId == 0) return OpenTypeHandlerFrom(context);

			ITypeHandler4 elementHandler = Container(context).TypeHandlerForClassMetadataID(elementTypeId);
			return elementHandler ?? OpenTypeHandlerFrom(context);
		}

		private static ClassMetadata DetectElementTypeErasingNullables(ObjectContainerBase container, IEnumerable collection)
		{
			Type elementType = ElementTypeOf(collection);
			if (IsNullableInstance(elementType))
			{
				return container.ClassMetadataForReflectClass(container.Handlers.IclassObject);
			}
			return container.ProduceClassMetadata(container.Reflector().ForClass(elementType));
		}

		private static bool IsNullableInstance(Type elementType)
		{
			return elementType.IsGenericType && (elementType.GetGenericTypeDefinition() == typeof(Nullable<>));
		}

		private static Type ElementTypeOf(IEnumerable collection)
		{
			Type genericCollectionType = GenericCollectionTypeFor(collection.GetType());
			return genericCollectionType.GetGenericArguments()[0];
		}

		private static Type GenericCollectionTypeFor(Type type)
		{
            if (type == null)
            {
                throw new InvalidOperationException();
            }

            if (IsGenericCollectionType(type))
			{
				return type;
			}

			return GenericCollectionTypeFor(type.BaseType);
		}

		private static bool IsGenericCollectionType(Type type)
		{
			return type.IsGenericType && Array.IndexOf(_supportedCollections, type.GetGenericTypeDefinition()) >= 0;
		}

		public bool DescendsIntoMembers()
		{
			return true;
		}

		public void RegisterSupportedTypesWith(Action<Type> registrationAction)
		{
			foreach (Type collectionType in _supportedCollections)
			{
				registrationAction(collectionType);
			}
		}

		private static readonly Type[] _supportedCollections = new Type[]
												{
													typeof(List<>),
													typeof(LinkedList<>),
													typeof(Stack<>),
													typeof(Queue<>),
													typeof(System.Collections.ObjectModel.Collection<>),
													typeof(ActivatableList<>),
#if NET_3_5 && !CF && !SILVERLIGHT
													typeof(HashSet<>),
#endif
												};
	}

}
