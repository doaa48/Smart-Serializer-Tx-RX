using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifyWaterCard.DataModels;

namespace WaterMeter_id
{


    public class SEL_Recharging
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

        public BLL_Configurations BLL_Configurations_Data = new BLL_Configurations();
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

        public List<string> ALarm_Names = new List<string>(); // Change to List<string> for dynamic size allocation
        public List<string> MeterError = new List<string>(); // Change to List<string> for dynamic size allocation
        public ushort ALarm_Cost = 0;



        private void SetMeterSataMalfun(int count, int day, int month, BLL_Configurations CostConf)
        {
            string data = "";


            if (count == 1)
            {
                data = "D:" + day.ToString() + ",M:" + month.ToString() + ",E:" + "Attempting to remove the meter cover.";
                ALarm_Names.Add(data); // Changed Append to Add
                ALarm_Cost += (ushort)CostConf.Confuguration_TryToRemoveTheMeterCover;
            }
            else if (count == 2)
            {
                data = "D:" + day.ToString() + ",M:" + month.ToString() + ",E:" + "Placing the meter in a magnetic field.";
                ALarm_Names.Add(data);
                ALarm_Cost += (ushort)CostConf.Confuguration_PlaceTheMeterInMagneticField;
            }
            else if (count == 3)
            {
                data = "D:" + day.ToString() + ",M:" + month.ToString() + ",E:" + "Removing the battery.";
                ALarm_Names.Add(data);
                ALarm_Cost += (ushort)CostConf.Confuguration_RemoveTheBattery;
            }
            else if (count == 4)
            {
                data = "D:" + day.ToString() + ",M:" + month.ToString() + ",E:" + "Disconnection between the mechanical meter and the communication unit";
                ALarm_Names.Add(data);
                ALarm_Cost += (ushort)CostConf.Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit;
            }
            else if (count == 5)
            {
                data = "D:" + day.ToString() + ",M:" + month.ToString() + ",E:" + "Disruption of communication between the control unit and the motor.";
                ALarm_Names.Add(data);
                ALarm_Cost += (ushort)CostConf.Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening;
            }
            else
            {
                data = "  ";
                ALarm_Names.Add(data);
            }
        }

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
                bll_creditBalance.CreditBalance_Date = DateTime.Now;
                bll_MeterState.MeterState_Date = DateTime.Now;
                bll_Readings.Readings_ReadingBy = "System";
                bll_creditBalance.CreditBalance_ReadingBy = "System";
                bll_MeterState.MeterState_ReadingBy = "System";
                bll_ChargeBasicInf.ChargeBasicInf_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;
                bll_Deductions.Deductions_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;
                bll_Readings.Readings_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;
                bll_creditBalance.CreditBalance_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;
                bll_MeterState.MeterState_ClientInfoID = BLL_ReadingCardInfoData.ClientInfo_ID;





                DAL_Configurations Object = new DAL_Configurations();
                BLL_Configurations_Data = Object.Select();
                SetMeterSataMalfun(bll_MeterState.MeterState_Malfun1_Count, bll_MeterState.MeterState_Malfun1_Day, bll_MeterState.MeterState_Malfun1_Month, BLL_Configurations_Data);
                SetMeterSataMalfun(bll_MeterState.MeterState_Malfun2_Count, bll_MeterState.MeterState_Malfun2_Day, bll_MeterState.MeterState_Malfun2_Month, BLL_Configurations_Data);
                SetMeterSataMalfun(bll_MeterState.MeterState_Malfun3_Count, bll_MeterState.MeterState_Malfun3_Day, bll_MeterState.MeterState_Malfun3_Month, BLL_Configurations_Data);
                SetMeterSataMalfun(bll_MeterState.MeterState_Malfun4_Count, bll_MeterState.MeterState_Malfun4_Day, bll_MeterState.MeterState_Malfun4_Month, BLL_Configurations_Data);
                SetMeterSataMalfun(bll_MeterState.MeterState_Malfun5_Count, bll_MeterState.MeterState_Malfun5_Day, bll_MeterState.MeterState_Malfun5_Month, BLL_Configurations_Data);


                string text7 = Convert.ToString(bll_MeterState.MeterState_MeterErrors, 2).PadLeft(8, '0');
                bool flag43 = text7[0] == '1';
                if (flag43)
                {
                    MeterError.Add("Good Battery");
                }
                else
                {
                    MeterError.Add("Battery Low");
                }
                bool flag44 = text7[1] == '1';
                if (flag44)
                {
                    MeterError.Add("Opened Valve");
                }
                else
                {
                    MeterError.Add("Closed Valve");
                }
                bool flag45 = text7[2] == '1';
                if (flag45)
                {
                    MeterError.Add("Valve Closing Error");
                }
                else
                {
                    MeterError.Add("Valve Closing Succeeded");
                }
                bool flag46 = text7[3] == '1';
                if (flag46)
                {
                    MeterError.Add("Valve Opening Error");
                }
                else
                {
                    MeterError.Add("Valve Opening Succeeded");
                }


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


