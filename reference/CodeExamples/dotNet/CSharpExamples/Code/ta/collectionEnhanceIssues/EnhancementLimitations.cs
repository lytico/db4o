using System.Collections.Generic;
using Db4objects.Db4o.Collections;

namespace Db4oDoc.Code.TA.CollectionEnhanceIssues
{
    // #example: Can be enhanced by the db4o-tools
    public class CanBeEnhanced
    {
        private IList<string> _names = new List<string>();

        public bool ContainsName(string item)
        {
            return _names.Contains(item);
        }

        public void AddName(string item)
        {
            _names.Add(item);
        }
    }
    // #end example
    namespace AfterEnhancement
    {
        // #example: Is enhanced to
        public class CanBeEnhanced
        {
            private IList<string> _names = new ActivatableList<string>();

            public bool ContainsName(string item)
            {
                return _names.Contains(item);
            }

            public void AddName(string item)
            {
                _names.Add(item);
            }
        }
        // #end example
    
    }


    // #example: Cannot be enhanced by the db4o-tools
    public class CannotBeEnhanced
    {
        // cannot be enhanced, because it uses the concrete type
        private List<string> _names = new List<string>();

        public bool ContainsName(string item)
        {
            return _names.Contains(item);
        }

        public void AddName(string item)
        {
            _names.Add(item);
        }
    }
    // #end example
}