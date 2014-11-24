/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Metadata;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Metadata
{
	/// <exclude></exclude>
	public class HierarchyAnalyzer
	{
		public class Diff
		{
			private readonly Db4objects.Db4o.Internal.ClassMetadata _classMetadata;

			public Diff(Db4objects.Db4o.Internal.ClassMetadata classMetadata)
			{
				if (classMetadata == null)
				{
					throw new ArgumentNullException();
				}
				_classMetadata = classMetadata;
			}

			public override bool Equals(object obj)
			{
				if (GetType() != obj.GetType())
				{
					return false;
				}
				HierarchyAnalyzer.Diff other = (HierarchyAnalyzer.Diff)obj;
				return _classMetadata == other._classMetadata;
			}

			public override string ToString()
			{
				return ReflectPlatform.SimpleName(GetType()) + "(" + _classMetadata.GetName() + ")";
			}

			public virtual Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
			{
				return _classMetadata;
			}

			public virtual bool IsRemoved()
			{
				return false;
			}
		}

		public class Same : HierarchyAnalyzer.Diff
		{
			public Same(ClassMetadata classMetadata) : base(classMetadata)
			{
			}
		}

		public class Removed : HierarchyAnalyzer.Diff
		{
			public Removed(ClassMetadata classMetadata) : base(classMetadata)
			{
			}

			public override bool IsRemoved()
			{
				return true;
			}
		}

		private ClassMetadata _storedClass;

		private IReflectClass _runtimeClass;

		private readonly IReflectClass _objectClass;

		public HierarchyAnalyzer(ClassMetadata storedClass, IReflectClass runtimeClass)
		{
			if (storedClass == null || runtimeClass == null)
			{
				throw new ArgumentNullException();
			}
			_storedClass = storedClass;
			_runtimeClass = runtimeClass;
			_objectClass = runtimeClass.Reflector().ForClass(typeof(object));
		}

		public virtual IList Analyze()
		{
			IList ancestors = new ArrayList();
			ClassMetadata storedAncestor = _storedClass.GetAncestor();
			IReflectClass runtimeAncestor = _runtimeClass.GetSuperclass();
			while (storedAncestor != null)
			{
				if (runtimeAncestor == storedAncestor.ClassReflector())
				{
					ancestors.Add(new HierarchyAnalyzer.Same(storedAncestor));
				}
				else
				{
					do
					{
						ancestors.Add(new HierarchyAnalyzer.Removed(storedAncestor));
						storedAncestor = storedAncestor.GetAncestor();
						if (null == storedAncestor)
						{
							if (IsObject(runtimeAncestor))
							{
								return ancestors;
							}
							ThrowUnsupportedAdd(runtimeAncestor);
						}
						if (runtimeAncestor == storedAncestor.ClassReflector())
						{
							ancestors.Add(new HierarchyAnalyzer.Same(storedAncestor));
							break;
						}
					}
					while (storedAncestor != null);
				}
				storedAncestor = storedAncestor.GetAncestor();
				runtimeAncestor = runtimeAncestor.GetSuperclass();
			}
			if (runtimeAncestor != null && (!IsObject(runtimeAncestor)))
			{
				ThrowUnsupportedAdd(runtimeAncestor);
			}
			return ancestors;
		}

		private void ThrowUnsupportedAdd(IReflectClass runtimeAncestor)
		{
			throw new InvalidOperationException("Unsupported class hierarchy change. Class " 
				+ runtimeAncestor.GetName() + " was added to hierarchy of " + _runtimeClass.GetName
				());
		}

		private bool IsObject(IReflectClass clazz)
		{
			return _objectClass == clazz;
		}
	}
}
