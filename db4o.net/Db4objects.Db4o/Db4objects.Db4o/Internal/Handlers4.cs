/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class Handlers4
	{
		public const int IntId = 1;

		public const int LongId = 2;

		public const int FloatId = 3;

		public const int BooleanId = 4;

		public const int DoubleId = 5;

		public const int ByteId = 6;

		public const int CharId = 7;

		public const int ShortId = 8;

		public const int StringId = 9;

		public const int DateId = 10;

		public const int UntypedId = 11;

		public const int AnyArrayId = 12;

		public const int AnyArrayNId = 13;

		public static bool IsQueryLeaf(ITypeHandler4 handler)
		{
			ITypeHandler4 baseTypeHandler = BaseTypeHandler(handler);
			if (!(baseTypeHandler is IQueryableTypeHandler))
			{
				return false;
			}
			if (baseTypeHandler is ArrayHandler)
			{
				return false;
			}
			return baseTypeHandler is IValueTypeHandler;
		}

		public static bool HandlesArray(ITypeHandler4 handler)
		{
			return handler is ArrayHandler;
		}

		public static bool HandlesMultidimensionalArray(ITypeHandler4 handler)
		{
			return handler is MultidimensionalArrayHandler;
		}

		public static bool HandlesClass(ITypeHandler4 handler)
		{
			return BaseTypeHandler(handler) is ICascadingTypeHandler;
		}

		public static IReflectClass PrimitiveClassReflector(ITypeHandler4 handler, IReflector
			 reflector)
		{
			ITypeHandler4 baseTypeHandler = BaseTypeHandler(handler);
			if (baseTypeHandler is PrimitiveHandler)
			{
				return ((PrimitiveHandler)baseTypeHandler).PrimitiveClassReflector();
			}
			return null;
		}

		public static ITypeHandler4 BaseTypeHandler(ITypeHandler4 handler)
		{
			if (handler is ArrayHandler)
			{
				return ((ArrayHandler)handler).DelegateTypeHandler();
			}
			if (handler is PrimitiveTypeMetadata)
			{
				return ((PrimitiveTypeMetadata)handler).TypeHandler();
			}
			return handler;
		}

		public static IReflectClass BaseType(IReflectClass clazz)
		{
			if (clazz == null)
			{
				return null;
			}
			if (clazz.IsArray())
			{
				return BaseType(clazz.GetComponentType());
			}
			return clazz;
		}

		public static bool IsClassAware(ITypeHandler4 typeHandler)
		{
			return typeHandler is IBuiltinTypeHandler || typeHandler is StandardReferenceTypeHandler;
		}

		public static int CalculateLinkLength(ITypeHandler4 handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException();
			}
			if (handler is ILinkLengthAware)
			{
				return ((ILinkLengthAware)handler).LinkLength();
			}
			if (handler is IReferenceTypeHandler)
			{
				return Const4.IdLength;
			}
			if (handler is IVariableLengthTypeHandler)
			{
				return Const4.IndirectionLength;
			}
			// TODO: For custom handlers there will have to be a way 
			//       to calculate the length in the slot.
			//        Options:
			//        (1) Remember when the first object is marshalled.
			//        (2) Add a #defaultValue() method to TypeHandler4,
			//            marshall the default value and check.
			//        (3) Add a way to test the custom handler when it
			//            is installed and remember the length there. 
			throw new NotImplementedException("Unexpected typehandler: " + handler);
		}

		public static bool HoldsValueType(ITypeHandler4 handler)
		{
			return IsValueType(BaseTypeHandler(handler));
		}

		public static bool IsValueType(ITypeHandler4 handler)
		{
			return !(handler is IReferenceTypeHandler);
		}

		public static bool IsCascading(ITypeHandler4 handler)
		{
			return handler is ICascadingTypeHandler;
		}

		public static bool IsUntyped(ITypeHandler4 handler)
		{
			return handler is OpenTypeHandler;
		}

		public static bool IsVariableLength(ITypeHandler4 handler)
		{
			return handler is IVariableLengthTypeHandler;
		}

		public static IFieldAwareTypeHandler FieldAwareTypeHandler(ITypeHandler4 typeHandler
			)
		{
			if (typeHandler is IFieldAwareTypeHandler)
			{
				return (IFieldAwareTypeHandler)typeHandler;
			}
			return NullFieldAwareTypeHandler.Instance;
		}

		public static void CollectIDs(QueryingReadContext context, ITypeHandler4 typeHandler
			)
		{
			if (typeHandler is ICascadingTypeHandler)
			{
				((ICascadingTypeHandler)typeHandler).CollectIDs(context);
			}
		}

		public static bool UseDedicatedSlot(IContext context, ITypeHandler4 handler)
		{
			if (IsValueType(handler))
			{
				return false;
			}
			if (IsUntyped(handler))
			{
				return false;
			}
			return true;
		}

		public static ITypeHandler4 ArrayElementHandler(ITypeHandler4 handler, QueryingReadContext
			 queryingReadContext)
		{
			if (!(handler is ICascadingTypeHandler))
			{
				return null;
			}
			ICascadingTypeHandler cascadingHandler = (ICascadingTypeHandler)HandlerRegistry.CorrectHandlerVersion
				(queryingReadContext, handler);
			return HandlerRegistry.CorrectHandlerVersion(queryingReadContext, cascadingHandler
				.ReadCandidateHandler(queryingReadContext));
		}

		public static object NullRepresentationInUntypedArrays(ITypeHandler4 handler)
		{
			if (handler is PrimitiveHandler)
			{
				return ((PrimitiveHandler)handler).NullRepresentationInUntypedArrays();
			}
			return null;
		}

		public static bool HandleAsObject(ITypeHandler4 typeHandler)
		{
			if (IsValueType(typeHandler))
			{
				return false;
			}
			if (IsUntyped(typeHandler))
			{
				return false;
			}
			return true;
		}

		public static void CascadeActivation(IActivationContext context, ITypeHandler4 handler
			)
		{
			if (!(handler is ICascadingTypeHandler))
			{
				return;
			}
			((ICascadingTypeHandler)handler).CascadeActivation(context);
		}

		public static bool HandlesPrimitiveArray(ITypeHandler4 typeHandler)
		{
			return typeHandler is ArrayHandler;
		}

		//	    	&& isPrimitive(((ArrayHandler)typeHandler).delegateTypeHandler());
		public static bool HasClassIndex(ITypeHandler4 typeHandler)
		{
			if (typeHandler is StandardReferenceTypeHandler)
			{
				return ((StandardReferenceTypeHandler)typeHandler).ClassMetadata().HasClassIndex(
					);
			}
			return false;
		}

		public static bool CanLoadFieldByIndex(ITypeHandler4 handler)
		{
			if (handler is IQueryableTypeHandler)
			{
				return !((IQueryableTypeHandler)handler).DescendsIntoMembers();
			}
			return true;
		}

		public static object WrapWithTransactionContext(Transaction transaction, object value
			, ITypeHandler4 handler)
		{
			if (IsValueType(handler))
			{
				return value;
			}
			return transaction.Wrap(value);
		}

		public static void CollectIdsInternal(CollectIdContext context, ITypeHandler4 handler
			, int linkLength, bool doWithSlotIndirection)
		{
			if (!(IsCascading(handler)))
			{
				IReadBuffer buffer = context.Buffer();
				buffer.Seek(buffer.Offset() + linkLength);
				return;
			}
			if (handler is StandardReferenceTypeHandler)
			{
				context.AddId();
				return;
			}
			LocalObjectContainer container = (LocalObjectContainer)context.Container();
			SlotFormat slotFormat = context.SlotFormat();
			if (HandleAsObject(handler))
			{
				// TODO: Code is similar to QCandidate.readArrayCandidates. Try to refactor to one place.
				int collectionID = context.ReadInt();
				ByteArrayBuffer collectionBuffer = container.ReadBufferById(context.Transaction()
					, collectionID);
				ObjectHeader objectHeader = new ObjectHeader(container, collectionBuffer);
				QueryingReadContext subContext = new QueryingReadContext(context.Transaction(), context
					.HandlerVersion(), collectionBuffer, collectionID, context.Collector());
				objectHeader.ClassMetadata().CollectIDs(subContext);
				return;
			}
			QueryingReadContext queryingReadContext = new QueryingReadContext(context.Transaction
				(), context.HandlerVersion(), context.Buffer(), 0, context.Collector());
			IClosure4 collectIDsFromQueryingContext = new _IClosure4_263(handler, queryingReadContext
				);
			if (doWithSlotIndirection)
			{
				slotFormat.DoWithSlotIndirection(queryingReadContext, handler, collectIDsFromQueryingContext
					);
			}
			else
			{
				collectIDsFromQueryingContext.Run();
			}
		}

		private sealed class _IClosure4_263 : IClosure4
		{
			public _IClosure4_263(ITypeHandler4 handler, QueryingReadContext queryingReadContext
				)
			{
				this.handler = handler;
				this.queryingReadContext = queryingReadContext;
			}

			public object Run()
			{
				((ICascadingTypeHandler)handler).CollectIDs(queryingReadContext);
				return null;
			}

			private readonly ITypeHandler4 handler;

			private readonly QueryingReadContext queryingReadContext;
		}

		public static bool IsIndirectedIndexed(ITypeHandler4 handler)
		{
			return IsValueType(handler) && (handler is IVariableLengthTypeHandler) && (handler
				 is IIndexableTypeHandler);
		}

		public static IPreparedComparison PrepareComparisonFor(ITypeHandler4 handler, IContext
			 context, object obj)
		{
			if (!(handler is IComparable4))
			{
				return null;
			}
			return ((IComparable4)handler).PrepareComparison(context, obj);
		}

		public static IReflectClass PrimitiveClassReflector(ClassMetadata classMetadata, 
			IReflector reflector)
		{
			if (classMetadata is PrimitiveTypeMetadata)
			{
				return PrimitiveClassReflector(((PrimitiveTypeMetadata)classMetadata).TypeHandler
					(), reflector);
			}
			return null;
		}

		public static void Activate(UnmarshallingContext context, ITypeHandler4 handler)
		{
			if (handler is IReferenceTypeHandler)
			{
				((IReferenceTypeHandler)handler).Activate(context);
			}
		}

		public static void Write(ITypeHandler4 handler, IWriteContext context, object obj
			)
		{
			handler.Write(context, obj);
		}

		public static object ReadValueType(IReadContext context, ITypeHandler4 handler)
		{
			return ((IValueTypeHandler)handler).Read(context);
		}

		public static bool IsStandaloneTypeHandler(ITypeHandler4 customTypeHandler)
		{
			return IsValueType(customTypeHandler) || customTypeHandler is OpenTypeHandler;
		}

		public static ClassMetadata ErasedFieldType(ObjectContainerBase container, IReflectClass
			 fieldType)
		{
			return fieldType.IsInterface() ? container.ClassMetadataForID(UntypedId) : container
				.ProduceClassMetadata(BaseType(fieldType));
		}
	}
}
