using System.Linq;

namespace Db4oDoc.Code.Practises.Relations
{
    internal class Dog
    {
        // #example: Bidirectional 1-N relations. The dog has a owner
        private Person owner;

        public Person Owner
        {
            get { return owner; }
            set
            {
                // This setter ensures that the model is always consistent
                if (null != this.owner)
                {
                    Person oldOwner = this.owner;
                    this.owner = null;
                    oldOwner.RemoveOwnerShipOf(this);
                }
                if (null != value && !value.OwnedDogs.Contains(this))
                {
                    value.AddOwnerShipOf(this);
                }
                this.owner = value;
            }
        }
        // #end example
    }
}