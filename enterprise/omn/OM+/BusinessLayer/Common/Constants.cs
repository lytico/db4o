namespace OManager.BusinessLayer.Common
{
    public class BusinessConstants
    {
        #region All Primitive Types

        
        public const string STRING = "System.String";
        public const string SINGLE = "System.Single";
        public const string DATETIME = "System.DateTime";
        public const string BYTE = "System.Byte";
        public const string CHAR = "System.Char";
        public const string BOOLEAN = "System.Boolean";
        public const string DECIMAL = "System.Decimal";
        public const string DOUBLE = "System.Double";
        public const string INT16 = "System.Int16";
        public const string INT32 = "System.Int32";
        public const string INT64 = "System.Int64";
        public const string INTPTR = "System.IntPtr";
        public const string SBYTE = "System.SByte";
        public const string UINT16 = "System.UInt16";
        public const string UINT32 = "System.UInt32";
        public const string UINT64 = "System.UInt64";
        public const string UINTPTR = "System.UIntPtr";

        #endregion

        #region Conditions for primitive type

        ////Conditions for string data type
        internal const string CONDITION_CONTAINS = "Contains";
        internal const string CONDITION_STARTSWITH = "Starts With";
        internal const string CONDITION_ENDSWITH = "Ends With";
        internal const string CONDITION_EQUALS = "Equals";
        internal const string CONDITION_NOTEQUALS = "Not Equals";

        //Conditions for integer data type
        internal const string CONDITION_GREATERTHAN = "Greater Than";
        internal const string CONDITION_GREATERTHANEQUAL = "Greater Than Equal";
        internal const string CONDITION_LESSTHAN = "Less Than";
        internal const string CONDITION_LESSTHANEQUAL = "Less Than Equal";

        //Conditions for boolean data type
        public const string CONDITION_TRUE = "True";
        public const string CONDITION_FALSE = "False";

        #endregion

        public const string OPERATOR_AND = "AND";
        public const string OPERATOR_OR = "OR";

        //Common Constant
        public const string DB4OBJECTS_GCLASS = "GenericClass";
        public const string DB4OBJECTS_REF = "db4oObjRef";
        public const string DB4OBJECTS_SYS = "System";
        public const string DB4OBJECTS_NULL = "null";
        public const string DB4OBJECTS_KEY = "_key";
        public const string DB4OBJECTS_VALUE = "_value";
        public const string DB4OBJECTS_DUMMY = "dummy";
        public const string DB4OBJECTS_VALUE1 = "value";
        public const string DB4OBJECTS_TREEGRIDVIEW = "treeGridView";
        public const string DB4OBJECTS_FIELD = "Field";
        public const string DB4OBJECTS_FIELDCOLOUMN = "fieldColumn";
        public const string DB4OBJECTS_VALUEFORGRID = "Value";
        public const string DB4OBJECTS_VALUECOLUMN = "valueColumn";
        public const string DB4OBJECTS_TYPE = "Type";
        public const string DB4OBJECTS_TYPECOLUMN = "typeColumn";
    }
}
