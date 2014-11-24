/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	[System.Serializable]
	public class FieldIndexException : Exception
	{
		private string _className;

		private string _fieldName;

		public FieldIndexException(FieldMetadata field) : this(null, null, field)
		{
		}

		public FieldIndexException(string msg, FieldMetadata field) : this(msg, null, field
			)
		{
		}

		public FieldIndexException(Exception cause, FieldMetadata field) : this(null, cause
			, field)
		{
		}

		public FieldIndexException(string msg, Exception cause, FieldMetadata field) : this
			(msg, cause, field.ContainingClass().GetName(), field.GetName())
		{
		}

		public FieldIndexException(string msg, Exception cause, string className, string 
			fieldName) : base(EnhancedMessage(msg, className, fieldName), cause)
		{
			_className = className;
			_fieldName = fieldName;
		}

		public virtual string ClassName()
		{
			return _className;
		}

		public virtual string FieldName()
		{
			return _fieldName;
		}

		private static string EnhancedMessage(string msg, string className, string fieldName
			)
		{
			string enhancedMessage = "Field index for " + className + "#" + fieldName;
			if (msg != null)
			{
				enhancedMessage += ": " + msg;
			}
			return enhancedMessage;
		}
	}
}
