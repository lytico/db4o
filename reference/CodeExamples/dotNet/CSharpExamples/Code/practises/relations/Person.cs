using System.Collections.Generic;

namespace Db4oDoc.Code.Practises.Relations
{
    internal class Person
    {
        private string sirname;
        private string firstname;

        public Person(string sirname, string firstname, Country bornIn)
        {
            this.sirname = sirname;
            this.firstname = firstname;
            this.bornIn = bornIn;
        }


        // #example: Simple 1-n relation. Navigating from the person to the countries
        // Optionally we can index this field, when we want to find all people for a certain country
        private Country bornIn;

        public Country BornIn
        {
            get { return bornIn; }
        }
        // #end example

        // #example: One directional N-N relation
        private readonly ICollection<Country> citizenIn = new HashSet<Country>();

        public void RemoveCitizenship(Country o)
        {
            citizenIn.Remove(o);
        }

        public void AddCitizenship(Country country)
        {
            citizenIn.Add(country);
        }
        // #end example

        // #example: Bidirectional 1-N relations. The person has dogs
        private readonly ICollection<Dog> owns = new HashSet<Dog>();

        // The add and remove method ensure that the relations is always consistent
        public void AddOwnerShipOf(Dog dog)
        {
            owns.Add(dog);
            dog.Owner = this;
        }

        public void RemoveOwnerShipOf(Dog dog)
        {
            owns.Remove(dog);
            dog.Owner = null;
        }

        public IEnumerable<Dog> OwnedDogs
        {
            get { return owns; }
        }
        // #end example

        // #example: Bidirectional N-N relation
        private readonly ICollection<Club> memberIn = new HashSet<Club>();

        public void Join(Club club)
        {
            if (!memberIn.Contains(club))
            {
                memberIn.Add(club);
                club.AddMember(this);
            }
        }

        public void Leave(Club club)
        {
            if (memberIn.Contains(club))
            {
                memberIn.Remove(club);
                club.RemoveMember(this);
            }
        }

        public IEnumerable<Club> MemberOf
        {
            get { return memberIn; }
        }
        // #end example
    }
}