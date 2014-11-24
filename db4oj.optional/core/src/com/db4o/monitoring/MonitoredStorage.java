package com.db4o.monitoring;

import com.db4o.*;
import com.db4o.io.*;

import static com.db4o.foundation.Environments.*;

/**
 * Publishes storage statistics to JMX.
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class MonitoredStorage extends StorageDecorator {

	public MonitoredStorage(Storage storage) {
		super(storage);
	}
	
	@Override
	protected Bin decorate(BinConfiguration config, Bin bin) {
		return new MonitoredBin(config.uri(), bin);
	}
	
	private static class MonitoredBin extends BinDecorator {

		private IO _ioMBean;

		public MonitoredBin(String uri, Bin bin) {
			super(bin);
			_ioMBean = Db4oMBeans.newIOStatsMBean(my(ObjectContainer.class));
		}
		
		@Override
		public void sync() {
			super.sync();
			_ioMBean.notifySync();
		}
		
		@Override
		public void sync(Runnable runnable) {
			super.sync(runnable);
			_ioMBean.notifySync();
		}
		
		@Override
		public int read(long position, byte[] bytes, int bytesToRead) {
			int bytesRead = super.read(position, bytes, bytesToRead);
			_ioMBean.notifyBytesRead(bytesRead);
			return bytesRead;
		}
		
		@Override
		public int syncRead(long position, byte[] bytes, int bytesToRead) {
			int bytesRead = super.syncRead(position, bytes, bytesToRead);
			_ioMBean.notifyBytesRead(bytesRead);
			return bytesRead;
		}
		
		@Override
		public void write(long position, byte[] bytes, int bytesToWrite) {
			super.write(position, bytes, bytesToWrite);
			_ioMBean.notifyBytesWritten(bytesToWrite);
		}
	}
}
