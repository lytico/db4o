package com.db4odoc.android;


import com.db4odoc.android.R;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

public class AndroidExample extends Activity {
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
		setContentView(R.layout.main);

		final TextView console = (TextView) findViewById(R.id.console);
		console.setText("Compare db4o vs SQLite");
		TextView consoleDb4o = (TextView) findViewById(R.id.db4o_console);
		TextView consoleSql = (TextView) findViewById(R.id.sqlite_console);

		// Initialize database modules
		final Db4oExample db4o = new Db4oExample(this, consoleDb4o);
		final SqlExample sqlLite = new SqlExample(this, consoleSql);

		Button openButton = (Button) findViewById(R.id.open_button);

		openButton.setOnClickListener(new View.OnClickListener() {

			public void onClick(View arg0) {
				db4o.database();
				sqlLite.database();
			}
		});

		Button storeButton = (Button) findViewById(R.id.store_button);

		storeButton.setOnClickListener(new View.OnClickListener() {

			public void onClick(View arg0) {
				try {
					db4o.fillUpDB();
					sqlLite.fillUpDB();
				} catch (Exception e) {
					console.setText("Unexpected exception: " + e.getMessage());
				}
			}
		});

		Button retrieveButton = (Button) findViewById(R.id.retrieve_button);

		retrieveButton.setOnClickListener(new View.OnClickListener() {

			public void onClick(View arg0){
				try{
					db4o.selectCarAndUpdate();
					sqlLite.selectCarAndUpdate();					
				}catch (Exception e) {
					console.setText("Unexpected exception: " + e.getMessage());					
				}
			}
		});

		Button closeButton = (Button) findViewById(R.id.close_button);

		closeButton.setOnClickListener(new View.OnClickListener() {

			public void onClick(View arg0) {
				db4o.close();
				sqlLite.close();
			}
		});
        
    }
}