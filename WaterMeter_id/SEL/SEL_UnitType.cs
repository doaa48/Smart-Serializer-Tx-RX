using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
  
    public class SEL_UnitType
    {
        DAL_UnitType UnitType_Obj = new DAL_UnitType();
        BLL_UnitType UnitType_Data = new BLL_UnitType();

        public bool AddNew()
        {
            bool Status = false;
            Status = UnitType_Obj.Insert(UnitType_Data);
            return Status;
        }
        public bool Update()
        {
            bool status = UnitType_Obj.Update(UnitType_Data);
            return status;

        }
        public DataTable GetTable()
        {
            return UnitType_Obj.Select();
        }
    }
}
