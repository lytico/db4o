/* Copyright (C) 2004 - 2011 Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4objects.Db4o.Bench.Logging;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;

public class LoggingStorage : StorageDecorator 
{
	public static int LOG_READ 	= 1;
	public static int LOG_WRITE = 2;
	public static int LOG_SYNC 	= 4;
	public static int LOG_SEEK  = 8;
	
	public static int LOG_ALL = LOG_READ + LOG_WRITE + LOG_SYNC + LOG_SEEK;
	
	private string _fileName;
	private int _config;

    public LoggingStorage(IStorage delegateAdapter, string fileName) : this(delegateAdapter, fileName, LOG_ALL)
	{
    }

    public LoggingStorage(IStorage IStorage, string fileName, int config) :base(IStorage)
	{
        _fileName = fileName;
        _config = config;
    }
    
	protected override IBin Decorate(BinConfiguration config, IBin bin) 
	{
        try 
		{
			var file = new FileStream(_fileName, FileMode.Create);

			var @out = new StreamWriter(file);
			return new LoggingBin(bin, @out, _config);
		} 
        catch(FileNotFoundException e) 
		{
			throw new Db4oIOException(e);
		}
	}
	
	class LoggingBin : BinDecorator 
	{
		private TextWriter _out;
		private int _config;

		public LoggingBin(IBin bin, TextWriter @out, int config) : base(bin)
		{
			_out = @out;
			_config = config;
		}
		
		public override void Close()
		{
			_out.Flush();
			_out.Close();
			base.Close();
		}
	
	    public override int Read(long pos, byte[] bytes, int length) 
		{
	    	if(Config(LOG_READ)) {
	    		Println(LogConstants.ReadEntry + pos + " " + length);
	    	}
	        return _bin.Read(pos, bytes, length);
	    }
	
	    public override void Sync() 
		{
	    	if(Config(LOG_SYNC)) {
	    		Println(LogConstants.SyncEntry);
	    	}
	        _bin.Sync();
	    }
	
	    public override void Write(long pos, byte[] buffer, int length)
		{
	    	if(Config(LOG_WRITE)) {
	    		Println(LogConstants.WriteEntry + pos + " " + length);
	    	}
	        _bin.Write(pos, buffer, length);
		}
	    
	    private void Println(string s)
		{
	    	_out.WriteLine("{0}", s);
	    }

	    private bool Config(int mask) 
		{
	    	return (_config & mask) != 0;
	    }
	}
    
}
