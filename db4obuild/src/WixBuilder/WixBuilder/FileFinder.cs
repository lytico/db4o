using System.Collections.Generic;
using System.Linq;

namespace WixBuilder
{
	public class FileFinder
	{
		private readonly IFolder _root;

		public FileFinder(IFolder root)
		{
			_root = root;
		}

		public IEnumerable<IFile> FindAll(System.Predicate<string> pattern)
		{
			return FindAllIn(_root.GetAllFilesRecursively(), pattern);
		}

		public IEnumerable<IFile> FindAllIn(IEnumerable<IFile> fileSet, System.Predicate<string> pattern)
		{
			return from file in fileSet where pattern(RelativePathFor(file)) select file;
		}

		public string RelativePathFor(IFileSystemItem file)
		{
			Stack<string> path = new Stack<string>();
			IFileSystemItem current = file;
			do
			{
				path.Push(current.Name);
				current = current.Parent;
			}
			while (current != _root);
			return path.Skip(1).Aggregate(path.Peek(), (acc, item) => acc + @"\" + item);
		}
	}
}
