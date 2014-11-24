using System.Collections.Generic;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Collections;

namespace Db4oDoc.Code.TA.Collections
{
    // #example: Using the activation aware collections
    public class Team : ActivatableBase
    {
        private readonly IList<Pilot> pilots = new ActivatableList<Pilot>();

        public void Add(Pilot pilot)
        {
            Activate(ActivationPurpose.Write);
            pilots.Add(pilot);
        }

        public ICollection<Pilot> Pilots
        {
            get
            {
                Activate(ActivationPurpose.Read);
                return pilots;
            }
        }
    }
    // #end example
}