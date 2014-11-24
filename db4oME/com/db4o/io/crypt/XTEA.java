/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. 

XTEA.java:

Java version of XTEA encryption algorithm was developed using the prinsiple
described in http://en.wikipedia.org/wiki/XTEA. 


*/
package com.db4o.io.crypt;

/**
 * @exclude
 */
public class XTEA {

	/**
	 * @exclude
	 */
	public final static class IterationSpec {
		int _iterations;
		int _deltaSumInitial;
		
		IterationSpec(int iterations,int deltaSumInitial) {
			_iterations=iterations;
			_deltaSumInitial=deltaSumInitial;
		}
	}

	public final static IterationSpec ITERATIONS8=new IterationSpec(8,0xF1BBCDC8);
	public final static IterationSpec ITERATIONS16=new IterationSpec(16,0xE3779B90);
	public final static IterationSpec ITERATIONS32=new IterationSpec(32,0xC6EF3720);
	public final static IterationSpec ITERATIONS64=new IterationSpec(64,0x8DDE6E40);

	private final IterationSpec _iterationSpec;
	
	private static final int DELTA = 0x9E3779B9;

	private int[] _key;

	/**
	 * creates an XTEA object from the given String key and iterations count.
	 * 
	 * @param key
	 *            the key, used in ecryption/decryption routine.
	 * @param iterationSpec
	 *            The iteration spec containing iterations count. 
     *            Possible values are 8, 16, 32 and 64.
	 * 
	 */
	public XTEA(String key, IterationSpec iterationSpec) {
		this(new KeyGenerator().core(key), iterationSpec);
	}

	/**
	 * creates an XTEA object from the given String key. The default value of
	 * rounds is 32;
	 * 
	 * @param key
	 *            the key, used in ecryption/decryption routine.
	 * 
	 * 
	 */
	public XTEA(String key) {
		this(new KeyGenerator().core(key), ITERATIONS32);
	}

	/**
	 * creates an XTEA object from an int array of four
	 * @throws IllegalArgumentException
	 * 
	 */
	private XTEA(int[] key, IterationSpec iterationSpec) throws IllegalArgumentException {
		if (key.length != 4) {
			throw new IllegalArgumentException();
		}
		_key = key;
		_iterationSpec=iterationSpec;
	}

	/**
	 * converts incoming array of eight bytes from offset to array of two
	 * integer values.<br>
	 * (An Integer is represented in memory as four bytes.)
	 * 
	 * @param bytes
	 *            Incoming byte array of length eight to be converted<br>
	 * @param offset
	 *            Offset from which to start converting bytes<br>
	 * @param res
	 *            Int array of length two which contains converted array bytes.
	 * 
	 */
	private void byte2int(byte[] bytes, int offset, int[] res) {
		res[0] = (int) ((((int) bytes[offset] & 0xff) << 24)
				| (((int) bytes[offset + 1] & 0xff) << 16)
				| (((int) bytes[offset + 2] & 0xff) << 8) | ((int) bytes[offset + 3] & 0xff));
		res[1] = (int) ((((int) bytes[offset + 4] & 0xff) << 24)
				| (((int) bytes[offset + 5] & 0xff) << 16)
				| (((int) bytes[offset + 6] & 0xff) << 8) | ((int) bytes[offset + 7] & 0xff));
	}

	/**
	 * converts incoming array of two integers from offset to array of eight
	 * bytes.<br>
	 * (An Integer is represented in memory as four bytes.)
	 * 
	 * @param i
	 *            Incoming integer array of two to be converted<br>
	 * @param offset
	 *            Offset from which to start converting integer values<br>
	 * @param res
	 *            byte array of length eight which contains converted integer
	 *            array i.
	 */
	private void int2byte(int[] i, int offset, byte[] res) {
		res[offset] = (byte) ((i[0] & 0xff000000) >>> 24);
		res[offset + 1] = (byte) ((i[0] & 0x00ff0000) >>> 16);
		res[offset + 2] = (byte) ((i[0] & 0x0000ff00) >>> 8);
		res[offset + 3] = (byte) (i[0] & 0x000000ff);
		res[offset + 4] = (byte) ((i[1] & 0xff000000) >>> 24);
		res[offset + 5] = (byte) ((i[1] & 0x00ff0000) >>> 16);
		res[offset + 6] = (byte) ((i[1] & 0x0000ff00) >>> 8);
		res[offset + 7] = (byte) (i[1] & 0x000000ff);

	}

	/**
	 * enciphers two int values
	 * 
	 * @param block 
	 *            int array to be encipher according to the XTEA encryption
	 *            algorithm<br>
	 */
	private void encipher(int[] block) {
		int n = _iterationSpec._iterations;
		int delta_sum = 0;
		while (n-- > 0) {
			block[0] += ((block[1] << 4 ^ block[1] >> 5) + block[1])
					^ (delta_sum + _key[delta_sum & 3]);
			delta_sum += DELTA;
			block[1] += ((block[0] << 4 ^ block[0] >> 5) + block[0])
					^ (delta_sum + _key[delta_sum >> 11 & 3]);

		}
	}

	/**
	 * deciphers two int values
	 * 
	 * @param e_block
	 *            int array to be decipher according to the XTEA encryption
	 *            algorithm<br>
	 */
	private void decipher(int[] e_block) {
		int delta_sum = _iterationSpec._deltaSumInitial;
		int n = _iterationSpec._iterations;
		while (n-- > 0) {
			e_block[1] -= ((e_block[0] << 4 ^ e_block[0] >> 5) + e_block[0])
					^ (delta_sum + _key[delta_sum >> 11 & 3]);
			delta_sum -= DELTA;
			e_block[0] -= ((e_block[1] << 4 ^ e_block[1] >> 5) + e_block[1])
					^ (delta_sum + _key[delta_sum & 3]);
		}
	}

	/**
	 * encrypts incoming byte array according XTEA
	 * 
	 * @param buffer
	 *            incoming byte array to be encrypted
	 */
	public void encrypt(byte[] buffer) {
		int[] asInt = new int[2];
		for (int i = 0; i < buffer.length; i += 8) {
			byte2int(buffer, i, asInt);
			encipher(asInt);
			int2byte(asInt, i, buffer);
		}
	}

	/**
	 * decrypts incoming byte array according XTEA
	 * 
	 * @param buffer
	 *            incoming byte array to be decrypted
	 * 
	 */
	public void decrypt(byte[] buffer) {
		int[] asInt = new int[2];
		for (int i = 0; i < buffer.length; i += 8) {
			byte2int(buffer, i, asInt);
			decipher(asInt);
			int2byte(asInt, i, buffer);
		}
	}
}
