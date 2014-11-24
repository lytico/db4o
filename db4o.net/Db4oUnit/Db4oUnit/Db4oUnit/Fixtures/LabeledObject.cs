/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Fixtures;

namespace Db4oUnit.Fixtures
{
	public class LabeledObject : ILabeled
	{
		private readonly object _value;

		private readonly string _label;

		public LabeledObject(object value, string label)
		{
			_value = value;
			_label = label;
		}

		public LabeledObject(object value) : this(value, null)
		{
		}

		public virtual string Label()
		{
			if (_label == null)
			{
				return _value.ToString();
			}
			return _label;
		}

		public virtual object Value()
		{
			return _value;
		}

		public static Db4oUnit.Fixtures.LabeledObject[] ForObjects(object[] values)
		{
			Db4oUnit.Fixtures.LabeledObject[] labeledObjects = new Db4oUnit.Fixtures.LabeledObject
				[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				labeledObjects[i] = new Db4oUnit.Fixtures.LabeledObject(values[i]);
			}
			return labeledObjects;
		}
	}
}
