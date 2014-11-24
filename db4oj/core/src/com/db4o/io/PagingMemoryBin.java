/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */
package com.db4o.io;

import java.util.*;

import com.db4o.ext.*;

/**
 * @exclude
 */
public class PagingMemoryBin implements Bin {

	private final int _pageSize;
	private List<byte[]> _pages = new ArrayList<byte[]>();
	private int _lastPageLength;
	
	public PagingMemoryBin(int pageSize) {
		this(pageSize, 0);
    }

	public PagingMemoryBin(int pageSize, long initialLength) {
		_pageSize = pageSize;
		ensureLength(initialLength);
    }

	public long length() {
		if(_pages.size() == 0) {
			return 0;
		}
		return (_pages.size() - 1) * _pageSize + _lastPageLength;
	}
	
	public int read(long pos, byte[] buffer, int length) throws Db4oIOException {
		final long avail = length() - pos;
		if (avail <= 0) {
			return - 1;
		}
		final int bytesToRead = Math.min((int)avail, length);
		int offset = pageOffset(pos);
		int pageIdx = pageIdx(pos);
		int bytesRead = 0;
		while(bytesRead < bytesToRead) {
			byte[] curPage = _pages.get(pageIdx);
			int chunkLength = Math.min(length - bytesRead, _pageSize - offset);
			System.arraycopy(curPage, offset, buffer, bytesRead, chunkLength);
			bytesRead += chunkLength;
			pageIdx++;
			offset = 0;
		}
		return bytesToRead;
	}

	public void sync() throws Db4oIOException {
	}
	
	public int syncRead(long position, byte[] bytes, int bytesToRead) {
		return read(position, bytes, bytesToRead);
	}
	
	public void close() {
	}

	public void write(long pos, byte[] buffer, int length) throws Db4oIOException {
		ensureLength(pos + length);
		int offset = pageOffset(pos);
		int pageIdx = pageIdx(pos);
		int bytesWritten = 0;
		while(bytesWritten < length) {
			byte[] curPage = _pages.get(pageIdx);
			int chunkLength = Math.min(length - bytesWritten, _pageSize - offset);
			System.arraycopy(buffer, bytesWritten, curPage, offset, chunkLength);
			bytesWritten += chunkLength;
			pageIdx++;
			offset = 0;
		}
	}
	
	private void ensureLength(long length) {
		if (length <= 0) {
			return;
		}
		
		long lastPos = length - 1;
		int lastPosPageIdx = pageIdx(lastPos);
		int lastPosPageLength = pageOffset(lastPos) + 1;
		
		if (lastPosPageIdx == _pages.size() - 1) {
			_lastPageLength = Math.max(lastPosPageLength, _lastPageLength);
			return;
		}
		
		if (lastPosPageIdx < _pages.size()) {
			return;
		}
		
		for(int newPageIdx = _pages.size(); newPageIdx <= lastPosPageIdx; newPageIdx++) {
			_pages.add(new byte[_pageSize]);
		}
		_lastPageLength = lastPosPageLength;
	}
	
	private int pageIdx(long pos) {
		return (int)(pos / _pageSize);
	}
	
	private int pageOffset(long pos) {
		return (int)(pos % _pageSize);
	}
	
	public void sync(Runnable runnable) {
		runnable.run();
	}

}