package com.db4odoc.tutorial.faulttolerance;

/**
 * @author roman.stoffel@gamlor.info
 * @since 20.04.11
 */
public class OperationSuccess {
    private final Exception exception;

    protected OperationSuccess(Exception exception) {
        this.exception = exception;
    }

    public static OperationSuccess failed(Exception ex){
        return new OperationSuccess(ex);
    }

    public static OperationSuccess success() {
        return new OperationSuccess(null);
    }

    public boolean wasSuccessful(){
        return exception == null;
    }


    public Exception getException() {
        return exception;
    }
}