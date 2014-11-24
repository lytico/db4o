using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Reflect;
using OManager.Business.Config;
using OManager.BusinessLayer.Config;
using OManager.BusinessLayer.QueryRenderer;
using OManager.DataLayer.DataProcessing;
using OManager.DataLayer.PropertyDetails;
using OManager.DataLayer.QueryParser;
using OManager.DataLayer.Connection;
using OManager.BusinessLayer.Login;
using System.Windows.Forms;
using OManager.DataLayer.Maintanence;
using OManager.DataLayer.CommonDatalayer;
using OManager.DataLayer.DemoDBCreation;
using OME.Logging.Common;

namespace OManager.BusinessLayer.UIHelper
{
	public class dbInteraction
	{
        public static void SetIndexedConfiguration(ArrayList fieldname, string className, ArrayList isIndexed, string dbPath,bool customConfig)
		{
			ClassDetails classDetails = new ClassDetails(className);
			classDetails.SetIndex(fieldname, className, isIndexed, dbPath, customConfig );
		}

        public static IType GetFieldType(string declaringClassName, string name)
        {
            return new FieldDetails(declaringClassName, name).GetFieldType();
        }

        public static string GetClassName(long id)
        {
            object obj = GetObjById(id);
            return DataLayerCommon.ReflectClassFor(obj).GetName();
        }

        public static int GetFieldCount(string classname)
        {
            ClassDetails clsDetails = new ClassDetails(classname);
            return clsDetails.GetFieldCount();
        }


        public static IType GetType(string declaringClassName)
        {
            IReflectClass refClass = DataLayerCommon.ReflectClassForName(declaringClassName);
            IType type = Db4oClient.TypeResolver.Resolve(refClass);
            return type;
        }

        public static void SetFieldToNull(long id, string fieldName)
        {
            try
            {
                object obj = Db4oClient.Client.Ext().GetByID(id);
                Db4oClient.Client.Ext().Activate(obj, 1);
                IReflectClass klass = DataLayerCommon.ReflectClassFor(obj);
                if (klass != null)
                {
                    IReflectField field = DataLayerCommon.GetDeclaredFieldInHeirarchy(klass, fieldName);
                    if (field == null)
                        return;

                    field.Set(obj, null);
                    Db4oClient.Client.Store(obj);

                    Db4oClient.Client.Commit();
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
        }

		public static void SaveCollection(long id, int depth)
		{
			ModifyCollections.SaveCollections(id, depth);
		}
		
		public static Hashtable FetchStoredFields(string classname)
		{
			ClassDetails clsDetails = new ClassDetails(classname);
			return clsDetails.GetFields();
		}
		
        public static void SetPathForConnection(string path)
        {
            DataLayer.Connection.Config.Instance.DbPath = path;
        }
        public static void SaveAssemblyPath(string path)
        {
            if (path != string.Empty)
                DataLayer.Connection.Config.Instance.SaveAssemblySearchPath(path);
            DataLayer.Connection.Config.Instance.AssemblySearchPath = null;
        }
        

	    public static void SetAssemblyPathtoNull()
	    {
            DataLayer.Connection.Config.Instance.AssemblySearchPath = null;
	        DataLayer.Connection.Config.Instance.DbPath = string.Empty;
	    }

	    public static ISearchPath GetAssemblySearchPath()
        {
            return DataLayer.Connection.Config.Instance.AssemblySearchPath;
        }

	    public static int NoOfObjectsforAClass(string classname)
		{
			ClassDetails db = new ClassDetails(classname);
			return db.GetNumberOfObjects();
		}


        public static bool CheckForCollection(long id)
        {
            object obj = GetObjById(id);
            IReflectClass clazz = DataLayerCommon.ReflectClassFor(obj);
            IType type = Db4oClient.TypeResolver.Resolve(clazz);
            return CheckIsCollection(type);

        }
        public static bool CheckForCollection(long id, string fieldname)
        {
            string className = GetClassName(id);
            return CheckForCollection(className, fieldname);
        }
        public static bool CheckForCollection(string className, string fieldname)
        {
           IType type = new FieldDetails(className, fieldname).GetFieldType();
            return CheckIsCollection(type);
        }
        public static bool CheckIsCollection(IType type)
        {
            if (type == null)
                return false;
            if (type.IsCollection || type.IsArray)
                return true;
            return false;
        }

        
		public List<TreeGridViewRenderer> ExpandTreeGidNode(bool readOnly,object id, bool activate)
		{
            if (id == null)
                return null;

			RenderHierarcyDetails clsRenderHierarchyDetails = new RenderHierarcyDetails(readOnly);
			object currObject;
			long lngId;
			if (id is ICollection || id is DictionaryEntry  )
			{
				currObject = id;
				if (IsArray(id))
					return clsRenderHierarchyDetails.ExpandArrayNode(currObject);
			}

			lngId = Convert.ToInt64(id);
			currObject = Db4oClient.Client.Ext().GetByID(lngId);
            if (CheckForCollection(lngId) || currObject is IEnumerable)
				return clsRenderHierarchyDetails.ExpandCollectionNode(currObject as IEnumerable);
			if (IsArray(lngId))
				return clsRenderHierarchyDetails.ExpandArrayNode(currObject);
            //check this later
            if (!IsPrimitive(lngId))
                return null;
			return clsRenderHierarchyDetails.ExpandObjectNode(currObject, activate);
		}


        public bool IsPrimitive(long id)
        {
            return DataLayerCommon.IsPrimitive(id);
        }
        

       public static object GetObjById(long id)
		{
			ObjectDetails objDetails = new ObjectDetails(id);
			return objDetails.GetObjById(id); 

		}
		
		public static int GetDepth(long id)
		{
			ObjectDetails objDetails = new ObjectDetails(id);
			return objDetails.GetDepth(id);
		}

		public static bool IsArray(object id)
		{
			return DataLayerCommon.IsArray(id);
		}

		
		public static TreeGridViewRenderer GetTreeGridViewDetails(long selectedObj, string classname)
		{
			return new RenderHierarcyDetails(false).GetRootObject(selectedObj, classname);
		}
		

		public static bool BackUpDatabase(string LocationToBackUp)
		{
          
			db4oBackup backup = new db4oBackup(LocationToBackUp); 
			bool check = false;
			try
			{
				backup.db4oBackupDatabase();
			}
			catch (Exception oEx)
			{
				check = true;
				LoggingHelper.HandleException(oEx);
				MessageBox.Show(oEx.Message, "ObjectManager Enterprise",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}

			return check;            
		}

		
		


		public static string ConnectoToDB(ConnectionDetails recConnection, bool customConfig)
		{
			DBConnect db = new DBConnect();
			return db.dbConnection(recConnection.ConnParam, customConfig );                     

		}
	

		

		public static bool CreateDemoDb(string demoFilePath)
		{
			try
			{
				DemoDatabaseCreation dbCreationObj = new DemoDatabaseCreation();
				dbCreationObj.CreateDemoDb(demoFilePath);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public static void Closedb()
		{

			
			Db4oClient.CloseConnection();
		}
		
       
		public static bool CheckIfDbConnected()
		{
			return Db4oClient.IsConnected;
		}
        public static bool CheckIfClientServer()
        {
            return Db4oClient.IsClient;
        }
		public static IType ResolveType(string typeDetails)
		{
			return Db4oClient.TypeResolver.Resolve(typeDetails);
		}

		public static bool CheckReadonlyStatus()
		{
			return Db4oClient.CurrentConnParams.ConnectionReadOnly;
		}
	}
}