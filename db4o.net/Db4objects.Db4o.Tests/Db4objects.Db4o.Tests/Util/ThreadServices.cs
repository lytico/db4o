/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Util
{
	public class ThreadServices
	{
		/// <exception cref="System.Exception"></exception>
		public static void SpawnAndJoin(string threadName, int threadCount, ICodeBlock codeBlock
			)
		{
			Thread[] threads = new Thread[threadCount];
			for (int i = 0; i < threadCount; i++)
			{
				threads[i] = new Thread(new _IRunnable_16(codeBlock), threadName);
				threads[i].Start();
			}
			for (int i = 0; i < threads.Length; i++)
			{
				threads[i].Join();
			}
		}

		private sealed class _IRunnable_16 : IRunnable
		{
			public _IRunnable_16(ICodeBlock codeBlock)
			{
				this.codeBlock = codeBlock;
			}

			public void Run()
			{
				try
				{
					codeBlock.Run();
				}
				catch (Exception t)
				{
					Sharpen.Runtime.PrintStackTrace(t);
				}
			}

			private readonly ICodeBlock codeBlock;
		}
	}
}
