/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// a simple Alias for a single Class or Type, using equals on
	/// the names in the resolve method.
	/// </summary>
	/// <remarks>
	/// a simple Alias for a single Class or Type, using equals on
	/// the names in the resolve method.
	/// <br /><br />See
	/// <see cref="IAlias">IAlias</see>
	/// for concrete examples.
	/// </remarks>
	public class TypeAlias : IAlias
	{
		private readonly string _storedType;

		private readonly string _runtimeType;

		/// <summary>
		/// pass the stored name as the first
		/// parameter and the desired runtime name as the second parameter.
		/// </summary>
		/// <remarks>
		/// pass the stored name as the first
		/// parameter and the desired runtime name as the second parameter.
		/// </remarks>
		public TypeAlias(string storedType, string runtimeType)
		{
			if (null == storedType || null == runtimeType)
			{
				throw new ArgumentException();
			}
			_storedType = storedType;
			_runtimeType = runtimeType;
		}

		public TypeAlias(Type storedClass, Type runtimeClass) : this(ReflectPlatform.FullyQualifiedName
			(storedClass), ReflectPlatform.FullyQualifiedName(runtimeClass))
		{
		}

		/// <summary>returns the stored type name if the alias was written for the passed runtime type name
		/// 	</summary>
		public virtual string ResolveRuntimeName(string runtimeTypeName)
		{
			return _runtimeType.Equals(runtimeTypeName) ? _storedType : null;
		}

		/// <summary>returns the runtime type name if the alias was written for the passed stored type name
		/// 	</summary>
		public virtual string ResolveStoredName(string storedTypeName)
		{
			return _storedType.Equals(storedTypeName) ? _runtimeType : null;
		}
	}
}
