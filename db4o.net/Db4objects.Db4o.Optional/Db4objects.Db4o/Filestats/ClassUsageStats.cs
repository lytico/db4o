/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Text;
using Db4objects.Db4o.Filestats;

namespace Db4objects.Db4o.Filestats
{
	/// <summary>Statistics for the byte usage for a single class (instances, indices, etc.) in a db4o database file.
	/// 	</summary>
	/// <remarks>Statistics for the byte usage for a single class (instances, indices, etc.) in a db4o database file.
	/// 	</remarks>
	/// <exclude></exclude>
	public class ClassUsageStats
	{
		private readonly string _className;

		private readonly long _slotUsage;

		private readonly long _classIndexUsage;

		private readonly long _fieldIndexUsage;

		private readonly long _miscUsage;

		private readonly int _numInstances;

		internal ClassUsageStats(string className, long slotSpace, long classIndexUsage, 
			long fieldIndexUsage, long miscUsage, int numInstances)
		{
			_className = className;
			_slotUsage = slotSpace;
			_classIndexUsage = classIndexUsage;
			_fieldIndexUsage = fieldIndexUsage;
			_miscUsage = miscUsage;
			_numInstances = numInstances;
		}

		/// <returns>the name of the persistent class</returns>
		public virtual string ClassName()
		{
			return _className;
		}

		/// <returns>number of bytes used slots containing the actual class instances</returns>
		public virtual long SlotUsage()
		{
			return _slotUsage;
		}

		/// <returns>number of bytes used for the index of class instances</returns>
		public virtual long ClassIndexUsage()
		{
			return _classIndexUsage;
		}

		/// <returns>number of bytes used for field indexes, if any</returns>
		public virtual long FieldIndexUsage()
		{
			return _fieldIndexUsage;
		}

		/// <returns>
		/// number of bytes used for features that are specific to this class (ex.: the BTree encapsulated within a
		/// <see cref="Db4objects.Db4o.Internal.Collections.BigSet{E}">Db4objects.Db4o.Internal.Collections.BigSet&lt;E&gt;
		/// 	</see>
		/// instance)
		/// </returns>
		public virtual long MiscUsage()
		{
			return _miscUsage;
		}

		/// <returns>number of persistent instances of this class</returns>
		public virtual int NumInstances()
		{
			return _numInstances;
		}

		/// <returns>aggregated byte usage for this persistent class</returns>
		public virtual long TotalUsage()
		{
			return _slotUsage + _classIndexUsage + _fieldIndexUsage + _miscUsage;
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			ToString(str);
			return str.ToString();
		}

		internal virtual void ToString(StringBuilder str)
		{
			str.Append(ClassName()).Append("\n");
			str.Append(FileUsageStatsUtil.FormatLine("Slots", SlotUsage()));
			str.Append(FileUsageStatsUtil.FormatLine("Class index", ClassIndexUsage()));
			str.Append(FileUsageStatsUtil.FormatLine("Field indices", FieldIndexUsage()));
			if (MiscUsage() > 0)
			{
				str.Append(FileUsageStatsUtil.FormatLine("Misc", MiscUsage()));
			}
			str.Append(FileUsageStatsUtil.FormatLine("Total", TotalUsage()));
		}
	}
}
#endif // !SILVERLIGHT
