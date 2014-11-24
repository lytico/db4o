package com.db4odoc.tutorial.faulttolerance;

import com.db4odoc.tutorial.utils.ExceptionUtils;

/**
 * @author roman.stoffel@gamlor.info
 * @since 20.04.11
 */
public final class OperationResult<T> extends OperationSuccess {
    private final T resultData;

    private OperationResult(Exception exeception) {
        super(exeception);
        this.resultData = null;
    }
    private OperationResult(T resultData) {
        super(null);
        if(null==resultData){
            throw new IllegalArgumentException("Null is a invalid result");
        }
        this.resultData = resultData;
    }

    public T getResultData() {
        if(wasSuccessful()){
            return resultData;
        }
        throw ExceptionUtils.reThrow(getException());
    }

    public static <T> OperationResult<T> fail(Exception exception){
        return new OperationResult<T>(exception);
    }
    public static <T> OperationResult<T> success(T result){
        return new OperationResult<T>(result);
    }

    public <TOther> OperationResult<TOther> failToAny() {
        if(!wasSuccessful()){
            return OperationResult.fail(getException());
        }
        throw new IllegalStateException("Cannot cast a success to other type");
    }
}
