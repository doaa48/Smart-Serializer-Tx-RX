using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_SmartConfiguration
    {
        DAL_SmartConfiguration dAL_SmartConfiguration_obj = new DAL_SmartConfiguration();

        public BLL_SmartConfiguration bLL_SmartConfiguration = new BLL_SmartConfiguration();

        public DataTable GetConfigurations()
        {
            DataTable configTable = dAL_SmartConfiguration_obj.Select();

            if (configTable != null)
            {
                return configTable;
            }

            else
            {
                if (dAL_SmartConfiguration_obj.Insert())
                {
                    configTable = dAL_SmartConfiguration_obj.Select();
                }
            }
            return configTable;
        }
        public bool UpdateData()
        {
            return dAL_SmartConfiguration_obj.Update(bLL_SmartConfiguration); //blll
        }

        public BLL_SmartConfiguration GetSmartConfiguration()
        {
            return dAL_SmartConfiguration_obj.SelectData();
        }
    }
}
