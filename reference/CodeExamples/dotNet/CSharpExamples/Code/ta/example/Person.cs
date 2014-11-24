using System;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4oDoc.Ta.Example
{
    // #example: Implement the required activatable interface and add activator
    public class Person : IActivatable
    {
        [NonSerialized] private IActivator activator;
        // #end example 

        private string name;
        private Person mother;

        public Person(string name)
        {
            this.name = name;
        }

        public Person(string name, Person mother)
        {
            this.name = name;
            this.mother = mother;
        }

        public Person Mother
        {
            get
            {
                Activate(ActivationPurpose.Read);
                return mother;
            }
            set
            {
                Activate(ActivationPurpose.Write);
                mother = value;
            }
        }

        // #example: Call the activate method on every field access
        public string Name
        {
            get
            {
                Activate(ActivationPurpose.Read);
                return name;
            }
            set
            {
                Activate(ActivationPurpose.Write);
                name = value;
            }
        }


        public override string ToString()
        {
            // use the getter/setter withing the class,
            // to ensure the activate-method is called
            return Name;
        }
        // #end example

        // #end example

        // #example: Implement the activatable interface methods
        public void Bind(IActivator activator)
        {
            if (this.activator == activator)
            {
                return;
            }
            if (activator != null && null != this.activator)
            {
                throw new InvalidOperationException("Object can only be bound to one activator");
            }
            this.activator = activator;
        }

        public void Activate(ActivationPurpose activationPurpose)
        {
            if (null != activator)
            {
                activator.Activate(activationPurpose);
            }
        }

        // #end example

        public static Person PersonWithHistory()
        {
            return CreateFamily(10);
        }

        private static Person CreateFamily(int generation)
        {
            if (0 < generation)
            {
                int previousGeneration = generation - 1;
                return new Person("Joanna the " + generation,
                                  CreateFamily(previousGeneration));
            }
            else
            {
                return null;
            }
        }
    }
}