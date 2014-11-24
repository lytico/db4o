package decaf;

import java.lang.annotation.*;

@Target({ElementType.TYPE, ElementType.METHOD, ElementType.CONSTRUCTOR, ElementType.FIELD})
@Retention(RetentionPolicy.SOURCE)
public @interface Public {

	Platform value() default Platform.JDK11;

}
