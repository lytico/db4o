package com.db4o.devtools.ant;

import java.io.*;
import java.util.concurrent.CountDownLatch;

import org.apache.tools.ant.*;

public class RerunUntilPatternTask extends Task {

	public class LineReader {

		private Thread thread;
		private BufferedReader reader;
		
		private StringBuffer buffer = new StringBuffer();

		public LineReader(final InputStream in) {
			reader = new BufferedReader(new InputStreamReader(in));
			thread = new Thread() {
				@Override
				public void run() {
					String line;
					try {
						while ((line = reader.readLine()) != null) {
							buffer.append(line);
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
		
		public String getOutput(){
			return buffer.toString();
		}
	}

	private String pattern;
	private String command;
	private long timeout;
	private Thread timeoutThread;
	protected boolean timeoutError;
	private Process process;
	private CountDownLatch barrier = new CountDownLatch(1);
	private LineReader _inputReader;
	private LineReader _errorReader;
	
	private int _sleepBetweenRetries = 1;
	
	public void setSleepBetweenRetries(int sleepBetweenRetries) {
		_sleepBetweenRetries = sleepBetweenRetries;
	}

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
			
			boolean timeoutThreadStarted = false;
			
			while(true) {

				startProcess();
				
				if(!timeoutThreadStarted){
					if (getTimeout() != 0) {
						setupTimeoutThread();
					}
					timeoutThreadStarted = true;
				}
	
				setupStdReaders();
	
				waitForProcess();
				
				// System.out.println(_inputReader.getOutput());
				
				if(_inputReader.getOutput().contains(pattern)){
					
					break;
				}
				
				if(timeoutError){
					throw new BuildException("Timeout while waiting for output '" + pattern + "' from process " + command); 
				}
				
				try {
					Thread.sleep(_sleepBetweenRetries);
				} catch (InterruptedException e) {
					throw new BuildException(e);
				}
			}
			

		} catch (IOException e) {
			throw new BuildException(e);
		} finally{
			if (getTimeout() != 0) {
				releaseTimeoutThread();
			}
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
			process.waitFor();
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}

	private void setupStdReaders() {
		_inputReader = new LineReader(process.getInputStream());
		_errorReader = new LineReader(process.getErrorStream());
	}

	private void startProcess() throws IOException {
		process = Runtime.getRuntime().exec(getCommand());
	}

	private void setupTimeoutThread() {
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
