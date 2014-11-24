/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

using System;
using System.Collections.Generic;
using Db4objects.Db4o.Collections;
using Db4objects.Db4o.Internal.Collections;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Net;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
    class TypeHandlerConfigurationDotNet : TypeHandlerConfiguration
    {
        public TypeHandlerConfigurationDotNet(Config4Impl config) : base(config)
        {
            ListTypeHandler(new CollectionTypeHandler());
            MapTypeHandler(new MapTypeHandler());
        }

        public override void Apply()
        {
#if !SILVERLIGHT
			RegisterCollection(typeof(System.Collections.ArrayList));
            RegisterCollection(typeof (System.Collections.CollectionBase));
            RegisterMap(typeof (System.Collections.Hashtable));
            RegisterMap(typeof (System.Collections.Specialized.HybridDictionary));
#if !CF
            RegisterMap(typeof (System.Collections.DictionaryBase));
#endif
#endif
			RegisterGenericTypeHandlers();
			RegisterBigSetTypeHandler();
            RegisterSystemArrayTypeHandler();
        }

    	private void RegisterBigSetTypeHandler()
    	{
    		RegisterGenericTypeHandler(typeof(BigSet<>), new BigSetTypeHandler());
    	}

    	private void RegisterGenericTypeHandlers()
        {
			GenericCollectionTypeHandler handler = new GenericCollectionTypeHandler();
    		handler.RegisterSupportedTypesWith(delegate(Type type) 
			{
				RegisterGenericTypeHandler(type, handler);
    		});

#if NET_3_5 && !CF && !SILVERLIGHT
			_config.Reflector().RegisterCollection(new GenericCollectionTypePredicate(typeof(HashSet<>)));
#endif 

			Type[] dictionaryTypes = new Type[] {
				typeof(ActivatableDictionary<,>),
				typeof(Dictionary<,>),
#if !SILVERLIGHT
				typeof(SortedList<,>),
#if !CF
				typeof(SortedDictionary<,>),
#endif
#endif
			};
            _config.RegisterTypeHandler(new GenericTypeHandlerPredicate(dictionaryTypes), new MapTypeHandler());

        }

    	private void RegisterGenericTypeHandler(Type genericTypeDefinition, ITypeHandler4 handler)
    	{
    		_config.RegisterTypeHandler(new GenericTypeHandlerPredicate(genericTypeDefinition), handler);
    	}

    	internal class GenericTypeHandlerPredicate : ITypeHandlerPredicate
        {
            private readonly Type[] _genericTypes;

            internal GenericTypeHandlerPredicate(params Type[] genericType)
            {
                _genericTypes = genericType;
            }

            public bool Match(IReflectClass classReflector)
            {
                Type type = NetReflector.ToNative(classReflector);
                if (type == null)
                {
                    return false;
                }

                if (!type.IsGenericType)
                {
                    return false;
                }
            	return ((IList<Type>) _genericTypes).Contains(type.GetGenericTypeDefinition());
            }
        }

        private void RegisterSystemArrayTypeHandler()
        {
            _config.RegisterTypeHandler(new SystemArrayPredicate(), new SystemArrayTypeHandler());
        }

        internal class GenericCollectionTypePredicate : IReflectClassPredicate
        {
            private readonly Type _type;

            internal GenericCollectionTypePredicate(Type t)
            {
                _type = t;
            }


            public bool Match(IReflectClass classReflector)
            {
                Type type = NetReflector.ToNative(classReflector);
                if (type == null)
                {
                    return false;
                }
                if (!type.IsGenericType)
                {
                    return false;
                }
                return _type == type.GetGenericTypeDefinition();
            }
        }

    }
}
