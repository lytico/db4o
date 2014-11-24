package com.db4odoc.practises.relations;


class Dog {

    // #example: Bidirectional 1-N relations. The dog has a owner
    private Person owner;

    // This setter ensures that the model is always consistent
    public void setOwner(Person owner) {
        if(null!=this.owner){
            Person oldOwner = this.owner;
            this.owner = null;
            oldOwner.removeOwnerShipOf(this);
        }
        if(null!=owner && !owner.ownedDogs().contains(this)) {
            owner.addOwnerShipOf(this);
        }
        this.owner = owner;
    }

    public Person getOwner() {
        return owner;
    }
    // #end example
}
