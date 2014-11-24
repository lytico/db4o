/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>
	/// Diagnostic if
	/// <see cref="Db4objects.Db4o.Config.IObjectClass.ObjectField(string)">Db4objects.Db4o.Config.IObjectClass.ObjectField(string)
	/// 	</see>
	/// was called on a
	/// field that does not exist.
	/// </summary>
	public class ObjectFieldDoesNotExist : DiagnosticBase
	{
		public readonly string _className;

		public readonly string _fieldName;

		public ObjectFieldDoesNotExist(string className, string fieldName)
		{
			_className = className;
			_fieldName = fieldName;
		}

		public override string Problem()
		{
			return "ObjectField was configured but does not exist.";
		}

		public override object Reason()
		{
			return _className + "." + _fieldName;
		}

		public override string Solution()
		{
			return "Check your configuration.";
		}
	}
}
