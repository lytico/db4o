
using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Caching;
using Db4objects.Db4o.Linq.Caching;
using Db4oUnit;
using Db4oUnit.Mocking;

namespace Db4objects.Db4o.Linq.Tests.Caching
{
	public class GenericCacheFactoryTestCase : ITestCase
	{
		public class Cache4Mock : ICache4
		{
			private readonly MethodCallRecorder _recorder = new MethodCallRecorder();

			public object Produce(object key, IFunction4 producer, IProcedure4 onDiscard)
			{
				Record("Produce", key);
				return producer.Apply(key);
			}

			public void Verify(params MethodCall[] expectedCalls)
			{
				_recorder.Verify(expectedCalls);
			}

			private void Record(string methodName, params object[] args)
			{
				_recorder.Record(new MethodCall(methodName, args));
			}

			public void Reset()
			{
				_recorder.Reset();
			}

			#region Implementation of IEnumerable
			public IEnumerator GetEnumerator()
			{
				throw new System.NotImplementedException();
			}
			#endregion
		}

		public void TestProduce()
		{
			var cache4Mock = new Cache4Mock();
			var subject = CacheFactory<string, int>.For(cache4Mock);

			cache4Mock.Verify();
			Assert.AreEqual(42, subject.Produce("42", key => 42));
			cache4Mock.Verify(new MethodCall("Produce", new object[] { "42" }));

			cache4Mock.Reset();
			Assert.AreEqual(-1, subject.Produce("42", key => -1));
			cache4Mock.Verify(new MethodCall("Produce", new object[] { "42" }));

		}

		public void TestProduceWithEqualityComparer()
		{
			var cache4 = CacheFactory.NewLRUCache(2);
			var subject = CacheFactory<string, int>.For(cache4, StringComparer.CurrentCultureIgnoreCase);

			Assert.AreEqual(42, subject.Produce("foo", key=>42));
			Assert.AreEqual(42, subject.Produce("FOO", key=>-1));

			Iterator4Assert.AreEqual(new object[] { 42 }, cache4);

		}
	}
}
