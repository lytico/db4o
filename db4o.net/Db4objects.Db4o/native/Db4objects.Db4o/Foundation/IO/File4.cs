/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
#if !SILVERLIGHT
using System;

using System.IO;

namespace Db4objects.Db4o.Foundation.IO
{
    public class File4
    {
        public static void Delete(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        public static void Copy(string from, string to)
        {
            File.Copy(from, to, true);
        }

		public static long Size(string filePath)
		{
			return new System.IO.FileInfo(filePath).Length;
		}
	}
}
#endif
