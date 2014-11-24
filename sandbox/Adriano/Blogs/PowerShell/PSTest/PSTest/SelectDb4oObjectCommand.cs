using System.Collections;
using System.Management.Automation;
using Db4objects.Db4o;

namespace CmdLets.Db4objects
{
    [Cmdlet(VerbsCommon.Select, "db4o-object")]
    public class SelectDb4oObjectCommand : Db4oObjectCommandBase
    {
        protected override void ProcessRecord()
        {
            using (var container = Db4oEmbedded.OpenFile(Configure(), DatabasePath))
            {
				DumpResults(container.Query<PSObject>());
            }
        }

        private void DumpResults(IEnumerable results)
        {
			foreach (var result in results)
			{
				if (Match((PSObject)result))
				{
					WriteObject(result);
				}
			}
        }

        private bool Match(PSObject o)
        {
			if (null == _value) return true;

            foreach (var property in o.Properties)
            {
                if (property.Value.GetType() == _value.GetType())
                {
                	if (property.Value.Equals(_value))
                	{
                		return true;
                	}
                }
            }

            return false;
        }

		[Parameter(Mandatory = false, HelpMessage = "Specifies the value to be looked up.")]
		public object Value
		{
			set
			{
				_value = value;
			}
		}

        private object _value;
    }
}