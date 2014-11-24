using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMAddinDataTransferLayer.TypeMauplation;
using OManager.DataLayer.PropertyTable;

namespace OMAddinDataTransferLayer.DataBaseDetails
{
	[Serializable ]
	class ProxyFieldProperties
	{

		public string Field { get; set; }
		public bool Indexed { get; set; }
		public bool Public { get; set; }
		public string DataType { get; set; }
		public ProxyType Type { get; set; }

			

		public ArrayList  GetFieldProperties(string className)

		{
			ArrayList arr1 = new ArrayList();
			 List<FieldProperties> arr=FieldProperties.FieldsFrom(className);
			foreach (FieldProperties f in arr)
			{
				ProxyFieldProperties p = ConverFieldProperties(f);
				arr1.Add(p);
			}
			return arr1;
		}

		private ProxyFieldProperties ConverFieldProperties(FieldProperties properties)
		{
			ProxyFieldProperties p = new ProxyFieldProperties();

			p.Field = properties.Field;
			p.Indexed = properties.Indexed;
			p.Public = properties.Public;
			p.DataType = properties.DataType;
			p.Type = new ProxyType().CovertITypeToProxyType(properties.Type);
			return p;
		}
	}
	
}
