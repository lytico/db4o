using System;
using OMAddinDataTransferLayer.TypeMauplation;
using OManager.BusinessLayer.QueryRenderer;

namespace OMAddinDataTransferLayer.DataPopulation
{
	[Serializable]
	public class ProxyTreeGridRenderer
	{
        private string displayfieldName;
		private string fieldName;
		private string fieldValue;
        private string qualifiedName;
		private string fieldType;
		private long objectId;
		private bool readOnlyStatus;
		private bool hasSubNode;
		private object subObject;
		private ProxyType objectType;

        public string DisplayFieldName
        {
            get { return displayfieldName; }
            set { displayfieldName = value; }
        }
		public bool ReadOnlyStatus
		{
			get { return readOnlyStatus; }
			set { readOnlyStatus = value; }

		}
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

		public string FieldValue
		{
			get { return fieldValue; }
			set { fieldValue = value; }
		}
        public string QualifiedName
        {
            get { return qualifiedName; }
            set { qualifiedName = value; }
        }
		public string FieldType
		{
			get { return fieldType; }
			set { fieldType = value; }
		}

		public long ObjectId
		{
			get { return objectId; }
			set { objectId = value; }
		}

		public bool HasSubNode
		{
			get { return hasSubNode; }
			set { hasSubNode = value; }

		}
		public object SubObject
		{
			get { return subObject; }
			set { subObject = value; }
		}
		public ProxyType  ObjectType
		{
			get { return objectType; }
			set { objectType = value; }
		}

		public ProxyTreeGridRenderer ConvertObject(TreeGridViewRenderer obj)
		{
			ProxyTreeGridRenderer proxy = null;
			if (obj != null)
			{
			    proxy = new ProxyTreeGridRenderer
			                {
			                    DisplayFieldName = obj.DisplayFieldName,
			                    fieldName = obj.FieldName,
			                    ObjectId = obj.ObjectId,
			                    QualifiedName = obj.QualifiedName,
			                    fieldValue = obj.FieldValue,
			                    fieldType = obj.FieldType
			                };

			    proxy.ObjectId = obj.ObjectId;
				proxy.readOnlyStatus = obj.ReadOnlyStatus;
				proxy.subObject = obj.SubObject;
				proxy.hasSubNode = obj.HasSubNode;
              
				if (obj.ObjectType != null)
					proxy.ObjectType = new ProxyType().CovertITypeToProxyType(obj.ObjectType  );
			}
			return proxy;
		}
	}
}