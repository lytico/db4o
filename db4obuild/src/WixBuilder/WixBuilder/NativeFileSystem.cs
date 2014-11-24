using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WixBuilder
{
	public class NativeFileSystem
	{
		public static IFolder GetFolder(string path)
		{
			return new FolderImpl(path);
		}

		public static IFolderBuilder FolderBuilderFor(string path)
		{
			return new FolderBuilder(path);
		}

		internal class FolderBuilder : IFolderBuilder
		{
			private readonly string _path;

			public FolderBuilder(string path)
			{
				_path = path;
				Directory.CreateDirectory(_path);
			}

			public string FullPath
			{
				get { return _path; }
			}

			public IFolderBuilder EnterFolder(string newFolder)
			{
				var newFolderPath = Rebase(newFolder);
				return new ChildFolderBuilder(this, newFolderPath);
			}

			protected string Rebase(string newFolder)
			{
				return System.IO.Path.Combine(_path, newFolder);
			}

			private class ChildFolderBuilder : FolderBuilder
			{
				private readonly IFolderBuilder _parent;

				public ChildFolderBuilder(IFolderBuilder parent, string basePath)
					: base(basePath)
				{
					_parent = parent;
				}

				public override IFolderBuilder LeaveFolder()
				{
					return _parent;
				}
			}

			public IFolderBuilder AddFiles(params string[] fileNames)
			{
				foreach (var fileName in fileNames)
				{
					File.WriteAllText(Rebase(fileName), fileName);
				}
				return this;
			}

			public virtual IFolderBuilder LeaveFolder()
			{
				throw new InvalidOperationException();
			}

			public IFolder GetFolder()
			{
				return NativeFileSystem.GetFolder(_path);
			}
		}

		class FileSystemItemImpl : IFileSystemItem
		{
			private string _path;
			private IFolder _parent;

			public FileSystemItemImpl(string path)
			{
				_path = Path.GetFullPath(path);
			}

			public FileSystemItemImpl(IFolder parent, string path) : this(path)
			{
				_parent = parent;
			}

			public string Name
			{
				get { return Path.GetFileName(_path); }
			}

			public IFolder Parent
			{
				get
				{
					if (_parent != null)
						return _parent;
					return _parent = NativeFileSystem.GetFolder(Path.GetDirectoryName(_path));
				}
			}

			public string FullPath
			{
				get { return _path; }
			}

			public string ShortPathName
			{
				get { return System.IO.Path.GetFileName(GetShortPathName(FullPath)); }
			}

			public override string ToString()
			{
				return FullPath;
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != GetType()) return false;
				return Equals((FileSystemItemImpl) obj);
			}

			public bool Equals(FileSystemItemImpl obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				return Equals(obj._path, _path);
			}

			public override int GetHashCode()
			{
				return (_path != null ? _path.GetHashCode() : 0);
			}
		}

		class FolderImpl : FileSystemItemImpl, IFolder
		{
			public FolderImpl(string path) : base(path)
			{	
			}

			private FolderImpl(IFolder parent, string path) : base(parent, path)
			{
			}

			public IFileSystemItem this[string name]
			{
				get { return (from item in Children where item.Name == name select item).FirstOrDefault(); }
			}

			public IEnumerable<IFileSystemItem> Children
			{
				get
				{
					foreach (var file in Directory.GetFiles(FullPath))
						yield return new FileImpl(this, file);
					foreach (var folder in Directory.GetDirectories(FullPath))
						yield return new FolderImpl(this, folder);
				}
			}
		}

		class FileImpl : FileSystemItemImpl, IFile
		{
			public FileImpl(IFolder parent, string file) : base(parent, file)
			{
			}
		}

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern int GetShortPathName(
			string lpszLongPath,
			[Out] StringBuilder lpszShortPath,
			int cchBuffer);

		static string GetShortPathName(string name)
		{
			var builder = new StringBuilder(new string(' ', name.Length+1));
			int cch = GetShortPathName(name, builder, builder.Capacity);
			if (cch > builder.Capacity)
			{
				builder.Capacity = cch;
				cch = GetShortPathName(name, builder, builder.Capacity);
			}
			builder.Length = cch;
			return builder.ToString();
		}
	}
}
