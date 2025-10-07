using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_MeterList
    {
        public BLL_SchedulerMeter bLL_SchedulerMeter_data = new BLL_SchedulerMeter();
        DAL_SchedulerMeter dAL_SchedulerMeter_obj = new DAL_SchedulerMeter();

        public DataTable GetTable()
        {
            return dAL_SchedulerMeter_obj.Select();
        }

        public bool AddNewGetway()
        {
            return dAL_SchedulerMeter_obj.Insert(bLL_SchedulerMeter_data);
        }

        public bool UpdateGetway()
        {
            return dAL_SchedulerMeter_obj.Update(bLL_SchedulerMeter_data);
        }

        public bool DeleteGetway()
        {
            return dAL_SchedulerMeter_obj.Delete(bLL_SchedulerMeter_data);
        }

        public bool GetwayByNum(int num)
        {
            bLL_SchedulerMeter_data = dAL_SchedulerMeter_obj.getSchedulerMeterData(num);

            if (bLL_SchedulerMeter_data == null)
            {
                return false;
            }
            return true;
        }

        public DataTable getThisRow(int num)
        {
            return dAL_SchedulerMeter_obj.SelectMeterList(num);
        }
    }
}

