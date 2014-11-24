package decaf;

import java.lang.annotation.*;

@Target({ElementType.METHOD, ElementType.CONSTRUCTOR})
@Retention(RetentionPolicy.SOURCE)
public @interface InsertFirst {

	String value();
	
	Platform platform() default Platform.ALL;
}
