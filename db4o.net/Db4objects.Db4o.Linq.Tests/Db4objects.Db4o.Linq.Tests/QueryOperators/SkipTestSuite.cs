/* Copyright (C) 2011 Versant Inc.  http://www.db4o.com */
using System;
using Db4objects.Db4o.Config;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Linq.Tests.QueryOperators
{
	public class SkipTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override Type[] TestUnits()
		{
			return new[]
			       	{
			       		typeof (SkipTestUnit),
			       	};
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[]
			       	{
			       		new Db4oFixtureProvider(),

						new SimpleFixtureProvider(
									SkipTestSuiteVariables.EvaluationMode, 
									new [] 
									{
										new LabeledQueryEvaluationMode(QueryEvaluationMode.Immediate),
										new LabeledQueryEvaluationMode(QueryEvaluationMode.Lazy),
										new LabeledQueryEvaluationMode(QueryEvaluationMode.Snapshot)
									})
					};
		}
	}

	public class LabeledQueryEvaluationMode : ILabeled
	{
		public LabeledQueryEvaluationMode(QueryEvaluationMode mode)
		{
			_mode = mode;
		}

		public QueryEvaluationMode Mode
		{
			get { return _mode; }
		}

		public string Label()
		{
			return _mode.ToString();
		}

		private QueryEvaluationMode _mode;
	}
}
