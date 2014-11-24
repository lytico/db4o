package com.db4o.util;
/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */


/**
 * This file was taken from the db4oj.tests project from
 * the com.db4o.db4ounit.util package. 
 * TODO: move to own project and supply as separate Jar.
 */


import java.io.*;

import com.db4o.foundation.*;
import com.db4o.foundation.io.Path4;

public class IOServices {
    
	public static String buildTempPath(String fname) {
		return Path4.combine(Path4.getTempPath(), fname);
	}

	public static String safeCanonicalPath(String path) {
		try {
			return new File(path).getCanonicalPath();
		} catch (IOException e) {
			e.printStackTrace();
			return path;
		}
	}
	
	public static boolean killProcess(String processName) throws IOException, InterruptedException{
		ProcessInfo processInfo = findProcess(processName);
		if(processInfo == null){
			return false;
		}
		ProcessResult processResult = exec("taskkill.exe", new String[]{"/PID", Long.toString(processInfo.processId), "/F"});
		return processResult.returnValue == 0;
	}
	
	public static ProcessInfo findProcess(String processName) throws IOException, InterruptedException{
		ProcessInfo[] runningProcesses = runningProcesses();
		for (ProcessInfo processInfo : runningProcesses) {
			if(processName.equals(processInfo.name)){
				return processInfo;
			}
		}
		return null;
	}
	
	public static ProcessInfo[] runningProcesses() throws IOException, InterruptedException{
		ProcessResult processResult = exec("tasklist.exe", new String[]{"/fo", "csv", "/nh"});
		String[] lines = processResult.out.split("\n");
		ProcessInfo[] result = new ProcessInfo[lines.length];
		for (int i = 0; i < lines.length; i++) {
			String[] infos = lines[i].split(",");
			String processName = unquote(infos[0]);
			long processId = Long.parseLong(unquote(infos[1]));
			result[i] = new ProcessInfo(processName, processId);
		}
		return result;
	}
	
	private static String unquote(String str){
		return str.replaceAll("\"", "");
	}
	
	public static ProcessResult exec(String program) throws IOException, InterruptedException{
	    return exec(program, null);
	}
	
	public static ProcessResult exec(String program, String[] arguments) throws IOException, InterruptedException{
	    ProcessRunner runner = new ProcessRunner(program, arguments);
	    runner.waitFor();
	    return runner.processResult();  
	}

	public static ProcessRunner start(String program, String[] arguments) throws IOException {
	    return new ProcessRunner(program, arguments);
	}

	public static ProcessResult execAndDestroy(String program, String[] arguments, String expectedOutput, long timeout) throws IOException{
        ProcessRunner runner = new ProcessRunner(program, arguments);
        runner.destroy(expectedOutput, timeout);
        return runner.processResult();
    }
	
	public static class DestroyTimeoutException extends RuntimeException{
	}
	
	public static class ProcessTerminatedBeforeDestroyException extends RuntimeException{
	}
	
	public static class ProcessRunner{
	    
	    final long _startTime;
	    
	    private final String _command;
	    
        private final StreamReader _inputReader;
        
        private final StreamReader _errorReader;
        
        private final BlockingQueue<String> in = new BlockingQueue<String>();
        
        private final Process _process;
        
        private int _result;
        
        private StringBuilder _inputBuffer = new StringBuilder();
        private StringBuilder _errorBuffer = new StringBuilder();
	    
	    public ProcessRunner(String program, String[] arguments) throws IOException{
	    	String[] args = new String[arguments.length + 1];
	    	args[0] = program;
	    	System.arraycopy(arguments, 0, args, 1, arguments.length);
    		_command = generateCommand(program, arguments);
    		_process = Runtime.getRuntime().exec(args);
            _inputReader = new StreamReader("ProcessRunner Input Thread ["+program+" " + toString(arguments) +"]", _process.getInputStream(), new DelegatingBlockingQueue<String>(in) {
            	@Override
            	public void add(String obj) {
            		_inputBuffer.append(obj);
            		_inputBuffer.append("\n");
            		super.add(obj);
            		output(obj);
            	}
            });
            _errorReader = new StreamReader("ProcessRunner Output Thread ["+program+" " + toString(arguments) +"]", _process.getErrorStream(), new DelegatingBlockingQueue<String>(in) {
               	@Override
            	public void add(String obj) {
               		_errorBuffer.append(obj);
               		_errorBuffer.append("\n");
            		super.add(obj);
            		output(obj);
            	}
            });
            _startTime = System.currentTimeMillis();
	    }
	    
