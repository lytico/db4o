using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Db4objects.Db4o;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using OManager.Business.Config;
using OManager.BusinessLayer.Common;
using OManager.BusinessLayer.QueryRenderer;
using OManager.DataLayer.CommonDatalayer;
using OManager.DataLayer.Connection;
using OManager.DataLayer.PropertyDetails;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;
using Sharpen.Lang;

namespace OManager.DataLayer.QueryParser
{

	public class RenderHierarcyDetails
	{
	   
		private IObjectContainer container;
		private bool readOnly;
		private List<TreeGridViewRenderer> listTreeGridViewRenderers;
		public RenderHierarcyDetails(bool readOnly)
		{
			container = Db4oClient.Client;
			this.readOnly = readOnly;
			listTreeGridViewRenderers = new List<TreeGridViewRenderer>();
		}
        public TreeGridViewRenderer GetRootObject(long id, string classname)
        {
            return FillValuesInTreeGridViewRenderer(id, classname);
        }

	    public TreeGridViewRenderer FillValuesInTreeGridViewRenderer(long id, string classname)
		{
			TreeGridViewRenderer treeGridViewRenderer = new TreeGridViewRenderer();
            try
            {

                object currObj = Db4oClient.Client.Ext().GetByID(id);
                Db4oClient.Client.Ext().Activate(currObj, 1);
                IReflectClass rclass = DataLayerCommon.ReflectClassForName(classname);
                IType type = ResolveType(rclass);
                treeGridViewRenderer.QualifiedName = rclass.GetName();
                treeGridViewRenderer.DisplayFieldName = AppendIDTo(type.FullName, id, type);
                treeGridViewRenderer.FieldName = type.FullName;
                treeGridViewRenderer.FieldValue = type.DisplayName;
                treeGridViewRenderer.FieldType = SetFieldType(type);
                treeGridViewRenderer.ReadOnlyStatus = true;
                treeGridViewRenderer.ObjectId = id;
                treeGridViewRenderer.ObjectType = type;
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
            }
			
			return treeGridViewRenderer;
	    }

		private  string SetFieldType(IType type)
		{
			if (type.IsNullable)
			{
				GenericTypeReference typeRef = (GenericTypeReference)TypeReference.FromString(type.FullName);
				TypeReference wrappedType = typeRef.GenericArguments[0];
				return wrappedType.SimpleName;
			}
			return type.DisplayName;
		}

		private static string AppendIDTo(string prefix, long id, IType type)
		{
			string result = prefix;
			if (type.HasIdentity)
			{
				result = result + " (Object ID : " + id + " )";
			}

			return result;
		}


        public List<TreeGridViewRenderer> TraverseObjTree(long id, string classname)
        {
            container = Db4oClient.Client;
            try
            {
                object currObj = Db4oClient.Client.Ext().GetByID(id);
                if (currObj != null)
                    container.Ext().Activate(currObj, 2);
                IReflectClass refClass =
                    DataLayerCommon.ReflectClassForName(DataLayerCommon.RemoveGFromClassName(classname));

                if (refClass != null)
                {

                    IReflectField[] fieldArr = DataLayerCommon.GetDeclaredFieldsInHeirarchy(refClass);
                    TraverseFields(id, fieldArr);
                    return listTreeGridViewRenderers;
                }
            }

            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
           

            return listTreeGridViewRenderers;
        }
        public List<TreeGridViewRenderer> PopulatePrimitiveValues(object currObj, string fieldType)
        {

            if (!String.IsNullOrEmpty(fieldType))
            {
                try
                {

                    TreeGridViewRenderer TreeviewRenderer = new TreeGridViewRenderer();
                    TreeviewRenderer.DisplayFieldName = fieldType;
                    TreeviewRenderer.FieldName = TreeviewRenderer.DisplayFieldName;
                    TreeviewRenderer.FieldValue = currObj.ToString();
                    TreeviewRenderer.QualifiedName =
                        DataLayerCommon.RemoveGFromClassName(container.Ext().Reflector().ForObject(currObj).GetName());
                    TreeviewRenderer.FieldType = fieldType;
                    TreeviewRenderer.ReadOnlyStatus = readOnly;
                    TreeviewRenderer.ObjectId = 0;
                    TreeviewRenderer.HasSubNode = false;
                    listTreeGridViewRenderers.Add(TreeviewRenderer);
                }
                catch (Exception oEx)
                {
                    LoggingHelper.HandleException(oEx);
                    return null;
                }
            }

            return listTreeGridViewRenderers;
        }

