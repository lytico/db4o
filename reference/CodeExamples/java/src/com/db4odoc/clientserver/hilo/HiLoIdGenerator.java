package com.db4odoc.clientserver.hilo;

import com.db4o.ObjectContainer;
import com.db4o.query.Predicate;

import java.util.HashMap;
import java.util.List;
import java.util.Map;


class HiLoIdGenerator {
    private static final int CAPACITY = 1024;
    private static final String SEMAPHORE_PREFIX = "HiLoIdGenerator_";
    private final Map<Class, LoHiValues> values = new HashMap<Class, LoHiValues>();
    private static final int TIMEOUT_IN_MS = 500;

    public int nextIdFor(Class type, ObjectContainer container){
        LoHiValues value = values.get(type);
        if(null==value){
            value = initializeFor(type,container);
            values.put(type,value);
        }
        if(value.outOfHiValues()){
            value.newHighValue(hiValueFromServer(type,container));
        }
        return value.nextValue();
    }

    private LoHiValues initializeFor(final Class type, ObjectContainer container) {
        int hiValue = hiValueFromServer(type,container);
        return new LoHiValues(hiValue);
    }

    private int hiValueFromServer(final Class type, ObjectContainer container) {
        try
        {
            aquireLock(type,container);
            HiValue hiValueInfo = queryForHiValue(type,container);
            int result = hiValueInfo.increment();
            container.store(hiValueInfo);
            return result;
        }finally{
            releaseLock(type,container);
        }
    }

    private HiValue queryForHiValue(final Class type, ObjectContainer container) {
        final List<HiValue> hiValue = container.query(new Predicate<HiValue>() {
            @Override
            public boolean match(HiValue o) {
                return o.forType.equals(type.getName());
            }
        });
        if (hiValue.isEmpty()) {
            return new HiValue(type);
        } else if (hiValue.size() == 1) {
            return hiValue.get(0);
        } else {
            throw new IllegalStateException("Don't expect more than one Hi-Value info for a type: " + type);
        }
    }

    private void aquireLock(Class type, ObjectContainer container) {
        if(!container.ext().setSemaphore(lockName(type), TIMEOUT_IN_MS)){
            throw new IllegalStateException("Couldn't acquire semaphore "+ lockName(type));
        }
    }

    private void releaseLock(Class type,ObjectContainer container) {
        container.ext().releaseSemaphore(lockName(type));
    }

    private String lockName(Class type){
        return SEMAPHORE_PREFIX + type.getName();
    }


    static class LoHiValues {
        private int lowValue;
        private int hiValue;

        LoHiValues(int hiValue) {
            newHighValue(hiValue);
        }


        int nextValue(){
            if(!outOfHiValues()){
                throw new IllegalStateException("Cannot provide a new value." +
                        " Check outOfHiValues and set a new value with newHighValue");
            } else{
                lowValue++;
                return (hiValue) * CAPACITY + lowValue;
            }
        }

        boolean outOfHiValues(){
            return lowValue < CAPACITY;
        }

        void newHighValue(int hiValue){
            this.hiValue = hiValue;
            this.lowValue = 0;
        }
    }


    static class HiValue{
        private String forType;
        private int hiValue = -1;

        HiValue(Class forType) {
            this.forType = forType.getName();
        }

        int increment(){
            hiValue++;
            return hiValue;
        }
    }

}
