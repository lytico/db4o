package com.db4odoc.practises.relations;


import org.junit.Assert;
import org.junit.Test;

public class TestDogPersonRelation {

    private static final Country USA = new Country("USA");

    @Test
    public void addDogToPerson(){
        Person p = new Person("test","test", USA);
        final Dog dog = new Dog();
        p.addOwnerShipOf(dog);

        assertIsConsistent(p, dog);
    }

    @Test
    public void setOwner(){
        Person p = new Person("test","test", USA);
        final Dog dog = new Dog();
        dog.setOwner(p);

        assertIsConsistent(p, dog);
    }

    @Test
    public void settingOwnerRemovesOldOwner(){
        final Dog dog = new Dog();
        final Person oldOwner = new Person("old-owner", "old-owner", USA);
        dog.setOwner(oldOwner);
        Person p = new Person("test","test", USA);
        dog.setOwner(p);

        assertIsConsistent(p, dog);
        Assert.assertEquals(0,oldOwner.ownedDogs().size());
    }

    @Test
    public void removeOwner(){
        final Dog dog = new Dog();
        final Person oldOwner = new Person("old-owner", "old-owner", USA);
        dog.setOwner(oldOwner);
        dog.setOwner(null);

        Assert.assertNull(dog.getOwner());
        Assert.assertEquals(0,oldOwner.ownedDogs().size());
    }
    @Test
    public void removeOwnedDog(){
        final Dog dog = new Dog();
        final Person oldOwner = new Person("old-owner", "old-owner", USA);
        dog.setOwner(oldOwner);
        oldOwner.removeOwnerShipOf(dog);

        Assert.assertNull(dog.getOwner());
        Assert.assertEquals(0,oldOwner.ownedDogs().size());
    }
    @Test
    public void changeOwnerShip(){
        final Dog dog = new Dog();
        final Person oldOwner = new Person("old-owner", "old-owner", USA);
        oldOwner.addOwnerShipOf(dog);
        Person p = new Person("test","test", USA);
        p.addOwnerShipOf(dog);

        assertIsConsistent(p, dog);
        Assert.assertEquals(0,oldOwner.ownedDogs().size());
    }


    private void assertIsConsistent(Person p, Dog dog) {
        Assert.assertSame(p, dog.getOwner());
        Assert.assertTrue(p.ownedDogs().contains(dog));
    }
}
