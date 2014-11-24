package decaf;

import java.lang.annotation.*;

@Target(ElementType.TYPE)
@Retention(RetentionPolicy.SOURCE)
public @interface Remove {

	Platform value() default Platform.ALL;
	
	Platform[] platforms() default {Platform.ALL};
	
	Platform[] unlessCompatible() default {};

}
