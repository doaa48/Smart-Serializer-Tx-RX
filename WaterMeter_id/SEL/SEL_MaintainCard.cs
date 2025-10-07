using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{    
    //config 
    public class SEL_MaintainCard
    {
        public BLL_MaintCard BLLMaintainData = new BLL_MaintCard();

        DAL_MaintCard DALMaintain_Object = new DAL_MaintCard();
        unifyWaterCard_Comm UnifyCard_Object = new unifyWaterCard_Comm();
        public int GetMinStartCustomerID()
        {
            DAL_Client OBject = new DAL_Client();
            return OBject.GetMINClientNum();
        }
        public int GetMaxEndCustomerID()
        {
            DAL_Client OBject = new DAL_Client();
            return OBject.GetMAXClientNum();
        }

        public string[] GetPriceSchdulerNames()
        {
            DAL_PriceSchduler DAL_PriceSchduler_Obj = new DAL_PriceSchduler();
            return DAL_PriceSchduler_Obj.SelectPriceScheduleNames();
        }

        public string[] GetOFFTimeNames()
        {
            DAL_Offtimes DAL_Offtimes_Obj = new DAL_Offtimes();
            return DAL_Offtimes_Obj.SelectOFFTimeNames();
        }
        public bool Maint_SetMaintCard()
        {
            bool status = false;
            //select object of priceshduler by name of priceshduler 
            //select object of offtime by it name 
            DAL_PriceSchduler DALPriceshduler_Object = new DAL_PriceSchduler();
            DAL_Offtimes DALOfftimes_Oject = new  DAL_Offtimes();


            UnifyCard_Object.BLL_PriceScheduler_Data = DALPriceshduler_Object.GetPriceSchedulerDataByName(BLLMaintainData.MaintCard_PriceSchdukerName);
            UnifyCard_Object.BLL_Offtimes_Obj = DALOfftimes_Oject.GetOFFTimeData(BLLMaintainData.MaintCard_OfftimeName);
            BLLMaintainData.MaintCard_PriceScheduleNum = UnifyCard_Object.BLL_PriceScheduler_Data.PriceSchedule_ID;
            BLLMaintainData.MaintCard_OFFTimeNum = UnifyCard_Object.BLL_Offtimes_Obj.OFFTime_ID;
            status = UnifyCard_Object. MaintCard_WriteCard(BLLMaintainData);


            return status;

        }
        public bool Main_ReadMaintCard()
        {

            bool status = false;

            return status;
        }
        
    }
}
