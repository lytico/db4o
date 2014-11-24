using System.Collections.Generic;

namespace WixBuilder
{
	public interface IFileSystemItem
	{
		IFolder Parent { get; set; }
		string Path { get; }
	}

	public interface IFile : IFileSystemItem
	{
	}

	public interface IFolder : IFileSystemItem
	{
		IEnumerable<IFileSystemItem> Children { get; }
	}
}
