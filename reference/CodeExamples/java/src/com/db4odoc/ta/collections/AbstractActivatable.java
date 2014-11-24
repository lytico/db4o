package com.db4odoc.ta.collections;

import com.db4o.activation.ActivationPurpose;
import com.db4o.activation.Activator;
import com.db4o.ta.Activatable;


public abstract class AbstractActivatable implements Activatable {
    private transient Activator activator;

    public void bind(Activator activator) {
        if (this.activator == activator) {
            return;
        }
        if (activator != null && null != this.activator) {
            throw new IllegalStateException("Object can only be bound to one activator");
        }
        this.activator = activator;
    }

    public void activate(ActivationPurpose activationPurpose) {
        if(null!=activator){
            activator.activate(activationPurpose);
        }
    }
}