        public List<TreeGridViewRenderer> PopulateDictionaryEntry(object currObj)
        {
            object kvpKey = null, kvpValue = null;
            try
            {
                if (currObj != null)
                {
                    Type valueType = currObj.GetType();
                    if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() != null &&
                        valueType.GetGenericTypeDefinition() == typeof (KeyValuePair<,>))
                    {
                        kvpKey = valueType.GetProperty("Key").GetValue(currObj, null);
                        kvpValue = valueType.GetProperty("Value").GetValue(currObj, null);
                    }

                    else
                    {
                        if (currObj is DictionaryEntry)
                        {
                            DictionaryEntry dict = (DictionaryEntry) currObj;
                            kvpKey = dict.Key;
                            kvpValue = dict.Value;
                        }
                    }
                }

                TreeGridViewRenderer treeviewRenderer = PopulateDictItems(kvpKey, "Key", true);
                listTreeGridViewRenderers.Add(treeviewRenderer);
                TreeGridViewRenderer treeviewRenderer1 = PopulateDictItems(kvpValue, "Value", readOnly);
                listTreeGridViewRenderers.Add(treeviewRenderer1);
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
            return listTreeGridViewRenderers;
        }

        private TreeGridViewRenderer PopulateDictItems(object displayData, string displayName, bool readonlystatus)
        {
            TreeGridViewRenderer treeviewRenderer = new TreeGridViewRenderer();
            treeviewRenderer.DisplayFieldName = displayName;
            treeviewRenderer.FieldValue = displayData.ToString();
            treeviewRenderer.FieldType = "System.Object";
            treeviewRenderer.ReadOnlyStatus = readonlystatus;
            treeviewRenderer.HasSubNode = displayData is GenericObject;
            if (treeviewRenderer.HasSubNode)
            {
                long id = GetLocalID(displayData);
                if (displayData is GenericObject)
                    treeviewRenderer.ObjectId = id;
                else if (id != 0)
                    treeviewRenderer.ObjectId = id;
                else
                    treeviewRenderer.SubObject = displayData;
            }
            return treeviewRenderer;
        }

        private void TraverseFields(long id, IEnumerable<IReflectField> fields)
        {
            if (fields == null)
                return;
            foreach (IReflectField field in fields)
            {
                if (field.GetName() != "com.db4o.config.TNull" && !field.IsStatic()  )
                {
                    IType fieldType = ResolveFieldType(field);
                    if (fieldType == null)
                        continue;
                    if (fieldType.IsEditable )
                        CreatePrimitiveNode(field, id, fieldType);
                    else if (fieldType.IsCollection)
                        RenderCollection(id, field);
                    else if (fieldType.IsArray)
                        RenderArray(id, field);
                    else
                        RenderSubObject(id, field);
                }
            }

        }


	    private void RenderArray(long id, IReflectField field)
		{
			
            object currObj = Db4oClient.Client.Ext().GetByID(id);
		    Db4oClient.Client.Ext().Activate(currObj, 2);
			object obj = field.Get(currObj);
			if (obj != null)
			{
				container.Ext().Activate(obj, 2);
				int length = container.Ext().Reflector().Array().GetLength(obj);
				CreateCollectionNode(field, obj, length);
			}
			else
				CreateCollectionNode(field, obj, 0);
		}

        private void RenderCollection(long id, IReflectField field)
        {
            container = Db4oClient.Client;
            object currObj = container.Ext().GetByID(id);
            object value = field.Get(currObj);
            if (value != null)
            {
                container.Ext().Activate(value, 2);
                ICollection coll = (ICollection) value;
                CreateCollectionNode(field, value, coll.Count);
            }
            else
                CreateCollectionNode(field, value, 0);

        }

