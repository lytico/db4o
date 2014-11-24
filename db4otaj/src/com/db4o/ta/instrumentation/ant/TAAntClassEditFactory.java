package com.db4o.ta.instrumentation.ant;

import java.util.*;

import org.apache.tools.ant.*;
import org.apache.tools.ant.types.*;
import org.apache.tools.ant.util.regexp.*;

import com.db4o.instrumentation.ant.*;
import com.db4o.instrumentation.classfilter.*;
import com.db4o.instrumentation.core.*;
import com.db4o.ta.instrumentation.*;

/**
 * @exclude
 */
public class TAAntClassEditFactory extends ProjectComponent implements AntClassEditFactory {

	private final List _regExp = new ArrayList();
	private final List _filters = new ArrayList();
	private final boolean _withCollections;

	public TAAntClassEditFactory() {
		this(true);
	}

	public TAAntClassEditFactory(boolean withCollections) {
		_withCollections = withCollections;
	}
	
	public RegularExpression createRegexp() {
        RegularExpression regExp = new RegularExpression();
        _regExp.add(regExp);
        return regExp;
	}
	
	public void add(ClassFilter classFilter) {
		_filters.add(classFilter);
	}
	
	public BloatClassEdit createEdit(ClassFilter clazzFilter) {
		final List filters = new ArrayList(2);
		for (Iterator filterIter = _filters.iterator(); filterIter.hasNext();) {
			ClassFilter filter = (ClassFilter) filterIter.next();
			filters.add(filter);
		}
		if(!_regExp.isEmpty()) {
			Regexp[] regExp = new Regexp[_regExp.size()];
			int idx = 0;
			for (Iterator reIter = _regExp.iterator(); reIter.hasNext();) {
				RegularExpression re = (RegularExpression) reIter.next();
				regExp[idx++] = re.getRegexp(getProject());
			}
			filters.add(new AntRegExpClassFilter(regExp));
		}
		ClassFilter userClassFilter = null;
		switch(filters.size()) {
			case 0:
				userClassFilter = new AcceptAllClassesFilter();
				break;
			case 1:
				userClassFilter = (ClassFilter) filters.get(0);
				break;
			default:
				userClassFilter = new CompositeOrClassFilter((ClassFilter[]) filters.toArray(new ClassFilter[filters.size()]));
		}
		if(clazzFilter != null) {
			userClassFilter = new CompositeAndClassFilter(new ClassFilter[]{ clazzFilter, userClassFilter });
		}
		return new InjectTransparentActivationEdit(userClassFilter, _withCollections);
	}

}
