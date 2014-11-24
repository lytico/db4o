using System;
using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4oDoc.Code.Configuration.IO
{
    // #example: A logging bin decorator
    public class LoggingBin : BinDecorator, IBin
    {
        public LoggingBin(IBin bin) : base(bin)
        {
        }


        public override void Close()
        {
            Console.WriteLine("Called LoggingBin.Close()");
            base.Close();
        }


        public override long Length()
        {
            Console.WriteLine("Called LoggingBin.Length()");
            return base.Length();
        }


        public override int Read(long position, byte[] bytes, int bytesToRead)
        {
            Console.WriteLine("Called LoggingBin.Read(" + position + ", ...," + bytesToRead + ")");
            return base.Read(position, bytes, bytesToRead);
        }


        public override void Sync()
        {
            Console.WriteLine("Called LoggingBin.Sync()");
            base.Sync();
        }


        public override int SyncRead(long position, byte[] bytes, int bytesToRead)
        {
            Console.WriteLine("Called LoggingBin.SyncRead(" + position + ", ...," + bytesToRead + ")");
            return base.SyncRead(position, bytes, bytesToRead);
        }


        public override void Write(long position, byte[] bytes, int bytesToWrite)
        {
            Console.WriteLine("Called LoggingBin.Write(" + position + ", ...," + bytesToWrite + ")");
            base.Write(position, bytes, bytesToWrite);
        }


        public override void Sync(IRunnable runnable)
        {
            Console.WriteLine("Called LoggingBin.Sync(" + runnable + ")");
            base.Sync(runnable);
        }
    }
    // #end example
}