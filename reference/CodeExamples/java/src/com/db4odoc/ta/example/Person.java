package com.db4odoc.ta.example;

// #example: Implement the required activatable interface and add activator
import com.db4o.activation.ActivationPurpose;
import com.db4o.activation.Activator;
import com.db4o.ta.Activatable;

public class Person implements Activatable{

    private transient Activator activator;
    // #end example 

	private String name;
	private Person mother;

    public Person(String name)  {
        this.name=name;
    }

    public Person(String name, Person mother)  {
        this.name=name;
        this.mother=mother;
    }


    public Person getMother() {
        activate(ActivationPurpose.READ);
        return mother;
    }

    public void setMother(Person mother) {
        activate(ActivationPurpose.WRITE);
        this.mother = mother;
    }
    // #example: Call the activate method on every field access
    public void setName(String name) {
        activate(ActivationPurpose.WRITE);
        this.name = name;
    }

    public String getName()  {
        activate(ActivationPurpose.READ);
        return name;
    }

    public String toString()  {
        // use the getter/setter withing the class,
        // to ensure the activate-method is called
        return getName();
    }
    // #end example

    // #example: Implement the activatable interface methods
    public void bind(Activator activator) {
        if (this.activator == activator) {
            return;
        }
        if (activator != null && null != this.activator) {
            throw new IllegalStateException("Object can only be bound to one activator");
        }
        this.activator = activator;
    }

    public void activate(ActivationPurpose activationPurpose) {
        if(null!=activator){
            activator.activate(activationPurpose);
        }
    }
    // #end example

    public static Person personWithHistory(){
        return createFamily(10);
    }

    private static Person createFamily(int generation) {
        if(0<generation){
            int previousGeneration = generation-1;
            return new Person("Joanna the "+generation,
                    createFamily(previousGeneration));
        } else{
            return null;
        }
    }
}