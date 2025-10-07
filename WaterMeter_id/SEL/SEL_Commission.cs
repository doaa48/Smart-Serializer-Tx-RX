using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifyWaterCard.DataModels;

namespace WaterMeter_id
{
    public class SEL_Commission
    {

        DAL_UnitType DAL_UnitType_Obj=new DAL_UnitType();
        DAL_MeterIssues DAL_MeterIssues_Obj = new DAL_MeterIssues();
        DAL_PriceSchduler DAL_PriceSchduler_Obj = new DAL_PriceSchduler();
        DAL_Offtimes DAL_Offtimes_Obj = new DAL_Offtimes();
        DAL_Client DAL_Client_Obj = new DAL_Client();
        DAL_CardIssues DAL_CardIssues_Obj = new DAL_CardIssues();
        DAL_ClientInfo DAL_ClientInfo_obj = new DAL_ClientInfo();
        DAL_Deductions DAL_Deductions_Obj=new DAL_Deductions();

        public BLL_Commission Commission_Data=new BLL_Commission();
        unifyWaterCard_Comm UnifyCard_Object = new unifyWaterCard_Comm();


        public string[] GetUnitType()
        {
            return DAL_UnitType_Obj.SelectUnitTypeNames();
        }
        public string[] GetMeterNum()
        {
            return DAL_MeterIssues_Obj.SelectMeterNumbersNotInClientInfo();
        }
        public string[] GetPriceSchdulerNames()
        {
            return DAL_PriceSchduler_Obj.SelectPriceScheduleNames();
        }
        public string[] GetPriceSchdulerNames(string unitName)
        {
            return DAL_PriceSchduler_Obj.SelectPriceScheduleNames(unitName);
        }
        public string[] GetOFFTimeNames()
        {
            return DAL_Offtimes_Obj.SelectOFFTimeNames();
        }
        