	    protected void output(String line) {
		}

		public ProcessResult processResult() {
			return new ProcessResult(_command, _inputBuffer.toString(), _errorBuffer.toString(), _result);
		}

	    private String toString(String[] arguments) {
	    	String r = "";
	    	for (String s : arguments) {
	    		if (r.length() > 0) r += " ";
				r += "\"" + s + "\"";
			}
			return r;
		}

		private String generateCommand (String program, String[] arguments){
            String command = program;
            if(arguments != null){
                for (int i = 0; i < arguments.length; i++) {
                    command += " " + arguments[i];
                }
            }
            return command;
	    }
	    
	    public int waitFor() throws InterruptedException{
	        _result = _process.waitFor();
	        joinReaders();
	        return _result;
	    }
	    
	    public void destroy(String expectedOutput, long timeout){
	        try{
    	        waitFor(expectedOutput, timeout);
	        } 
	        finally {
	        	destroy();
	        }
	    }

	    public void destroy(){
	        try{
    	        checkIfTerminated();
    	        
    	        // Race condition: If the process is terminated right here , it may
    	        // terminate successfully before being destroyed.
    	        
	        } finally {
	            _process.destroy();
	            try {
					joinReaders();
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
	        }
	    }

	    public void write(String msg) throws IOException {
	    	OutputStreamWriter out = new OutputStreamWriter(_process.getOutputStream());
			out.write(msg + "\n");
			out.flush();
	    }
	    
        public boolean waitFor(String expectedOutput, long timeout) {
			long now = System.currentTimeMillis();
			while (timeout > 0 ){
				String nextInput = in.next(timeout);
				if(nextInput != null && nextInput.indexOf(expectedOutput) != -1){
					return true;
				}
				long l = now;
				now = System.currentTimeMillis();
				timeout -= now-l;
			};
			return timeout > 0;
        }

        public boolean waitForLine(String expectedOutput, long timeout) {
			long now = System.currentTimeMillis();
			while (timeout > 0 && !expectedOutput.equals(in.next(timeout))) {
				long l = now;
				now = System.currentTimeMillis();
				timeout -= now-l;
			};
			return timeout > 0;
        }

        private void checkIfTerminated() {
	        try{
	            _process.exitValue();
	            throw new ProcessTerminatedBeforeDestroyException();
	        }catch (IllegalThreadStateException ex){
	        }
        }
	    
	    private void joinReaders() throws InterruptedException{
	        _inputReader.join();
	        _errorReader.join();
	    }
	    
	}

    static class StreamReader implements Runnable {
        
        private final InputStream _stream;
        
        private final Thread _thread;
        
		private final Queue4<String> _in;
        
        StreamReader(String threadName, InputStream stream, Queue4<String> in){
            _stream = stream;
			_in = in;
            _thread = new Thread(this, threadName);
            _thread.setDaemon(true);
            _thread.start();
        }
        
        public void run() {
        	BufferedReader in = new BufferedReader(new InputStreamReader(_stream));
            try {
            	String line;
                while((line=in.readLine()) != null){
                    _in.add(line);
                }
            } catch (IOException e) {
            }
        }
        
        public void join() throws InterruptedException{
        	_thread.join();
        }
        
    }
    
    public static String joinArgs(String separator, String[] args, boolean doQuote)
    {
        StringBuffer buffer = new StringBuffer();
        for (String arg : args)
        {
            if (buffer.length() > 0) buffer.append(separator);
            buffer.append((doQuote ? quote(arg) : arg));
        }
        return buffer.toString();
    }
    
    public static String quote(String s)
    {
        if (s.startsWith("\"")) return s;
        return "\"" + s + "\"";
    }

}
