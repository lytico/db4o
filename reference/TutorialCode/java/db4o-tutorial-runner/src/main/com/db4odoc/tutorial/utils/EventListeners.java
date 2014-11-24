package com.db4odoc.tutorial.utils;

import java.lang.reflect.InvocationHandler;
import java.lang.reflect.Method;
import java.lang.reflect.Proxy;
import java.util.HashSet;
import java.util.Set;

import static com.db4odoc.tutorial.utils.ExceptionUtils.reThrow;

/**
 * @author roman.stoffel@gamlor.info
 * @since 26.07.2010
 */
public final class EventListeners<T> implements Disposable {
    private final Set<T> listeners = new HashSet<T>();
    private T invoker;

    public EventListeners(Class classInfo) {
        this.invoker = buildInvoker(classInfo);
    }

    @Override
    public void dispose() {
        listeners.clear();
    }

    public Disposable add(final T event) {
        listeners.add(event);
        return new Disposable() {
            @Override
            public void dispose() {
                removeHandler(event);
            }
        };
    }

    public T invoker() {
        return invoker;
    }

    public static <T> EventListeners<T> create(Class classInfo) {
        return new EventListeners<T>(classInfo);
    }


    private void removeHandler(T event) {
        listeners.remove(event);
    }

    private void invokeEventHandling(Method method, Object[] args) {
        for (T listener : listeners) {
            try {
                method.invoke(listener, args);
            } catch (Exception e) {
                reThrow(e);
            }
        }
    }



    private T buildInvoker(Class classInfo) {
        return (T) Proxy.newProxyInstance(classInfo.getClassLoader(),
                new Class[]{classInfo},
                createInvocationHandler());
    }

    private InvocationHandler createInvocationHandler() {
        return new InvocationHandler() {
            @Override
            public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
                invokeEventHandling(method, args);
                return null;
            }
        };
    }
}
