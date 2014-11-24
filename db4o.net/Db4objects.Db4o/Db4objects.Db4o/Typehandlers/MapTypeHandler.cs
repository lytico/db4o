/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;
using Db4objects.Db4o.Typehandlers.Internal;

namespace Db4objects.Db4o.Typehandlers
{
	/// <summary>Typehandler for classes that implement IDictionary.</summary>
	/// <remarks>Typehandler for classes that implement IDictionary.</remarks>
	public class MapTypeHandler : IReferenceTypeHandler, ICascadingTypeHandler, IVariableLengthTypeHandler
	{
		public virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			// TODO Auto-generated method stub
			return null;
		}

		public virtual void Write(IWriteContext context, object obj)
		{
			IDictionary map = (IDictionary)obj;
			KeyValueHandlerPair handlers = DetectKeyValueTypeHandlers(Container(context), map
				);
			WriteClassMetadataIds(context, handlers);
			WriteElementCount(context, map);
			WriteElements(context, map, handlers);
		}

		public virtual void Activate(IReferenceActivationContext context)
		{
			UnmarshallingContext unmarshallingContext = (UnmarshallingContext)context;
			IDictionary map = (IDictionary)unmarshallingContext.PersistentObject();
			map.Clear();
			KeyValueHandlerPair handlers = ReadKeyValueTypeHandlers(context, context);
			int elementCount = context.ReadInt();
			for (int i = 0; i < elementCount; i++)
			{
				object key = unmarshallingContext.ReadFullyActivatedObjectForKeys(handlers._keyHandler
					);
				if (key == null && !unmarshallingContext.LastReferenceReadWasReallyNull())
				{
					continue;
				}
				object value = context.ReadObject(handlers._valueHandler);
				map[key] = value;
			}
		}

		private void WriteElementCount(IWriteContext context, IDictionary map)
		{
			context.WriteInt(map.Count);
		}

		private void WriteElements(IWriteContext context, IDictionary map, KeyValueHandlerPair
			 handlers)
		{
			IEnumerator elements = map.GetEnumerator();
			while (elements.MoveNext())
			{
				DictionaryEntry entry = (DictionaryEntry)elements.Current;
				context.WriteObject(handlers._keyHandler, entry.Key);
				context.WriteObject(handlers._valueHandler, entry.Value);
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
			KeyValueHandlerPair handlers = ReadKeyValueTypeHandlers(context, context);
			int elementCount = context.ReadInt();
			for (int i = elementCount; i > 0; i--)
			{
				handlers._keyHandler.Delete(context);
				handlers._valueHandler.Delete(context);
			}
		}

		public virtual void Defragment(IDefragmentContext context)
		{
			KeyValueHandlerPair handlers = ReadKeyValueTypeHandlers(context, context);
			int elementCount = context.ReadInt();
			for (int i = elementCount; i > 0; i--)
			{
				context.Defragment(handlers._keyHandler);
				context.Defragment(handlers._valueHandler);
			}
		}

		public void CascadeActivation(IActivationContext context)
		{
			IDictionary map = (IDictionary)context.TargetObject();
			IEnumerator keys = (map).Keys.GetEnumerator();
			while (keys.MoveNext())
			{
				object key = keys.Current;
				context.CascadeActivationToChild(key);
				context.CascadeActivationToChild(map[key]);
			}
		}

		public virtual ITypeHandler4 ReadCandidateHandler(QueryingReadContext context)
		{
			return this;
		}

		public virtual void CollectIDs(QueryingReadContext context)
		{
			KeyValueHandlerPair handlers = ReadKeyValueTypeHandlers(context, context);
			int elementCount = context.ReadInt();
			for (int i = 0; i < elementCount; i++)
			{
				context.ReadId(handlers._keyHandler);
				context.SkipId(handlers._valueHandler);
			}
		}

		private void WriteClassMetadataIds(IWriteContext context, KeyValueHandlerPair handlers
			)
		{
			context.WriteInt(0);
			context.WriteInt(0);
		}

		private KeyValueHandlerPair ReadKeyValueTypeHandlers(IReadBuffer buffer, IContext
			 context)
		{
			buffer.ReadInt();
			buffer.ReadInt();
			ITypeHandler4 untypedHandler = (ITypeHandler4)Container(context).Handlers.OpenTypeHandler
				();
			return new KeyValueHandlerPair(untypedHandler, untypedHandler);
		}

		private KeyValueHandlerPair DetectKeyValueTypeHandlers(IInternalObjectContainer container
			, IDictionary map)
		{
			ITypeHandler4 untypedHandler = (ITypeHandler4)container.Handlers.OpenTypeHandler(
				);
			return new KeyValueHandlerPair(untypedHandler, untypedHandler);
		}
	}
}
