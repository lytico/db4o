using System.Collections.Generic;
using System.Linq;

namespace WixBuilder
{
	public interface IFileSystemItem
	{
		string Name { get; }
		IFolder Parent { get; }
		string FullPath { get; }
		string ShortPathName { get; }
	}

	public interface IFile : IFileSystemItem
	{
	}

	public interface IFolder : IFileSystemItem
	{
		IEnumerable<IFileSystemItem> Children { get; }
		IFileSystemItem this[string name] { get; }
	}

	public interface IFolderBuilder
	{
		string FullPath { get;  }
		IFolderBuilder AddFiles(params string[] fileNames);
		IFolderBuilder EnterFolder(string folderName);
		IFolderBuilder LeaveFolder();
		IFolder GetFolder();
	}

	public static class FileSystemExtensions
	{
        public static IFileSystemItem GetItem(this IFolder source, string filePath)
        {
            IFolder path = source;
            string[] pathComponents = filePath.Split(new[] { '/', '\\' });
            foreach (var part in pathComponents.Take(pathComponents.Length - 1))
            {
                path = (IFolder)path[part];
                if (path == null)
                    return null;
            }

            return path[pathComponents[pathComponents.Length - 1]];
        }
		public static IEnumerable<IFile> GetAllFilesRecursively(this IFolder root)
		{
			foreach (var item in root.Children)
			{
				IFile file = item as IFile;
				if (null != file)
				{
					yield return file;
					continue;
				}

				IFolder folder = (IFolder)item;
				foreach (var subFile in GetAllFilesRecursively(folder))
				{
					yield return subFile;
				}
			}
		}
	}
}
