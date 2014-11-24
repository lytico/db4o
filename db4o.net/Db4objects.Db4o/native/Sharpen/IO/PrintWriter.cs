/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.IO;

namespace Sharpen.IO {

    public class PrintWriter {

    	private readonly TextWriter _out;
    	private readonly bool _autoflush;

		public PrintWriter(TextWriter @out, bool autoflush)
		{
			this._out = @out;
			_autoflush = autoflush;
		}

		public PrintWriter(OutputStream @out, bool autoflush)
		{
			this._out = new StreamWriter(@out.UnderlyingStream);
			_autoflush = autoflush;
		}

		public void Println(string msg)
		{
			_out.WriteLine(msg);
			AutoFlush();
		}

    	private void AutoFlush()
    	{
			if (_autoflush)
			{
				_out.Flush();
			}
    	}
    }

}
