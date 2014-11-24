package com.db4odoc.tp.enhancement;

import java.lang.annotation.*;

// #example: Annotation to mark persisted classes
@Retention(RetentionPolicy.RUNTIME)
@Documented
@Target(ElementType.TYPE)
public @interface TransparentPersisted {
}
// #end example