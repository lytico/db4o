package decaf;

import java.lang.annotation.*;

@Target(ElementType.TYPE)
@Retention(RetentionPolicy.SOURCE)
public @interface Mixin {

	Platform value() default Platform.ALL;

}
