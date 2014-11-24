/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class Config4Abstract
	{
		protected KeySpecHashtable4 _config;

		private static readonly KeySpec CascadeOnActivateKey = new KeySpec(TernaryBool.Unspecified
			);

		private static readonly KeySpec CascadeOnDeleteKey = new KeySpec(TernaryBool.Unspecified
			);

		private static readonly KeySpec CascadeOnUpdateKey = new KeySpec(TernaryBool.Unspecified
			);

		private static readonly KeySpec NameKey = new KeySpec(null);

		public Config4Abstract() : this(new KeySpecHashtable4(10))
		{
		}

		protected Config4Abstract(KeySpecHashtable4 config)
		{
			_config = (KeySpecHashtable4)config.DeepClone(this);
		}

		public virtual void CascadeOnActivate(bool flag)
		{
			PutThreeValued(CascadeOnActivateKey, flag);
		}

		public virtual void CascadeOnDelete(bool flag)
		{
			PutThreeValued(CascadeOnDeleteKey, flag);
		}

		public virtual void CascadeOnUpdate(bool flag)
		{
			PutThreeValued(CascadeOnUpdateKey, flag);
		}

		protected virtual void PutThreeValued(KeySpec spec, bool flag)
		{
			_config.Put(spec, TernaryBool.ForBoolean(flag));
		}

		protected virtual void PutThreeValuedInt(KeySpec spec, bool flag)
		{
			_config.Put(spec, flag ? 1 : -1);
		}

		public virtual TernaryBool CascadeOnActivate()
		{
			return Cascade(CascadeOnActivateKey);
		}

		public virtual TernaryBool CascadeOnDelete()
		{
			return Cascade(CascadeOnDeleteKey);
		}

		public virtual TernaryBool CascadeOnUpdate()
		{
			return Cascade(CascadeOnUpdateKey);
		}

		private TernaryBool Cascade(KeySpec spec)
		{
			return _config.GetAsTernaryBool(spec);
		}

		internal abstract string ClassName();

		/// <summary>Will raise an exception if argument class doesn't match this class - violates equals() contract in favor of failing fast.
		/// 	</summary>
		/// <remarks>Will raise an exception if argument class doesn't match this class - violates equals() contract in favor of failing fast.
		/// 	</remarks>
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (null == obj)
			{
				return false;
			}
			if (GetType() != obj.GetType())
			{
				Exceptions4.ShouldNeverHappen();
			}
			return GetName().Equals(((Db4objects.Db4o.Internal.Config4Abstract)obj).GetName()
				);
		}

		public override int GetHashCode()
		{
			return GetName().GetHashCode();
		}

		public virtual string GetName()
		{
			return _config.GetAsString(NameKey);
		}

		protected virtual void SetName(string name)
		{
			_config.Put(NameKey, name);
		}
	}
}
