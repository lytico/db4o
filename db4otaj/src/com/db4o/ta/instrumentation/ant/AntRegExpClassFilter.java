package com.db4o.ta.instrumentation.ant;

import org.apache.tools.ant.util.regexp.*;

import com.db4o.instrumentation.core.*;

/**
 * @exclude
 */
class AntRegExpClassFilter implements ClassFilter {
	private final Regexp[] _regExp;

	public AntRegExpClassFilter(Regexp[] regExp) {
        _regExp = regExp;
	}

	public boolean accept(Class clazz) {
		for (int reIdx = 0; reIdx < _regExp.length; reIdx++) {
			Regexp re = _regExp[reIdx];
			if(re.matches(clazz.getName())) {
				return true;
			}
		}
		return false;
	}
}
