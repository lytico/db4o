package com.db4odoc.practises.relations;


import java.util.Collection;
import java.util.Collections;
import java.util.HashSet;
import java.util.Set;

class Club {
    // #example: Bidirectional N-N relation
    private Set<Person> members = new HashSet<Person>();

    public void addMember(Person person) {
        if (!members.contains(person)) {
            members.add(person);
            person.join(this);
        }
    }

    public void removeMember(Person person) {
        if (members.contains(person)) {
            members.remove(person);
            person.leave(this);
        }
    }
    // #end example

    public Collection<Person> members() {
        return Collections.unmodifiableCollection(members);
    }
}
