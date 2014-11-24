/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Reflect.Generic;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <summary>
	/// db4o provides GenericReflector as a wrapper around specific
	/// reflector (delegate).
	/// </summary>
	/// <remarks>
	/// db4o provides GenericReflector as a wrapper around specific
	/// reflector (delegate). GenericReflector is set when an
	/// ObjectContainer is opened. All subsequent reflector
	/// calls are routed through this interface.<br /><br />
	/// An instance of GenericReflector can be obtained through
	/// <see cref="Db4objects.Db4o.Ext.IExtObjectContainer.Reflector()">Db4objects.Db4o.Ext.IExtObjectContainer.Reflector()
	/// 	</see>
	/// .<br /><br />
	/// GenericReflector keeps list of known classes in memory.
	/// When the GenericReflector is called, it first checks its list of
	/// known classes. If the class cannot be found, the task is
	/// transferred to the delegate reflector. If the delegate fails as
	/// well, generic objects are created, which hold simulated
	/// "field values" in an array of objects.<br /><br />
	/// Generic reflector makes possible the following usecases:<ul>
	/// <li>running a db4o server without deploying application classes;</li>
	/// <li>running db4o on Java dialects without reflection (J2ME CLDC, MIDP);</li>
	/// <li>easier access to stored objects where classes or fields are not available;</li>
	/// <li>running refactorings in the reflector;</li>
	/// <li>building interfaces to db4o from any programming language.</li></ul>
	/// <br /><br />
	/// One of the live usecases is ObjectManager, which uses GenericReflector
	/// to read C# objects from Java.
	/// </remarks>
	public class GenericReflector : IReflector, IDeepClone
	{
		private KnownClassesRepository _repository;

		private IReflector _delegate;

		private GenericArrayReflector _array;

		private Collection4 _collectionPredicates = new Collection4();

		private readonly Hashtable4 _classByClass = new Hashtable4();

		private Transaction _trans;

		private ObjectContainerBase _stream;

		/// <summary>Creates an instance of GenericReflector</summary>
		/// <param name="trans">transaction</param>
		/// <param name="delegateReflector">
		/// delegate reflector,
		/// providing specific reflector functionality. For example
		/// </param>
		public GenericReflector(Transaction trans, IReflector delegateReflector)
		{
			// todo: Why have this when there is already the _repository by name? Redundant
			_repository = new KnownClassesRepository(new GenericClassBuilder(this, delegateReflector
				));
			SetTransaction(trans);
			_delegate = delegateReflector;
			if (_delegate != null)
			{
				_delegate.SetParent(this);
			}
		}

		public GenericReflector(IReflector delegateReflector) : this(null, delegateReflector
			)
		{
		}

		/// <summary>Creates a clone of provided object</summary>
		/// <param name="obj">object to copy</param>
		/// <returns>copy of the submitted object</returns>
		public virtual object DeepClone(object obj)
		{
			Db4objects.Db4o.Reflect.Generic.GenericReflector myClone = new Db4objects.Db4o.Reflect.Generic.GenericReflector
				(null, (IReflector)_delegate.DeepClone(this));
			myClone._collectionPredicates = (Collection4)_collectionPredicates.DeepClone(myClone
				);
			// Interesting, adding the following messes things up.
			// Keep the code, since it may make sense to carry the
			// global reflectors into a running db4o session.
			//        Iterator4 i = _classes.iterator();
			//        while(i.hasNext()){
			//            GenericClass clazz = (GenericClass)i.next();
			//            clazz = (GenericClass)clazz.deepClone(myClone);
			//            myClone._classByName.put(clazz.getName(), clazz);
			//            myClone._classes.add(clazz);
			//        }
			return myClone;
		}

		internal virtual ObjectContainerBase GetStream()
		{
			return _stream;
		}

		/// <summary>If there is a transaction assosiated with the current refector.</summary>
		/// <remarks>If there is a transaction assosiated with the current refector.</remarks>
		/// <returns>true if there is a transaction assosiated with the current refector.</returns>
		public virtual bool HasTransaction()
		{
			return _trans != null;
		}

		/// <summary>Associated a transaction with the current reflector.</summary>
		/// <remarks>Associated a transaction with the current reflector.</remarks>
		/// <param name="trans"></param>
		public virtual void SetTransaction(Transaction trans)
		{
			if (trans != null)
			{
				_trans = trans;
				_stream = trans.Container();
			}
			_repository.SetTransaction(trans);
		}

		/// <returns>generic reflect array instance.</returns>
		public virtual IReflectArray Array()
		{
			if (_array == null)
			{
				_array = new GenericArrayReflector(this);
			}
			return _array;
		}

		internal virtual Db4objects.Db4o.Reflect.Generic.GenericClass EnsureDelegate(IReflectClass
			 clazz)
		{
			if (clazz == null)
			{
				return null;
			}
			Db4objects.Db4o.Reflect.Generic.GenericClass claxx = (Db4objects.Db4o.Reflect.Generic.GenericClass
				)_repository.LookupByName(clazz.GetName());
			if (claxx == null)
			{
				//  We don't have to worry about the superclass, it can be null
				//  because handling is delegated anyway
				claxx = GenericClass(clazz);
				_repository.Register(claxx);
			}
			return claxx;
		}

		private Db4objects.Db4o.Reflect.Generic.GenericClass GenericClass(IReflectClass clazz
			)
		{
			Db4objects.Db4o.Reflect.Generic.GenericClass ret;
			string name = clazz.GetName();
			if (name.Equals(ReflectPlatform.FullyQualifiedName(typeof(GenericArray))))
			{
				// special case, comparing name because can't compare class == class directly with ReflectClass
				ret = new GenericArrayClass(this, clazz, name, null);
			}
			else
			{
				ret = new Db4objects.Db4o.Reflect.Generic.GenericClass(this, clazz, name, null);
			}
			return ret;
		}

		/// <summary>Returns a ReflectClass instance for the specified class</summary>
		/// <param name="clazz">class</param>
		/// <returns>a ReflectClass instance for the specified class</returns>
		/// <seealso cref="Db4objects.Db4o.Reflect.IReflectClass">Db4objects.Db4o.Reflect.IReflectClass
		/// 	</seealso>
		public virtual IReflectClass ForClass(Type clazz)
		{
			if (clazz == null)
			{
				return null;
			}
			IReflectClass claxx = (IReflectClass)_classByClass.Get(clazz);
			if (claxx != null)
			{
				return claxx;
			}
			if (!clazz.IsArray && ReflectPlatform.IsNamedClass(clazz))
			{
				claxx = ForName(ReflectPlatform.FullyQualifiedName(clazz));
				if (claxx != null)
				{
					_classByClass.Put(clazz, claxx);
					return claxx;
				}
			}
			claxx = _delegate.ForClass(clazz);
			if (claxx == null)
			{
				return null;
			}
			claxx = EnsureDelegate(claxx);
			_classByClass.Put(clazz, claxx);
			return claxx;
		}

		/// <summary>Returns a ReflectClass instance for the specified class name</summary>
		/// <param name="className">class name</param>
		/// <returns>a ReflectClass instance for the specified class name</returns>
		/// <seealso cref="Db4objects.Db4o.Reflect.IReflectClass">Db4objects.Db4o.Reflect.IReflectClass
		/// 	</seealso>
		public virtual IReflectClass ForName(string className)
		{
			return ((IReflectClass)WithLock(new _IClosure4_190(this, className)));
		}

		private sealed class _IClosure4_190 : IClosure4
		{
			public _IClosure4_190(GenericReflector _enclosing, string className)
			{
				this._enclosing = _enclosing;
				this.className = className;
			}

			public object Run()
			{
				IReflectClass clazz = this._enclosing._repository.LookupByName(className);
				if (clazz != null)
				{
					return clazz;
				}
				clazz = this._enclosing._delegate.ForName(className);
				if (clazz != null)
				{
					return this._enclosing.EnsureDelegate(clazz);
				}
				return this._enclosing._repository.ForName(className);
			}

			private readonly GenericReflector _enclosing;

			private readonly string className;
		}

		/// <summary>Returns a ReflectClass instance for the specified class object</summary>
		/// <param name="obj">class object</param>
		/// <returns>a ReflectClass instance for the specified class object</returns>
		/// <seealso cref="Db4objects.Db4o.Reflect.IReflectClass">Db4objects.Db4o.Reflect.IReflectClass
		/// 	</seealso>
		public virtual IReflectClass ForObject(object obj)
		{
			if (obj is GenericObject)
			{
				return ForGenericObject((GenericObject)obj);
			}
			if (obj is GenericArray)
			{
				return ((GenericArray)obj)._clazz;
			}
			return _delegate.ForObject(obj);
		}

		private IReflectClass ForGenericObject(GenericObject genericObject)
		{
			Db4objects.Db4o.Reflect.Generic.GenericClass claxx = genericObject._class;
			if (claxx == null)
			{
				throw new InvalidOperationException();
			}
			string name = claxx.GetName();
			if (name == null)
			{
				throw new InvalidOperationException();
			}
			Db4objects.Db4o.Reflect.Generic.GenericClass existingClass = (Db4objects.Db4o.Reflect.Generic.GenericClass
				)ForName(name);
			if (existingClass == null)
			{
				_repository.Register(claxx);
				return claxx;
			}
			// TODO: Using .equals() here would be more consistent with 
			//       the equals() method in GenericClass.
			if (existingClass != claxx)
			{
				throw new InvalidOperationException();
			}
			return claxx;
		}

		/// <summary>Returns delegate reflector</summary>
		/// <returns>delegate reflector</returns>
		public virtual IReflector GetDelegate()
		{
			return _delegate;
		}

		/// <summary>Determines if a candidate ReflectClass is a collection</summary>
		/// <param name="candidate">candidate ReflectClass</param>
		/// <returns>true  if a candidate ReflectClass is a collection.</returns>
		public virtual bool IsCollection(IReflectClass candidate)
		{
			//candidate = candidate.getDelegate(); 
			IEnumerator i = _collectionPredicates.GetEnumerator();
			while (i.MoveNext())
			{
				if (((IReflectClassPredicate)i.Current).Match(candidate))
				{
					return true;
				}
			}
			return _delegate.IsCollection(candidate.GetDelegate());
		}

		//TODO: will need knowledge for .NET collections here
		// possibility: call registercollection with strings
		/// <summary>Register a class as a collection</summary>
		/// <param name="clazz">class to be registered</param>
		public virtual void RegisterCollection(Type clazz)
		{
			RegisterCollection(ClassPredicate(clazz));
		}

		/// <summary>Register a predicate as a collection</summary>
		/// <param name="predicate">predicate to be registered</param>
		public virtual void RegisterCollection(IReflectClassPredicate predicate)
		{
			_collectionPredicates.Add(predicate);
		}

		private IReflectClassPredicate ClassPredicate(Type clazz)
		{
			IReflectClass collectionClass = ForClass(clazz);
			IReflectClassPredicate predicate = new _IReflectClassPredicate_290(collectionClass
				);
			return predicate;
		}

		private sealed class _IReflectClassPredicate_290 : IReflectClassPredicate
		{
			public _IReflectClassPredicate_290(IReflectClass collectionClass)
			{
				this.collectionClass = collectionClass;
			}

			public bool Match(IReflectClass candidate)
			{
				return collectionClass.IsAssignableFrom(candidate);
			}

			private readonly IReflectClass collectionClass;
		}

		/// <summary>Register a class</summary>
		/// <param name="clazz">class</param>
		public virtual void Register(Db4objects.Db4o.Reflect.Generic.GenericClass clazz)
		{
			WithLock(new _IClosure4_303(this, clazz));
		}

		private sealed class _IClosure4_303 : IClosure4
		{
			public _IClosure4_303(GenericReflector _enclosing, Db4objects.Db4o.Reflect.Generic.GenericClass
				 clazz)
			{
				this._enclosing = _enclosing;
				this.clazz = clazz;
			}

			public object Run()
			{
				string name = clazz.GetName();
				if (this._enclosing._repository.LookupByName(name) == null)
				{
					this._enclosing._repository.Register(clazz);
				}
				return null;
			}

			private readonly GenericReflector _enclosing;

			private readonly Db4objects.Db4o.Reflect.Generic.GenericClass clazz;
		}

		/// <summary>Returns an array of classes known to the reflector</summary>
		/// <returns>an array of classes known to the reflector</returns>
		public virtual IReflectClass[] KnownClasses()
		{
			return ((IReflectClass[])WithLock(new _IClosure4_319(this)));
		}

		private sealed class _IClosure4_319 : IClosure4
		{
			public _IClosure4_319(GenericReflector _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				return new KnownClassesCollector(this._enclosing._stream, this._enclosing._repository
					).Collect();
			}

			private readonly GenericReflector _enclosing;
		}

		/// <summary>Registers primitive class</summary>
		/// <param name="id">class id</param>
		/// <param name="name">class name</param>
		/// <param name="converter">class converter</param>
		public virtual void RegisterPrimitiveClass(int id, string name, IGenericConverter
			 converter)
		{
			WithLock(new _IClosure4_333(this, id, converter, name));
		}

		private sealed class _IClosure4_333 : IClosure4
		{
			public _IClosure4_333(GenericReflector _enclosing, int id, IGenericConverter converter
				, string name)
			{
				this._enclosing = _enclosing;
				this.id = id;
				this.converter = converter;
				this.name = name;
			}

			public object Run()
			{
				Db4objects.Db4o.Reflect.Generic.GenericClass existing = (Db4objects.Db4o.Reflect.Generic.GenericClass
					)this._enclosing._repository.LookupByID(id);
				if (existing != null)
				{
					if (null != converter)
					{
					}
					else
					{
						//						existing.setSecondClass();
						existing.SetConverter(null);
					}
					return null;
				}
				IReflectClass clazz = this._enclosing._delegate.ForName(name);
				Db4objects.Db4o.Reflect.Generic.GenericClass claxx = null;
				if (clazz != null)
				{
					claxx = this._enclosing.EnsureDelegate(clazz);
				}
				else
				{
					claxx = new Db4objects.Db4o.Reflect.Generic.GenericClass(this._enclosing, null, name
						, null);
					this._enclosing.Register(claxx);
					claxx.InitFields(new GenericField[] { new GenericField(null, null, true) });
					claxx.SetConverter(converter);
				}
				//			    claxx.setSecondClass();
				claxx.SetPrimitive();
				this._enclosing._repository.Register(id, claxx);
				return null;
			}

			private readonly GenericReflector _enclosing;

			private readonly int id;

			private readonly IGenericConverter converter;

			private readonly string name;
		}

		/// <summary>method stub: generic reflector does not have a parent</summary>
		public virtual void SetParent(IReflector reflector)
		{
		}

		// do nothing, the generic reflector does not have a parant
		public virtual void Configuration(IReflectorConfiguration config)
		{
			if (_delegate != null)
			{
				_delegate.Configuration(config);
			}
		}

		private object WithLock(IClosure4 block)
		{
			if (_stream == null || _stream.IsClosed())
			{
				return block.Run();
			}
			return _stream.SyncExec(block);
		}
	}
}
