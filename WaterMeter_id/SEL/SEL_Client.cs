using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{

    public class SEL_Client
    {
        DAL_Client DAL_Client_Obj = new DAL_Client();
       public BLL_Client Cleint_Data = new BLL_Client();
        
        public DataTable GetTable()
        {
            return DAL_Client_Obj.Select();
        }
        public int GetNewClientNum()
        {
            return (DAL_Client_Obj.GetMaxClientId()+1);
        }
        public  bool AddNewClient()
        {
            return DAL_Client_Obj.Insert(Cleint_Data);
        }
        public bool UpdateClient()
        {
            return DAL_Client_Obj.Update(Cleint_Data);
        }

        public bool DeleteClient()
        {
            return DAL_Client_Obj.Delete(Cleint_Data);
        }
      public bool GetClientData(string name)
        {
            bool Status = false;
            DataTable table = DAL_Client_Obj.Search(name);

            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];

                Cleint_Data.Client_ID = Convert.ToInt32(row["Client_ID"]);
                Cleint_Data.Client_FullName= row["Client_FullName"].ToString();
                Cleint_Data.Client_Number = Convert.ToInt32(row["Client_Number"]);
                Cleint_Data.Client_NationID= row["Client_NationID"].ToString();
                Cleint_Data.Client_phone= row["Client_phone"].ToString();
                Cleint_Data.Client_Email= row["Client_Email"].ToString();

              
                Status = true;
            }
                return Status;
        }
    }
}
