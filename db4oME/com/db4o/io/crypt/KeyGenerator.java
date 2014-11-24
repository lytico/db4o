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

KeyGenerator.java:

Computation of 128-bit int number, returned as an array of four.
Implementation based of the RSA Data Security, Inc. MD5 Message Digest
Algorithm, as defined in RFC 1321.

MD5 Message Digest Algorithm description in "Applied Cryptography.
Protocols, Algorithms, and Source Code in C", Bruce Schneier, second
edition, Chapter 18, P.436.
*/
package com.db4o.io.crypt;

/**
 * @exclude
 */
public class KeyGenerator {
	/**
	 * default constructor, creates an instance of KeyGenerator.
	 */
	public KeyGenerator() {

	}

	/**
	 * generates an int[] array which has length four and produced according
	 * MD5 Message Digest Algorithm, as defined in RFC 1321.
	 * 
	 * @param message -
	 *            represents key to be converted as integer 128-bits<br>
	 *            (will be used in XTEA encryption algorithm as key.)
	 * @return int array of size 128-bit.
	 */
	public int[] core(String message) {

		int[] messageAsInt = string2integer_padding(message);
		int a = 0x01234567;
		int b = 0x89abcdef;
		int c = 0xfedcba98;
		int d = 0x76543210;

		for (int i = 0; i < messageAsInt.length; i += 16) {
			int variable_a = a;
			int variable_b = b;
			int variable_c = c;
			int variable_d = d;

			a = ff(a, b, c, d, messageAsInt[i + 0], 7, 0xD76AA478);
			d = ff(d, a, b, c, messageAsInt[i + 1], 12, 0xE8C7B756);
			c = ff(c, d, a, b, messageAsInt[i + 2], 17, 0x242070DB);
			b = ff(b, c, d, a, messageAsInt[i + 3], 22, 0xC1BDCEEE);
			a = ff(a, b, c, d, messageAsInt[i + 4], 7, 0xF57C0FAF);
			d = ff(d, a, b, c, messageAsInt[i + 5], 12, 0x4787C62A);
			c = ff(c, d, a, b, messageAsInt[i + 6], 17, 0xA8304613);
			b = ff(b, c, d, a, messageAsInt[i + 7], 22, 0xFD469501);
			a = ff(a, b, c, d, messageAsInt[i + 8], 7, 0x698098D8);
			d = ff(d, a, b, c, messageAsInt[i + 9], 12, 0x8B44F7AF);
			c = ff(c, d, a, b, messageAsInt[i + 10], 17, 0xFFFF5BB1);
			b = ff(b, c, d, a, messageAsInt[i + 11], 22, 0x895CD7BE);
			a = ff(a, b, c, d, messageAsInt[i + 12], 7, 0x6B901122);
			d = ff(d, a, b, c, messageAsInt[i + 13], 12, 0xFD987193);
			c = ff(c, d, a, b, messageAsInt[i + 14], 17, 0xA679438E);
			b = ff(b, c, d, a, messageAsInt[i + 15], 22, 0x49B40821);

			a = gg(a, b, c, d, messageAsInt[i + 1], 5, 0xF61E2562);
			d = gg(d, a, b, c, messageAsInt[i + 6], 9, 0xC040B340);
			c = gg(c, d, a, b, messageAsInt[i + 11], 14, 0x265E5A51);
			b = gg(b, c, d, a, messageAsInt[i + 0], 20, 0xE9B6C7AA);
			a = gg(a, b, c, d, messageAsInt[i + 5], 5, 0xD62F105D);
			d = gg(d, a, b, c, messageAsInt[i + 10], 9, 0x02441453);
			c = gg(c, d, a, b, messageAsInt[i + 15], 14, 0xD8A1E681);
			b = gg(b, c, d, a, messageAsInt[i + 4], 20, 0xE7D3FBC8);
			a = gg(a, b, c, d, messageAsInt[i + 9], 5, 0x21E1CDE6);
			d = gg(d, a, b, c, messageAsInt[i + 14], 9, 0xC33707D6);
			c = gg(c, d, a, b, messageAsInt[i + 3], 14, 0xF4D50D87);
			b = gg(b, c, d, a, messageAsInt[i + 8], 20, 0x455A14ED);
			a = gg(a, b, c, d, messageAsInt[i + 13], 5, 0xA9E3E905);
			d = gg(d, a, b, c, messageAsInt[i + 2], 9, 0xFCEFA3F8);
			c = gg(c, d, a, b, messageAsInt[i + 7], 14, 0x676F02D9);
			b = gg(b, c, d, a, messageAsInt[i + 12], 20, 0x8D2A4C8A);

			a = hh(a, b, c, d, messageAsInt[i + 5], 4, 0xFFFA3942);
			d = hh(d, a, b, c, messageAsInt[i + 8], 11, 0x8771F681);
			c = hh(c, d, a, b, messageAsInt[i + 11], 16, 0x6D9D6122);
			b = hh(b, c, d, a, messageAsInt[i + 14], 23, 0xFDE5380C);
			a = hh(a, b, c, d, messageAsInt[i + 1], 4, 0xA4BEEA44);
			d = hh(d, a, b, c, messageAsInt[i + 4], 11, 0x4BDECFA9);
			c = hh(c, d, a, b, messageAsInt[i + 7], 16, 0xF6BB4B60);
			b = hh(b, c, d, a, messageAsInt[i + 10], 23, 0xBEBFBC70);
			a = hh(a, b, c, d, messageAsInt[i + 13], 4, 0x289B7EC6);
			d = hh(d, a, b, c, messageAsInt[i + 0], 11, 0xEAA127FA);
			c = hh(c, d, a, b, messageAsInt[i + 3], 16, 0xD4EF3085);
			b = hh(b, c, d, a, messageAsInt[i + 6], 23, 0x04881D05);
			a = hh(a, b, c, d, messageAsInt[i + 9], 4, 0xD9D4D039);
			d = hh(d, a, b, c, messageAsInt[i + 12], 11, 0xE6DB99E5);
			c = hh(c, d, a, b, messageAsInt[i + 15], 16, 0x1FA27CF8);
			b = hh(b, c, d, a, messageAsInt[i + 2], 23, 0xC4AC5665);

			a = ii(a, b, c, d, messageAsInt[i + 0], 6, 0xF4292244);
			d = ii(d, a, b, c, messageAsInt[i + 7], 10, 0x432AFF97);
			c = ii(c, d, a, b, messageAsInt[i + 14], 15, 0xAB9423A7);
			b = ii(b, c, d, a, messageAsInt[i + 5], 21, 0xFC93A039);
			a = ii(a, b, c, d, messageAsInt[i + 12], 6, 0x655B59C3);
			d = ii(d, a, b, c, messageAsInt[i + 3], 10, 0x8F0CCC92);
			c = ii(c, d, a, b, messageAsInt[i + 10], 15, 0xFFEFF47D);
			b = ii(b, c, d, a, messageAsInt[i + 1], 21, 0x85845DD1);
			a = ii(a, b, c, d, messageAsInt[i + 8], 6, 0x6FA87E4F);
			d = ii(d, a, b, c, messageAsInt[i + 15], 10, 0xFE2CE6E0);
			c = ii(c, d, a, b, messageAsInt[i + 6], 15, 0xA3014314);
			b = ii(b, c, d, a, messageAsInt[i + 13], 21, 0x4E0811A1);
			a = ii(a, b, c, d, messageAsInt[i + 4], 6, 0xF7537E82);
			d = ii(d, a, b, c, messageAsInt[i + 11], 10, 0xBD3AF235);
			c = ii(c, d, a, b, messageAsInt[i + 2], 15, 0x2AD7D2BB);
			b = ii(b, c, d, a, messageAsInt[i + 9], 21, 0xEB86D391);

			a = addInteger_wrappingAt32(a, variable_a);
			b = addInteger_wrappingAt32(b, variable_b);
			c = addInteger_wrappingAt32(c, variable_c);
			d = addInteger_wrappingAt32(d, variable_d);
		}
		return new int[] { a, b, c, d };
	}

