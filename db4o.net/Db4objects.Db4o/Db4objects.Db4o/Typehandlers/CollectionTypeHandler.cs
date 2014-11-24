/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers
{
	/// <summary>TypeHandler for Collections.</summary>
	/// <remarks>
	/// TypeHandler for Collections.
	/// On the .NET side, usage is restricted to instances of IList.
	/// </remarks>
	public partial class CollectionTypeHandler : IReferenceTypeHandler, ICascadingTypeHandler
		, IVariableLengthTypeHandler, IQueryableTypeHandler
	{
		public virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			// TODO Auto-generated method stub
			return null;
		}

		public virtual void Write(IWriteContext context, object obj)
		{
			ICollection collection = (ICollection)obj;
			ITypeHandler4 elementHandler = DetectElementTypeHandler(Container(context), collection
				);
			WriteElementClassMetadataId(context, elementHandler);
			WriteElementCount(context, collection);
			WriteElements(context, collection, elementHandler);
		}

		public virtual void Activate(IReferenceActivationContext context)
		{
			ICollection collection = (ICollection)((UnmarshallingContext)context).PersistentObject
				();
			ClearCollection(collection);
			ITypeHandler4 elementHandler = ReadElementTypeHandler(context, context);
			int elementCount = context.ReadInt();
			for (int i = 0; i < elementCount; i++)
			{
				object element = context.ReadObject(elementHandler);
				AddToCollection(collection, element);
			}
		}

		private void WriteElementCount(IWriteContext context, ICollection collection)
		{
			context.WriteInt(collection.Count);
		}

		private void WriteElements(IWriteContext context, ICollection collection, ITypeHandler4
			 elementHandler)
		{
			IEnumerator elements = collection.GetEnumerator();
			while (elements.MoveNext())
			{
				context.WriteObject(elementHandler, elements.Current);
			}
		}

		private ObjectContainerBase Container(IContext context)
		{
			return ((IInternalObjectContainer)context.ObjectContainer()).Container;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Delete(IDeleteContext context)
		{
			if (!context.CascadeDelete())
			{
				return;
			}
			ITypeHandler4 handler = ReadElementTypeHandler(context, context);
			int elementCount = context.ReadInt();
			for (int i = elementCount; i > 0; i--)
			{
				handler.Delete(context);
			}
		}

		public virtual void Defragment(IDefragmentContext context)
		{
			ITypeHandler4 handler = ReadElementTypeHandler(context, context);
			int elementCount = context.ReadInt();
			for (int i = 0; i < elementCount; i++)
			{
				handler.Defragment(context);
			}
		}

		public void CascadeActivation(IActivationContext context)
		{
			IEnumerator all = ((ICollection)context.TargetObject()).GetEnumerator();
			while (all.MoveNext())
			{
				context.CascadeActivationToChild(all.Current);
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

		private void WriteElementClassMetadataId(IWriteContext context, ITypeHandler4 elementHandler
			)
		{
			context.WriteInt(0);
		}

		private ITypeHandler4 ReadElementTypeHandler(IReadBuffer buffer, IContext context
			)
		{
			buffer.ReadInt();
			return (ITypeHandler4)Container(context).Handlers.OpenTypeHandler();
		}

		private ITypeHandler4 DetectElementTypeHandler(IInternalObjectContainer container
			, ICollection collection)
		{
			return (ITypeHandler4)container.Handlers.OpenTypeHandler();
		}

		public virtual bool DescendsIntoMembers()
		{
			return true;
		}
	}
}