	    private void RenderSubObject(long id, IReflectField field)
		{
			try
			{
				object currObj = Db4oClient.Client.Ext().GetByID(id);
				object value = field.Get(currObj);
                 IType type = ResolveFieldType(field);
				if (value != null)
				{
					container.Ext().Activate(value, 1);
                    if(value.GetType().IsArray)
                    {
                        RenderArray(id, field);
                        return;
                    }
				}
                if (GetLocalID(value) == 0)
                {
                    CreateSimplePrimitiveNode(field, value, type);
                }
                else
                {


                   
                    TreeGridViewRenderer treeGridViewRenderer = new TreeGridViewRenderer();
                    treeGridViewRenderer.DisplayFieldName = AppendIDTo(field.GetName(), GetLocalID(value), type);
                    treeGridViewRenderer.FieldName = field.GetName();
                    treeGridViewRenderer.FieldValue = value != null ? type.FullName : BusinessConstants.DB4OBJECTS_NULL;
                    treeGridViewRenderer.QualifiedName = field.GetFieldType().GetName();
                    treeGridViewRenderer.FieldType = SetFieldType(type);
                    treeGridViewRenderer.ReadOnlyStatus = readOnly;
                    treeGridViewRenderer.ObjectId = GetLocalID(value);
                    treeGridViewRenderer.ObjectType = type;
                    treeGridViewRenderer.HasSubNode = type.IsCollection || type.IsArray;
                    if (treeGridViewRenderer.HasSubNode)
                    {
                        long longid = GetLocalID(value);
                        if (value is GenericObject)
                            treeGridViewRenderer.ObjectId = longid;

                        else if (longid != 0)
                            treeGridViewRenderer.ObjectId = longid;
                        else
                        {
                            treeGridViewRenderer.SubObject = value;
                        }
                    }
                    listTreeGridViewRenderers.Add(treeGridViewRenderer);
                    if (currObj is DictionaryEntry && field.GetName() == BusinessConstants.DB4OBJECTS_KEY)
                        treeGridViewRenderer.ReadOnlyStatus = true;
                    else if (currObj is DictionaryEntry && field.GetName() == BusinessConstants.DB4OBJECTS_VALUE)
                        treeGridViewRenderer.ReadOnlyStatus = readOnly;
                    else if (field.Get(currObj) == null)
                        treeGridViewRenderer.ReadOnlyStatus = true;
                    else
                        treeGridViewRenderer.ReadOnlyStatus = true;
                }

			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}



        private static string ClassNameFor(string type)
        {
            try
            {
                if (type != "null")
                {
                    return DataLayerCommon.RemoveGFromClassName(type);
                }
                return null;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }

	    public List<TreeGridViewRenderer > CreateCollectionNode(IReflectField field, object ownerObject, int count)
		{
			try
			{
				TreeGridViewRenderer TreeviewRenderer = new TreeGridViewRenderer();
				TreeviewRenderer.DisplayFieldName = field.GetName() + " ( " + count + " items )";
			    TreeviewRenderer.FieldName = field.GetName();
                TreeviewRenderer.FieldType = field.GetName();
			    TreeviewRenderer.QualifiedName = field.GetFieldType().GetName();  
				TreeviewRenderer.ObjectId = 0;
				if (ownerObject != null)
				TreeviewRenderer.FieldValue = ownerObject.ToString();
				else
					TreeviewRenderer.FieldValue = BusinessConstants.DB4OBJECTS_NULL;
				if (count == 0)
					TreeviewRenderer.HasSubNode  = false;
				else
				{
					TreeviewRenderer.HasSubNode  = true;
					long id = GetLocalID(ownerObject);
					if (ownerObject is GenericObject || ownerObject is GenericArray )
						TreeviewRenderer.ObjectId = id;
					else
						if (id != 0)
							TreeviewRenderer.ObjectId = id;
						else
							TreeviewRenderer.SubObject = ownerObject;
				}
				IType type = ResolveFieldType(field);
				TreeviewRenderer.FieldType = SetFieldType(type );
				TreeviewRenderer.ObjectType = type;
				TreeviewRenderer.ReadOnlyStatus = true ;
				listTreeGridViewRenderers.Add(TreeviewRenderer);
				return listTreeGridViewRenderers;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				return null;
			}
		}

		public List<TreeGridViewRenderer> ExpandCollectionNode(IEnumerable collection)
		{
			try
			{
				if (collection != null)
					Db4oClient.Client.Activate(collection, 2);
				foreach (object item in collection)
					PopulateTreeGrid(item);
				return listTreeGridViewRenderers;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				return null;
			}
		}

	
		private void ExpandGenericArrayNode(GenericArray collection)
		{
			container = Db4oClient.Client;
			IEnumerator enumerable = collection.Iterator();
			while (enumerable.MoveNext())
			{
				object item = enumerable.Current;
				PopulateTreeGrid(item);
			}
		}

		private void PopulateTreeGrid(object item)
		{
			if (item == null)
				return;

			IType itemType = ResolveType(DataLayerCommon.ReflectClassFor(item));
			if (itemType.IsPrimitive)
				PopulatePrimitiveValues(item, itemType.DisplayName);
			else
			{
				container.Ext().Activate(item, 1);
				Type type = item .GetType();
				
                if (item is DictionaryEntry || (type.IsGenericType && type.GetGenericTypeDefinition() != null && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)))
					PopulateDictionaryEntry(item);
				else
				{
                    TreeGridViewRenderer treeviewRenderer = PopulateTreeGridViewRenderer(item, itemType);
                    listTreeGridViewRenderers.Add(treeviewRenderer);
					
				}
				
			}
		}

        private TreeGridViewRenderer PopulateTreeGridViewRenderer(object item, IType itemType)
        {
            TreeGridViewRenderer treeviewRenderer = new TreeGridViewRenderer();
            treeviewRenderer.DisplayFieldName = AppendIDTo(itemType.DisplayName, GetLocalID(item), itemType);
            treeviewRenderer.FieldName = itemType.FullName;
            treeviewRenderer.QualifiedName = DataLayerCommon.RemoveGFromClassName(container.Ext().Reflector().ForObject(item).GetName());
            treeviewRenderer.FieldValue = ClassNameFor(itemType.FullName);
            treeviewRenderer.FieldType = SetFieldType(itemType);
            treeviewRenderer.ReadOnlyStatus = true;
            treeviewRenderer.ObjectId = GetLocalID(item);
            treeviewRenderer.ObjectType = itemType;
            treeviewRenderer.HasSubNode = itemType.IsCollection || itemType.IsArray;
            if (treeviewRenderer.HasSubNode)
            {
                long longid = GetLocalID(itemType);
                if (itemType is GenericObject)
                    treeviewRenderer.ObjectId = longid;

                else if (longid != 0)
                    treeviewRenderer.ObjectId = longid;
                else
                {
                    treeviewRenderer.SubObject = item;
                }
            }
            return treeviewRenderer;
        }
		public List< TreeGridViewRenderer > ExpandObjectNode(object obj, bool activate)
		{
			try
			{
				//object obj = Db4oClient.Client.Ext().GetByID(id);
				if (obj == null)
					return null;

				IReflectClass rclass = DataLayerCommon.ReflectClassFor(obj);
				if (rclass == null)
					return null;
				string classname = rclass.GetName();
				if (classname != string.Empty)
					TraverseObjTree(GetLocalID(obj ), classname);
				return listTreeGridViewRenderers;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				return null;
			}
		}

		public List<TreeGridViewRenderer> ExpandArrayNode(object currObj)
		{
			try
			{
				
				if (currObj is GenericArray)
					ExpandGenericArrayNode((GenericArray)currObj);
				else
					ExpandCollectionNode((ICollection)currObj);
				return listTreeGridViewRenderers;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				return null;
			}
		}

		public void CreatePrimitiveNode(IReflectField  field, long id,  IType type)
		{
			try
			{
				object currObj = Db4oClient.Client.Ext().GetByID(id);
				object value = field.Get(currObj);
				var treeGridViewRenderer = FillValuesInTreeGridViewRenderer(field, type, value);
			    listTreeGridViewRenderers.Add(treeGridViewRenderer);
				
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}
	

	    public void CreatePrimitiveNode(IReflectField field, object currObj, IType type)
		{
			try
			{
				object value = field.Get(currObj);
                var treeGridViewRenderer = FillValuesInTreeGridViewRenderer(field, type, value);
				if (currObj is IDictionary)
				{
					treeGridViewRenderer.ReadOnlyStatus = field.GetName() != BusinessConstants.DB4OBJECTS_VALUE1;
					treeGridViewRenderer.HasSubNode = true;
					treeGridViewRenderer.SubObject = currObj;  
				}

                listTreeGridViewRenderers.Add(treeGridViewRenderer);
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}
        public void CreateSimplePrimitiveNode(IReflectField field, object value, IType type)
        {
            try
            {
                var treeGridViewRenderer = FillValuesInTreeGridViewRenderer(field, type, value);
                treeGridViewRenderer.ReadOnlyStatus = !type.IsPrimitive || readOnly;
                listTreeGridViewRenderers.Add(treeGridViewRenderer);
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
        }
        private TreeGridViewRenderer FillValuesInTreeGridViewRenderer(IReflectField field, IType type, object value)
        {
            TreeGridViewRenderer treeGridViewRenderer = new TreeGridViewRenderer();
            treeGridViewRenderer.DisplayFieldName = field.GetName();
            treeGridViewRenderer.FieldName = field.GetName();
            treeGridViewRenderer.FieldValue = value != null ? value.ToString() : BusinessConstants.DB4OBJECTS_NULL;
            treeGridViewRenderer.FieldType = SetFieldType(type);
            treeGridViewRenderer.ReadOnlyStatus = readOnly;
            treeGridViewRenderer.ObjectType = type;
            treeGridViewRenderer.ObjectId = 0;
            return treeGridViewRenderer;
        }
		/**
		 * Each node in "details view" holds a reference for its corresponding
		 * object in the object model. While this works fine with reference
		 * types, it fails miserably with nested value types. 
		 * 
		 * For more details see OMNUnitTest.RenderHierarchyTestCase.
		 */


        //public static bool TryUpdateValueType(TreeGridNode node, object newValue)
        //{
        //    if (node == null || node.Parent == null)
        //        return false;

        //    ValueTypeChange change = ValueTypeChangeFor(node.Parent, 0);
        //    if (change == null)
        //        return false;

        //    FieldInfo fieldInfo = FieldInfoFor(node);
        //    if (fieldInfo == null)
        //        return false;

        //    fieldInfo.SetValueDirect(TypedReference.MakeTypedReference(change.TargetObject, change.FieldPath.ToArray()), newValue);
        //    return true;
        //}

        //private static ValueTypeChange ValueTypeChangeFor(TreeGridNode node, int depth)
        //{
        //    IType omnType = FieldTypeFor(node);
        //    if (omnType.IsCollection || omnType.IsArray)
        //        return null;

        //    Type type = Type.GetType(omnType.FullName);
        //    if (type == null)
        //        return null;

        //    if (type.IsValueType)
        //    {
        //        ValueTypeChange change = ValueTypeChangeFor(node.Parent, depth + 1);
        //        if (change != null)
        //            change.FieldPath.Add(FieldInfoFor(node));

        //        return change;
        //    }

        //    return depth == 0 ? null : new ValueTypeChange(node.Tag);
        //}


        //private static FieldInfo FieldInfoFor(TreeGridNode node)
        //{
        //    Type parentType = Type.GetType(FieldTypeFor(node.Parent).FullName);
        //    return (parentType != null)
        //            ? parentType.GetField(FieldNameFor(node), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
        //            : null;
        //}

        //private static string FieldNameFor(TreeGridNode node)
        //{
        //    return (string)node.Cells[0].Value;
        //}

        //private static IType FieldTypeFor(TreeGridNode node)
        //{
        //    long id = (long)node.Tag;
        //    object obj = Db4oClient.Client.Ext().GetByID(id);
        //    IType type  = ResolveType(DataLayerCommon.ReflectClassFor(obj));
        //    return type;
        //}
		
        public long GetLocalID(object obj)
		{
			ObjectDetails objDetails = new ObjectDetails(obj);
			return objDetails.GetLocalID();
		}

		public IType ResolveFieldType(IReflectField field)
		{
			IReflectClass type = field.GetFieldType();
			return type != null ? ResolveType(type) : null;
		}

		private static IType ResolveType(IReflectClass klass)
		{
			return Db4oClient.TypeResolver.Resolve(klass);
		}

        internal class ValueTypeChange
        {
            public ValueTypeChange(object targetObject)
            {
                TargetObject = targetObject;
                FieldPath = new List<FieldInfo>();
            }

            public object TargetObject;
            public List<FieldInfo> FieldPath;
        }

	}
}
