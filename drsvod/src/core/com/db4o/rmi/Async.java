package com.db4o.rmi;

import java.lang.annotation.*;

/**
 * <p>
 * Parameters annotated as {@link Async} will instruct the underlying remote
 * invocation layer to create a asynchronous proxy for this object on the other
 * peer.
 */
@Retention(RetentionPolicy.RUNTIME)
public @interface Async {

}
