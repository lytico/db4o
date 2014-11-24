package com.db4o.devtools.ant;

import java.io.*;
import java.util.concurrent.CountDownLatch;

import org.apache.tools.ant.*;

public class WaitingForPatternTask extends Task {

	public class LineReader {

		private Thread thread;
		private BufferedReader reader;

		public LineReader(final InputStream in, final LineReaderListener listener) {
			reader = new BufferedReader(new InputStreamReader(in));
			thread = new Thread() {
				@Override
				public void run() {
					String line;
					try {
						while ((line = reader.readLine()) != null) {
							if (!listener.lineReady(line)) {
								break;
							}
						}
					} catch (IOException e) {
					} finally {
						try {
							reader.close();
						} catch (IOException e) {
						}
					}
				};
			};
			thread.setDaemon(true);
			thread.start();
		}

		public void dispose() {
			try {
				reader.close();
			} catch (IOException e) {
			}
		}
	}

	public interface LineReaderListener {

		public boolean lineReady(String line);

	}

	private String pattern;
	private String command;
	private long timeout;
	private Thread timeoutThread;
	protected boolean timeoutError;
	private Process process;
	private CountDownLatch barrier = new CountDownLatch(1);

	public long getTimeout() {
		return timeout;
	}

	public void setTimeout(long timeout) {
		this.timeout = timeout;
	}

	public String getPattern() {
		return pattern;
	}

	public void setPattern(String pattern) {
		this.pattern = pattern;
	}

	public String getCommand() {
		return command;
	}

	public void setCommand(String command) {
		this.command = command;
	}

	@Override
	public void execute() throws BuildException {

		try {

			startProcess();

			setupStdReaders();

			if (getTimeout() != 0) {
				setupTimeoutThread(process);
			}

			waitForProcess();

			if (getTimeout() != 0) {
				releaseTimeoutThread();
			}

		} catch (IOException e) {
			throw new BuildException(e);
		}
	}

	private void releaseTimeoutThread() {
		Thread t = timeoutThread;
		timeoutThread = null;
		synchronized (t) {
			t.notifyAll();
		}
	}

	private void waitForProcess() {
		try {
			barrier.await();
		} catch (InterruptedException e1) {
		}
		process.destroy();
		if (timeoutError) {
			throw new BuildException(WaitingForPatternTask.class.getSimpleName() + " task failed due to timeout");
		}
	}

	private void setupStdReaders() {
		new LineReader(process.getInputStream(), new LineReaderListener() {

			public boolean lineReady(String line) {
				return evaluatePattern(process, line);
			}

		});

		new LineReader(process.getErrorStream(), new LineReaderListener() {

			public boolean lineReady(String line) {
				System.err.println(line);
				return true;
			}

		});
	}

	private void startProcess() throws IOException {
		process = Runtime.getRuntime().exec(getCommand());
	}

	private void setupTimeoutThread(final Process process) {
		timeoutThread = new Thread() {
			@Override
			public void run() {
				try {
					synchronized (this) {
						this.wait(getTimeout());
					}
					if (timeoutThread != null) {
						timeoutError = true;
						process.destroy();
					}
				} catch (InterruptedException e) {
				}
			};
		};
		timeoutThread.setDaemon(true);
		timeoutThread.start();
	}

	private boolean evaluatePattern(final Process process, String line) {
		if (line.contains(getPattern())) {
			barrier.countDown();
			return false;
		}
		return true;
	}
}
