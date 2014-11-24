/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class TreeIntObject : TreeInt
	{
		public object _object;

		public TreeIntObject(int a_key) : base(a_key)
		{
		}

		public TreeIntObject(int a_key, object a_object) : base(a_key)
		{
			_object = a_object;
		}

		public override object ShallowClone()
		{
			return ShallowCloneInternal(new Db4objects.Db4o.Internal.TreeIntObject(_key));
		}

		protected override Tree ShallowCloneInternal(Tree tree)
		{
			Db4objects.Db4o.Internal.TreeIntObject tio = (Db4objects.Db4o.Internal.TreeIntObject
				)base.ShallowCloneInternal(tree);
			tio._object = _object;
			return tio;
		}

		public virtual object GetObject()
		{
			return _object;
		}

		public virtual void SetObject(object obj)
		{
			_object = obj;
		}

		public override object Read(ByteArrayBuffer a_bytes)
		{
			int key = a_bytes.ReadInt();
			object obj = null;
			if (_object is TreeInt)
			{
				obj = new TreeReader(a_bytes, (IReadable)_object).Read();
			}
			else
			{
				obj = ((IReadable)_object).Read(a_bytes);
			}
			return new Db4objects.Db4o.Internal.TreeIntObject(key, obj);
		}

		public override void Write(ByteArrayBuffer a_writer)
		{
			a_writer.WriteInt(_key);
			if (_object == null)
			{
				a_writer.WriteInt(0);
			}
			else
			{
				if (_object is TreeInt)
				{
					TreeInt.Write(a_writer, (TreeInt)_object);
				}
				else
				{
					((IReadWriteable)_object).Write(a_writer);
				}
			}
		}

		public override int OwnLength()
		{
			if (_object == null)
			{
				return Const4.IntLength * 2;
			}
			return Const4.IntLength + ((IReadable)_object).MarshalledLength();
		}

		internal override bool VariableLength()
		{
			return true;
		}

		public static Db4objects.Db4o.Internal.TreeIntObject Add(Db4objects.Db4o.Internal.TreeIntObject
			 tree, int key, object value)
		{
			return ((Db4objects.Db4o.Internal.TreeIntObject)Tree.Add(tree, new Db4objects.Db4o.Internal.TreeIntObject
				(key, value)));
		}
	}
}
