package com.db4odoc.practises.relations;


import org.junit.Assert;
import org.junit.Test;

public class TestClubPersonRelationship {

    private static final Country USA = new Country("USA");

    @Test
    public void joinClub(){
        Person p = new Person("test","test", USA);
        final Club theClub = new Club();
        p.join(theClub);

        assertIsConsistent(p, theClub);
    }
    @Test
    public void addMember(){
        Person p = new Person("test","test", USA);
        final Club theClub = new Club();
        theClub.addMember(p);

        assertIsConsistent(p, theClub);
    }
    @Test
    public void removeMember(){
        Person p1 = new Person("test","test", USA);
        Person p2 = new Person("test","test", USA);
        final Club theClub = new Club();
        theClub.addMember(p1);
        theClub.addMember(p2);
        theClub.removeMember(p1);

        assertIsConsistent(p2, theClub);
        Assert.assertEquals(0,p1.memberOf().size());
        Assert.assertEquals(1,theClub.members().size());
    }
    @Test
    public void leaveClub(){
        Person p1 = new Person("test","test", USA);
        Person p2 = new Person("test","test", USA);
        final Club theClub = new Club();
        theClub.addMember(p1);
        theClub.addMember(p2);
        p1.leave(theClub);

        assertIsConsistent(p2, theClub);
        Assert.assertEquals(0,p1.memberOf().size());
        Assert.assertEquals(1,theClub.members().size());
    }
    @Test
    public void mulitpleClubs(){
        Person p1 = new Person("test","test", USA);
        Person p2 = new Person("test","test", USA);
        final Club club1 = new Club();
        final Club club2 = new Club();
        p1.join(club1);
        p1.join(club2);
        club1.addMember(p2);
        club2.addMember(p2);

        assertIsConsistent(p1, club1);
        assertIsConsistent(p1, club2);
        assertIsConsistent(p2, club1);
        assertIsConsistent(p1, club2);
    }

    private void assertIsConsistent(Person person, Club club) {
        Assert.assertTrue(person.memberOf().contains(club));
        Assert.assertTrue(club.members().contains(person));
    }
}
