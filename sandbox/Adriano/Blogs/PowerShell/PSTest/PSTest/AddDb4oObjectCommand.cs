using System;
using System.Management.Automation;
using Db4objects.Db4o;

namespace CmdLets.Db4objects
{
    [Cmdlet(VerbsCommon.Add , "db4o-object")]
    public class AddDb4oObjectCommand : Db4oObjectCommandBase
    {
        protected override void ProcessRecord()
        {
            using (var container = Db4oEmbedded.OpenFile(Configure(), DatabasePath))
            {
                if (null != Item)
                {
					container.Store(Item);
                }
            }
        }

        [Parameter(Position = 0)]
        public PSObject Item
        {
            get; set;
        }
    }
}
