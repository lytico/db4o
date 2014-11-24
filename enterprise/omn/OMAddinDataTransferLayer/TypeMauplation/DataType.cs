using System;
using OManager.Business.Config;
using OManager.BusinessLayer.UIHelper;
using OManager.DataLayer.PropertyDetails;


namespace OMAddinDataTransferLayer.TypeMauplation
{
	public class DataType : MarshalByRefObject,IDataType 
	{
		public string CastedValueOrNullConstant(object value, string name, string className)
		{
			IType type = new FieldDetails(className, name).GetFieldType();
			if (type == null)
				return null;
			return value != null && value.ToString() != "null" ? type.Cast(value).ToString()  : "null";
		}

		public bool ValidateDataType(string classname, string fieldname, object data)
		{
			if (null == data && "null" == data.ToString())
				return false;
            IType objectType = new FieldDetails(classname, fieldname).GetFieldType();
		    if (objectType == null)
				return false;
			objectType.Cast(data);
			return true;
		}
        
        public object ReturnCastObject(string classname, object data)
        {
            if (null == data && "null" == data.ToString())
                return false;
            IType objectType = dbInteraction.GetType(classname);
            if (objectType == null)
                return false;
           return  objectType.Cast(data);
        }

	    public bool CheckIfObjectCanBeCasted(string classname, object data)
        {
            if (null == data && "null" == data.ToString())
                return false;
            IType objectType = dbInteraction.GetType(classname);
            if (objectType == null)
                return false;
            objectType.Cast(data);
            return true;
        }

		public ProxyType GetFieldType(string declaringClassName, string name)
		{
		    IType type = dbInteraction.GetFieldType(declaringClassName, name);
			if (type == null)
				return null;
            return new ProxyType().CovertITypeToProxyType(type);
		}

        public bool CheckIfCollection(long id, string fieldname)
        {
          return dbInteraction.CheckForCollection(id,fieldname );
        }

        public bool CheckIfCollection(long id)
        {
            return dbInteraction.CheckForCollection(id);
        }
        public bool CheckIfCollection(string className, string fieldName)
        {
            return dbInteraction.CheckForCollection(className, fieldName);
        }
	    public object CheckIfObjectCanBeCasted(string classname, string fieldname, object data)
		{
			if (null == data && "null" == data.ToString())
				return false;
			IType objectType = new FieldDetails(classname, fieldname).GetFieldType();
			if (objectType == null)
				return null;
			return objectType.Cast(data);
		}
		public ProxyType ResolveType(string typeDetails)
		{
			IType type = dbInteraction.ResolveType(typeDetails);
			if (type == null)
				return null;
			return new ProxyType().CovertITypeToProxyType(type);
		}

		public string ReturnDisplayNameOfType(string typeDetail)
		{
			IType type = dbInteraction.ResolveType(typeDetail);
			if (type == null)
				return null;
			return type.IsNullable ? type.UnderlyingType.DisplayName : type.DisplayName;
		}

		public bool CheckTypeIsSame(string typeDetail, Type datatype)
		{
			IType type = dbInteraction.ResolveType(typeDetail);
			if (type == null)
				return false;
			return type.IsSameAs(datatype);
		}
		
		public override object InitializeLifetimeService()
		{

			return null;
		} 
	}
}
