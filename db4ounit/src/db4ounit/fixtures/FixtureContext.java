/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;

/**
 * Set of live {@link FixtureVariable}/value pairs.
 * 
 */
public class FixtureContext {
	
	private static final DynamicVariable _current = new DynamicVariable() {
		private final FixtureContext EMPTY_CONTEXT = new FixtureContext();
		
		protected Object defaultValue() {
			return EMPTY_CONTEXT;
		}
	};
	
	/**
	 * @sharpen.property
	 */
	public static FixtureContext current() {
		return (FixtureContext)_current.value();
	}
	
	public Object run(Closure4 closure) {
		return _current.with(this, closure);
	}

	public void run(Runnable block) {
		_current.with(this, block);
	}
	
	static class Found {
		public final Object value;
		
		public Found(Object value_) {
			value = value_;
		}
	}
	
	Found get(FixtureVariable fixture) {
		return null;
	}
	
	public FixtureContext combine(final FixtureContext parent) {
		return new FixtureContext() {
			Found get(FixtureVariable fixture) {
				Found found = FixtureContext.this.get(fixture);
				if (null != found) return found;
				return parent.get(fixture);
			}
		};
	}

	FixtureContext add(final FixtureVariable fixture, final Object value) {
		return new FixtureContext() {
			Found get(FixtureVariable key) {
				if (key == fixture) {
					return new Found(value);
				}
				return FixtureContext.this.get(key);
			}
		};
	}
}
