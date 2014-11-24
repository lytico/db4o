package com.db4odoc.tutorial.utils;

/**
 * @author roman.stoffel@gamlor.info
 * @since 19.07.2010
 */
public final class ExceptionUtils {
    private ExceptionUtils(){}



    public static <TException extends Throwable> RuntimeException reThrow(TException exception){
        return ExceptionUtils.<RuntimeException,RuntimeException>reThrowInternal(exception);
    }


    private static <TException extends Throwable,TReturnValue> TReturnValue reThrowInternal(Throwable exception) throws TException {
        throw (TException) exception;
    }
}
