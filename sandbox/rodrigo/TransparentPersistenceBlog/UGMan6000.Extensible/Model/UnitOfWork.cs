using System;
using System.Collections.Generic;

namespace UGMan6000.Extensible
{
	public class UnitOfWork
	{	
		/// <summary>
		/// Adds the object to the list of affected objects in the current
		/// unit of work.
		/// </summary>
		/// <param name="o">the affected object</param>
		public static void Affected(object o)
		{
			List<object> current = UnitOfWork._current;
			if (current == null) return;
			if (current.Contains(o)) return;
			current.Add(o);
		}
		
		public delegate void Block();

		/// <summary>
		/// Runs a block of code in the scope of a unit of work and returns the
		/// list of affected objects.
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static List<object> Run(Block code)
		{	
			List<object> affectedList = new List<object>();
			List<object> saved = _current;
			_current = affectedList;
			try
			{
				code();
			}
			finally
			{
				_current = saved;
			}
			return affectedList;
		}

		[ThreadStatic] private static List<object> _current;
	}
}
