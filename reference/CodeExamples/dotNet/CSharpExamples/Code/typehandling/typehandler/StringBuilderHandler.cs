using System;
using System.Text;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4oDoc.Code.TypeHandling.TypeHandler
{
    ///
    /// This is a very simple example for a type handler.
    /// Take a look at the existing db4o type handlers.
    /// #example: A StringBuilder type handler
    internal class StringBuilderHandler : IValueTypeHandler
    {
        // #example: Delete the content
        public void Delete(IDeleteContext deleteContext)
        {
            SkipData(deleteContext);
        }

        private static void SkipData(IReadBuffer deleteContext)
        {
            int numBytes = deleteContext.ReadInt();
            deleteContext.Seek(deleteContext.Offset() + numBytes);
        }
        // #end example

        // #example: Defragment the content
        public void Defragment(IDefragmentContext defragmentContext)
        {
            SkipData(defragmentContext);
        }
        // #end example

        // #example: Write the StringBuilder
        public void Write(IWriteContext writeContext, object o)
        {
            StringBuilder builder = (StringBuilder) o;
            string str = builder.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            writeContext.WriteInt(bytes.Length);
            writeContext.WriteBytes(bytes);
        }
        // #end example

        // #example: Read the StringBuilder
        public object Read(IReadContext readContext)
        {
            int length = readContext.ReadInt();
            byte[] data = new byte[length];
            readContext.ReadBytes(data);
            return new StringBuilder(Encoding.UTF8.GetString(data));
        }
        // #end example
    }
    // #end example
}