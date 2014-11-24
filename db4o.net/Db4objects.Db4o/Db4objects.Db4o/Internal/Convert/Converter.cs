/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Internal.Convert;
using Db4objects.Db4o.Internal.Convert.Conversions;

namespace Db4objects.Db4o.Internal.Convert
{
	/// <exclude></exclude>
	public class Converter
	{
		public const int Version = VersionNumberToCommitTimestamp_8_0.Version;

		public static bool Convert(ConversionStage stage)
		{
			if (!NeedsConversion(stage.ConverterVersion()))
			{
				return false;
			}
			return Instance().RunConversions(stage);
		}

		private static Db4objects.Db4o.Internal.Convert.Converter _instance;

		private IDictionary _conversions;

		private int _minimumVersion = int.MaxValue;

		private Converter()
		{
			_conversions = new Hashtable();
			// TODO: There probably will be Java and .NET conversions
			//       Create Platform4.registerConversions() method ann
			//       call from here when needed.
			CommonConversions.Register(this);
		}

		public static Db4objects.Db4o.Internal.Convert.Converter Instance()
		{
			if (_instance == null)
			{
				_instance = new Db4objects.Db4o.Internal.Convert.Converter();
			}
			return _instance;
		}

		public virtual Conversion ConversionFor(int version)
		{
			return ((Conversion)_conversions[version]);
		}

		private static bool NeedsConversion(int converterVersion)
		{
			return converterVersion < Version;
		}

		public virtual void Register(int introducedVersion, Conversion conversion)
		{
			if (_conversions.Contains(introducedVersion))
			{
				throw new InvalidOperationException();
			}
			if (introducedVersion < _minimumVersion)
			{
				_minimumVersion = introducedVersion;
			}
			_conversions[introducedVersion] = conversion;
		}

		public virtual bool RunConversions(ConversionStage stage)
		{
			int startingVersion = Math.Max(stage.ConverterVersion() + 1, _minimumVersion);
			for (int version = startingVersion; version <= Version; version++)
			{
				Conversion conversion = ConversionFor(version);
				if (conversion == null)
				{
					throw new InvalidOperationException("Could not find a conversion for version " + 
						version);
				}
				stage.Accept(conversion);
			}
			return true;
		}
	}
}
