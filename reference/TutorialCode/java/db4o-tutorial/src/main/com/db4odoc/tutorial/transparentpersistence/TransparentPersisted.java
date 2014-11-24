package com.db4odoc.tutorial.transparentpersistence;

import java.lang.annotation.*;

// #example: Annotation to mark persisted classes
@Retention(RetentionPolicy.RUNTIME)
@Documented
@Target(ElementType.TYPE)
public @interface TransparentPersisted {
}
// #end example
