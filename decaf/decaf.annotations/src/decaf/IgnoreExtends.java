package decaf;

import java.lang.annotation.*;

@Target(ElementType.TYPE)
@Retention(RetentionPolicy.SOURCE)
public @interface IgnoreExtends {

	Platform value() default Platform.ALL;
	
	Platform platform() default Platform.ALL;
	
	Platform[] platforms() default { Platform.ALL };

}
