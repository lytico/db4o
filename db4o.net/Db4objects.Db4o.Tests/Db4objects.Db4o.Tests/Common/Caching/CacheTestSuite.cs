/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Internal.Caching;
using Db4objects.Db4o.Tests.Common.Caching;

namespace Db4objects.Db4o.Tests.Common.Caching
{
	public class CacheTestSuite : FixtureTestSuiteDescription
	{
		public CacheTestSuite()
		{
			{
				// initializer
				FixtureProviders(new IFixtureProvider[] { new SubjectFixtureProvider(new IDeferred4
					[] { new _IDeferred4_12(), new _IDeferred4_16(), new _IDeferred4_20(), new _IDeferred4_25
					() }) });
				// The following doesn' sharpen. Ignore for now.
				//			,new Deferred4() {
				//				public Object value() {
				//					return new Cache4() {
				//						
				//						private final Cache4 _delegate = CacheFactory.newLRULongCache(10); 
				//	
				//						public Object produce(Object key, final Function4 producer, Procedure4 finalizer) {
				//							Function4 delegateProducer = new Function4<Long, Object>() {
				//								public Object apply(Long arg) {
				//									return producer.apply(arg.intValue());
				//								}
				//							};
				//							return _delegate.produce(((Integer)key).longValue(), delegateProducer, finalizer);
				//						}
				//	
				//						public Iterator iterator() {
				//							return _delegate.iterator();
				//						}
				//					};
				//				}
				//			}
				TestUnits(new Type[] { typeof(CacheTestUnit) });
			}
		}

		private sealed class _IDeferred4_12 : IDeferred4
		{
			public _IDeferred4_12()
			{
			}

			public object Value()
			{
				return CacheFactory.NewLRUCache(10);
			}
		}

		private sealed class _IDeferred4_16 : IDeferred4
		{
			public _IDeferred4_16()
			{
			}

			public object Value()
			{
				return CacheFactory.New2QCache(10);
			}
		}

		private sealed class _IDeferred4_20 : IDeferred4
		{
			public _IDeferred4_20()
			{
			}

			public object Value()
			{
				return CacheFactory.New2QXCache(10);
			}
		}

		private sealed class _IDeferred4_25 : IDeferred4
		{
			public _IDeferred4_25()
			{
			}

			public object Value()
			{
				return CacheFactory.NewLRUIntCache(10);
			}
		}
	}
}
