package com.db4o.internal.logging;

import java.lang.reflect.*;

import com.db4o.ext.*;
import com.db4o.internal.*;

public final class LoggingWrapper<T> implements Logging<T> {
	
	private static final Class[] loggerConstructorParameterTypes = new Class[] {LoggingWrapper.class, Level.class};

	private final Class<T> _logInterface;
	
	final T trace;
	final T debug;
	final T info;
	final T warn;
	final T error;
	final T fatal;

	private T _forward;
	private T nullImpl;
	
	private Level loggingLevel = null;
	private Constructor<T> _ctorLoggerClass;

	LoggingWrapper(Class<T> clazz) {
		_logInterface = clazz;
		try {
			
			String loggingImplBaseName = loggingSupportBaseName() + "_LoggingSupport"+ReflectPlatform.INNER_CLASS_SEPARATOR + loggingQualifiedBaseName();
			
			String loggerClassName = ReflectPlatform.adjustClassName(loggingImplBaseName + "Logger", clazz);
			String nullImplClassName = ReflectPlatform.adjustClassName(loggingImplBaseName + "Adapter", clazz);
			
			Class logerClass = ReflectPlatform.forName(loggerClassName);
			if (logerClass == null) {
				throw new IllegalArgumentException("Cannot find logging support for " + ReflectPlatform.simpleName(_logInterface));
			}
			
			_ctorLoggerClass = logerClass.getConstructor(loggerConstructorParameterTypes);
			nullImpl = (T) ReflectPlatform.createInstance(nullImplClassName);

		} catch (SecurityException e) {
			throw new java.lang.RuntimeException("Error accessing logging support for class " + clazz.getName(), e);
		} catch (NoSuchMethodException e) {
			throw new java.lang.RuntimeException("Error accessing logging support for class " + clazz.getName(), e);
		}
		trace = createProxy(Logger.TRACE);
		debug = createProxy(Logger.DEBUG);
		info = createProxy(Logger.INFO);
		warn = createProxy(Logger.WARN);
		error = createProxy(Logger.ERROR);
		fatal = createProxy(Logger.FATAL);
	}

	private String loggingSupportBaseName() {
		return ReflectPlatform.containerName(_logInterface) + "." + loggingQualifiedBaseName();
	}

	private String loggingQualifiedBaseName() {
		String simpleName = "";
		Class parent = _logInterface;
		while (parent != null) {
			if (simpleName.length() > 0) {
				simpleName = "_" + simpleName;
			}
			simpleName = ReflectPlatform.getJavaInterfaceSimpleName(parent) + simpleName;
			parent = parent.getEnclosingClass();
		}
		return simpleName;
	}

	private T createProxy(Level loggingLevel) {
		try {
			return ReflectPlatform.newInstance(_ctorLoggerClass, this, loggingLevel);
		} catch (Db4oException e) {
			throw new java.lang.RuntimeException("Error creating proxy", e);
		}
	}

	private T selectLevel(Level level, T logger) {
		if (level.ordinal() < loggingLevel().ordinal()) {
			return nullImpl;
		}

		return logger;
	}

	@Override
	public final T trace() {
		return selectLevel(Logger.TRACE, trace);
	}

	@Override
	public final T debug() {
		return selectLevel(Logger.DEBUG, debug);
	}

	@Override
	public final T info() {
		return selectLevel(Logger.INFO, info);
	}

	@Override
	public final T warn() {
		return selectLevel(Logger.WARN, warn);
	}

	@Override
	public final T error() {
		return selectLevel(Logger.ERROR, error);
	}

	@Override
	public final T fatal() {
		return selectLevel(Logger.FATAL, fatal);
	}

	@Override
	public void loggingLevel(Level loggingLevel) {
		this.loggingLevel = loggingLevel;
	}

	@Override
	public Level loggingLevel() {
		return loggingLevel == null ? Logger.loggingLevel : loggingLevel;
	}

	@Override
	public void forward(T forward) {
		_forward = forward;
	}

	@Override
	public T forward() {
		return _forward;
	}

	public void log(Level loggingLevel, String method, Object[] args) {
		Logger.rootInterceptor.log(loggingLevel, method, args);
	}
	
	public void exceptionCaughtInForward(String methodName, Object[] args, Throwable exceptionThrown) {
		Logger.rootInterceptor.log(Logger.WARN, "exceptionCaughtInForward", new Object[]{methodName});
	}
	
	public void pushCurrentLevel(Level level) {
		Logger.currentThreadLoggingLevel.set(level);
	}
	
	public void popCurrentLevel() {
		Logger.currentThreadLoggingLevel.set(null);
	}
}
