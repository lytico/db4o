/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */

using Sharpen;

namespace Db4objects.Db4o.Bench.Util
{
	public class IoBenchmarkArgumentParser
	{
		private string _resultsFile2;

		private string _resultsFile1;

		private int _objectCount;

		private bool _delayed;

		public IoBenchmarkArgumentParser(string[] arguments)
		{
			ValidateArguments(arguments);
		}

		private void ValidateArguments(string[] arguments)
		{
			if (arguments.Length != 1 && arguments.Length != 3)
			{
				Sharpen.Runtime.Out.WriteLine("Usage: IoBenchmark <object-count> [<results-file-1> <results-file-2>]"
					);
                throw new System.Exception("Usage: IoBenchmark <object-count> [<results-file-1> <results-file-2>]");
			}
			_objectCount = int.Parse(arguments[0]);
			if (arguments.Length > 1)
			{
				_resultsFile1 = arguments[1];
				_resultsFile2 = arguments[2];
				_delayed = true;
			}
		}

		public virtual int ObjectCount()
		{
			return _objectCount;
		}

		public virtual string ResultsFile1()
		{
			return _resultsFile1;
		}

		public virtual string ResultsFile2()
		{
			return _resultsFile2;
		}

		public virtual bool Delayed()
		{
			return _delayed;
		}
	}
}
