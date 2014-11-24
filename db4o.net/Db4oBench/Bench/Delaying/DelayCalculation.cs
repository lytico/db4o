/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using Db4objects.Db4o.Bench.Logging;
using Db4objects.Db4o.Bench.Timing;

namespace Db4objects.Db4o.Bench.Delaying
{
	public class DelayCalculation
	{
		private const int AdjustmentIterations = 10000;

		private MachineCharacteristics _machine1;

		private MachineCharacteristics _machine2;

		private MachineCharacteristics _fasterMachine;

		private MachineCharacteristics _slowerMachine;

		/// <exception cref="NumberFormatException"></exception>
		/// <exception cref="IOException"></exception>
		public DelayCalculation(string logFileName1, string logFileName2)
		{
			_machine1 = new MachineCharacteristics(logFileName1);
			_machine2 = new MachineCharacteristics(logFileName2);
		}

		public virtual void ValidateData()
		{
			if (_machine1.IsFasterThan(_machine2))
			{
				_fasterMachine = _machine1;
				_slowerMachine = _machine2;
				Sharpen.Runtime.Out.WriteLine("> machine1 (" + _machine1.LogFileName() + ") is faster!"
					);
			}
			else
			{
				if (_machine2.IsFasterThan(_machine1))
				{
					_fasterMachine = _machine2;
					_slowerMachine = _machine1;
					Sharpen.Runtime.Out.WriteLine("> machine2 (" + _machine2.LogFileName() + ") is faster!"
						);
				}
			}
		}

		public virtual bool IsValidData()
		{
			return ((_fasterMachine != null) && (_slowerMachine != null));
		}

		public virtual Delays CalculatedDelays()
		{
			long[] tempDelays = new long[Delays.Count];
			for (int i = 0; i < Delays.Count; i++)
			{
				tempDelays[i] = _slowerMachine.times.values[i] - _fasterMachine.times.values[i];
			}
			return new Delays(tempDelays[Delays.Read], tempDelays[Delays.Write], tempDelays[Delays.Sync]);
		}

		public virtual void AdjustDelays(Delays delays)
		{
			for (int i = 0; i < Delays.Count; i++)
			{
				AdjustDelay(delays, i);
			}
		}

		private void AdjustDelay(Delays delays, int index)
		{
			TicksStopWatch watch = new TicksStopWatch();
			TicksTiming timing = new TicksTiming();
			long difference;
			long differencePerIteration;
			long average = 0;
			long oldAverage = 0;
			long delay = delays.values[index];
			long adjustedDelay = delay;
			int adjustmentRuns = 1;
			long targetRuntime = AdjustmentIterations * delay;
			long minimumDelay = MinimumDelay();
			WarmUpIterations(delay, timing);
			do
			{
				watch.Start();
				for (int i = 0; i < AdjustmentIterations; i++)
				{
					timing.WaitTicks(adjustedDelay);
				}
				watch.Stop();
				difference = targetRuntime - watch.Elapsed();
				differencePerIteration = difference / AdjustmentIterations;
				if (-differencePerIteration > adjustedDelay)
				{
					adjustedDelay /= 2;
				}
				else
				{
					adjustedDelay += differencePerIteration;
					oldAverage = average;
					if (adjustmentRuns == 1)
					{
						average = adjustedDelay;
					}
					else
					{
						average = ((average * (adjustmentRuns - 1)) / adjustmentRuns) + (adjustedDelay / 
							adjustmentRuns);
					}
					adjustmentRuns++;
				}
				if (adjustedDelay <= 0)
				{
					break;
				}
				if ((Math.Abs(average - oldAverage) < (0.01 * delay)) && adjustmentRuns > 10)
				{
					break;
				}
			}
			while (true);
			if (average < minimumDelay)
			{
				Sharpen.Runtime.Err.WriteLine(">> Smallest achievable delay: " + minimumDelay);
				Sharpen.Runtime.Err.WriteLine(">> Required delay setting: " + average);
				Sharpen.Runtime.Err.WriteLine(">> Using delay(0) to wait as short as possible.");
				Sharpen.Runtime.Err.WriteLine(">> Results will not be accurate.");
				average = 0;
			}
			delays.values[index] = average;
		}

		private void WarmUpIterations(long delay, TicksTiming timing)
		{
			for (int i = 0; i < AdjustmentIterations; i++)
			{
				timing.WaitTicks(delay);
			}
		}

		private long MinimumDelay()
		{
			TicksStopWatch watch = new TicksStopWatch();
			TicksTiming timing = new TicksTiming();
			watch.Start();
			for (int i = 0; i < AdjustmentIterations; i++)
			{
				timing.WaitTicks(0);
			}
			watch.Stop();
			return watch.Elapsed() / AdjustmentIterations;
		}
	}

	internal class MachineCharacteristics
	{
		private string _logFileName;

		public Delays times;

		/// <exception cref="NumberFormatException"></exception>
		/// <exception cref="IOException"></exception>
		public MachineCharacteristics(string logFileName)
		{
			_logFileName = logFileName;
			ParseLog();
		}

		/// <exception cref="NumberFormatException"></exception>
		/// <exception cref="IOException"></exception>
		private void ParseLog()
		{
			StreamReader reader = new StreamReader(_logFileName);
			long readTime = 0;
			long writeTime = 0;
			long syncTime = 0;
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				if (line.StartsWith(LogConstants.ReadEntry))
				{
					readTime = ExtractNumber(line);
				}
				else
				{
					if (line.StartsWith(LogConstants.WriteEntry))
					{
						writeTime = ExtractNumber(line);
					}
					else if (line.StartsWith(LogConstants.SyncEntry))
					{
						syncTime = ExtractNumber(line);
					}
				}
			}
			reader.Close();
			times = new Delays(readTime, writeTime, syncTime);
		}

		private long ExtractNumber(string line)
		{
			return long.Parse(ExtractNumberString(line));
		}

		private string ExtractNumberString(string line)
		{
			int start = line.IndexOf(' ') + 1;
			int end = line.IndexOf(' ', start);
			return Sharpen.Runtime.Substring(line, start, end);
		}

		public virtual bool IsFasterThan(Db4objects.Db4o.Bench.Delaying.MachineCharacteristics
			 otherMachine)
		{
			bool result = true;
			for (int i = 0; i < Delays.Count; i++)
			{
				result = result && (times.values[i] <= otherMachine.times.values[i]);
			}
			return result;
		}

		public virtual string LogFileName()
		{
			return _logFileName;
		}
	}
}
