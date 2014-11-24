using System;
using System.ComponentModel.DataAnnotations;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;

namespace Db4oDoc.Code.Validation
{
    public class DataValidation
    {
        public static void Main(string[] args)
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4"))
            {
                // #example: Register validation for the create and update event
                IEventRegistry events = EventRegistryFactory.ForObjectContainer(container);
                events.Creating += ValidateObject;
                events.Updating += ValidateObject;
                // #end example


                // #example: Storing a valid pilot
                var pilot = new Pilot {Name = "Joe"};
                container.Store(pilot);
                // #end example


                // #example: Storing a invalid pilot throws exception
                var otherPilot = new Pilot {Name = ""};
                try
                {
                    container.Store(otherPilot);
                }
                catch (EventException e)
                {
                    ValidationException cause = (ValidationException) e.InnerException;
                    Console.WriteLine(cause.ValidationResult.ErrorMessage);
                }
                // #end example
            }
        }

        // #example: Validation support
        private static void ValidateObject(object sender,
                                           CancellableObjectEventArgs eventInfo)
        {
            ValidationContext context = new ValidationContext(eventInfo.Object, null, null);
            // This throws when the object isn't valid.
            Validator.ValidateObject(eventInfo.Object, context, true);
        }
        // #end example 
    }

    // #example: Validation attributes
    class Pilot
    {
        private string name;

        [Required]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
    // #end example

    internal class Car
    {
        private string name;
        private Pilot drivenBy;

        public Car(string name)
        {
            this.name = name;
        }

        [Required]
        public Pilot DrivenBy
        {
            get { return drivenBy; }
            set { drivenBy = value; }
        }

        [Required]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}