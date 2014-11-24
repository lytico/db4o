using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WixBuilder.Tests
{
	internal class BaseFileSystemMock : IFileSystemItem
	{
		protected readonly string _name;
		protected IFolder _parent;

		public BaseFileSystemMock(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}

		public IFolder Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}

		public string FullPath
		{
			get
			{
				return _parent != null
							? _parent.FullPath + "\\" + _name
							: _name;
			}
		}

		public string ShortPathName
		{
			get { return "ShortPathName(" + Name + ")"; }
		}
	}

	internal class FileMock : BaseFileSystemMock, IFile
	{
		public FileMock(string fileName)
			: base(fileName)
		{
		}

		public override string ToString()
		{
			return "File(" + FullPath + ")";
		}
	}

	internal class FolderMock : BaseFileSystemMock, IFolder, IFolderBuilder
	{
		private readonly List<BaseFileSystemMock> _items = new List<BaseFileSystemMock>();

		public FolderMock(string name) : base(name)
		{	
		}

		public FolderMock(string name, IEnumerable<BaseFileSystemMock> items)
			: base(name)
		{
			AddAll(items);
		}

		private void AddAll(IEnumerable<BaseFileSystemMock> items)
		{
			foreach (var item in items)
			{	
				Add(item);
			}
		}

		private void Add(BaseFileSystemMock item)
		{
			item.Parent = this;
			_items.Add(item);
		}

		public IEnumerable<IFileSystemItem> Children
		{
			get { return _items.Cast<IFileSystemItem>(); }
		}

		public IFileSystemItem this[string name]
		{
			get { return (from item in _items where item.Name == name select item).FirstOrDefault(); }
		}

		public override string ToString()
		{
			return "Folder(" + FullPath + "[" + ChildrenString() + "])";
		}

		private string ChildrenString()
		{
			return Children.Select(i => i.Name).Aggregate("", (acc, value) => (acc + ", " + value));
		}

		public IFolderBuilder AddFiles(params string[] fileNames)
		{
			AddAll(MockFilesFor(fileNames));
			return this;
		}

		public IFolderBuilder EnterFolder(string folderName)
		{
			var folder = new FolderMock(folderName);
			Add(folder);
			return folder;
		}

		public IFolderBuilder LeaveFolder()
		{
			return (IFolderBuilder) _parent;
		}

		public IFolder GetFolder()
		{
			return this;
		}

		private IEnumerable<BaseFileSystemMock> MockFilesFor(IEnumerable<string> fileNames)
		{
			return (from fileName in fileNames select (BaseFileSystemMock)new FileMock(fileName));
		}
	}
}
