/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.internal.delete.*;
import com.db4o.internal.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public class TypeHandlerAspect extends ClassAspect {
    
    public final TypeHandler4 _typeHandler;
	private final ClassMetadata _ownerMetadata;
    
    public TypeHandlerAspect(ClassMetadata classMetadata, TypeHandler4 typeHandler) {
    	if(Handlers4.isValueType(typeHandler)){
    		throw new IllegalStateException();
    	}
    	_ownerMetadata = classMetadata;
        _typeHandler = typeHandler;
	}

	public boolean equals(Object obj) {
        if(obj == this){
            return true;
        }
        if(obj == null || obj.getClass() != getClass()){
            return false;
        }
        TypeHandlerAspect other = (TypeHandlerAspect) obj;
        return _typeHandler.equals(other._typeHandler);
    }
    
    public int hashCode() {
        return _typeHandler.hashCode();
    }

    public String getName() {
        return _typeHandler.getClass().getName();
    }

    public void cascadeActivation(ActivationContext context) {
    	if(! Handlers4.isCascading(_typeHandler)){
    		return;
    	}
    	Handlers4.cascadeActivation(context, _typeHandler);
    }

    public void collectIDs(final CollectIdContext context) {
    	if(! Handlers4.isCascading(_typeHandler)){
    		incrementOffset(context, context);
    		return;
    	}
    	context.slotFormat().doWithSlotIndirection(context, new Closure4() {
			public Object run() {
		    	QueryingReadContext queryingReadContext = new QueryingReadContext(context.transaction(), context.handlerVersion(), context.buffer(), 0, context.collector());
		    	((CascadingTypeHandler)_typeHandler).collectIDs(queryingReadContext);
				return null;
			}
    	});
    }

    public void defragAspect(final DefragmentContext context) {
    	context.slotFormat().doWithSlotIndirection(context, new Closure4() {
			public Object run() {
				_typeHandler.defragment(context);
				return null;
			}
		
		});
    }

    public int linkLength(HandlerVersionContext context) {
        return Const4.INDIRECTION_LENGTH;
    }

    public void marshall(MarshallingContext context, Object obj) {
    	context.createIndirectionWithinSlot();
    	
    	if (isNotHandlingConcreteType(context)) {
    		_typeHandler.write(context, obj);
    		return;
    	}
    	
    	if (_typeHandler instanceof InstantiatingTypeHandler) {
			InstantiatingTypeHandler instantiating = (InstantiatingTypeHandler) _typeHandler;
			instantiating.writeInstantiation(context, obj);
			instantiating.write(context, obj);
		} else {
			_typeHandler.write(context, obj);
		}
    }

	private boolean isNotHandlingConcreteType(MarshallingContext context) {
		return context.classMetadata() != _ownerMetadata;
	}

	public AspectType aspectType() {
        return AspectType.TYPEHANDLER;
    }

    public void activate(final UnmarshallingContext context) {
    	if(! checkEnabled(context, context)){
    		return;
    	}
    	context.slotFormat().doWithSlotIndirection(context, new Closure4() {
			public Object run() {
		        Handlers4.activate(context, _typeHandler);
				return null;
			}
		});
    }

	public void delete(final DeleteContextImpl context, boolean isUpdate) {
    	context.slotFormat().doWithSlotIndirection(context, new Closure4() {
			public Object run() {
				_typeHandler.delete(context);
				return null;
			}
		});
	}

	public void deactivate(ActivationContext context) {
		cascadeActivation(context);
	}

	public boolean canBeDisabled() {
		return true;
	}

}
