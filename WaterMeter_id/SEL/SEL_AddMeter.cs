using System.Data;

namespace WaterMeter_id
{
    public class SEL_AddMeter
    {
        DAL_AddMeter dAL_AddMeter = new DAL_AddMeter();
        public BLL_MeterList bLL_MeterList_data = new BLL_MeterList();

        

        public DataTable getThisAgg(int aggNum)
        {
            return dAL_AddMeter.GetThisAgg(aggNum);
        }

        public int meterID(int meterNum)
        {
            return dAL_AddMeter.meterid(meterNum);
        }

        public DataTable getMeterData(int meterNum)
        {
            int aggId = dAL_AddMeter.aggregationid(meterNum);
            
            bLL_MeterList_data.MeterList_AggregationID = aggId;
            return dAL_AddMeter.GetMeterList(aggId);
        }

        public bool addMeter()
        {
            return dAL_AddMeter.InsertMeter(bLL_MeterList_data);
        }

        public DataTable GetMeterTable(int aggId)
        {
            
            return dAL_AddMeter.SelectTheMeters(aggId);
        }
    }
}
