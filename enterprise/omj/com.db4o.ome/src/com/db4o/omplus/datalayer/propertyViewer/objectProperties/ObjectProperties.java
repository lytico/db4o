package com.db4o.omplus.datalayer.propertyViewer.objectProperties;



import com.db4o.ext.Db4oUUID;

public class ObjectProperties {
	
	private long	localID;
	
	private	int		depth;
	
	private	long	version;
	
	private	Db4oUUID	uuid;
	
	public long getLocalID() {
		return localID;
	}

	public void setLocalID(long localID) {
		this.localID = localID;
	}

	public int getDepth() {
		return depth;
	}

	public void setDepth(int depth) {
		this.depth = depth;
		
	}

	public long getVersion() {
		return version;
	}

	public void setVersion(long version) {
		this.version = version;
	}

	public Db4oUUID getUuid()
	{
		return uuid;
	}
	
	public String getUuidAsString()
	{
		if(uuid !=null){
			StringBuilder sb = new StringBuilder(50);
			sb.append(uuid.getSignaturePart());
			sb.append(uuid.getLongPart());
			return sb.toString();
		}
		else
			return "NA";
	}

	public void setUuid(Db4oUUID uuid) {
		this.uuid = uuid;
	}

}
