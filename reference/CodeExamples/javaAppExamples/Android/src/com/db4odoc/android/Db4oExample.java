package com.db4odoc.android;


import android.content.Context;
import android.widget.TextView;
import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.AndroidSupport;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;

import java.io.File;
import java.util.Date;


public class Db4oExample {
	
	private static ObjectContainer container = null;
	private Context context = null;
	private TextView console = null;
	
	
	public Db4oExample(Context context, TextView console){
		this.context = context;
		this.console = console;
	}

	
	public ObjectContainer database(){
		long startTime = 0;
		try {
			if(container == null){
				startTime = System.currentTimeMillis();
				// #example: open a db4o database
				String filePath = context.getFilesDir() + "/android.db4o";
				ObjectContainer db = Db4oEmbedded.openFile(configure(), filePath);
				// #end example
				container = db;
			}
		} catch (Exception e) {
            e.printStackTrace();
			return null;
		}
		logToConsole(startTime, "Database opened: ", false);
		return container;
	}
	

	public void close() {
		if(container != null){
			long startTime = System.currentTimeMillis();
			// #example: close db4o
			container.close();
			// #end example
			logToConsole(startTime, "Database committed and closed: ", false);
			container = null;
		}
	}
	
	
	public void fillUpDB() throws Exception {
		close();
		String filePath = context.getFilesDir() + "/android.db4o";
		new File(filePath).delete();
		ObjectContainer container=database();
		if (container != null){
			long startTime = System.currentTimeMillis();
			for (int i=0; i<100;i++){
				addCar(container,i);
			}
			logToConsole(startTime, "Stored 100 objects: ", false);
			startTime = System.currentTimeMillis();
		}
	}
	
	public Car selectCar(){
		// #example: select a car from db4o
		ObjectContainer db = database();
		ObjectSet<Car> cars = db.query(new Predicate<Car>() {
			@Override
			public boolean match(Car car) {
				return car.getPilot().getPoints() == 15;
			}
		});
		
		Car car = cars.get(0);
		// #end example
		return car;
		
	}

	
	public void selectCarAndUpdate() {
		long startTime = System.currentTimeMillis();
		
		// #example: update a car with db4o
		ObjectContainer container = database();
		if (container != null){
			ObjectSet<Car>  result = container.query(new Predicate<Car>(){
				@Override
				public boolean match(Car car) {
					return car.getPilot().getPoints()==15;
				}
				
			});
			if (!result.hasNext()){
				logToConsole(0, "Car not found, refill the database to continue.", false);
			} else {
				Car car = result.next();
				logToConsole(startTime, "Selected Car (" + car + "): ", false);
				startTime = System.currentTimeMillis();
				car.setRegistration(new RegistrationRecord("A1", new Date()));
				logToConsole(startTime, "Updated Car (" + car + "): ", true);
			}			
		}
		// #end example
	}
	
	public void deleteCar() {
		// #example: delete a car with db4o
		ObjectContainer db = database();
		ObjectSet<Car> cars = db.query(new Predicate<Car>() {
			public boolean match(Car car) {
				return car.getPilot().getPoints()==5;
			}			
		});
		for(Car car : cars){
			db.delete(car);
		}
		// #end example
	}
	
	private void logToConsole(long startTime, String message, boolean add) {
		if (console != null){
			long diff = 0;
			if (startTime != 0){
				diff = (System.currentTimeMillis() - startTime);
			} 
			if (add){
				console.setText(console.getText() + "\n" + message + diff + " ms.");
			} else {
				console.setText("db4o: " + message + diff + " ms.");
			}
		}
	}
	
	private static EmbeddedConfiguration configure(){
		// #example: configure db4o
		EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().add(new AndroidSupport());
		configuration.common().objectClass(Car.class).objectField("pilot").indexed(true);
		configuration.common().objectClass(Pilot.class).objectField("points").indexed(true);
		// #end example
		return configuration;
	}

	
	private static void addCar(ObjectContainer container, int points)
	{
		// #example: store a car in db4o
		Car car = new Car("BMW");
		car.setPilot(new Pilot("Tester", points));
		container.store(car);
		// #end example
	}
		
}

