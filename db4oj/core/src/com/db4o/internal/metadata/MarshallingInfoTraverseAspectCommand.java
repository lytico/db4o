/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.metadata;

import com.db4o.internal.*;
import com.db4o.internal.marshall.*;

/**
 * @exclude
 */
public abstract class MarshallingInfoTraverseAspectCommand implements TraverseAspectCommand {
	
    private boolean _cancelled=false;
    
	protected final MarshallingInfo _marshallingInfo;
	
	public MarshallingInfoTraverseAspectCommand(MarshallingInfo marshallingInfo) {
		_marshallingInfo = marshallingInfo;
	}
	
	public final int declaredAspectCount(ClassMetadata classMetadata) {
		int aspectCount= internalDeclaredAspectCount(classMetadata);
		_marshallingInfo.declaredAspectCount(aspectCount);
		return aspectCount;
	}

	protected int internalDeclaredAspectCount(ClassMetadata classMetadata) {
		return classMetadata.readAspectCount(_marshallingInfo.buffer());
	}
    
    public boolean cancelled() {
        return _cancelled;
    }
    
    protected void cancel() {
        _cancelled=true;
    }
    
    public boolean accept(ClassAspect aspect){
        return true;
    }
    
    public void processAspectOnMissingClass(ClassAspect aspect, int currentSlot){
		if(_marshallingInfo.isNull(currentSlot)){
			return;
		}
    	aspect.incrementOffset(_marshallingInfo.buffer(), (HandlerVersionContext) _marshallingInfo);
    }
    
    public void processAspect(ClassAspect aspect,int currentSlot){
		if(accept(aspect)){
			processAspect(aspect, currentSlot, _marshallingInfo.isNull(currentSlot));
	    }
	    _marshallingInfo.beginSlot();
    }
 
    protected abstract void processAspect(ClassAspect aspect,int currentSlot, boolean isNull);
}