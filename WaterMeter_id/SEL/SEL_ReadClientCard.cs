using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifyWaterCard.DataModels;


namespace WaterMeter_id
{

   
    public class SEL_ReadClientCard
     {
        DAL_UnitType DAL_UnitType_Obj = new DAL_UnitType();
        DAL_MeterIssues DAL_MeterIssues_Obj = new DAL_MeterIssues();
        DAL_PriceSchduler DAL_PriceSchduler_Obj = new DAL_PriceSchduler();
        DAL_Offtimes DAL_Offtimes_Obj = new DAL_Offtimes();
        DAL_Client DAL_Client_Obj = new DAL_Client();
        DAL_CardIssues DAL_CardIssues_Obj = new DAL_CardIssues();
        DAL_ClientInfo DAL_ClientInfo_obj = new DAL_ClientInfo();
        unifyWaterCard_Comm UnifyCard_Object = new unifyWaterCard_Comm();
        DAL_Readings DAL_Readings_Obj = new DAL_Readings();
        //
        public BLL_ClientInfo bll_ClientInfo = new BLL_ClientInfo();
        public BLL_Readings bll_Readings = new BLL_Readings();
        public BLL_ChargeBasicInf bll_ChargeBasicInf = new BLL_ChargeBasicInf();
        public BLL_Deductions bll_Deductions = new BLL_Deductions();
        public BLL_CreditBalance bll_creditBalance = new BLL_CreditBalance();
        public BLL_MeterState bll_MeterState = new BLL_MeterState();
        public BLL_MeterIssues BLL_MeterIssues = new BLL_MeterIssues();
        public BLL_CardIssues BLL_CardIssues = new BLL_CardIssues();
        public BLL_MeterIssues BLL_MeterInfo = new BLL_MeterIssues();
        public BLL_ReadingCardInfo BLL_ReadingCardInfoData = new BLL_ReadingCardInfo();
        public bool ReadClientCard()
       {
            bool Status = false;

            Status = UnifyCard_Object.card_ReadBasic();

            if (Status == false)
            {
                return Status;
            }
          

         


                if (UnifyCard_Object.ReadClientCard())
                {

                    int subscri = UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_SubscriberID;

                    //select information of client by subscriber 
                //    BLL_ReadingCardInfoData = DAL_Readings_Obj.ReadCardData(UnifyCard_Object.BLL_Client_Data.Client_Number, UnifyCard_Object.BLL_MeterIssues_Data.Meter_MeterNum, UnifyCard_Object.BLLCardIssues_Data.CardNum);

                BLL_ReadingCardInfoData = DAL_Readings_Obj.ReadCardData(subscri);

                //put the clientInfoID in BLL_(ChargeBasicInfo, Deduction,Reading,CreditBalance)
           

                
                
                    bll_Readings = UnifyCard_Object.BLL_Readings_Data;
                    bll_creditBalance = UnifyCard_Object.BLL_CreditBalance_Data;
                    bll_Deductions = UnifyCard_Object.BLL_Deductions_Data;
                    bll_ChargeBasicInf = UnifyCard_Object.BLL_ChargeBasicInf_Data;
                    bll_MeterState = UnifyCard_Object.BLL_MeterState_Data;
                    BLL_CardIssues = DAL_CardIssues_Obj.CardIssueDataSelection(UnifyCard_Object.BLLCardIssues_Data.CardNum);
                   
                     bll_Readings.Readings_Date = DateTime.Now;
                     bll_creditBalance.CreditBalance_Date= DateTime.Now;
                     bll_MeterState.MeterState_Date = DateTime.Now;
                     bll_Readings.Readings_ReadingBy = "System";
                     bll_creditBalance.CreditBalance_ReadingBy = "System";
                     bll_MeterState.MeterState_ReadingBy = "System";
                     bll_ChargeBasicInf.ChargeBasicInf_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;
                     bll_Deductions.Deductions_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;
                     bll_Readings.Readings_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;
                     bll_creditBalance.CreditBalance_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;
                     bll_MeterState.MeterState_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;

                if (UnifyCard_Object.BLLCardIssues_Data.LastTranscationDate > BLL_CardIssues.LastTranscationDate)
                    {
                        try
                            {
                       /*     DAL_Readings_Obj.InsertReading(bll_Readings);

                            DAL_Readings_Obj.InsertCreditBalance(bll_creditBalance);

                            DAL_Readings_Obj.InsertDeduction(bll_Deductions);

                            DAL_Readings_Obj.InsertChargeBasicInfo(bll_ChargeBasicInf);

                            DAL_Readings_Obj.InsertMeterState(bll_MeterState);

                            DAL_Readings_Obj.UpdateCard(UnifyCard_Object.BLLCardIssues_Data);*/
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }


                    }
                
                   BLL_CardIssues = UnifyCard_Object.BLLCardIssues_Data;
                    Status = true;
                  }

            else
            {
                MessageBox.Show("Error Card Data not inserted in Database before ");
                Status = false;

            }
            return Status;
        }

      public  DataTable GetChargeBasic(int ClientinfoID)
        {
            DAL_ChargeBasicInf RechargeHistoryTable = new DAL_ChargeBasicInf();
            return RechargeHistoryTable.Select(ClientinfoID);
        }


        public DataTable GetMeterStata(int ClientinfoID)
        {
            DAL_MeterState AlarmHistoryTable = new DAL_MeterState();
            return AlarmHistoryTable.Select(ClientinfoID);
        }

        
    }
}
