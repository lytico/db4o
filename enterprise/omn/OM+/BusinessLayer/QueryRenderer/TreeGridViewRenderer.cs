using System;
using OManager.Business.Config;

namespace OManager.BusinessLayer.QueryRenderer
{
	[Serializable]
	public class TreeGridViewRenderer
    {
		private string displayFieldName;
	    private string fieldName;
        private string qualifiedName;
		private string fieldValue;
		private string fieldType;
		private long objectId;
		private bool readOnlyStatus;
		private bool hasSubNode;
		private object subObject;
		private IType objectType;

		public bool ReadOnlyStatus
		{
			get { return readOnlyStatus; }
			set { readOnlyStatus = value; }
		}

		public string DisplayFieldName
		{
			get { return displayFieldName;}
			set { displayFieldName=value;}
		}

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        public string QualifiedName
        {
            get { return qualifiedName; }
            set { qualifiedName = value; }
        }

		public string FieldValue
		{
			get { return fieldValue; }
			set { fieldValue = value; }
		}
		public string FieldType
		{
			get { return fieldType; }
			set { fieldType = value; }

		}

		public long ObjectId
		{
			get { return objectId ; }
			set { objectId  = value; }

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

		public IType ObjectType
		{
			get { return objectType; }
			set { objectType = value; }

		}
    }
}
