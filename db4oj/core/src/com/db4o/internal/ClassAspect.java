/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.internal.delete.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public abstract class ClassAspect {
    
    // used for identification when sending in C/S mode 
	protected int              _handle;
    
    private int _disabledFromAspectCountVersion = AspectVersionContextImpl.ALWAYS_ENABLED.declaredAspectCount();
    
    public abstract AspectType aspectType();
    
    public abstract String getName();
    
    public abstract void cascadeActivation(ActivationContext context);
    
    public abstract int linkLength(HandlerVersionContext context);
    
    public final void incrementOffset(ReadBuffer buffer, HandlerVersionContext context) {
        buffer.seek(buffer.offset() + linkLength(context));
    }

    public abstract void defragAspect(DefragmentContext context);

    public abstract void marshall(MarshallingContext context, Object child);

    public abstract void collectIDs(CollectIdContext context);
    
    public void setHandle(int handle) {
        _handle = handle;
    }

    public abstract void activate(UnmarshallingContext context);

	public abstract void delete(DeleteContextImpl context, boolean isUpdate);
	
	public abstract boolean canBeDisabled();
	
    protected boolean checkEnabled(AspectVersionContext context, HandlerVersionContext versionContext){
    	if(! isEnabledOn(context)){
    		incrementOffset((ReadBuffer)context, versionContext);
    		return false;
    	}
    	return true;
    }

	
	public void disableFromAspectCountVersion(int aspectCount) {
		if(! canBeDisabled()){
			return;
		}
		if(aspectCount < _disabledFromAspectCountVersion){
			_disabledFromAspectCountVersion = aspectCount;
		}
	}
	
	public final boolean isEnabledOn(AspectVersionContext context){
		return _disabledFromAspectCountVersion  > context.declaredAspectCount();	
	}

	public abstract void deactivate(ActivationContext context);

	public boolean isVirtual() {
		return false;
	}

}
