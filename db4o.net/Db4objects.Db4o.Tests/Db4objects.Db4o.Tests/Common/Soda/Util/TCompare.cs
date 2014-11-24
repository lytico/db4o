/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Reflection;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal;
using Sharpen.Lang.Reflect;

namespace Db4objects.Db4o.Tests.Common.Soda.Util
{
	public class TCompare
	{
		public static bool IsEqual(object a_compare, object a_with)
		{
			return IsEqual(a_compare, a_with, null, new ArrayList());
		}

		private static bool IsEqual(object a_compare, object a_with, string a_path, ArrayList
			 a_list)
		{
			if (a_compare == null)
			{
				return a_with == null;
			}
			if (a_with == null)
			{
				return false;
			}
			Type clazz = a_compare.GetType();
			if (clazz != a_with.GetType())
			{
				return false;
			}
			if (Platform4.IsSimple(clazz))
			{
				return a_compare.Equals(a_with);
			}
			// takes care of repeating calls to the same object
			if (a_list.Contains(a_compare))
			{
				return true;
			}
			a_list.Add(a_compare);
			if (a_compare.GetType().IsArray)
			{
				return AreArraysEqual(NormalizeNArray(a_compare), NormalizeNArray(a_with), a_path
					, a_list);
			}
			if (HasPublicConstructor(a_compare.GetType()))
			{
				return AreFieldsEqual(a_compare, a_with, a_path, a_list);
			}
			return a_compare.Equals(a_with);
		}

		private static bool AreFieldsEqual(object a_compare, object a_with, string a_path
			, ArrayList a_list)
		{
			string path = GetPath(a_compare, a_with, a_path);
			FieldInfo[] fields = Sharpen.Runtime.GetDeclaredFields(a_compare.GetType());
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo field = fields[i];
				if (Db4oUnitPlatform.IsUserField(field))
				{
					Platform4.SetAccessible(field);
					try
					{
						if (!IsFieldEqual(field, a_compare, a_with, path, a_list))
						{
							return false;
						}
					}
					catch (Exception e)
					{
						Sharpen.Runtime.Err.WriteLine("TCompare failure executing path:" + path);
						Sharpen.Runtime.PrintStackTrace(e);
						return false;
					}
				}
			}
			return true;
		}

		private static bool IsFieldEqual(FieldInfo field, object a_compare, object a_with
			, string path, ArrayList a_list)
		{
			object compare = GetFieldValue(field, a_compare);
			object with = GetFieldValue(field, a_with);
			return IsEqual(compare, with, path + field.Name + ":", a_list);
		}

		private static object GetFieldValue(FieldInfo field, object obj)
		{
			try
			{
				return field.GetValue(obj);
			}
			catch (MemberAccessException)
			{
				// probably JDK 1
				// never mind this field
				return null;
			}
		}

		private static bool AreArraysEqual(object compare, object with, string path, ArrayList
			 a_list)
		{
			int len = Sharpen.Runtime.GetArrayLength(compare);
			if (len != Sharpen.Runtime.GetArrayLength(with))
			{
				return false;
			}
			else
			{
				for (int j = 0; j < len; j++)
				{
					object elementCompare = Sharpen.Runtime.GetArrayValue(compare, j);
					object elementWith = Sharpen.Runtime.GetArrayValue(with, j);
					if (!IsEqual(elementCompare, elementWith, path, a_list))
					{
						return false;
					}
				}
			}
			return true;
		}

		private static string GetPath(object a_compare, object a_with, string a_path)
		{
			if (a_path != null && a_path.Length > 0)
			{
				return a_path;
			}
			if (a_compare != null)
			{
				return a_compare.GetType().FullName + ":";
			}
			if (a_with != null)
			{
				return a_with.GetType().FullName + ":";
			}
			return a_path;
		}

		internal static bool HasPublicConstructor(Type a_class)
		{
			if (a_class != typeof(string))
			{
				try
				{
					return System.Activator.CreateInstance(a_class) != null;
				}
				catch
				{
				}
			}
			return false;
		}

		internal static object NormalizeNArray(object a_object)
		{
			if (Sharpen.Runtime.GetArrayLength(a_object) > 0)
			{
				object first = Sharpen.Runtime.GetArrayValue(a_object, 0);
				if (first != null && first.GetType().IsArray)
				{
					int[] dim = ArrayDimensions(a_object);
					object all = new object[ArrayElementCount(dim)];
					NormalizeNArray1(a_object, all, 0, dim, 0);
					return all;
				}
			}
			return a_object;
		}

		internal static int NormalizeNArray1(object a_object, object a_all, int a_next, int
			[] a_dim, int a_index)
		{
			if (a_index == a_dim.Length - 1)
			{
				for (int i = 0; i < a_dim[a_index]; i++)
				{
					Sharpen.Runtime.SetArrayValue(a_all, a_next++, Sharpen.Runtime.GetArrayValue(a_object
						, i));
				}
			}
			else
			{
				for (int i = 0; i < a_dim[a_index]; i++)
				{
					a_next = NormalizeNArray1(Sharpen.Runtime.GetArrayValue(a_object, i), a_all, a_next
						, a_dim, a_index + 1);
				}
			}
			return a_next;
		}

		internal static int[] ArrayDimensions(object a_object)
		{
			int count = 0;
			for (Type clazz = a_object.GetType(); clazz.IsArray; clazz = clazz.GetElementType
				())
			{
				count++;
			}
			int[] dim = new int[count];
			for (int i = 0; i < count; i++)
			{
				dim[i] = Sharpen.Runtime.GetArrayLength(a_object);
				a_object = Sharpen.Runtime.GetArrayValue(a_object, 0);
			}
			return dim;
		}

		internal static int ArrayElementCount(int[] a_dim)
		{
			int elements = a_dim[0];
			for (int i = 1; i < a_dim.Length; i++)
			{
				elements *= a_dim[i];
			}
			return elements;
		}

		private TCompare()
		{
		}
	}
}
