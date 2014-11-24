/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.example;

public class Student {
	public String name;
	public int age;
	public Student tortue;
	public float size;

	public Student(int age, String name, float budget) {
		this(age,name,budget,null);
	}

	public Student(int age, String name, float size, Student tortue) {
		this.age = age;
		this.name = name;
		this.tortue=tortue;
		this.size=size;
	}

	public int getAge() {
		return age/*-1*/; // TODO
	}

	public float getSize() {
		return size;
	}

	public String getName() {
		return name;
	}

	public Student getTortue() {
		return tortue;
	}

	public String toString() {
		return name+"/"+age+"/"+tortue;
	}
}
