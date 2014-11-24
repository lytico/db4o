package profiling;

import com.db4o.reflect.jdk.*;

public class CustomItemReflector extends JdkReflector {

	public CustomItemReflector() {
		super(Item.class.getClassLoader());
	}
	
	protected JdkClass createClass(Class clazz) {
		if (clazz == Item.class) {
			return new CustomItemClass(_parent, this);
		}
		return super.createClass(clazz);
	}

}
