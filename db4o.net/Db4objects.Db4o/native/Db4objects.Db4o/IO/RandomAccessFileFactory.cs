using System;
using System.IO;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Sharpen.IO;
using File=Sharpen.IO.File;

namespace Db4objects.Db4o.IO
{
    public class RandomAccessFileFactory
    {
        public static RandomAccessFile NewRandomAccessFile(String path, bool readOnly, bool lockFile)
        {
            RandomAccessFile raf = null;
            bool ok = false;
            try
            {
                raf = new RandomAccessFile(path, readOnly, lockFile);
                if (lockFile)
                {
                    Platform4.LockFile(path, raf);
                }
                ok = true;
                return raf;
            }
            catch (IOException x)
            {
                if (new File(path).Exists())
                {
                    throw new DatabaseFileLockedException(path, x);
                }
                throw new Db4oIOException(x);
            } 
            finally
            {
                if(!ok && raf != null)
                {
                    raf.Close();
                }
            }
        }
    }
}
