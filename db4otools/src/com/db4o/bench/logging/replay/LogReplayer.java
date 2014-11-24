/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.bench.logging.replay;

import java.io.*;
import java.util.*;

import com.db4o.bench.logging.*;
import com.db4o.bench.logging.replay.commands.*;
import com.db4o.foundation.*;
import com.db4o.io.*;
/**
 * @sharpen.ignore
 */
public class LogReplayer {
	
	private String _logFilePath;
	private Bin _bin;
	private Set _commands;
	private Map _counts;
	
	public LogReplayer(String logFilePath, Bin bin, Set commands) {
		_logFilePath = logFilePath;
		_bin = bin;
		_commands = commands;
		_counts = new HashMap();
		Iterator it = commands.iterator();
		while (it.hasNext()) {
			_counts.put(it.next(), new Long(0));
		}
	}
	
	public LogReplayer(String logFilePath, Bin bin) {
		this(logFilePath, bin, LogConstants.allEntries());
	}
	
	
	public void replayLog() throws IOException {
		playCommandList(readCommandList());
	}
	
	public List4 readCommandList() throws IOException {
		List4 list = null;
		BufferedReader reader = new BufferedReader(new FileReader(_logFilePath));
		String line = null;
		while ( (line = reader.readLine()) != null ) {
			IoCommand ioCommand = readLine(line);
			if(ioCommand != null){
				list = new List4(list, ioCommand);
			}
		}
		reader.close();
		return list;
	}
	
	public void playCommandList(List4 commandList){
		while(commandList != null){
			IoCommand ioCommand = (IoCommand) commandList._element;
			ioCommand.replay(_bin);
			commandList = commandList._next;
		}
	}
	
	
	private IoCommand readLine(String line) {
		String commandName;
		if ((commandName = acceptedCommandName(line)) != null) {
			incrementCount(commandName);
			return commandForLine(line);
		}
		return null;
	}

	private String acceptedCommandName(String line) {
		if (line.length() == 0) {
			return null;
		}
		Iterator it = _commands.iterator();
		while (it.hasNext()) {
			String commandName = (String)it.next();
			if ( line.startsWith(commandName) ) {
				return commandName;
			}
		}
		return null;
	}
	
	private IoCommand commandForLine(String line) {
		if (line.startsWith(LogConstants.READ_ENTRY)) {
			Param param = parameter(LogConstants.READ_ENTRY,  line);
			return new ReadCommand(param.pos, param.len);
		}
		if ( line.startsWith(LogConstants.WRITE_ENTRY) ) {
			Param param = parameter(LogConstants.WRITE_ENTRY,  line);
			return new WriteCommand(param.pos, param.len);
		}
		if ( line.startsWith(LogConstants.SYNC_ENTRY) ) {
			return new SyncCommand();
		}
		
		return null;
	}

	
	private Param parameter(String command, String line){
		return parameter(command.length(),  line);
	}

	private Param parameter(int start, String line) {
		String[] paramStr = line.substring(start).split(" ");
		long pos = Long.parseLong(paramStr[0]);
		int len = Integer.parseInt(paramStr[1]);
		return new Param(pos, len);
	}

	private static class Param {
		public final long pos;
		public final int len;

		public Param(long pos, int len) {
			this.pos = pos;
			this.len = len;
		}
	}
	
	private void incrementCount(String key) {
		long count = ((Long)_counts.get(key)).longValue();
		_counts.put(key, new Long(count+1));
	}
	
	public Map operationCounts() {
		return _counts;
	}
}
