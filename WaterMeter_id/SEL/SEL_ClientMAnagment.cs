using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_ClientMAnagment
    {
        DAL_ClientManagment DAL_ClientManagment_Obj = new DAL_ClientManagment();

        public string SearchText = "";
        public string UnitTypeName = "";
        public DateTime DateCommsioned;


       
        //ComboBox_operations



        public DataTable GetDecommsionendTable()   //select client data that not commised to motor 
        {
            DataTable Table= DAL_ClientManagment_Obj.Search_Client(SearchText);

            Table.Columns.Remove("Client_ID");
            Table.Columns["Client_FullName"].ColumnName = "Name";
            Table.Columns["Client_Number"].ColumnName = "IDm";
            Table.Columns["Client_NationID"].ColumnName = "National ID";
            Table.Columns["Client_phone"].ColumnName = "Phone";
            Table.Columns["Client_Email"].ColumnName = "Email";
            return Table;
        }
        public DataTable GetDecommsionendTable(string Text)   //select client data that not commised to motor 
        {
          
            SearchText = Text;
            return GetDecommsionendTable();
        }



        public DataTable GetcommsionedTable()
        { 
        DataTable Table = DAL_ClientManagment_Obj.Search_ClientManagment(SearchText, UnitTypeName, DateCommsioned);
         
            
            return Table;


        }
        public DataTable GetcomiisendTable(string Search_Text,string unit_Type,DateTime Date_commsioned)
        {
            SearchText = Search_Text;
            UnitTypeName= unit_Type;
            DateCommsioned = Date_commsioned;

            return GetcommsionedTable(); 


        }
       public string[] GetDataOFUnitType()
        {
            return DAL_ClientManagment_Obj.SelectUnitTypeNames();
        }
        
        public DateTime GetMinDateIssues()
        {
            return DAL_ClientManagment_Obj.Select_MinIssueDate();
        }



    }
}
