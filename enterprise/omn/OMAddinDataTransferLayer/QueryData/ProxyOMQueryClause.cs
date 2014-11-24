using System;
using System.Collections.Generic;
using System.Text;
using OManager.BusinessLayer.Common;
using OManager.DataLayer.CommonDatalayer;
namespace OMAddinDataTransferLayer.QueryData
{
	[Serializable ]
    public class ProxyOMQueryClause
    {

     

        public string Classname { get; set;}
       
       

        public string Operator { get; set;}
        
       

        public string Value { get; set;}
      
       

        public string Fieldname { get; set;}
       
      

        public string FieldType { get; set;}
       

       

        public CommonValues.LogicalOperators ClauseLogicalOperator { get; set;}
        

      
        

        

    }
}
