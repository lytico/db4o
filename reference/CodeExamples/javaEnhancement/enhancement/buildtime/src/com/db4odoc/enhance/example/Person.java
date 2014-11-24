package com.db4odoc.enhance.example;;


public class Person{

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
        return mother;
    }

    public void setMother(Person mother) {
        this.mother = mother;
    }
    
    public void setName(String name) {
        this.name = name;
    }

    public String getName()  {
        return name;
    }

    public String toString()  {
        return name;
    }
    

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
