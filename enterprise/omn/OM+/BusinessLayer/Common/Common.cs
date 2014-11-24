using Sharpen.Lang;

namespace OManager.BusinessLayer.Common
{
    public static class CommonValues
    {
       
        public enum LogicalOperators
        {   
            EMPTY=1,
            AND ,
            OR                   
        }

        public static string[] StringConditions = new string[]{
                                                BusinessConstants.CONDITION_CONTAINS,
                                                BusinessConstants.CONDITION_STARTSWITH,
                                                BusinessConstants.CONDITION_ENDSWITH,
                                                BusinessConstants.CONDITION_EQUALS,
                                                BusinessConstants.CONDITION_NOTEQUALS
                                                };

        public static string[] NumericConditions = new string[] {
                                                BusinessConstants.CONDITION_GREATERTHAN,
                                                BusinessConstants.CONDITION_GREATERTHANEQUAL,
                                                BusinessConstants.CONDITION_LESSTHAN,
                                                BusinessConstants.CONDITION_LESSTHANEQUAL,
                                                BusinessConstants.CONDITION_EQUALS,
                                                BusinessConstants.CONDITION_NOTEQUALS
                                                };       

        public static string[] BooleanConditions = new string[] {
                                                BusinessConstants.CONDITION_EQUALS,
                                                BusinessConstants.CONDITION_NOTEQUALS
                                                };

        public static string[] DateTimeConditions = new string[] {
                                                BusinessConstants.CONDITION_GREATERTHAN,
                                                BusinessConstants.CONDITION_LESSTHAN,
                                                BusinessConstants.CONDITION_EQUALS  
                                                };
        public static string[] CharacterCondition = new string[] {
                                                BusinessConstants.CONDITION_EQUALS,
                                                BusinessConstants.CONDITION_NOTEQUALS
                                                };


        public static string[] Operators = new string[] {
                                                BusinessConstants.OPERATOR_AND,
                                                BusinessConstants.OPERATOR_OR
                                                };
        public static char[] charArray = new char[] { 'G', 'e', 'n', 'e', 'r', 'i', 'c', 'C', 'l', 'a', 's', 's', ' ' };

    
    	public static string DecorateNullableName(string nullableTypeName)
    	{
			GenericTypeReference typeRef = (GenericTypeReference) TypeReference.FromString(nullableTypeName);
    		TypeReference wrappedType = typeRef.GenericArguments[0];
    		
			return "Nullable<" + wrappedType.SimpleName + ">";
    	}

    	public static string UndecorateFieldName(string fieldName)
    	{
    		int index = fieldName.LastIndexOf('(');
    		return index >= 0 ? fieldName.Remove(index - 1, fieldName.Length - index + 1) : fieldName;
    	}
        public static string GetSimpleNameForNullable(string nullableTypeName)
        {
            GenericTypeReference typeRef = (GenericTypeReference)TypeReference.FromString(nullableTypeName);
            TypeReference wrappedType = typeRef.GenericArguments[0];

            return wrappedType.SimpleName;
        }
    }
}
