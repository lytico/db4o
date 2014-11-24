/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Qlin
{
	/// <summary>
	/// Internal implementation class, access should not be necessary,
	/// except for implementors.
	/// </summary>
	/// <remarks>
	/// Internal implementation class, access should not be necessary,
	/// except for implementors.
	/// Use the static methods in
	/// <see cref="QLinSupport">QLinSupport</see>
	/// 
	/// <see cref="QLinSupport.Ascending()">QLinSupport.Ascending()</see>
	/// and
	/// <see cref="QLinSupport.Descending()">QLinSupport.Descending()</see>
	/// </remarks>
	/// <exclude></exclude>
	public class QLinOrderByDirection
	{
		private readonly string _direction;

		private readonly bool _ascending;

		private QLinOrderByDirection(string direction, bool ascending)
		{
			_direction = direction;
			_ascending = ascending;
		}

		internal static readonly Db4objects.Db4o.Qlin.QLinOrderByDirection Ascending = new 
			Db4objects.Db4o.Qlin.QLinOrderByDirection("ascending", true);

		internal static readonly Db4objects.Db4o.Qlin.QLinOrderByDirection Descending = new 
			Db4objects.Db4o.Qlin.QLinOrderByDirection("descending", false);

		public virtual bool IsAscending()
		{
			return _ascending;
		}

		public virtual bool IsDescending()
		{
			return !_ascending;
		}

		public override string ToString()
		{
			return _direction;
		}
	}
}
