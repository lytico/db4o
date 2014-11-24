package com.db4odoc.tutorial.utils;

/**
 * @author roman.stoffel@gamlor.info
 * @since 26.07.2010
 */
public interface OneArgAction<T> {
    OneArgAction NO_ACTION = new OneArgAction() {
        @Override
        public void invoke(Object arg) {
        }
    };


    void invoke(T arg);
}
