/* Copyright (C) 2004-2007   Versant Inc.   http://www.db4o.com */
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.CLI2.TA
{
	public class Named : ActivatableImpl
	{
		public string _name;

		public Named(string name)
		{
			_name = name;
		}

		/// <summary>
		/// Activatable based implementation. Activates the
		/// object before field access.
		/// </summary>
		public string Name
		{
			get
			{
				Activate(ActivationPurpose.Read);
				return _name;
			}
		}

		public string PassThroughName
		{
			get { return _name; }
		}
	}

	public class NullableContainer<T> : Named where T : struct
	{
		public T? _value;

		public NullableContainer(string name, T? value) : base(name)
		{
			_value = value;
		}

		/// <summary>
		/// Activatable based implementation. Activates the
		/// object before field access.
		/// </summary>
		public T? Value
		{
			get
			{
				Activate(ActivationPurpose.Read);
				return _value;
			}
		}

		/// <summary>
		/// Bypass activation and access the field directly.
		/// </summary>
		public T? PassThroughValue
		{
			get { return _value; }
		}
	}

	public class Container<T> : Named where T : struct 
	{
		public T _value;

		public Container(string name, T value) : base(name)
		{	
			_value = value;
		}

		/// <summary>
		/// Activatable based implementation. Activates the
		/// object before field access.
		/// </summary>
		public T Value
		{
			get
			{
				Activate(ActivationPurpose.Read);
				return _value;
			}
		}

		/// <summary>
		/// Bypass activation and access the field directly.
		/// </summary>
		public T PassThroughValue
		{
			get { return _value; }
		}
	}
}