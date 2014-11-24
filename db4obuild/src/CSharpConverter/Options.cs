using System;

namespace CSharpConverter
{
	class Options
	{
		public static Options Parse(string[] args)
		{
			Options options = new Options();
			foreach (string arg in args)
			{
				if (null == arg || 0 == arg.Length) continue;
				
				if (arg.StartsWith("-"))
				{
					if ("-vb" == arg)
					{
						options.VisualBasicOutput = true;
					}
					else
					{
						InvalidArgument(arg);
					}					
				}
				else
				{
					if (null == options.SourceDir)
					{
						options.SourceDir = arg;
					}
					else
					{
						if (null != options.TargetDir) InvalidArgument(arg);
						options.TargetDir = arg;
					}
				}
			}
			return options;
		}
		
		static void InvalidArgument(string arg)
		{
			throw new ArgumentException("Invalid argument: " + arg, "args");
		}
		
		string _sourceDir;
		string _targetDir;
		bool _vb;
		
		private Options()
		{
		}
		
		public string SourceDir
		{
			get
			{
				return _sourceDir;
			}
			
			set
			{
				if (null == value) throw new ArgumentNullException("SourceDir");
				_sourceDir = value;
			}
		}
		
		public string TargetDir
		{
			get
			{
				return _targetDir;
			}
			
			set
			{
				if (null == value) throw new ArgumentNullException("TargetDir");
				_targetDir = value;
			}
		}
		
		public bool VisualBasicOutput
		{
			get
			{
				return _vb;
			}
			
			set
			{
				_vb = value;
			}
		}
	}	
}
