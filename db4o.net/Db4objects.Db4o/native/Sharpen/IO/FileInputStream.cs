/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.IO;

namespace Sharpen.IO {

    public class FileInputStream : InputStream {

        public FileInputStream(File file) : base(new FileStream(file.GetPath(), FileMode.Open, FileAccess.Read)) {
        }

    }
}
