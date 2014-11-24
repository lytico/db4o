package com.db4odoc.query.nq;

import com.db4o.query.Predicate;

// #example: Query as class
class AllJohns extends Predicate<Pilot> {
    @Override
    public boolean match(Pilot pilot) {
        return pilot.getName().equals("John");
    }
}
// #end example
