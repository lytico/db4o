package db4ounit.extensions;

import com.db4o.config.*;

public interface RuntimeConfigureAction {
	void apply(Configuration config);
}
