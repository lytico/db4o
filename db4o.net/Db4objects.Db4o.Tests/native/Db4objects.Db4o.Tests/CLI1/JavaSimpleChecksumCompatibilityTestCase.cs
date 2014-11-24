/* Copyright (C) 2010   Versant Inc.   http://www.db4o.com */
using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.CLI1
{

    public class JavaSimpleChecksumCompatibilityTestCase : JavaCompatibilityTestCaseBase
    {
        private String _expectedJavaOutput;

        public void Test()
        {
            int[] ints = new int[]{
				int.MinValue, 
				-1, 
				0, 
				1, 
				int.MaxValue};
            int bufferLength = Const4.IntLength * ints.Length;
            ByteArrayBuffer buffer = new ByteArrayBuffer(bufferLength);
            for (int i = 0; i < ints.Length; i++)
            {
                buffer.WriteInt(ints[i]);
            }
            long checkSum = CRC32.CheckSum(buffer._buffer, 0, buffer._buffer.Length);

            _expectedJavaOutput = checkSum.ToString();
            RunTest();
        }

        protected override string ExpectedJavaOutput()
        {
            return _expectedJavaOutput;
        }

        protected override void PopulateContainer(IObjectContainer container)
        {
            //  do nothing
        }

        protected override JavaSnippet JavaCode()
        {
            return new JavaSnippet("com.db4o.test.compatibility.Program", @"
package com.db4o.test.compatibility;

import com.db4o.foundation.*;
import com.db4o.internal.*;

public class Program {
	public static void main(String[] args) {

		int[] ints = new int[]{
				Integer.MIN_VALUE, 
				-1, 
				0, 
				1, 
				Integer.MAX_VALUE};
		int bufferLength = Const4.INT_LENGTH * ints.length;
		ByteArrayBuffer buffer = new ByteArrayBuffer(bufferLength);
		for (int i = 0; i < ints.length; i++) {
			buffer.writeInt(ints[i]);
		}
		long checkSum = CRC32.checkSum(buffer._buffer, 0, buffer._buffer.length);
		System.out.print(checkSum);

	}

}");
        }

    }
}
