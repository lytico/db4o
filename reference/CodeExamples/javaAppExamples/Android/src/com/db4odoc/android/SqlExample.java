/* Copyright (C) 2004 - 2007 db4objects Inc. http://www.db4o.com */
package com.db4odoc.android;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Map;

import com.db4odoc.android.RegistrationRecord;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;
import android.widget.TextView;

public class SqlExample {

	private static final String DATABASE_NAME = "android";
	private static final String DB_TABLE_PILOT = "pilot";
	private static final String DB_TABLE_CAR = "car";

	private SQLiteDatabase _db = null;
	private final Context _context;
	private final TextView _console;

	public SqlExample(Context context, TextView console) {
		_context = context;
		_console = console;
	}

	public SQLiteDatabase database() {
		if (null != _db) {
			return _db;
		}
		long startTime = 0;

		// #example: opening SQLite database
		SQLiteDatabase db = _context.openOrCreateDatabase(DATABASE_NAME,
				Context.MODE_PRIVATE, null);
		// #end example
		if (!schemaExists(db)) {
			createSchema(db);
		}
		logToConsole(startTime, "Database opened: ", false);
		_db = db;
		return db;
	}

	public void close() {
		if (null == _db) {
			return;
		}
		long startTime = System.currentTimeMillis();
		// #example: close SQLite
		_db.close();
		// #end example
		logToConsole(startTime, "Database committed and closed: ", false);
		_db = null;
	}

	public void fillUpDB() throws Exception {
		close();
		_context.deleteDatabase(DATABASE_NAME);
		SQLiteDatabase db = database();
		if (db != null) {
			long startTime = System.currentTimeMillis();
			for (int i = 0; i < 100; i++) {
				addCar(db, i);
			}
			logToConsole(startTime, "Stored 100 objects: ", false);
			startTime = System.currentTimeMillis();
		}
	}

	public Car selectCar() {
		// #example: select a car from SQLite
		SQLiteDatabase db = database();
		Cursor cursor = db.rawQuery(
				"SELECT c.model, p.name, p.points, r.id, r.year" + " FROM "
						+ DB_TABLE_CAR + " c, " + DB_TABLE_PILOT + " p "
						+ "WHERE c.pilot = p.id AND p.points = ?;",
				new String[] { "15" });
		cursor.moveToFirst();

		Pilot pilot = new Pilot();
		pilot.setName(cursor.getString(1));
		pilot.setPoints(cursor.getInt(2));

		Car car = new Car();
		car.setModel(cursor.getString(0));
		car.setPilot(pilot);

		// #end example
		return car;

	}

	public void deleteCar() {
		// #example: delete a car with SQLite
		SQLiteDatabase db = database();
		db.delete(DB_TABLE_CAR,
				"pilot in (select id from pilot where points = ?)",
				new String[]{"5"});
		// #end example
	}

	public void selectCarAndUpdate() {
		long startTime = System.currentTimeMillis();
	
		// #example: update a car with SQLite
		SQLiteDatabase db = database();
		db.execSQL("INSERT INTO REG_RECORDS (id,year) VALUES ('A1', DATETIME('NOW'))");
		
		ContentValues updateValues = new ContentValues();
		updateValues.put("reg_record", "A1");
		int count = db.update(DB_TABLE_CAR, updateValues,
				"pilot IN (SELECT id FROM " + DB_TABLE_PILOT
						+ " WHERE points = 15)", null);
		if (count == 0) {
			logToConsole(0, "Car not found, refill the database to continue.",
					false);
		} else {
			Cursor c = db.rawQuery("SELECT c.model, r.id, r.year from car c, "
					+ "REG_RECORDS r, pilot p where c.reg_record = r.id "
					+ "AND c.pilot = p.id AND p.points = 15;", null);
			if (c.getCount() == 0) {
				logToConsole(0,
						"Car not found, refill the database to continue.",
						false);
				return;
			}
			c.moveToFirst();
			String date = c.getString(2);
			
			Date dt = parseDate(date);
			RegistrationRecord record = new RegistrationRecord(c.getString(1),dt);
			
			Car car = new Car();
			car.setModel(c.getString(0));
			car.setRegistration(record);
			logToConsole(startTime, "Updated Car (" + car + "): ", true);
		}
		// #end example
	}

	public static void upgradeDatabase(SQLiteDatabase db) {
		// #example: upgrade schema in SQLite
		db.execSQL("CREATE TABLE IF NOT EXISTS REG_RECORDS ("
				+ "id TEXT PRIMARY KEY, year DATE);");
		db.execSQL("CREATE INDEX  IF NOT EXISTS  IDX_REG_RECORDS ON REG_RECORDS (id);");
		db.execSQL("ALTER TABLE " + DB_TABLE_CAR + " ADD reg_record TEXT;");
		// #end example
	
	}

	private Date parseDate(String date) {
		SimpleDateFormat sf = new SimpleDateFormat("yyyy-MM-dd H:mm:ss");
		try {
			return sf.parse(date);
		} catch (ParseException e) {
			Log.e(SqlExample.class.getName(), e.toString());
			return new Date();
		}
	}

	private void createSchema(SQLiteDatabase db) {
		// #example: SQLite create the schema
		db.execSQL("CREATE TABLE IF NOT EXISTS " + DB_TABLE_PILOT + " ("
				+ "id INTEGER PRIMARY KEY AUTOINCREMENT, "
				+ "name TEXT NOT NULL, points INTEGER NOT NULL);");
		// Foreign key constraint is parsed but not enforced
		// Here it is used for documentation purposes
		db.execSQL("CREATE TABLE IF NOT EXISTS " + DB_TABLE_CAR + " ("
				+ "id INTEGER PRIMARY KEY AUTOINCREMENT, "
				+ "model TEXT NOT NULL, pilot INTEGER NOT NULL,"
				+ "FOREIGN KEY (pilot)"
				+ "REFERENCES pilot(id) ON DELETE CASCADE);");
		db.execSQL("CREATE INDEX IF NOT EXISTS CAR_PILOT ON " + DB_TABLE_CAR
				+ " (pilot);");
		// #end example
		upgradeDatabase(db);

	}

	private void logToConsole(long startTime, String message, boolean add) {
		long diff = 0;
		if (startTime != 0) {
			diff = (System.currentTimeMillis() - startTime);
		}
		if (add) {
			_console.setText(_console.getText() + "\n" + message + diff
					+ " ms.");
		} else {
			_console.setText("SQLite: " + message + diff + " ms.");
		}
	}

	private boolean schemaExists(SQLiteDatabase db) {
		Cursor c = db.rawQuery("SELECT * FROM SQLITE_MASTER"
				+ " WHERE type LIKE 'table' AND name LIKE '" + DB_TABLE_PILOT
				+ "'", new String[0]);
		try {
			return c.getCount() == 1;
		} finally {
			c.close();
		}
	}

	private void addCar(SQLiteDatabase db, int number) {
		// #example: store a car in SQLite
		ContentValues initialValues = new ContentValues();

		initialValues.put("id", number);
		initialValues.put("name", "Tester");
		initialValues.put("points", number);
		db.insert(DB_TABLE_PILOT, null, initialValues);

		initialValues = new ContentValues();

		initialValues.put("model", "BMW");
		initialValues.put("pilot", number);
		db.insert(DB_TABLE_CAR, null, initialValues);
		// #end example
	}

}
