using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterMeter_id
{
    public class BLL_CompanyCards_ReturnRead
    {
        public BLL_MeterIssues BLL_MeterISsues_Data    { get; set; }
        public ushort           Return_MeterAction  { get; set; }



    }

    public class BLL_RetrivalCard_ReturnRead
    {
        public BLL_PriceScheduler BLL_PriceScheduler_Data { get; set; }
        public BLL_Deductions BLL_Deductions_Data { get; set; }

        public BLL_Offtimes BLL_Offtimes_Data { get; set; }

        public BLL_ClientInfo BLL_ClientInfo_Data { get; set; }

        public BLL_ChargeBasicInf BLL_ChargeBasicInf_Data { get; set; }

        public BLL_CreditBalance BLL_CreditBalance_Data { get; set; }

        public BLL_Readings BLL_Readings_Data { get; set; }

        public BLL_MeterState BLL_MeterState_Data { get; set; }
    }
}
