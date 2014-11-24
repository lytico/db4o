using System;
using Db4objects.Db4o;

namespace Db4oDoc.Code.SystemInfo
{
    public class SystemInfoExamples
    {
        public static void Main(string[] args)
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                // #example: Freespace size info
                long freeSpaceSize = container.Ext().SystemInfo().FreespaceSize();
                Console.WriteLine("Freespace in bytes: {0}", freeSpaceSize);
                // #end example

                // #example: Freespace entry count info
                int freeSpaceEntries = container.Ext().SystemInfo().FreespaceEntryCount();
                Console.WriteLine("Freespace-entries count: {0}", freeSpaceEntries);
                // #end example

                // #example: Database size info
                long databaseSize = container.Ext().SystemInfo().TotalSize();
                Console.WriteLine("Database size: {0}", databaseSize);
                // #end example
            }
        }
    }
}