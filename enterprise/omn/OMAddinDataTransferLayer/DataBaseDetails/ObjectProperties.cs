using System;
using OManager.DataLayer.PropertyDetails;

namespace OMAddinDataTransferLayer.DataBaseDetails
{
	public class ObjectProperties : MarshalByRefObject, IObjectProperties
	{
		string objectUuid;
		long localId;
		int depth;
		long version;

		
		public long Version
		{
			get { return version; }
			set { version = value; }
		}

		public string ObjectUuid
		{
			get { return objectUuid; }
			set { objectUuid = value; }
		}
		public long LocalId
		{
			get { return localId; }
			set { localId = value; }
		}
		public int Depth
		{
			get { return depth; }
			set { depth = value; }
		}

		public ObjectProperties GetObjectProperties(long id)
		{
			ObjectDetails objDetails = new ObjectDetails(id);
			ObjectUuid = objDetails.GetUUID();
			LocalId = objDetails.GetLocalID();
			Version = objDetails.GetVersion();
			return this;

		}
		public override object InitializeLifetimeService()
		{

			return null;
		} 
	}
}
