/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Convert.Conversions;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <summary>
	/// Represents a db4o file format version, assembles all the marshallers
	/// needed to read/write this specific version.
	/// </summary>
	/// <remarks>
	/// Represents a db4o file format version, assembles all the marshallers
	/// needed to read/write this specific version.
	/// A marshaller knows how to read/write certain types of values from/to its
	/// representation on disk for a given db4o file format version.
	/// Responsibilities are somewhat overlapping with TypeHandler's.
	/// </remarks>
	/// <exclude></exclude>
	public class MarshallerFamily
	{
		public class FamilyVersion
		{
			public const int PreMarshaller = 0;

			public const int Marshaller = 1;

			public const int BtreeFieldIndexes = 2;

			public const int ClassAspects = 3;
		}

		private static int CurrentVersion = MarshallerFamily.FamilyVersion.ClassAspects;

		public readonly ClassMarshaller _class;

		public readonly IFieldMarshaller _field;

		public readonly PrimitiveMarshaller _primitive;

		private readonly int _converterVersion;

		private readonly int _handlerVersion;

		private static readonly MarshallerFamily[] allVersions;

		static MarshallerFamily()
		{
			allVersions = new MarshallerFamily[HandlerRegistry.HandlerVersion + 1];
			allVersions[0] = new MarshallerFamily(0, 0, new ClassMarshaller0(), new FieldMarshaller0
				(), new PrimitiveMarshaller0());
			// LEGACY => before 5.4
			allVersions[1] = new MarshallerFamily(ClassIndexesToBTrees_5_5.Version, 1, new ClassMarshaller1
				(), new FieldMarshaller0(), new PrimitiveMarshaller1());
			allVersions[2] = new MarshallerFamily(FieldIndexesToBTrees_5_7.Version, 2, new ClassMarshaller2
				(), new FieldMarshaller1(), new PrimitiveMarshaller1());
			for (int i = 3; i < allVersions.Length; i++)
			{
				allVersions[i] = LatestFamily(i);
			}
		}

		public MarshallerFamily(int converterVersion, int handlerVersion, ClassMarshaller
			 classMarshaller, IFieldMarshaller fieldMarshaller, PrimitiveMarshaller primitiveMarshaller
			)
		{
			_converterVersion = converterVersion;
			_handlerVersion = handlerVersion;
			_class = classMarshaller;
			_class._family = this;
			_field = fieldMarshaller;
			_primitive = primitiveMarshaller;
			_primitive._family = this;
		}

		public static MarshallerFamily LatestFamily(int version)
		{
			return new MarshallerFamily(ClassAspects_7_4.Version, version, new ClassMarshaller2
				(), new FieldMarshaller2(), new PrimitiveMarshaller1());
		}

		public static MarshallerFamily Version(int n)
		{
			CheckIfVersionIsTooNew(n);
			return allVersions[n];
		}

		private static void CheckIfVersionIsTooNew(int n)
		{
			if (n > allVersions.Length)
			{
				throw new IncompatibleFileFormatException("Databasefile was created with a newer db4o version. Marshaller version: "
					 + n);
			}
		}

		public static MarshallerFamily Current()
		{
			if (CurrentVersion < MarshallerFamily.FamilyVersion.BtreeFieldIndexes)
			{
				throw new InvalidOperationException("Using old marshaller versions to write database files is not supported, source code has been removed."
					);
			}
			return Version(CurrentVersion);
		}

		public static MarshallerFamily ForConverterVersion(int n)
		{
			MarshallerFamily result = allVersions[0];
			for (int i = 1; i < allVersions.Length; i++)
			{
				if (allVersions[i]._converterVersion > n)
				{
					return result;
				}
				result = allVersions[i];
			}
			return result;
		}

		public virtual int HandlerVersion()
		{
			return _handlerVersion;
		}
	}
}
