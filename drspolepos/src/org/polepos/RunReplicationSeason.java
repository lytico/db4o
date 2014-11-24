package org.polepos;


import java.util.*;

import org.polepos.circuits.kyalami.*;
import org.polepos.drs.*;
import org.polepos.framework.*;


/**
 * @exclude
 */
public class RunReplicationSeason {
    
    public static void main(String[] arguments) {
        List <Circuit> circuits = new ArrayList <Circuit>();
        List <Team> teams = new ArrayList <Team>();
        
        circuits.add(new Kyalami());
        teams.add(new DrsTeam());
        
        new Racer(circuits, teams).run();
        
    }

}
