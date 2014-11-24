using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace WixBuilder.Tests
{
    [TestFixture]
    public class FeaturesTestCase
    {
        [Test]
        public void TestIncludeSingleFile()
        {
        	IFile expected = new FileMock("OMNSetup.msi");
        	IFolder root = new FolderMock("root", new FolderMock("Bin", expected, new FileMock("OtherFile.ext")));
        	FileFinder finder = new FileFinder(root);
            
			Assert.AreEqual(expected, finder.FindAll(new InclusionPattern(@"Bin\OMNSetup.msi")).Single());
        }

		[Test]
		public void TestIncludeMultipleFiles()
		{
			IFile[] files = new IFile[] { new FileMock("OMNSetup.msi"), new FileMock("OtherFile.ext") };
			IFolder root = new FolderMock("root", new FolderMock("Bin", files));
			FileFinder finder = new FileFinder(root);

			Assert.AreEqual(files, finder.FindAll(new InclusionPattern(@"Bin\*")).ToArray());
		}

    }

	internal class BaseFileSystemMock : IFileSystemItem
	{
		protected readonly string _name;
		protected IFolder _parent;

		public BaseFileSystemMock(string name)
		{
			_name = name;
		}

		public IFolder Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}
		
		public string Path
		{
			get
			{
				return _parent != null ? _parent.Path + "\\" + _name : _name;
			}
		}
	}

	internal class FileMock : BaseFileSystemMock, IFile
    {
        public FileMock(string fileName) : base(fileName)
        {
        }
    }

	internal class FolderMock : BaseFileSystemMock, IFolder
    {
    	private readonly IEnumerable<IFileSystemItem> _items;

		public FolderMock(string name, params IFileSystemItem[] items) : base (name)
        {
    		_items = items;
    		SetParent(_items);
        }

    	private void SetParent(IEnumerable<IFileSystemItem> items)
    	{
    		foreach (var item in items)
    		{
    			item.Parent = this;
    		}
    	}

    	public IEnumerable<IFileSystemItem> Children
    	{
			get
			{
				return _items;
			}
    	}
    }
}
