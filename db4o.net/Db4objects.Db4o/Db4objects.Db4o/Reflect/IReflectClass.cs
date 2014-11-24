/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect
{
	/// <summary>Reflection Class representation.</summary>
	/// <remarks>
	/// Reflection Class representation
	/// <br/><br/>See documentation for System.Reflection API.
	/// </remarks>
	/// <seealso cref="IReflector">IReflector</seealso>
	public interface IReflectClass
	{
		IReflectClass GetComponentType();

		IReflectField[] GetDeclaredFields();

		IReflectField GetDeclaredField(string name);

		/// <summary>Returns the ReflectClass instance being delegated to.</summary>
		/// <remarks>
		/// Returns the ReflectClass instance being delegated to.
		/// If there's no delegation it should return this.
		/// </remarks>
		/// <returns>delegate or this</returns>
		IReflectClass GetDelegate();

		IReflectMethod GetMethod(string methodName, IReflectClass[] paramClasses);

		string GetName();

		IReflectClass GetSuperclass();

		bool IsAbstract();

		bool IsArray();

		bool IsAssignableFrom(IReflectClass type);

		bool IsCollection();

		bool IsInstance(object obj);

		bool IsInterface();

		bool IsPrimitive();

		object NewInstance();

		IReflector Reflector();

		object NullValue();

		/// <summary>
		/// Calling this method may change the internal state of the class, even if a usable
		/// constructor has been found on earlier invocations.
		/// </summary>
		/// <remarks>
		/// Calling this method may change the internal state of the class, even if a usable
		/// constructor has been found on earlier invocations.
		/// </remarks>
		/// <returns>true, if instances of this class can be created, false otherwise</returns>
		bool EnsureCanBeInstantiated();

		/// <summary>
		/// We need this for replication, to find out if a class needs to be traversed
		/// or if it simply can be copied across.
		/// </summary>
		/// <remarks>
		/// We need this for replication, to find out if a class needs to be traversed
		/// or if it simply can be copied across. For now we will simply return
		/// the classes that are
		/// <see cref="IsPrimitive()">IsPrimitive()</see>
		/// and
		/// <see cref="Db4objects.Db4o.Internal.Platform4.IsSimple(System.Type{T})">Db4objects.Db4o.Internal.Platform4.IsSimple(System.Type&lt;T&gt;)
		/// 	</see>
		/// We can think about letting users add an Immutable annotation.
		/// </remarks>
		bool IsSimple();
	}
}
