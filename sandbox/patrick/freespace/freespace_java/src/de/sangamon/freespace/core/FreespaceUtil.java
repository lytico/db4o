package de.sangamon.freespace.core;

public class FreespaceUtil {

	public static final int[] NUM_CLIENTS = { 1, 2, 4, 8, 16, 32 };
	public static final int NUM_ACQUISITIONS = 10;
	public static final int NUM_RUNS = 20;

	public static final int ACQUIRE_TICKS = 5;
	public static final int FREE_TICKS = 15;
	public static final int CLIENT_WORK_TICKS = 10;
	public static final int CLIENT_IDLE_TIME = 100;

	private static final int SLEEP_MEM_SIZE = 500;
	private static final int NUM_SLEEP_ITER = 500;
	
	private static final int PROBE_TICKS = 50;
	private static final int PROBE_TIMES = 500;

	public static void sleep(int time) {
		for(int i=0; i < NUM_SLEEP_ITER * time; i++) {
			byte[] b = new byte[SLEEP_MEM_SIZE];
			assert b != null;
		}
	}
	
	public static void wait(Object lock, int time) {
		synchronized(lock) {
			try {
				lock.wait(time);
			}
			catch (InterruptedException exc) {
				exc.printStackTrace();
			}
		}
	}

	public static void stats() {
		long start = System.currentTimeMillis();
		for(int i = 0; i < PROBE_TIMES; i++) {
			FreespaceUtil.sleep(PROBE_TICKS);
		}
		long end = System.currentTimeMillis();
		long time = end - start;
		double ticks_per_ms = (PROBE_TICKS * PROBE_TIMES) / time;
		System.out.println(ticks_per_ms + " ticks/ms");
		
		int numInvocations = NUM_RUNS * NUM_CLIENTS[0]; // 400
		int numItems = numInvocations * NUM_ACQUISITIONS; // 4k
		int clientTicks = (CLIENT_WORK_TICKS + CLIENT_IDLE_TIME) * 2; // 120
		int expectedTotalTicks = numItems * (ACQUIRE_TICKS + clientTicks + FREE_TICKS); //  124 * 4k = 49.6k
		int expectedFreespaceTicks = numItems * (ACQUIRE_TICKS + FREE_TICKS); // 4000 * 4 = 16k
		int expectedClientTicks = numInvocations * clientTicks; // 400 * 120 = 48k
		System.out.println(expectedFreespaceTicks + "/" + expectedClientTicks);
		int idealTicks = Math.max(expectedFreespaceTicks, expectedClientTicks);
		logTicks("TOTAL/SERIALIZED", ticks_per_ms, expectedTotalTicks);
		logTicks("IDEAL", ticks_per_ms, idealTicks);
		System.out.println("MAX QUEUE: " + numItems);
	}

	private static void logTicks(String id, double ticks_per_ms, int ticks) {
		System.out.println(id + ": " + ticks + " ticks  / " + (ticks / ticks_per_ms) + " ms");
	}

	
	public static void main(String[] args) {
		stats();
	}
	
	private FreespaceUtil() {
	}

}
