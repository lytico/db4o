using OManager.BusinessLayer.Common;

namespace OManager.BusinessLayer.ObjectExplorer
{
    public class QueryHelper
    {
        public static string[] ConditionsFor(string typeName)
        {
            string[] operatorList;

            switch (typeName)
            { 
                case BusinessConstants.STRING:
                    operatorList = CommonValues.StringConditions;
                    break;

                case BusinessConstants.CHAR:
                    operatorList = CommonValues.CharacterCondition;
                    break;

                case BusinessConstants.INT16:
                case BusinessConstants.DOUBLE:
                case BusinessConstants.DECIMAL:
                case BusinessConstants.INT32:
                case BusinessConstants.INT64:
                case BusinessConstants.INTPTR:
                case BusinessConstants.UINT16:
                case BusinessConstants.UINT32:
                case BusinessConstants.UINT64:
                case BusinessConstants.UINTPTR:
                case BusinessConstants.SINGLE:
                case BusinessConstants.SBYTE:
                case BusinessConstants.BYTE:
                    operatorList = CommonValues.NumericConditions;
                    break;

                case BusinessConstants.BOOLEAN:
                    operatorList = CommonValues.BooleanConditions;
                    break;

                case BusinessConstants.DATETIME:
                    operatorList = CommonValues.DateTimeConditions;
                    break;

                default:
                    operatorList = new string[0];
                    break;
            }

            return operatorList;
        }

        public static string[] GetOperators()
        {
            return CommonValues.Operators;
        }
    }
}
