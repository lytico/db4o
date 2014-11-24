/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Net;
using Db4objects.Db4o.Internal.Handlers.Array;


namespace Db4objects.Db4o.Typehandlers
{
    public class SystemArrayTypeHandler : ICascadingTypeHandler, IVariableLengthTypeHandler, IValueTypeHandler
    {
        public virtual IPreparedComparison PrepareComparison(IContext context, object obj)
        {
            return ReadArrayHandler(context).PrepareComparison(context, obj);
        }

        public virtual void Write(IWriteContext context, object obj)
        {
            Array collection = (Array) obj;
            ClassMetadata elementType = DetectElementTypeHandler(Container(context), collection);
            WriteElementTypeId(context, elementType);
            new ArrayHandler(elementType.TypeHandler(), false).Write(context, obj);
        }

    	public virtual object Read(IReadContext context)
        {
            return ReadArrayHandler(context).Read(context);
        }

        private static ArrayHandler ReadArrayHandler(IContext context)
        {
            ITypeHandler4 handler = ReadElementTypeHandler((IReadBuffer)context, context);
            return new ArrayHandler(handler, false);
        }

        public virtual void Delete(IDeleteContext context)
        {
            ReadArrayHandler(context).Delete(context);
        }

        public virtual void Defragment(IDefragmentContext context)
        {
            DefragmentElementHandlerId(context);
            ReadArrayHandler(context).Defragment(context);
        }

        public void CascadeActivation(IActivationContext context)
        {
            ICollection collection = ((ICollection)context.TargetObject());
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
            ReadArrayHandler(context).CollectIDs(context);
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

        private static void WriteElementTypeId(IWriteContext context, ClassMetadata elementType)
        {
            context.WriteInt(elementType.GetID());
        }

        private static ObjectContainerBase Container(IContext context)
        {
            return ((IInternalObjectContainer)context.ObjectContainer()).Container;
        }

        private static ITypeHandler4 ReadElementTypeHandler(IReadBuffer buffer, IContext context)
        {
            int elementHandlerId = buffer.ReadInt();
            if (elementHandlerId == 0) return OpenTypeHandlerFrom(context);

            ITypeHandler4 elementHandler = Container(context).TypeHandlerForClassMetadataID(elementHandlerId);
            return elementHandler ?? OpenTypeHandlerFrom(context);
        }

        private static ClassMetadata DetectElementTypeHandler(ObjectContainerBase container, Array collection)
        {
            Type elementType = ElementTypeOf(collection);
        	return container.ProduceClassMetadata(container.Reflector().ForClass(elementType));
        }

        private static bool IsNullableInstance(Type elementType)
        {
            return elementType.IsGenericType && (elementType.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        private static Type ElementTypeOf(Array array)
        {
            return array.GetType().GetElementType();
        }

    }

    internal class SystemArrayPredicate : ITypeHandlerPredicate
    {
        public bool Match(IReflectClass classReflector)
        {
            if(classReflector == null)
            {
                return false;
            }
            Type type = NetReflector.ToNative(classReflector);
            return type == typeof(System.Array);
        }
    }

}
