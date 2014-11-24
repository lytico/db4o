/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */
using System;
using System.IO;
using System.Collections;
using Sharpen.Util;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Foundation;

using Db4objects.Db4o.Bench.Logging.Replay.Commands;

namespace Db4objects.Db4o.Bench.Logging.Replay
{
    public class LogReplayer
    {
        private readonly String _logFilePath;
        private readonly IBin _bin;
        private readonly ISet _commands;
        private readonly Hashtable _counts;

        public LogReplayer(String logFilePath, IBin bin, ISet commands)
        {
            _logFilePath = logFilePath;
            _bin = bin;
            _commands = commands;
            _counts = new Hashtable();
            foreach (object com in commands)
            {
                _counts[com] = (Int64)0;
            }
        }

        public LogReplayer(String logFilePath, IBin bin): this(logFilePath, bin, LogConstants.AllEntries())
        {
        }

        public void ReplayLog()
        {
            PlayCommandList(ReadCommandList());
        }

        public List4 ReadCommandList()
        {
            List4 list = null;
            StreamReader reader = new StreamReader(_logFilePath);
            String line = null;
            while ((line = reader.ReadLine()) != null)
            {
                IIoCommand ioCommand = ReadLine(line);
                if (ioCommand != null)
                {
                    list = new List4(list, ioCommand);
                }
            }
            reader.Close();
            return list;
        }

        public void PlayCommandList(List4 commandList)
        {
            while (commandList != null)
            {
                IIoCommand ioCommand = (IIoCommand)commandList._element;
                ioCommand.Replay(_bin);
                commandList = commandList._next;
            }
        }


        private IIoCommand ReadLine(String line)
        {
            String commandName;
            if ((commandName = AcceptedCommandName(line)) != null)
            {
                IncrementCount(commandName);
                return CommandForLine(line);
            }
            return null;
        }

        private String AcceptedCommandName(String line)
        {
            if (line.Length == 0)
            {
                return null;
            }
            foreach (String commandName in _commands)
            {
                if (line.StartsWith(commandName))
                {
                    return commandName;
                }
            }
            return null;
        }

        private IIoCommand CommandForLine(String line)
        {
			if (line.StartsWith(LogConstants.ReadEntry))
            {
				var param = Parameter(LogConstants.ReadEntry, line);
				return new ReadCommand(param.pos, param.len);
            }

            if (line.StartsWith(LogConstants.WriteEntry))
            {
                var param = Parameter(LogConstants.WriteEntry, line);
                return new WriteCommand(param.pos, param.len);
            }
            
			if (line.StartsWith(LogConstants.SyncEntry))
            {
                return new SyncCommand();
            }

            return null;
        }


        private Param Parameter(String command, String line)
        {
            return Parameter(command.Length, line);
        }

		private Param Parameter(int start, String line)
		{
			String[] paramStr = line.Substring(start).Split(' ');
			long pos = long.Parse(paramStr[0]);
			int len = int.Parse(paramStr[1]);
			return new Param(pos, len);
		}

        private void IncrementCount(String key)
        {
            long count = (Int64)_counts[key];
            _counts[key] = (Int64)(count + 1);
        }

        public Hashtable OperationCounts()
        {
            return _counts;
        }
    }

	internal class Param 
	{
		public long pos;
		public int len;

		public Param(long pos, int len) {
			this.pos = pos;
			this.len = len;
		}
	}
}

