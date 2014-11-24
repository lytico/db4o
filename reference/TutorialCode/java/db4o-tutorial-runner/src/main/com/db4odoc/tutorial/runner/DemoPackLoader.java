package com.db4odoc.tutorial.runner;


import static com.db4odoc.tutorial.utils.ExceptionUtils.reThrow;

class DemoPackLoader {

    public static DemoPack loadByName(String nameOfDemo){
        final ClassLoader loader = DemoPackLoader.class.getClassLoader();
        final Class<?> theClass;
        try {
            theClass = loader.loadClass(nameOfDemo);
            return (DemoPack)theClass.newInstance();
        } catch (Exception e) {
            throw reThrow(e);
        }
    }

}
