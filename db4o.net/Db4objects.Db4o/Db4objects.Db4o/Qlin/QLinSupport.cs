/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Core;

namespace Db4objects.Db4o.Qlin
{
	/// <summary>
	/// static import support class for
	/// <see cref="IQLin">IQLin</see>
	/// queries.
	/// </summary>
	/// <since>8.0</since>
	public class QLinSupport
	{
		/// <summary>
		/// returns a prototype object for a specific class
		/// to be passed to the where expression of a QLin
		/// query.
		/// </summary>
		/// <remarks>
		/// returns a prototype object for a specific class
		/// to be passed to the where expression of a QLin
		/// query.
		/// </remarks>
		/// <seealso cref="IQLin.Where(object)">IQLin.Where(object)</seealso>
		public static object Prototype(Type clazz)
		{
			try
			{
				return _prototypes.PrototypeForClass(clazz);
			}
			catch (PrototypesException ex)
			{
				throw new QLinException(ex);
			}
		}

		/// <summary>sets the context for the next query on this thread.</summary>
		/// <remarks>
		/// sets the context for the next query on this thread.
		/// This method should never have to be called manually.
		/// The framework should set the context up.
		/// </remarks>
		public static void Context(IReflectClass claxx)
		{
			_context.Value = claxx;
		}

		/// <summary>sets the context for the next query on this thread.</summary>
		/// <remarks>
		/// sets the context for the next query on this thread.
		/// This method should never have to be called manually.
		/// The framework should set the context up.
		/// </remarks>
		public static void Context(Type clazz)
		{
			_context.Value = ReflectorUtils.ReflectClassFor(_prototypes.Reflector(), clazz);
		}

		/// <summary>
		/// shortcut for the
		/// <see cref="Prototype(System.Type{T})">Prototype(System.Type&lt;T&gt;)</see>
		/// method.
		/// </summary>
		public static object P(Type clazz)
		{
			return Prototype(clazz);
		}

		/// <summary>
		/// parameter for
		/// <see cref="IQLin.OrderBy(object, QLinOrderByDirection)">IQLin.OrderBy(object, QLinOrderByDirection)
		/// 	</see>
		/// </summary>
		public static QLinOrderByDirection Ascending()
		{
			return QLinOrderByDirection.Ascending;
		}

		/// <summary>
		/// parameter for
		/// <see cref="IQLin.OrderBy(object, QLinOrderByDirection)">IQLin.OrderBy(object, QLinOrderByDirection)
		/// 	</see>
		/// </summary>
		public static QLinOrderByDirection Descending()
		{
			return QLinOrderByDirection.Descending;
		}

		/// <summary>public for implementors, do not use directly</summary>
		public static IEnumerator BackingFieldPath(object expression)
		{
			CheckForNull(expression);
			if (expression is IReflectField)
			{
				return Iterators.Iterate(new string[] { ((IReflectField)expression).GetName() });
			}
			IEnumerator path = _prototypes.BackingFieldPath(((IReflectClass)_context.Value), 
				expression);
			if (path != null)
			{
				return path;
			}
			return Iterators.Iterate(new string[] { FieldByFieldName(expression).GetName() });
		}

		/// <summary>converts an expression to a single field.</summary>
		/// <remarks>converts an expression to a single field.</remarks>
		public static IReflectField Field(object expression)
		{
			CheckForNull(expression);
			if (expression is IReflectField)
			{
				return (IReflectField)expression;
			}
			IEnumerator path = _prototypes.BackingFieldPath(((IReflectClass)_context.Value), 
				expression);
			if (path != null)
			{
				if (path.MoveNext())
				{
					expression = path.Current;
				}
				if (path.MoveNext())
				{
					path.Reset();
					throw new QLinException("expression can not be converted to a single field. It evaluates to: "
						 + Iterators.Join(path, "[", "]", ", "));
				}
			}
			return FieldByFieldName(expression);
		}

		private static IReflectField FieldByFieldName(object expression)
		{
			if (expression is string)
			{
				IReflectField field = ReflectorUtils.Field(((IReflectClass)_context.Value), (string
					)expression);
				if (field != null)
				{
					return field;
				}
			}
			throw new QLinException("expression can not be mapped to a field");
		}

		private static void CheckForNull(object expression)
		{
			if (expression == null)
			{
				throw new QLinException("expression can not be null");
			}
		}

		private const bool IgnoreTransientFields = true;

		private const int RecursionDepth = 4;

		private static readonly Prototypes _prototypes = new Prototypes(Prototypes.DefaultReflector
			(), RecursionDepth, IgnoreTransientFields);

		private static readonly DynamicVariable _context = DynamicVariable.NewInstance();
	}
}
