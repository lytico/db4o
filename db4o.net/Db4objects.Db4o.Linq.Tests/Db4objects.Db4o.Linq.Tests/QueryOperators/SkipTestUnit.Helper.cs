/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
using Db4objects.Db4o.Config;
using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests.QueryOperators
{
	public partial class SkipTestUnit
	{
		public class Item
		{
			public string Name;
			public int Id;

			public override bool Equals(object obj)
			{
				Item other = obj as Item;
				if (other == null) return false;

				return other.Id == Id && other.Name == Name;
			}

			public override int GetHashCode()
			{
				return Id.GetHashCode() ^ Name.GetHashCode();
			}

			public override string ToString()
			{
				return string.Format("Item({0}, {1})", Id, Name);
			}
		}

		protected override void Store()
		{
			foreach (var item in Items())
			{
				Store(item);
			}
		}

		private void AssertActivationCount(int expectedActivationCount)
		{
			Assert.AreEqual(expectedActivationCount, _activationCount);
		}

		private void RegisterForActivationEvents()
		{
			EventRegistryFor(Db()).Activated += delegate
			{
				_activationCount++;
			};
		}

		private static QueryEvaluationMode EvaluationModeToTest()
		{
			return ((LabeledQueryEvaluationMode) SkipTestSuiteVariables.EvaluationMode.Value).Mode;
		}

		private int _activationCount;
	}
}
