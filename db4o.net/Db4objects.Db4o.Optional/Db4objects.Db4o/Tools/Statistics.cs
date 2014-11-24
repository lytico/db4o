/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tools;

namespace Db4objects.Db4o.Tools
{
	/// <summary>prints statistics about a database file to System.out.</summary>
	/// <remarks>
	/// prints statistics about a database file to System.out.
	/// <br /><br />Pass the database file path as an argument.
	/// <br /><br /><b>This class is not part of db4o.jar!</b><br />
	/// It is delivered as sourcecode in the
	/// path ../com/db4o/tools/<br /><br />
	/// </remarks>
	public class Statistics
	{
		/// <summary>the main method that runs the statistics.</summary>
		/// <remarks>the main method that runs the statistics.</remarks>
		/// <param name="args">
		/// a String array of length 1, with the name of the database
		/// file as element 0.
		/// </param>
		public static void Main(string[] args)
		{
			if (args == null || args.Length != 1)
			{
				Sharpen.Runtime.Out.WriteLine("Usage: java com.db4o.tools.Statistics <database filename>"
					);
			}
			else
			{
				new Statistics().Run(args[0]);
			}
		}

		public virtual void Run(string filename)
		{
			if (new Sharpen.IO.File(filename).Exists())
			{
				IObjectContainer con = null;
				try
				{
					IConfiguration config = Db4oFactory.NewConfiguration();
					config.MessageLevel(-1);
					con = Db4oFactory.OpenFile(config, filename);
					PrintHeader("STATISTICS");
					Sharpen.Runtime.Out.WriteLine("File: " + filename);
					PrintStats(con, filename);
					con.Close();
				}
				catch (Exception e)
				{
					Sharpen.Runtime.Out.WriteLine("Statistics failed for file: '" + filename + "'");
					Sharpen.Runtime.Out.WriteLine(e.Message);
					Sharpen.Runtime.PrintStackTrace(e);
				}
			}
			else
			{
				Sharpen.Runtime.Out.WriteLine("File not found: '" + filename + "'");
			}
		}

		private static bool CanCallConstructor(string className)
		{
			return ReflectPlatform.CreateInstance(className) != null;
		}

		private void PrintStats(IObjectContainer con, string filename)
		{
			Tree unavailable = new TreeString(Remove);
			Tree noConstructor = new TreeString(Remove);
			// one element too many, substract one in the end
			IStoredClass[] internalClasses = con.Ext().StoredClasses();
			for (int i = 0; i < internalClasses.Length; i++)
			{
				string internalClassName = internalClasses[i].GetName();
				Type clazz = ReflectPlatform.ForName(internalClassName);
				if (clazz == null)
				{
					unavailable = unavailable.Add(new TreeString(internalClassName));
				}
				else
				{
					if (!CanCallConstructor(internalClassName))
					{
						noConstructor = noConstructor.Add(new TreeString(internalClassName));
					}
				}
			}
			unavailable = unavailable.RemoveLike(new TreeString(Remove));
			noConstructor = noConstructor.RemoveLike(new TreeString(Remove));
			if (unavailable != null)
			{
				PrintHeader("UNAVAILABLE");
				unavailable.Traverse(new _IVisitor4_80());
			}
			if (noConstructor != null)
			{
				PrintHeader("NO PUBLIC CONSTRUCTOR");
				noConstructor.Traverse(new _IVisitor4_88());
			}
			PrintHeader("CLASSES");
			Sharpen.Runtime.Out.WriteLine("Number of objects per class:");
			ByRef ids = ByRef.NewInstance(new TreeInt(0));
			if (internalClasses.Length > 0)
			{
				Tree all = new TreeStringObject(internalClasses[0].GetName(), internalClasses[0]);
				for (int i = 1; i < internalClasses.Length; i++)
				{
					all = all.Add(new TreeStringObject(internalClasses[i].GetName(), internalClasses[
						i]));
				}
				all.Traverse(new _IVisitor4_107(ids));
			}
			PrintHeader("SUMMARY");
			Sharpen.Runtime.Out.WriteLine("File: " + filename);
			Sharpen.Runtime.Out.WriteLine("Stored classes: " + internalClasses.Length);
			if (unavailable != null)
			{
				Sharpen.Runtime.Out.WriteLine("Unavailable classes: " + unavailable.Size());
			}
			if (noConstructor != null)
			{
				Sharpen.Runtime.Out.WriteLine("Classes without public constructors: " + noConstructor
					.Size());
			}
			Sharpen.Runtime.Out.WriteLine("Total number of objects: " + (((Tree)ids.value).Size
				() - 1));
		}

		private sealed class _IVisitor4_80 : IVisitor4
		{
			public _IVisitor4_80()
			{
			}

			public void Visit(object obj)
			{
				Sharpen.Runtime.Out.WriteLine(((TreeString)obj)._key);
			}
		}

		private sealed class _IVisitor4_88 : IVisitor4
		{
			public _IVisitor4_88()
			{
			}

			public void Visit(object obj)
			{
				Sharpen.Runtime.Out.WriteLine(((TreeString)obj)._key);
			}
		}

		private sealed class _IVisitor4_107 : IVisitor4
		{
			public _IVisitor4_107(ByRef ids)
			{
				this.ids = ids;
			}

			public void Visit(object obj)
			{
				TreeStringObject node = (TreeStringObject)obj;
				long[] newIDs = ((IStoredClass)node._value).GetIDs();
				for (int j = 0; j < newIDs.Length; j++)
				{
					if (((Tree)ids.value).Find(new TreeInt((int)newIDs[j])) == null)
					{
						ids.value = ((TreeInt)((Tree)ids.value).Add(new TreeInt((int)newIDs[j])));
					}
				}
				Sharpen.Runtime.Out.WriteLine(node._key + ": " + newIDs.Length);
			}

			private readonly ByRef ids;
		}

		private void PrintHeader(string str)
		{
			int stars = (39 - str.Length) / 2;
			Sharpen.Runtime.Out.WriteLine("\n");
			for (int i = 0; i < stars; i++)
			{
				Sharpen.Runtime.Out.Write("*");
			}
			Sharpen.Runtime.Out.Write(" " + str + " ");
			for (int i = 0; i < stars; i++)
			{
				Sharpen.Runtime.Out.Write("*");
			}
			Sharpen.Runtime.Out.WriteLine();
		}

		private static readonly string Remove = "XXxxREMOVExxXX";
	}
}
