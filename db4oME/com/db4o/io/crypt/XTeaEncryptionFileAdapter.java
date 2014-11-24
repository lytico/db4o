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


*/
package com.db4o.io.crypt;

import java.io.*;

import com.db4o.*;
import com.db4o.io.*;

/**
* XTeaEncryptionFileAdapter is an encryption IoAdapter plugin for db4o file IO
* <br>
* that realized XTEA encryption algorithm. <br>
* <br>
* Configure db4o to add this encryption mechanism:<br>
* <code>Db4o.configure().io(new XTeaEncryptionFileAdapter("password"));
* </code><br>
* Any changes must be taken with the same password.<br>
* <br>
* 
* 
* Remember that any configuration settings must be set before opening
* ObjectContainer.
*/
public class XTeaEncryptionFileAdapter extends IoAdapter {
	// used as prototype in factory mode and as delegate in implementation mode
	private IoAdapter _adapter;

	// factory mode member
	private String _key;

	// implementation mode members
	private XTEA _xtea;

	private long _pos;

	private XTEA.IterationSpec _iterat;


	/**
	 * 
	 * Creates a new XTeaEncryptionFileAdapter instance using delegated
	 * IoAdapter, the given password and iterations count.
	 * 
	 * @param adapter 
	 *            delegated IoAdapter,
	 * @param password 
	 *            the key, used in ecryption/decryption routine.
	 * @param iterat
	 *            iterations count. Possible values are 8, 16, 32 and 64.
	 */
	public XTeaEncryptionFileAdapter(IoAdapter adapter, String password,
			XTEA.IterationSpec iterat) {
		_adapter = adapter;
		_key = password;
		_iterat = iterat;
	}
	/**
	 * 
	 * Creates a new XTeaEncryptionFileAdapter instance using delegated
	 * IoAdapter and the given password. The default value of rounds is 32. 
	 * 
	 * @param adapter 
	 *            delegated IoAdapter,
	 * @param password 
	 *            the key, used in ecryption/decryption routine.
	 * 
	 */
	public XTeaEncryptionFileAdapter(IoAdapter adapter, String password) {
		_adapter = adapter;
		_key = password;
		_iterat = XTEA.ITERATIONS32;
	}

	private XTeaEncryptionFileAdapter(IoAdapter adapter, XTEA xtea) {
		_adapter = adapter;
		_xtea = xtea;
	}

	/**
	 * implement to close the adapter
	 */
	public void close() throws IOException {
		_adapter.close();
	}

	public void delete(String path) {
		_adapter.delete(path);
	}
	
	public boolean exists(String path) {
		return _adapter.exists(path);
	}
	
	/**
	 * implement to return the absolute length of the file
	 */
	public long getLength() throws IOException {
		return _adapter.getLength();
	}

	/**
	 * implement to open the file
	 */
	public IoAdapter open(String path, boolean lockFile, long initialLength)
			throws IOException {
		//System.out.println(_iterat + " iteration number");
		return new XTeaEncryptionFileAdapter(_adapter.open(path, lockFile,
				initialLength), new XTEA(_key, _iterat));

	}

	/**
	 * implement to read and decrypt a buffer
	 */
	public int read(byte[] bytes, int length) throws IOException {
		long origPos = _pos;
		int fullLength = length;
		int prePad = (int) (_pos % 8);
		fullLength += prePad;
		int overhang = fullLength % 8;
		int postPad = (overhang == 0 ? 0 : 8 - (overhang));
		fullLength += postPad;
		byte[] pb = new byte[fullLength];
		if (prePad != 0) {
			seek(_pos - prePad);
		}
		int readResult = _adapter.read(pb);
		if (Deploy.debug) {
			log("3. before dencrypt/read->", pb);
		}
		_xtea.decrypt(pb);

		System.arraycopy(pb, prePad, bytes, 0, length);
		if (Deploy.debug) {
			log("4. after dencrypt/read->", pb);
		}
		seek(origPos + length);
		return readResult;
	}

	/**
	 * implement to set the read/write pointer in the file
	 */
	public void seek(long pos) throws IOException {
		_pos = pos;
		_adapter.seek(pos);
	}

	/**
	 * implement to flush the file contents to storage
	 */
	public void sync() throws IOException {
		_adapter.sync();
	}

	/**
	 * implement to write and encrypt a buffer
	 */
	public void write(byte[] buffer, int length) throws IOException {
		long origPos = _pos;
		int fullLength = length;
		int prePad = (int) (_pos % 8);
		fullLength += prePad;
		int overhang = fullLength % 8;
		int postPad = (overhang == 0 ? 0 : 8 - (overhang));
		fullLength += postPad;
		byte[] pb = new byte[fullLength];
		if (prePad != 0) {
			seek(origPos - prePad);
		}
		// NOTE: This heuristic may break if unaligned accesses within db4o (for example
		// due to config block layout changes) change behavior. To make this more safe
		// (but at the cost of a ~20% performance penalty on writes) make this read/seek
		// action unconditional, i.e. comment the if block.
		if (blockSize() % 8 != 0 || prePad != 0) {
			read(pb);
			seek(origPos - prePad);
		}
		System.arraycopy(buffer, 0, pb, prePad, length);
		if (prePad == 0) {
			// for (int i = buffer.length; i < pb.length; i++) {
			// pb[i] = (byte) (Math.random());
			// }
		}
		if (Deploy.debug) {
			log("1. before encrypt/write->", pb);
		}

		_xtea.encrypt(pb);
		if (Deploy.debug) {
			log("2. after encrypt/write->", pb);
		}
		_adapter.write(pb, pb.length);
		seek(origPos + length);
	}

	private void log(String msg, byte[] buf) {
		System.out.println("\n " + msg);
		for (int idx = 0; idx < buf.length; idx++) {
			System.out.print(buf[idx] + " ");
		}
	}

}
