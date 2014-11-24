using System.IO;
using System.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Types;
using File = Sharpen.IO.File;

namespace Db4oDoc.Code.TypeHandling.Blob
{
    public class BlobStorage
    {
        private IBlob blob;

        public BlobStorage()
        {
        }

        public void ReadFileIntoDb(File fileToStore)
        {
            // #example: Store the file as a db4o-blob
            blob.ReadFrom(fileToStore);
            // #end example
            WaitTillDbIsFinished();
        }

        public File ReadFromDbIntoFile()
        {
            File file = CreateTemporaryFile();
            // #example: Load a blob from a db4o-blob
            blob.WriteTo(file);
            // #end example
            WaitTillDbIsFinished();
            return file;
        }


        /// unfortunately there's no callback for blobs. So the only way it to poll for it
        private void WaitTillDbIsFinished()
        {
            // #example: wait until the operation is done
            while (blob.GetStatus() > Status.Completed)
            {
                Thread.Sleep(50);
            }
            // #end example
        }

        /// unfortunately the db4o-blob-type doesn't support streams. The only way is to use
        /// files. Therefore we create temporary-files
        private static File CreateTemporaryFile()
        {
            string path = Path.GetTempPath() + Path.GetRandomFileName();
            return Directory.Exists(path) ? CreateTemporaryFile() : new File(path);
        }
    }

    public class BlobExamples
    {
        public static void Main(string[] args)
        {
            StoreBlob();
            ReadBlob();
        }

        private static void ReadBlob()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                BlobStorage blob = container.Query<BlobStorage>()[0];
                File file = blob.ReadFromDbIntoFile();
            }
        }

        private static void StoreBlob()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                BlobStorage blob = new BlobStorage();
                container.Store(blob);
                blob.ReadFileIntoDb(new File("C:\\Pictures\\IMG_1.jpg"));
            }
        }
    }
}