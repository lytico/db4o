package profiling;

import java.lang.reflect.*;

import com.db4o.reflect.*;
import com.db4o.reflect.jdk.*;

public class CustomItemClass extends JdkClass {

	public CustomItemClass(Reflector reflector, CustomItemReflector customItemReflector) {
		super(reflector, customItemReflector, Item.class);
	}
	
	public Object newInstance() {
	    return new Item();
	}
	
	protected JdkField createField(Field field) {
		if (field.getName().equals("_intValue")) {
			return new JdkField(_reflector, field) {
			    
				public Object get(Object onObject) {
					return new Integer(((Item)onObject)._intValue);
				}
				
				public void set(Object onObject, Object attribute) {
					((Item)onObject)._intValue = ((Integer)attribute).intValue();
				}
			};
		}
		
		if (field.getName().equals("_stringValue")) {
			return new JdkField(_reflector, field) {
			    
				public Object get(Object onObject) {
					return ((Item)onObject)._stringValue;
				}
				
				public void set(Object onObject, Object attribute) {
					((Item)onObject)._stringValue = (String)attribute;
				}
			};
		}
		return super.createField(field);
	}

}
