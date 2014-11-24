using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMAddinDataTransferLayer.AssemblyInfo;

namespace OMAddinDataTransferLayer.TypeMauplation
{
	public interface IDataType
	{
		ProxyType GetFieldType(string declaringClassName, string name);
		ProxyType ResolveType(string typeDetails);
		string CastedValueOrNullConstant(object value, string name, string ClassName);
		bool ValidateDataType(string classname, string fieldname, object data);
        object CheckIfObjectCanBeCasted(string classname, string fieldname, object data);
        bool CheckIfObjectCanBeCasted(string classname, object data);
        object ReturnCastObject(string classname, object data);
		string ReturnDisplayNameOfType(string typeDetail);
		bool CheckTypeIsSame(string typeDetail, Type datatype);
	    bool CheckIfCollection(long id, string fieldname);
	    bool CheckIfCollection(long id);
	    bool CheckIfCollection(string className, string fieldName);
	   
	}
}