	/**
	 * core operation of MD5 Message Digest Algorithm
	 * 
	 */
	private int core_operation(int q, int a, int b, int x, int s, int t) {
		return addInteger_wrappingAt32(bitwiseRotate32BitNumberLeft(
				addInteger_wrappingAt32(addInteger_wrappingAt32(a, q),
						addInteger_wrappingAt32(x, t)), s), b);
	}

	private int ff(int a, int b, int c, int d, int x, int s, int t) {
		return core_operation(f(b, c, d), a, b, x, s, t);
	}

	/**
	 * nonlinear function, used in each round
	 * 
	 */
	private int f(int b, int c, int d) {
		return (b & c) | ((~b) & d);
	}

	private int gg(int a, int b, int c, int d, int x, int s, int t) {
		return core_operation(g(b, c, d), a, b, x, s, t);
	}

	/**
	 * nonlinear function, used in each round
	 * 
	 */
	private int g(int b, int c, int d) {
		return (b & d) | (c & (~d));
	}

	private int hh(int a, int b, int c, int d, int x, int s, int t) {
		return core_operation(h(b, c, d), a, b, x, s, t);
	}

	/**
	 * nonlinear function, used in each round
	 * 
	 */
	private int h(int b, int c, int d) {
		return b ^ c ^ d;
	}

	private int ii(int a, int b, int c, int d, int x, int s, int t) {
		return core_operation(i(b, c, d), a, b, x, s, t);
	}

	/**
	 * nonlinear function, used in each round
	 * 
	 */
	private int i(int b, int c, int d) {
		return c ^ (b | (~d));
	}

	/**
	 * converts incoming padded String to an int array of size 128-bit.
	 * 
	 * @param incomingString -
	 *            String to be converted to an int array of size 128-bit.
	 * @return 128-bit int array
	 */
	private int[] string2integer_padding(String incomingString) {
		int i = 0;
		int block = ((incomingString.length() + 8) >> 6) + 1;
		int[] padded = new int[block * 16];
		for (i = 0; i < block * 16; i++) {
			padded[i] = 0;
		}
		for (i = 0; i < incomingString.length(); i++) {
			padded[i >> 2] |= incomingString.charAt(i) << ((i % 4) * 8);
		}
		padded[i >> 2] |= 0x80 << ((i % 4) * 8);
		padded[block * 16 - 2] = incomingString.length() * 8;

		return padded;
	}

	private int addInteger_wrappingAt32(int x, int y) {
		return ((x & 0x7FFFFFFF) + (y & 0x7FFFFFFF)) ^ (x & 0x80000000)
				^ (y & 0x80000000);
	}

	private int bitwiseRotate32BitNumberLeft(int number, int p) {
		return (number << p) | (number >>> (32 - p));
	}

}



