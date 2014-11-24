using System;
using System.Collections;
using OManager.DataLayer.PropertyDetails;

namespace OMAddinDataTransferLayer.DataBaseDetails
{
	public class ClassProperties : MarshalByRefObject, IClassProperties
	{

		ArrayList  fieldEntries;
		int objectNumber;


		public int NoOfObjects
		{
			get { return objectNumber; }
			set { objectNumber = value; }
		}


		public ArrayList FieldEntries
		{
			get { return fieldEntries; }
			set { fieldEntries = value; }
		}

		public ClassProperties GetClassProperties(string className)
		{

			ClassDetails clsDetails = new ClassDetails(className);
			fieldEntries = new ProxyFieldProperties().GetFieldProperties(className);
			objectNumber = clsDetails.GetNumberOfObjects();
			return this;

		}

		public int GetObjectCountForAClass(string classname)
		{
			ClassDetails classDetails = new ClassDetails(classname);
			return classDetails.GetNumberOfObjects();
		}

		public void SetIndexedConfiguration(ArrayList fieldname, string className, ArrayList isIndexed, string dbPath, bool customConfig)
		{
			ClassDetails classDetails = new ClassDetails(className);
            classDetails.SetIndex(fieldname, className, isIndexed, dbPath, customConfig);
		}

		public override object InitializeLifetimeService()
		{

			return null;
		} 
	}

	

}