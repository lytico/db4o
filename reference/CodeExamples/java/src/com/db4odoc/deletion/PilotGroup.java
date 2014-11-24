package com.db4odoc.deletion;

import java.util.ArrayList;
import java.util.List;


class PilotGroup {
    private List<Pilot> pilots = new ArrayList<Pilot>();

    PilotGroup(Pilot...pilots) {
        for (Pilot pilot : pilots) {
            this.pilots.add(pilot);
        }
    }

    public List<Pilot> getPilots() {
        return pilots;
    }
}