        //this function use to make commsion meter with client and create customer card 
        public bool CreateCommission()
        {
            bool Status = false;

            Status = UnifyCard_Object.card_ReadBasic();

            if(Status == false)
            {
                return Status; 
            }
            Status = UnifyCard_Object.BLLCardIssues_Data.Func != CardFunctionEnum.CustomerCard.ToByte() && UnifyCard_Object.CardIsClear(UnifyCard_Object.BLLCardIssues_Data);
            if (Status)
            {
                MessageBox.Show("Please Clear Card First");
                return Status;
            }

            //reaad card first when card_num
            //complete data from database
       
           



            UnifyCard_Object.BLL_Client_Data = DAL_Client_Obj.GetClientDataBYClientNum(Commission_Data.Client_NUM);
            UnifyCard_Object.BLL_PriceScheduler_Data= DAL_PriceSchduler_Obj.GetPriceSchedulerDataByName(Commission_Data.PriceScheduler_Name);
            UnifyCard_Object.BLL_Offtimes_Obj = DAL_Offtimes_Obj.GetOFFTimeData(Commission_Data.OFFTime_Name);
            UnifyCard_Object.BLL_MeterIssues_Data= DAL_MeterIssues_Obj.GetDataByMeterNum(Commission_Data.Meter_Num);


            //  UnifyCard_Object.BLL_Deductions_Data.Deductions_Date= 
            UnifyCard_Object.BLL_Deductions_Data.Deductions_MonthFees = Commission_Data.DeductionMonthFee;
            UnifyCard_Object.BLL_Deductions_Data.Deductions_AppDate = Commission_Data.DeductionAPPDate;
            UnifyCard_Object.BLL_Deductions_Data.Deductions_Month = Commission_Data.DeductionMonthNum;
            // get id for customer and create card




            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_ClientID        = UnifyCard_Object.BLL_Client_Data.Client_ID;
            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_PriceScheduleID = UnifyCard_Object.BLL_PriceScheduler_Data.PriceSchedule_ID;
            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_OFFTimeID       = UnifyCard_Object.BLL_Offtimes_Obj.OFFTime_ID;
            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_MeterID         = UnifyCard_Object.BLL_MeterIssues_Data.Meter_ID;
            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_UnityTypeID     = DAL_UnitType_Obj.GetUnitTypeIDByUnitTypeName(Commission_Data.UnitTypeName);
            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_CardID          = DAL_CardIssues_Obj.GetCardIDByCardNum(UnifyCard_Object.BLLCardIssues_Data.CardNum);
            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_IssueDate         = DateTime.Now;





            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_Activity    = Commission_Data.Activity;
            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_SwGServices = Commission_Data.SWGService;
            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_NumOFUnit   = Commission_Data.UnitNum;

            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_Category    = Commission_Data.Category;
            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_Address     = Commission_Data.UnitAddress;

            UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_SubscriberID = DAL_ClientInfo_obj.GetMaxSubscriberID() +1;

            bool checkBoxCustomerSetCategoryType = false;
            bool checkBoxCustomerSetPriceSched = false;
            bool checkBoxCustomerSetOffTimes = false;
            bool checkBoxCustomerCharge = false;
            bool checkBoxCustomerSetDeductions = true;
            bool checkBoxCustomerSetOverDraftCredit = false;
            bool chkSetChargeDateInfo = false;

            string meterActionString = string.Concat(new string[]
              {
                         checkBoxCustomerSetCategoryType ? "1" : "0",
                         checkBoxCustomerSetPriceSched ? "1" : "0",
                         checkBoxCustomerSetOffTimes ? "1" : "0",
                         checkBoxCustomerCharge ? "1" : "0",
                         checkBoxCustomerSetDeductions ? "1" : "0",
                         checkBoxCustomerSetOverDraftCredit ? "1" : "0",
                         chkSetChargeDateInfo ? "1" : "0",
                           "0"
                });



            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToInt32(meterActionString, 2); ;

            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeDate = DateTime.Now;
            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_EnabledValvePeriod = 0;
           
            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = 0;
            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = 0;
            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = 0;
            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_MaxOverdraftCredit = 0;

            //to setup new client data 
            Status = UnifyCard_Object.SetNewClientCard();
            if (Status == true)
            {
                //create new client card 

                /*
                DAL_UnitType DAL_UnitType_Obj = new DAL_UnitType();
                DAL_MeterIssues DAL_MeterIssues_Obj = new DAL_MeterIssues();
                DAL_PriceSchduler DAL_PriceSchduler_Obj = new DAL_PriceSchduler();
                DAL_Offtimes DAL_Offtimes_Obj = new DAL_Offtimes();
                DAL_Client DAL_Client_Obj = new DAL_Client();
                DAL_CardIssues DAL_CardIssues_Obj = new DAL_CardIssues();
                DAL_ClientInfo DAL_ClientInfo_obj = new DAL_ClientInfo();*/


                //SaveFileDialog data 
                //updatecard_issue
                //UnifyCard_Object.BLLCardIssues_Data  //update
                //update card issues 
                //insert clinet info 
                
                DAL_CardIssues_Obj.db.BeginTransaction(); //to brgin transction 
                DAL_ClientInfo_obj.Insert(UnifyCard_Object.BLL_ClientInfo_Data);
               UnifyCard_Object.BLL_ClientInfo_Data = DAL_ClientInfo_obj.GetDataBySubscriberID(UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_SubscriberID);
                if (UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_ID > 0)
                {
                    UnifyCard_Object.BLL_Deductions_Data.Deductions_ClientInfoID= UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_ID;
                    UnifyCard_Object.BLL_Deductions_Data.Deductions_Date = DateTime.Now;
                    Status = DAL_Deductions_Obj.Insert(UnifyCard_Object.BLL_Deductions_Data);
                    if (Status)
                    {
                        if (DAL_CardIssues_Obj.Update(UnifyCard_Object.BLLCardIssues_Data))
                        {
                            DAL_CardIssues_Obj.db.CommitTransaction();
                            return true;
                        }

                    }
                   
                }

                DAL_CardIssues_Obj.db.RollbackTransaction();


                

            }

          

            return Status;
        }

  
    }
}
