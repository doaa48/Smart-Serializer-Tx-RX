using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterMeter_id.DAL;
using WaterMeter_id.BLL;

namespace WaterMeter_id
{

    public class SEL_Operator
    {
        public BLL_Operator operator_Data = new BLL_Operator();

        DAL_Operator DAL_Operator_Obj = new DAL_Operator();
        DAL_OperatorLog DAL_OperatorLog_Obj = new DAL_OperatorLog();
        public bool checkUsernameAndPassWord()
        {
            return DAL_Operator_Obj.checkPassWord(ref operator_Data);
        }
        public DataTable GetTable()
        {
            return DAL_Operator_Obj.Select();
        }
        public DataTable GetAllOperatorLogTable()
        {
            return DAL_OperatorLog_Obj.OperatorLogSelect();
        }
        public DataTable GetSpecificOperatorLogTable(string StartDateText, string EndDateText, string SelectOperatorNameComboBox, string SelectOperationComboBox)
        {
            return DAL_OperatorLog_Obj.SpecificSelect( StartDateText,  EndDateText,  SelectOperatorNameComboBox,  SelectOperationComboBox);
        }
       

        public void AddnewLogHistoryForOperator(int OperatorID, string OperatorAction, string OperatorAction_Disc, int OperatorTableNameID, string OperatorTableName)
        {
            BLL_OperatorLog OperatorLogObj = new BLL_OperatorLog();
            OperatorLogObj.OperatorLog_OperatorID = OperatorID;
            OperatorLogObj.OperatorLog_Date = DateTime.Now;
            OperatorLogObj.OperatorLog_Action = OperatorAction;
            OperatorLogObj.OperatorLog_Action_Disc = OperatorAction_Disc;
            OperatorLogObj.OperatorLog_TableName = OperatorTableName;
            OperatorLogObj.OperatorLog_TableNameID = OperatorTableNameID;

            DAL_OperatorLog_Obj.Insert(OperatorLogObj);

        }
        public bool AddNewOperator()
        {
            return DAL_Operator_Obj.Insert(operator_Data);
        }

        public bool UpdateOperator() 
        {
            return DAL_Operator_Obj.Update(operator_Data);
        }

        public bool DeleteOperator()
        {
            return DAL_Operator_Obj.Delete(operator_Data);
        }

        public bool GetSelectedOperator(string name)
        {
            bool Status = false;
            DataTable table = DAL_Operator_Obj.Search(name);

            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                operator_Data.id = Convert.ToInt32(row["Operator_id"]);
                operator_Data.Name = row["Operator_Name"].ToString();
                operator_Data.Position = row["Operator_Position"].ToString();
                operator_Data.Passwoed = row["Operator_Passwoed"].ToString();
                operator_Data.UaserName = row["OPerator_UaserName"].ToString();
                operator_Data.Privilage = Convert.ToInt32(row["Operator_Privilage"]);
                operator_Data.State = row["Operator_State"].ToString();
                operator_Data.Date = Convert.ToDateTime(row["Operator_Date"]);

                Status = true;  

            }

            return Status;
        }
    }
}
