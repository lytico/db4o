package com.db4o.omplus.datalayer.propertyViewer.dbProperties;

/**
 * Propeties associated with a database
 * 
 * @author prameela_nair
 *
 */
public class DBProperties
{
	private long dbSize;
	private long noOfClasses;
	private long freeSpace;
	public long getDbSize() {
		return dbSize;
	}
	public void setDbSize(long dbSize) {
		this.dbSize = dbSize;
	}
	public long getNoOfClasses() {
		return noOfClasses;
	}
	public void setNoOfClasses(long noOfClasses) {
		this.noOfClasses = noOfClasses;
	}
	public long getFreeSpace() {
		return freeSpace;
	}
	public void setFreeSpace(long freeSpace) {
		this.freeSpace = freeSpace;
	}
}
