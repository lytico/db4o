package com.db4odoc.tp.enhancement;

import com.db4o.instrumentation.core.ClassFilter;

import java.lang.annotation.Annotation;


// #example: Build a filter
public final class AnnotationFilter implements ClassFilter {

    public boolean accept(Class<?> aClass) {
        if(null==aClass || aClass.equals(Object.class)){
            return false;
        }
        return hasAnnotation(aClass)
                || accept(aClass.getSuperclass());
    }

    private boolean hasAnnotation(Class<?> aClass) {
        // We compare by name, to be class-loader independent
        Annotation[] annotations = aClass.getAnnotations();
        for (Annotation annotation : annotations) {
            if(annotation.annotationType().getName()
                    .equals(TransparentPersisted.class.getName())){
                return true;
            }
        }
        return false;
    }

}
// #end example
