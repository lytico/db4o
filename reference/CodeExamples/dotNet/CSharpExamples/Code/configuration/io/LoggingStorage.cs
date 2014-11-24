using System;
using Db4objects.Db4o.IO;

namespace Db4oDoc.Code.Configuration.IO
{
    // #example: A logging storage decorator
    public class LoggingStorage : StorageDecorator, IStorage
    {
        public LoggingStorage(IStorage storage) : base(storage)
        {
        }

        public override bool Exists(string uri)
        {
            Console.WriteLine("Called: LoggingStorage.Exists(" + uri + ")");
            return base.Exists(uri);
        }

        public override IBin Open(BinConfiguration config)
        {
            Console.WriteLine("Called: LoggingStorage.Open(" + config + ")");
            return base.Open(config);
        }

        protected override IBin Decorate(BinConfiguration config, IBin bin)
        {
            return new LoggingBin(base.Decorate(config, bin));
        }

        public override void Delete(string uri)
        {
            Console.WriteLine("Called: LoggingStorage.Delete(" + uri + ")");
            base.Delete(uri);
        }

        public override void Rename(string oldUri, string newUri)
        {
            Console.WriteLine("Called: LoggingStorage.Rename(" + oldUri + "," + newUri + ")");
            base.Rename(oldUri, newUri);
        }
    }
    //#end example
}