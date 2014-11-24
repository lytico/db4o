/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.IO;

namespace Sharpen.IO {

    public class FileOutputStream : OutputStream {

        public FileOutputStream(File file) : base(new FileStream(file.GetPath(), FileMode.Create, FileAccess.Write)) {
        }

    }
}
