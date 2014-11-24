/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.convert;

import java.util.*;

import com.db4o.internal.convert.conversions.*;

/**
 * @exclude
 */
public class Converter {
    
    public static final int VERSION = VersionNumberToCommitTimestamp_8_0.VERSION;
    
    public static boolean convert(ConversionStage stage) {
    	if(!needsConversion(stage.converterVersion())) {
    		return false;
    	}
    	return instance().runConversions(stage);
    }
    
    private static Converter _instance;
    
    private Map<Integer, Conversion> _conversions;

	private int _minimumVersion = Integer.MAX_VALUE;
    
    private Converter() {
        _conversions = new HashMap<Integer, Conversion>();
        
        // TODO: There probably will be Java and .NET conversions
        //       Create Platform4.registerConversions() method ann
        //       call from here when needed.
        CommonConversions.register(this);
    }

	public static Converter instance() {
	    if(_instance == null){
    		_instance = new Converter();
    	}
    	return _instance;
    }

	public Conversion conversionFor(int version) {
	    return _conversions.get(version);
    }
	
	private static boolean needsConversion(final int converterVersion) {
	    return converterVersion < VERSION;
    }

    public void register(int introducedVersion, Conversion conversion) {
        if(_conversions.containsKey(introducedVersion)){
            throw new IllegalStateException();
        }
        if (introducedVersion < _minimumVersion) {
        	_minimumVersion  = introducedVersion;
        }
        _conversions.put(introducedVersion, conversion);
    }
    
    public boolean runConversions(ConversionStage stage) {
    	int startingVersion = Math.max(stage.converterVersion() + 1, _minimumVersion);
        for (int version = startingVersion; version <= VERSION; version++) {
            Conversion conversion = conversionFor(version);
            if (conversion == null) {
            	throw new IllegalStateException("Could not find a conversion for version " + version);
            }
            stage.accept(conversion);
        }
        return true;
    }
    
}
