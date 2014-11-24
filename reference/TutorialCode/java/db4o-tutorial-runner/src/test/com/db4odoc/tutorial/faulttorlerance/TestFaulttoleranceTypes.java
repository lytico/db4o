package com.db4odoc.tutorial.faulttorlerance;

import com.db4odoc.tutorial.faulttolerance.OperationResult;
import com.db4odoc.tutorial.faulttolerance.OperationSuccess;
import org.testng.Assert;
import org.testng.annotations.Test;

import static org.testng.Assert.*;
import static org.testng.Assert.assertEquals;

/**
 * @author roman.stoffel@gamlor.info
 * @since 20.04.11
 */
public class TestFaulttoleranceTypes {

    @Test
    public void success(){
        final OperationSuccess success = OperationSuccess.success();
        assertTrue(success.wasSuccessful());
        assertNull(success.getException());
    }
    @Test
    public void failure(){
        final OperationSuccess success = OperationSuccess.failed(new Exception("Ex"));
        assertFalse(success.wasSuccessful());
        assertEquals("Ex",success.getException().getMessage());
    }
    @Test
    public void successWithData(){
        final OperationResult<String> success = OperationResult.success("data");
        assertTrue(success.wasSuccessful());
        assertNull(success.getException());
        assertEquals("data",success.getResultData());
    }
    @Test
    public void canFailToAny(){
        final OperationResult<String> failure = OperationResult.fail(new Exception("Ex"));
        final OperationResult<Integer> asInt = failure.failToAny();
        assertFalse(asInt.wasSuccessful());
        assertEquals("Ex",asInt.getException().getMessage());
    }
    @Test
    public void cannotCastSuccess(){
        final OperationResult<String> failure = OperationResult.success("Test");
        try {
            final OperationResult<Integer> asInt = failure.failToAny();
            Assert.fail();
        } catch (IllegalStateException e){

        }

    }
    @Test
    public void failureWithData(){
        final OperationResult<String> success = OperationResult.fail(new Exception("Ex"));
        assertFalse(success.wasSuccessful());
        assertEquals("Ex", success.getException().getMessage());
        try{
            success.getResultData();
            Assert.fail("Expected exception");
        } catch (Exception e){

        }
    }
}
