/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using Db4objects.Drs;
using Db4objects.Drs.Db4o;

namespace Db4objects.Drs.Inside
{
	public class ReplicationReflector
	{
		private IInternalObjectContainer _container;

		private IReflector _reflector;

		public ReplicationReflector(IReplicationProvider providerA, IReplicationProvider 
			providerB, IReflector reflector)
		{
			if (reflector == null)
			{
				if ((_container = ContainerFrom(providerA)) != null)
				{
					return;
				}
				if ((_container = ContainerFrom(providerB)) != null)
				{
					return;
				}
			}
			GenericReflector genericReflector = new GenericReflector(null, reflector == null ? 
				Platform4.ReflectorForType(typeof(Db4objects.Drs.Inside.ReplicationReflector)) : 
				reflector);
			Platform4.RegisterCollections(genericReflector);
			_reflector = genericReflector;
		}

		public virtual object[] ArrayContents(object array)
		{
			int[] dim = ArrayReflector().Dimensions(array);
			object[] result = new object[Volume(dim)];
			ArrayReflector().Flatten(array, dim, 0, result, 0);
			//TODO Optimize add a visit(Visitor) method to ReflectArray or navigate the array to avoid copying all this stuff all the time.
			return result;
		}

		private int Volume(int[] dim)
		{
			int result = dim[0];
			for (int i = 1; i < dim.Length; i++)
			{
				result = result * dim[i];
			}
			return result;
		}

		public virtual IReflectClass ForObject(object obj)
		{
			return Reflector().ForObject(obj);
		}

		public virtual IReflectClass ForClass(Type clazz)
		{
			return Reflector().ForClass(clazz);
		}

		internal virtual IReflectClass GetComponentType(IReflectClass claxx)
		{
			return ArrayReflector().GetComponentType(claxx);
		}

		internal virtual int[] ArrayDimensions(object obj)
		{
			return ArrayReflector().Dimensions(obj);
		}

		public virtual object NewArrayInstance(IReflectClass componentType, int[] dimensions
			)
		{
			return ArrayReflector().NewInstance(componentType, dimensions);
		}

		public virtual int ArrayShape(object[] a_flat, int a_flatElement, object a_shaped
			, int[] a_dimensions, int a_currentDimension)
		{
			return ArrayReflector().Shape(a_flat, a_flatElement, a_shaped, a_dimensions, a_currentDimension
				);
		}

		public virtual bool IsValueType(IReflectClass clazz)
		{
			if (_container == null)
			{
				return clazz.IsSimple();
			}
			ClassMetadata classMetadata = _container.ClassMetadataForReflectClass(clazz);
			if (classMetadata == null)
			{
				return false;
			}
			return classMetadata.IsValueType();
		}

		private IInternalObjectContainer ContainerFrom(IReplicationProvider provider)
		{
			if (!(provider is IDb4oReplicationProvider))
			{
				return null;
			}
			IDb4oReplicationProvider db4oProvider = (IDb4oReplicationProvider)provider;
			IExtObjectContainer container = db4oProvider.GetObjectContainer();
			if (!(container is IInternalObjectContainer))
			{
				return null;
			}
			return (IInternalObjectContainer)container;
		}

		private IReflectArray ArrayReflector()
		{
			return Reflector().Array();
		}

		//		return _container.reflector().array();
		private IReflector Reflector()
		{
			return _container == null ? _reflector : _container.Reflector();
		}

		public virtual void CopyState(object to, object from)
		{
			IReflectClass fromClass = Reflector().ForObject(from);
			// FIXME: We need to get the classes parents fields copied too.
			foreach (IReflectField f in fromClass.GetDeclaredFields())
			{
				if (f.IsStatic())
				{
					continue;
				}
				f.Set(to, f.Get(from));
			}
		}
	}
}
