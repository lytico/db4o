/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System.Diagnostics;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Monitoring
{
	public class Db4oPerformanceCounters
	{
		public static readonly string CategoryName = "Db4o";

		public static void Install()
		{
			if (CategoryExists())
			{
				return;
			}

			CounterCreationData[] creationData = CreationDataFor(PerformanceCounterSpec.All());
			CounterCreationDataCollection collection = new CounterCreationDataCollection(creationData);

			PerformanceCounterCategory.Create(CategoryName, "Db4o Performance Counters", PerformanceCounterCategoryType.MultiInstance, collection);
		}

		private static CounterCreationData[] CreationDataFor(PerformanceCounterSpec[] performanceCounterSpecs)
		{
			CounterCreationData[] creationData = new CounterCreationData[performanceCounterSpecs.Length];
			for(int i = 0; i < performanceCounterSpecs.Length; i++)
			{
				creationData[i] = performanceCounterSpecs[i].CounterCreationData();
			}
			return creationData;
		}

		public static void ReInstall()
		{
			if (CategoryExists()) DeleteCategory();
			Install();
		}

		public static PerformanceCounter CounterFor(PerformanceCounterSpec spec, bool readOnly)
		{
			return CounterFor(spec, My<IObjectContainer>.Instance, readOnly);
		}

		internal static PerformanceCounter CounterFor(PerformanceCounterSpec spec, IObjectContainer container, bool readOnly)
		{
			return NewDb4oCounter(spec.Id, container.ToString(), readOnly);
		}

		public static PerformanceCounter CounterFor(PerformanceCounterSpec spec, IObjectContainer container)
		{
			return NewDb4oCounter(spec.Id, container.ToString(), true);
		}

		protected static PerformanceCounter NewDb4oCounter(string counterName, string instanceName, bool readOnly)
		{
			Install();

			PerformanceCounter counter = new PerformanceCounter(CategoryName, counterName, instanceName, readOnly);

			if (readOnly) return counter;

			counter.RawValue = 0;
			return counter;
		}

		private static bool CategoryExists()
		{
			return PerformanceCounterCategory.Exists(CategoryName);
		}

		private static void DeleteCategory()
		{
			PerformanceCounterCategory.Delete(CategoryName);
		}
	}
}

#endif