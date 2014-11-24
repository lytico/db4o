package decaf;

import java.lang.annotation.*;

@Target({ElementType.METHOD, ElementType.CONSTRUCTOR})
@Retention(RetentionPolicy.SOURCE)
public @interface RemoveFirst {

	Platform value() default Platform.ALL;
	
	Platform platform() default Platform.ALL;
	
	Platform[] platforms() default { Platform.ALL };
	
	Platform[] unlessCompatible() default {};
	
}
