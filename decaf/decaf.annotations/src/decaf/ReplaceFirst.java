package decaf;

import java.lang.annotation.*;

@Target({ElementType.METHOD, ElementType.CONSTRUCTOR})
@Retention(RetentionPolicy.SOURCE)
public @interface ReplaceFirst {

	String value();
	
	Platform platform() default Platform.ALL;
	
	Platform[] platforms() default { Platform.ALL };
}
