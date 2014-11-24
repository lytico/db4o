namespace Db4oTool.Tests.Core
{
    public abstract class SingleResourceTestCase : AbstractCommandLineInstrumentationTestCase
    {
        protected override string[] Resources
        {
            get { return new string[] { ResourceName }; }
        }

        protected abstract string ResourceName
        {
            get;
        }
    }
}
