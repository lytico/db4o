package com.db4odoc.android;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;

import android.app.Activity;
import android.os.Bundle;
import com.db4o.config.AndroidSupport;
import com.db4o.config.EmbeddedConfiguration;

// #example: open db4o on Android
public class Db4oOnAndroidExample  extends Activity  {	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
		String filePath = this.getFilesDir() + "/android.db4o";
        final EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.common().add(new AndroidSupport());
        ObjectContainer db = Db4oEmbedded.openFile(config,filePath);
		// do your stuff
		db.close();
    	
    }
}
// #end example
