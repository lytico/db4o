package com.db4o.db4ounit.common.io;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class StackBasedLimitedSpaceThrowCondition implements ThrowCondition {

	private boolean _enabled;
	private long _sizeLimit;
	private final StackBasedConfiguration _config;
	private int _hitCount;
	
	public StackBasedLimitedSpaceThrowCondition(StackBasedConfiguration config) {
		_config = config;
		_sizeLimit = -1;
		_hitCount = 0;
	}
	
	public void enabled(boolean enabled) {
		_enabled = enabled;
	}
	
	public boolean shallThrow(long pos, int numBytes) {
		if(!_enabled) {
			return false;
		}
		if(_sizeLimit < 0) {
			if(!matchesStackTrace()) {
				return false;
			}
			_hitCount++;
			if(_hitCount < _config._hitThreshold) {
				return false;
			}
			_sizeLimit = pos + numBytes + 1;
			return false;
		}
		boolean exceedsLimit = pos + numBytes >= _sizeLimit;
		return exceedsLimit;
	}

	private boolean matchesStackTrace() {
		StackTraceElement[] stackTrace = new Exception().getStackTrace();
		for (StackTraceElement stackFrame : stackTrace) {
			if(stackFrame.getClassName().equals(_config._className) && stackFrame.getMethodName().equals(_config._methodName)) {
				return true;
			}
		}
		return false;
	}

}
