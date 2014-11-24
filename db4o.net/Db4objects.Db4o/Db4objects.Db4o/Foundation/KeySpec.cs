/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class KeySpec
	{
		public interface IDeferred
		{
			object Evaluate();
		}

		private object _defaultValue;

		public KeySpec(byte defaultValue)
		{
			_defaultValue = defaultValue;
		}

		public KeySpec(int defaultValue)
		{
			_defaultValue = defaultValue;
		}

		public KeySpec(bool defaultValue)
		{
			_defaultValue = defaultValue;
		}

		public KeySpec(object defaultValue)
		{
			_defaultValue = defaultValue;
		}

		public virtual object DefaultValue()
		{
			if (_defaultValue is KeySpec.IDeferred)
			{
				return ((KeySpec.IDeferred)_defaultValue).Evaluate();
			}
			return _defaultValue;
		}
	}
}
