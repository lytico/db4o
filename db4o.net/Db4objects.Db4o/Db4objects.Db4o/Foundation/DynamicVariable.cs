/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>A dynamic variable is a value associated to a specific thread and scope.
	/// 	</summary>
	/// <remarks>
	/// A dynamic variable is a value associated to a specific thread and scope.
	/// The value is brought into scope with the
	/// <see cref="With(object, IClosure4)">With(object, IClosure4)</see>
	/// method.
	/// </remarks>
	public class DynamicVariable
	{
		public static DynamicVariable NewInstance()
		{
			return new DynamicVariable();
		}

		private readonly ThreadLocal _value = new ThreadLocal();

		public virtual object Value
		{
			get
			{
				object value = _value.Get();
				return value == null ? DefaultValue() : value;
			}
			set
			{
				_value.Set(value);
			}
		}

		protected virtual object DefaultValue()
		{
			return null;
		}

		public virtual object With(object value, IClosure4 block)
		{
			object previous = _value.Get();
			_value.Set(value);
			try
			{
				return block.Run();
			}
			finally
			{
				_value.Set(previous);
			}
		}

		public virtual void With(object value, IRunnable block)
		{
			object previous = _value.Get();
			_value.Set(value);
			try
			{
				block.Run();
			}
			finally
			{
				_value.Set(previous);
			}
		}
	}
}
