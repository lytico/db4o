using System.ComponentModel;
using System.Management.Automation;

[RunInstaller(true)]
public class GetProcPSSnapIn01 : PSSnapIn
{
    /// <summary>
    /// Specify the name of the PowerShell snap-in.
    /// </summary>
    public override string Name
    {
        get
        {
            return "Db4o.PowerShellSnapin";
        }
    }

    /// <summary>
    /// Specify the vendor for the PowerShell snap-in.
    /// </summary>
    public override string Vendor
    {
        get
        {
            return "Db4o";
        }
    }

    /// <summary>
    /// Specify the localization resource information for the vendor. 
    /// Use the format: resourceBaseName,VendorName. 
    /// </summary>
    public override string VendorResource
    {
        get
        {
			return "Db4o.PowerShellSnapin,Db4o";
        }
    }

    /// <summary>
    /// Specify a description of the PowerShell snap-in.
    /// </summary>
    public override string Description
    {
        get
        {
            return "This is a PowerShell snap-in that includes the add-db4o-object cmdlet.";
        }
    }

    /// <summary>
    /// Specify the localization resource information for the description. 
    /// Use the format: resourceBaseName,Description. 
    /// </summary>
    public override string DescriptionResource
    {
        get
        {
			return "Db4o.PowerShellSnapin, This is a PowerShell snap-in that includes the add-db4o-object cmdlet.";
        }
    }
}
