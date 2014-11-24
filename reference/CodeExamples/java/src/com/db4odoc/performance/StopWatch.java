package com.db4odoc.performance;


import com.db4odoc.javalang.NoArgAction;

public final class StopWatch {
    private final long startedInMillis;

    private StopWatch(long started) {
        this.startedInMillis = started;
    }

    public static StopWatch startNew(){
        return new StopWatch(System.currentTimeMillis());
    }

    public void printElapsedMillisecs(){
        System.out.println("Time elapsed: "+(System.currentTimeMillis()-startedInMillis));
    }


    public static void time(NoArgAction taskToTime){
        StopWatch st = StopWatch.startNew();
        taskToTime.invoke();
        st.printElapsedMillisecs();
    }
}
