/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o.IO;

namespace Sharpen.IO
{
	public class File
	{
		public static readonly char separatorChar = Path.DirectorySeparatorChar;
		public static readonly string separator = separatorChar.ToString();
		
		private readonly string _path;

		public File(string path)
		{
			_path = path;
		}

		public static implicit operator string(File file)
		{
			return file.GetAbsolutePath();
		}

		public File(string dir, string file)
		{
			_path = dir == null ? file : Path.Combine(dir, file);
		}

		public virtual bool Delete()
		{
#if SILVERLIGHT
			return SilverlightIO.Delete(_path);
#else
			if (Exists())
			{
				System.IO.File.Delete(_path);
				return !Exists();
			}
			return false;
#endif
		}

		public bool Exists()
		{
#if CF
            string path = RemoveTrailingSlash(_path);
#elif SILVERLIGHT
			string path = _path;
			return SilverlightIO.Exists(path);
#else
            string path = _path;
#endif
            return System.IO.File.Exists(path) || Directory.Exists(path);
		}

	    private static string RemoveTrailingSlash(string path)
	    {
	        return (path.EndsWith("\\") || path.EndsWith("/")) ? path.Remove(path.Length - 1, 1) : path;
	    }

	    public string GetCanonicalPath()
		{
			return Path.GetFullPath(_path);
		}

		public File GetCanonicalFile()
		{
			return new File(GetCanonicalPath());
		}

		public string GetAbsolutePath()
		{
#if SILVERLIGHT
			return _path;
#else
			return Path.GetFullPath(_path);
#endif
		}

		public string GetName()
		{
			int index = _path.LastIndexOf(separator);
			return _path.Substring(index + 1);
		}

		public string GetPath()
		{
			return _path;
		}

		public bool IsDirectory()
		{
#if CF || SILVERLIGHT
			return Exists();
#else
			return (System.IO.File.GetAttributes(_path) & FileAttributes.Directory) != 0;
#endif
		}

		public long Length()
		{
#if SILVERLIGHT
			return SilverlightIO.Length(_path);
#else
			return new FileInfo(_path).Length;
#endif
		}

		public string[] List()
		{
#if SILVERLIGHT
            throw new NotImplementedException();
#else
			return Directory.GetFiles(_path);
#endif
        }

		public bool Mkdir()
		{
			if (Exists())
			{
				return false;
			}
			Directory.CreateDirectory(_path);
			return Exists();
		}

		public bool Mkdirs()
		{
			if (Exists())
			{
				return false;
			}
			int pos = _path.LastIndexOf(separator);
			if (pos > 0)
			{
				new File(_path.Substring(0, pos)).Mkdirs();
			}
			return Mkdir();
		}

		public void RenameTo(File file)
		{
			new FileInfo(_path).MoveTo(file.GetPath());
		}

        public File[] ListFiles(IFilenameFilter filter)
        {
            String[] ss = List();
            if (ss == null) return null;
            List<File> v = new List<File>();
            for (int i = 0; i < ss.Length; i++)
            {
                if ((filter == null) || filter.Accept(this, ss[i]))
                {
                    v.Add(new File(ss[i], this));
                }
            }
            return v.ToArray();
        }

		public override string ToString()
		{
			return _path;
		}
	}
}
