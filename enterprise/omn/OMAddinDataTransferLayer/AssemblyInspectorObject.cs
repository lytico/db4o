using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMAddinDataTransferLayer.AssemblyInfo;

using OMAddinDataTransferLayer.Connection;
using OMAddinDataTransferLayer.DataBaseDetails;
using OMAddinDataTransferLayer.DataEditing;
using OMAddinDataTransferLayer.DataPopulation;
using OMAddinDataTransferLayer.TypeMauplation;
using OME.Logging.ExceptionLogging;
using OME.Logging.Tracing;

namespace OMAddinDataTransferLayer
{
	public  class AssemblyInspectorObject
	{
		public static IAssemblyInspector AssemblyInspector;
		public static IConnection Connection;
		public static IPopulateData DataPopulation;
		public static IDataType DataType;
		public static ISaveData DataSave;
		public static IObjectProperties ObjectProperties;
		public static IClassProperties ClassProperties;
	    public static string InstallledPath;
		public static void ClearAll()
		{
			AssemblyInspector = null;
			Connection = null;
			DataPopulation = null;
			DataType = null;
			DataSave = null;
			ObjectProperties = null;
			ClassProperties = null;

		}
	}
}
