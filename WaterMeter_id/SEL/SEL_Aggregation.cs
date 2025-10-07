using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_Aggregation
    {
        public BLL_Aggregation aggregation_data = new BLL_Aggregation();
        DAL_Aggregation DAL_Aggregation_obj = new DAL_Aggregation();

        public DataTable GetTable()
        {
            return DAL_Aggregation_obj.Select();
        }

        public bool AddNewAggregation()
        {
            return DAL_Aggregation_obj.Insert(aggregation_data);
        }

        public bool UpdateAggregation()
        {
            return DAL_Aggregation_obj.Update(aggregation_data);
        }

        public int[] GetAllAggregationNumbersFromGetwayData(int GetWayNum)
        {
        return DAL_Aggregation_obj.getAllAggregationNumbersFromGetwayData(GetWayNum);

        }
        public bool DeleteAggregation()
        {
            return DAL_Aggregation_obj.Delete(aggregation_data);
        }

        public DataTable GetGetwayID()
        {
            return DAL_Aggregation_obj.GetGetwayID();
        }

        public bool GetwayByNum(int num)
        {
            aggregation_data = DAL_Aggregation_obj.getGetwayData(num);

            if (aggregation_data == null)
            {
                return false;
            }
            return true;
        }
    }
}
