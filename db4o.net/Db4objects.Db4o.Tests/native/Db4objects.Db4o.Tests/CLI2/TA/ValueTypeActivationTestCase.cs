/* Copyright (C) 2004-2007   Versant Inc.   http://www.db4o.com */
using Db4objects.Db4o.Config;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Tests.CLI2.TA
{
    using StringIntP = Pair<string, int>;
	using IntC = Container<int>;
	using IntCStringIntP = Pair<Container<int>, Pair<string, int>>;
	using IntCStringIntC = Container<Pair<Container<int>, Pair<string, int>>>;
	using IntCStringIntIntP = Pair<Container<Pair<Container<int>, Pair<string, int>>>, int>;
	using IntCStringIntIntPC = Container<Pair<Container<Pair<Container<int>, Pair<string, int>>>, int>>;

	public class ValueTypeActivationTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentActivationSupport());
		}

		// the object graph goes like this:
		// container
		//	pair
		//		first: container
		//			pair:
		//				first: container(21)
		//				second: pair
		//					first: "foo"
		//					second: 11
		//		second: 42

		protected override void Store()
		{
			Store(
				new IntCStringIntIntPC(
					"root",
					new IntCStringIntIntP(
						new IntCStringIntC(
							"child",
							new IntCStringIntP(
								new IntC("grandchild", 21),
								new StringIntP("foo", 11)
							)
						),
						42
					)
				)
			);
		}

		public void TestDepth0()
		{
			IntCStringIntIntPC root = GetRoot();
			ValueTypeActivationTestCase.NotActivated(root);
		}

		public void TestDepth1()
		{
			IntCStringIntIntPC root = GetRoot();
			Assert.AreEqual("root", root.Name, "root.Name");
			Assert.IsNotNull(root.Value.First, "root.Value.First");
			Assert.AreEqual(42, root.Value.Second, "root.Value.Second");

			ValueTypeActivationTestCase.NotActivated(root.Value.First);
		}

		public void TestDepthN()
		{
			IntCStringIntIntPC root = GetRoot();
			ValueTypeActivationTestCase.NotActivated(root.Value.First.Value.First);
			Assert.AreEqual(21, root.Value.First.Value.First.Value);
			Assert.AreEqual(new StringIntP("foo", 11), root.Value.First.Value.Second);
			
		}

		private IntCStringIntIntPC GetRoot()
		{
			return (IntCStringIntIntPC)NewQuery(typeof(IntCStringIntIntPC)).Execute().Next();
		}

		private static void NotActivated<T>(Container<T> container) where T : struct
		{
			Assert.IsNull(container.PassThroughName, "depth(0) shouldn't activate ref member");
			Assert.AreEqual(default(T), container.PassThroughValue, "depth(0) shouldn't activate nested value types");
		}
	}
}
