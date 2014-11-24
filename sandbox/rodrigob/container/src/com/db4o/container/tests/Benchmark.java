package com.db4o.container.tests;

import com.db4o.container.*;
import com.db4o.container.tests.internal.*;

public class Benchmark {
	
	public static void main(String[] args) {
	    long t0 = time("raw object instantiation", new Runnable() {
			public void run() {
				new SimpleServiceImpl().toString();
			}
		});
	    
	    long t1 = time("complex object instantiation", new Runnable() {
			public void run() {
				new ComplexServiceImpl(new SingletonServiceImpl(), new SimpleServiceImpl()).toString();
			}
		});
	    
	    final Container container = ContainerFactory.newContainer();
	    long t2 = time("container service instantiation", new Runnable() {
			public void run() {
				container.produce(SimpleService.class).toString();
			}
		});
	    long t3 = time("container complex instantiation", new Runnable() {
			public void run() {
				container.produce(ComplexService.class).toString();
			}
		});
	    System.out.println("Overhead for simple case is " + overhead(t0, t2) + "%");
	    System.out.println("Overhead for complex case is " + overhead(t1, t3) + "%");
    }

	private static float overhead(long t0, long t1) {
	    return (((((float)t1)/t0)-1)*100);
    }

	private static long time(String label, Runnable block) {
		final long t0 = System.nanoTime();
		for (int i=0; i<2000000; ++i)
			block.run();
		final long elapsed = System.nanoTime() - t0;
		System.out.println(label + ": " + (elapsed/1000000) + "ms");
		return elapsed;
    }
}
