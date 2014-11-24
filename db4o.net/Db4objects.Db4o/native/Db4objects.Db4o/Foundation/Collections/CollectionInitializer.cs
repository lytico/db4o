/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;

#if CF
using System.Reflection;
#endif

namespace Db4objects.Db4o.Foundation.Collections
{
	public interface ICollectionInitializer
	{
		void Clear();
		void Add(object o);
		void FinishAdding();
	    int Count();
	}

	public sealed class CollectionInitializer
	{
		private static readonly Dictionary<Type, Type> _initializerByType = new Dictionary<Type, Type>();

		static CollectionInitializer()
		{
			_initializerByType[typeof (ICollection<>)] = typeof (CollectionInitializerImpl<>);
			_initializerByType[typeof(Stack<>)] = typeof(StackInitializer<>);
			_initializerByType[typeof(Queue<>)] = typeof(QueueInitializer<>);
#if NET_3_5 && !CF && !SILVERLIGHT
		    _initializerByType[typeof (HashSet<>)] = typeof (HashSetInitializer<>);
#endif
		}

		public static ICollectionInitializer For(object destination)
		{
			if (IsNonGenericList(destination))
			{
			    return new ListInitializer((IList)destination);
			}

			return InitializerFor(destination);
		}

        private static ICollectionInitializer InitializerFor(object destination)
		{
			var destinationType = destination.GetType();
			if (!destinationType.IsGenericType)
			{
				throw new ArgumentException("Unknown collection: " + destination);
			}

			Type collElemType;
			var containerType = GenericContainerTypeFor(destination, out collElemType);
			if (containerType != null)
			{
				return GetInitializer(destination, _initializerByType[containerType], collElemType);
			}

			throw new ArgumentException("Unknown collection: " + destination);
		}

        private static Type GenericContainerTypeFor(object destination, out Type collElemType)
		{
			var type = destination.GetType();
			var genArgs = type.GetGenericArguments();
			var containerType = type.GetGenericTypeDefinition();
			
			var collType = ResolveCollectionAndElementType(containerType, genArgs, out collElemType);
			if (collType != null) return collType;

			while (containerType != null && !_initializerByType.ContainsKey(containerType))
			{
				foreach (var interfaceType in containerType.GetInterfaces())
				{
					collType = ResolveCollectionAndElementType(interfaceType, genArgs, out collElemType);
					if (collType != null) return collType;
				}

				containerType = containerType.BaseType;
			}

			return containerType;
		}

        private static Type ResolveCollectionAndElementType(Type interfaceType, IList<Type> genArgs, out Type collElemType)
		{
			collElemType = null;

			if (!interfaceType.IsGenericType)
			{
				return null;
			}

            var genericInterfaceType = interfaceType.GetGenericTypeDefinition();
			if (_initializerByType.ContainsKey(genericInterfaceType))
			{
				var elementType = interfaceType.GetGenericArguments()[0];
				if (!elementType.IsGenericParameter)
				{
					throw new Exception("Could not deduce generic type argument for collection ");
                }

#if CF_3_5
                if (genArgs.Count != 1)
                {
                    throw new NotSupportedException("Collection type not supported: " + interfaceType);
                }
                collElemType = genArgs[0];

                if (collElemType.IsGenericParameter)
                {
                    throw new NotSupportedException("Collection type not supported: " + interfaceType);
                }
#else
				collElemType = genArgs[elementType.GenericParameterPosition];
#endif
				return genericInterfaceType;
			}

			return null;
		}

		private static ICollectionInitializer GetInitializer(object destination, Type initializerType, Type collElementType)
		{
			ICollectionInitializer initializer = null;
			if (collElementType != null)
			{
				Type genericProtocolType = initializerType.MakeGenericType(collElementType);
				initializer = InstantiateInitializer(destination, genericProtocolType);
			}
			return initializer;
		}

		private static bool IsNonGenericList(object destination)
		{
			return !destination.GetType().IsGenericType && destination is IList;
		}

		private static ICollectionInitializer InstantiateInitializer(object destination, Type genericProtocolType)
	    {
#if !CF
            return (ICollectionInitializer) Activator.CreateInstance(genericProtocolType, destination);
#else
	        ConstructorInfo constructor = genericProtocolType.GetConstructors()[0];
	        return (ICollectionInitializer) constructor.Invoke(new object[] {destination});
#endif
	    }

		private sealed class ListInitializer : ICollectionInitializer
		{
			private readonly IList _list;

			public ListInitializer(IList list)
			{
				_list = list;
			}

			public void Clear()
			{
				_list.Clear();
			}

			public void Add(object o)
			{
				_list.Add(o);
			}

            public int Count()
            {
                return _list.Count;
            }

			public void FinishAdding()
			{
			}
		}

		private sealed class CollectionInitializerImpl<T> : ICollectionInitializer
		{
			private readonly ICollection<T> _collection;

			public CollectionInitializerImpl(ICollection<T> collection)
			{
				_collection = collection;
			}

			public void Clear()
			{
				_collection.Clear();
			}

            public int Count()
            {
                return _collection.Count;
            }

			public void Add(object o)
			{
				_collection.Add((T)o);
			}

			public void FinishAdding()
			{
			}
		}

		private sealed class StackInitializer<T> : ICollectionInitializer
		{
			private readonly Stack<T> _stack;
			private readonly Stack<T> _tempStack;

			public StackInitializer(Stack<T> stack)
			{
				_stack= stack;
				_tempStack = new Stack<T>();
			}

			public void Clear()
			{
				_tempStack.Clear();
				_stack.Clear();
			}

            public int Count()
            {
                return _stack.Count;
            }

			public void Add(object o)
			{
				_tempStack.Push((T) o);
			}

			public void FinishAdding()
			{
				foreach(T item in _tempStack)
				{
					_stack.Push(item);
				}

				_tempStack.Clear();
			}
		}

		private sealed class QueueInitializer<T> : ICollectionInitializer
		{
			private readonly Queue<T> _queue;

			public QueueInitializer(Queue<T> queue)
			{
				_queue = queue;
			}

			public void Clear()
			{
				_queue.Clear();
			}

            public int Count()
            {
                return _queue.Count;
            }

			public void Add(object o)
			{
				_queue.Enqueue((T) o);
			}

			public void FinishAdding()
			{
			}
		}

#if NET_3_5 && !CF && !SILVERLIGHT
        private sealed class HashSetInitializer<T> : ICollectionInitializer
        {
            private readonly HashSet<T> _hashSet;

            public HashSetInitializer(HashSet<T> stack)
            {
                _hashSet = stack;
            }

            public void Clear()
            {
                _hashSet.Clear();
            }

            public void Add(object o)
            {
                _hashSet.Add((T)o);
            }

            public int Count()
            {
                return _hashSet.Count;
            }

            public void FinishAdding()
            {
            }
        }
#endif

	}
}
