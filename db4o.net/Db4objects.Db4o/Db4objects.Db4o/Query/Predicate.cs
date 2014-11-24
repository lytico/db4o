/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Query
{
	/// <summary>Base class for native queries.</summary>
	/// <remarks>
	/// Base class for native queries. See
	/// <see cref="Db4objects.Db4o.IObjectContainer.Query(Predicate)">Db4objects.Db4o.IObjectContainer.Query(Predicate)
	/// 	</see>
	/// <br /><br />
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.IObjectContainer.Query(Predicate)">Db4objects.Db4o.IObjectContainer.Query(Predicate)
	/// 	</seealso>
	[System.Serializable]
	public abstract class Predicate
	{
		/// <summary>public for implementation reasons, please ignore.</summary>
		/// <remarks>public for implementation reasons, please ignore.</remarks>
		public static readonly string PredicatemethodName = "match";

		private Type _extentType;

		[System.NonSerialized]
		private MethodInfo cachedFilterMethod = null;

		public Predicate() : this(null)
		{
		}

		public Predicate(Type extentType)
		{
			_extentType = extentType;
		}

		public virtual MethodInfo GetFilterMethod()
		{
			if (cachedFilterMethod != null)
			{
				return cachedFilterMethod;
			}
			MethodInfo[] methods = GetType().GetMethods();
			for (int methodIdx = 0; methodIdx < methods.Length; methodIdx++)
			{
				MethodInfo method = methods[methodIdx];
				if ((!method.Name.Equals(PredicatePlatform.PredicatemethodName)) || Sharpen.Runtime.GetParameterTypes
					(method).Length != 1)
				{
					continue;
				}
				cachedFilterMethod = method;
				string targetName = Sharpen.Runtime.GetParameterTypes(method)[0].FullName;
				if (!"java.lang.Object".Equals(targetName))
				{
					break;
				}
			}
			if (cachedFilterMethod == null)
			{
				throw new ArgumentException("Invalid predicate.");
			}
			return cachedFilterMethod;
		}

		/// <summary>public for implementation reasons, please ignore.</summary>
		/// <remarks>public for implementation reasons, please ignore.</remarks>
		public virtual Type ExtentType()
		{
			if (_extentType == null)
			{
				_extentType = FilterParameterType();
			}
			return _extentType;
		}

		private Type FilterParameterType()
		{
			return (Type)Sharpen.Runtime.GetParameterTypes(GetFilterMethod())[0];
		}

		/// <summary>public for implementation reasons, please ignore.</summary>
		/// <remarks>public for implementation reasons, please ignore.</remarks>
		public virtual bool AppliesTo(object candidate)
		{
			try
			{
				MethodInfo filterMethod = GetFilterMethod();
				Platform4.SetAccessible(filterMethod);
				object ret = filterMethod.Invoke(this, new object[] { candidate });
				return ((bool)ret);
			}
			catch (Exception)
			{
				// TODO: log this exception somewhere?
				//			e.printStackTrace();
				return false;
			}
		}
	}
}
