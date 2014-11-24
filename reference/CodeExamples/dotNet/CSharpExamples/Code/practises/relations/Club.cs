using System.Collections.Generic;

namespace Db4oDoc.Code.Practises.Relations
{
    internal class Club
    {
        // #example: Bidirectional N-N relation
        private readonly ICollection<Person> members = new HashSet<Person>();

        public void AddMember(Person person)
        {
            if (!members.Contains(person))
            {
                members.Add(person);
                person.Join(this);
            }
        }

        public void RemoveMember(Person person)
        {
            if (members.Contains(person))
            {
                members.Remove(person);
                person.Leave(this);
            }
        }

        // #end example

        public IEnumerable<Person> Members
        {
            get { return members; }
        }
    }
}