        public bool chargeMeter()
        {
            DAL_UnitType DAL_UnitType_Obj = new DAL_UnitType();
            DAL_MeterIssues DAL_MeterIssues_Obj = new DAL_MeterIssues();
            DAL_PriceSchduler DAL_PriceSchduler_Obj = new DAL_PriceSchduler();
            DAL_Offtimes DAL_Offtimes_Obj = new DAL_Offtimes();
            DAL_Client DAL_Client_Obj = new DAL_Client();
            DAL_CardIssues DAL_CardIssues_Obj = new DAL_CardIssues();
            DAL_ClientInfo DAL_ClientInfo_obj = new DAL_ClientInfo();
            DAL_Deductions DAL_Deductions_Obj = new DAL_Deductions();

            unifyWaterCard_Comm UnifyCard_Object = new unifyWaterCard_Comm();

            bool Status = false;

            Status = UnifyCard_Object.card_ReadBasic();

            if (Status == false)
            {
                return Status;
            }
            Status = UnifyCard_Object.BLLCardIssues_Data.Func != CardFunctionEnum.CustomerCard.ToByte();
            if (Status)
            {
                MessageBox.Show("Please Clear Card First");
                return Status;
            }

            //reaad card first when card_num
            //complete data from database





            UnifyCard_Object.BLL_Client_Data = DAL_Client_Obj.GetClientDataBYClientNum(BLL_ReadingCardInfoData.Client_Number);
            UnifyCard_Object.BLL_PriceScheduler_Data = DAL_PriceSchduler_Obj.GetPriceSchedulerDataByName(BLL_ReadingCardInfoData.PriceSchedule_Name);
            UnifyCard_Object.BLL_Offtimes_Obj = DAL_Offtimes_Obj.GetOFFTimeData(BLL_ReadingCardInfoData.OFFTime_Name);
            UnifyCard_Object.BLL_MeterIssues_Data = DAL_MeterIssues_Obj.GetDataByMeterNum(BLL_ReadingCardInfoData.Meter_MeterNum);


            //  UnifyCard_Object.BLL_Deductions_Data.Deductions_Date=
            UnifyCard_Object.BLL_Deductions_Data.Deductions_MonthFees = bll_Deductions.Deductions_MonthFees;
            UnifyCard_Object.BLL_Deductions_Data.Deductions_AppDate = bll_Deductions.Deductions_AppDate;
            UnifyCard_Object.BLL_Deductions_Data.Deductions_Month = bll_Deductions.Deductions_Month;
            // get id for customer and create card
            UnifyCard_Object.BLL_ClientInfo_Data = DAL_ClientInfo_obj.GetDataByClientinfoID(BLL_ReadingCardInfoData.ClientInfo_ID);


            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = bll_ChargeBasicInf.ChargeBasicInf_MeterAction;

            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeDate = DateTime.Now;
            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_EnabledValvePeriod = bll_ChargeBasicInf.ChargeBasicInf_EnabledValvePeriod;

            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = bll_ChargeBasicInf.ChargeBasicInf_ChargeAmount;
            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = bll_ChargeBasicInf.ChargeBasicInf_ChargeNo;
            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = bll_ChargeBasicInf.ChargeBasicInf_CutoffWarningLimit;
            UnifyCard_Object.BLL_ChargeBasicInf_Data.ChargeBasicInf_MaxOverdraftCredit = bll_ChargeBasicInf.ChargeBasicInf_MaxOverdraftCredit;

            //to setup new client data
            Status = UnifyCard_Object.RechargeMeter();
            if (Status == true)
            {
                DAL_CardIssues_Obj.db.BeginTransaction(); //to brgin transction
                DAL_ClientInfo_obj.Insert(UnifyCard_Object.BLL_ClientInfo_Data);
                UnifyCard_Object.BLL_ClientInfo_Data = DAL_ClientInfo_obj.GetDataBySubscriberID(UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_SubscriberID);
                if (UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_ID > 0)
                {
                    UnifyCard_Object.BLL_Deductions_Data.Deductions_ClientInfoID = UnifyCard_Object.BLL_ClientInfo_Data.ClientInfo_ID;
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
