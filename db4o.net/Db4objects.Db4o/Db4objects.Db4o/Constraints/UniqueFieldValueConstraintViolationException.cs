/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Constraints;

namespace Db4objects.Db4o.Constraints
{
	/// <summary>
	/// This exception is thrown when a
	/// <see cref="UniqueFieldValueConstraint">UniqueFieldValueConstraint</see>
	/// is violated.<br /><br />
	/// </summary>
	/// <seealso cref="Db4objects.Db4o.Config.IObjectField.Indexed(bool)">Db4objects.Db4o.Config.IObjectField.Indexed(bool)
	/// 	</seealso>
	/// <seealso cref="Db4objects.Db4o.Config.ICommonConfiguration.Add(Db4objects.Db4o.Config.IConfigurationItem)
	/// 	">Db4objects.Db4o.Config.ICommonConfiguration.Add(Db4objects.Db4o.Config.IConfigurationItem)
	/// 	</seealso>
	[System.Serializable]
	public class UniqueFieldValueConstraintViolationException : ConstraintViolationException
	{
		/// <summary>
		/// Constructor with a message composed from the class and field
		/// name of the entity causing the exception.
		/// </summary>
		/// <remarks>
		/// Constructor with a message composed from the class and field
		/// name of the entity causing the exception.
		/// </remarks>
		/// <param name="className">class, which caused the exception</param>
		/// <param name="fieldName">field, which caused the exception</param>
		public UniqueFieldValueConstraintViolationException(string className, string fieldName
			) : base("class: " + className + " field: " + fieldName)
		{
		}
	}
}
