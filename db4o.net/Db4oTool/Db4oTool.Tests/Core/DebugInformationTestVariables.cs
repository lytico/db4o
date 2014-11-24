/* Copyright (C) 2004 - 2010 Versant Inc.  http://www.db4o.com */
using Db4oUnit.Fixtures;

namespace Db4oTool.Tests.Core
{
	sealed class DebugInformationTestVariables
	{
		public static readonly FixtureVariable SourceAvailable = new FixtureVariable("Source");
		public static readonly FixtureVariable DebugSymbolsAvailable = new FixtureVariable("DebugSymbols");

		public static readonly IFixtureProvider SourceFixtureProvider = new SimpleFixtureProvider(SourceAvailable, 
																					new object[]
																						{
																							LabeledBool.Create(true, "Available"), 
																							LabeledBool.Create(false, "Unavailable") 
																						});

		public static readonly IFixtureProvider DebugSymbolsFixtureProvider = new SimpleFixtureProvider(DebugSymbolsAvailable, 
																					new object[]
																						{
																							LabeledBool.Create(true, "Available"), 
																							LabeledBool.Create(false, "Unavailable") 
																						});

		public static bool TestWithSourceAvailable()
		{
			return (LabeledBool) SourceAvailable.Value;
		}

		public static bool TestWithDebugSymbolsAvailable()
		{
			return (LabeledBool) DebugSymbolsAvailable.Value;
		}
	}

	sealed class LabeledBool : ILabeled
	{
		public static LabeledBool Create(bool value, string label)
		{
			return new LabeledBool(value, label);
		}

		public string Label()
		{
			return _label;
		}
		
		public static implicit operator bool(LabeledBool source)
		{
			return source._value;
		}

		private LabeledBool(bool value, string label)
		{
			_value = value;
			_label = label;
		}
	
		private readonly string _label;
		private readonly bool _value;
	}
}
