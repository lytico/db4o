using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.Platform.AutoProperties
{
    // #example: Auto property
    class Person
    {
        public string Name { get; set; }
    }
    // #end example

    class AutoPropertyConfiguration
    {
        private void ConfigureAutoProperty()
        {
            // #example: Configure auto properties
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof(Person)).ObjectField("<Name>k__BackingField").Indexed(true);
            // #end example
        }
    }


    namespace HowItsImplemented
    {
        // #example: Auto property behind the scenes
        public class Person
        {
            // the name is actually <Name>k__BackingField 
            // but you cannot express that in C# code
            private string name;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
        }
        // #end example:
    
    }
}