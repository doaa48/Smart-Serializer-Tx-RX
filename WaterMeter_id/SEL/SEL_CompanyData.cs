using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_CompanyData
    {
        DAL_CompanyData CompanyData_Obj = new DAL_CompanyData();

        public DataTable GetCompanyData()
        {
            return CompanyData_Obj.Select();
        }
        public bool Update_CompanyData(BLL_CompanyData dataSt)
        {
            return CompanyData_Obj.Update(dataSt);
        }
        public bool Add_CompanyData(BLL_CompanyData dataSt)
        {
            return CompanyData_Obj.Insert(dataSt);
        }
    }
}
