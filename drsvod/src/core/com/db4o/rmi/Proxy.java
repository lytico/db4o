package com.db4o.rmi;

import java.lang.annotation.*;

/**
 * <p>Parameters annotated with this should not be serialized, they rather must
 * registered in the {@link Distributor} and a proxy must be instantiated on the
 * other peer, so subsequent calls to that proxy will be forwarded to the
 * original instance.
 */
@Retention(RetentionPolicy.RUNTIME)
public @interface Proxy {

}
