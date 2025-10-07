using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifyWaterCard.DataModels;
using UnifyWaterCard.Entities;
using UnifyWaterCard.Helpers;
namespace WaterMeter_id
{

    public class SmartTLV
    {
        //this function used to setup main data of card in card issues and setup function of it 
        const byte Index_Inst = 0x00;


        const byte Read_Inst = 0xB0;
        const byte Write_Inst = 0xD6;
        const byte WriteAndRead_Inst = 0xB1;
        const byte ACKResponse_Inst = 0x00;
        const byte NACKResponse_Inst = 0xff;

        const byte Index_Command = 0x01;
        const byte Clinet_Command = 0x3D;
        const byte Maint_Command = 0x45;
        const byte Config_Command = 0x41;
        const byte Retrival_Command = 0x48;
        const byte OnlyMeter_CommandType = 0x01;
        const byte ALLMeter_CommandType = 0x02;

        Secuirty Secuirty_Obj = new Secuirty();


        public BLL_Client BLL_Client_Data = new BLL_Client();

        public BLL_SchedulerMeter BLL_SchedulerMeter_Data = new BLL_SchedulerMeter();
        public BLL_MeterIssues BLL_MeterIssues_Data = new BLL_MeterIssues();
        public BLL_ClientInfo BLL_ClientInfo_Data = new BLL_ClientInfo();
        public BLL_ChargeBasicInf BLL_ChargeBasicInf_Data = new BLL_ChargeBasicInf();
        public BLL_PriceScheduler BLL_PriceScheduler_Data = new BLL_PriceScheduler();
        public BLL_Offtimes BLL_Offtimes_Obj = new BLL_Offtimes();
        public BLL_Deductions BLL_Deductions_Data = new BLL_Deductions();
        public BLL_Readings BLL_Readings_Data = new BLL_Readings();
        public BLL_CreditBalance BLL_CreditBalance_Data = new BLL_CreditBalance();
        public BLL_MeterState BLL_MeterState_Data = new BLL_MeterState();
        BLL_Orders BLL_Orders_Data = new BLL_Orders();



        DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();


        DAL_SmartConfiguration DAL_SmartConfiguration_Obj = new DAL_SmartConfiguration();
        BLL_SmartConfiguration BLL_SmartConfiguration_Data = new BLL_SmartConfiguration();
        BLL_WaterComp BLL_WaterComp_Data = new BLL_WaterComp();

        DAL_SchedulerMeter DAL_SchedulerMeter_Data = new DAL_SchedulerMeter();
        DAL_ClientInfo DAL_ClientInfo_Object = new DAL_ClientInfo();
        DAL_MeterState DAL_MeterState_Object = new DAL_MeterState();
        DAL_CreditBalance DAL_CreditBalance_OBject = new DAL_CreditBalance();
        DAL_Readings DAL_Readings_OBject = new DAL_Readings();

        DAL_Order DAL_Order_Obj = new DAL_Order();
       public SmartTLV()
        {
            BLL_WaterComp_Data = DAL_WaterComp_Obj.GetWaterCompData();
            BLL_SmartConfiguration_Data = DAL_SmartConfiguration_Obj.SelectData();
        }




        ///inst

        //0xff      NACK
        //0x00      ACK
        //0xb0      Read
        //0xd6     Write
        //0xb1    Write/Read


        public bool Add_NewMeter()
        {   //add new meter to schduler database after decode frame 
            //clientinfo,ClinetDataa,MeterISsues

            bool status = false;




            Meter meter2 = new Meter();
            //	WaterCompany waterCompanyById = this._TestToolRepository.GetWaterCompanyById(Convert.ToInt32(this.comboBoxClientCardCompany.SelectedValue));
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo
                                       });


            byte[] dataArray = GenerateFrame(payloadData, Read_Inst, Clinet_Command); //read data from client 
            BLL_SchedulerMeter_Data.SchedularMeter_Payloadarray(dataArray);

            BLL_SchedulerMeter_Data.SchedularMeter_TimeIssue = DateTime.Now;
            BLL_SchedulerMeter_Data.SchedularMeter_TimeLastReading = DateTime.Now.AddDays(-1);

            //insert meter in database 
            status = DAL_SchedulerMeter_Data.Insert(BLL_SchedulerMeter_Data);
            //insert this dataArray in table of schduler meter  and return true if inserted setup 
            return status;
        }
       
        public bool ParseClient_MeterData(byte []PayloadArray, BLL_SchedulerMeter Data )
        {

            bool Status = false;
            Status = CheckFrame(PayloadArray);
            if (Status)
            {
                if (PayloadArray[Index_Inst+1] == ACKResponse_Inst && PayloadArray[Index_Command+1] == Clinet_Command)
                {

                    //data setup true  parse payload of data 
                    //data setup true  parse payload of data 
                    Status = true;
                    APDUData apdudata2 = APDUData.ParseTLV(PayloadArray.Skip(3).Take(PayloadArray.Length - 4).ToArray<byte>());


                    Tlv tlv23 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
                    ClientCardInfo clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);


                    Tlv tlv6 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
                    CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

                    Tlv tlv7 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
                    MeterActions meterActions = MeterActions.Deserialize((tlv7 != null) ? tlv7.Value : null);

                    Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
                    SystemActions systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

                    Tlv tlv9 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
                    MeterInfo meterInfo = MeterInfo.Deserialize((tlv9 != null) ? tlv9.Value : null);

                    Tlv tlv10 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
                    CreadiTBalnceSmart creditBalance = CreadiTBalnceSmart.Deserialize((tlv10 != null) ? tlv10.Value : null);

                    Tlv tlv11 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
                    ReadingSmart readings = ReadingSmart.Deserialize((tlv11 != null) ? tlv11.Value : null);

                    Tlv tlv12 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
                    MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);

                 
          
                    //Clinet info
                    BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
                    //BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
                    BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
                    BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
                    BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

                    BLL_ClientInfo_Data.ClientInfo_SubscriberID = (int)clientCardInfo.ClientId;
                    //MeterInfo
                    BLL_MeterIssues_Data.Meter_MeterNum = (int)meterInfo.MeterId;
                    BLL_MeterIssues_Data.Meter_Origin = (int)(meterInfo.MeterOrigin + 2000);
                    BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
                    BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
                    BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;

                    //charge basci info 
                    BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
                    BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = creditInfo.CutoffWarningLimit;
                    BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;
                  
                    
                    
                    BLL_Readings_Data.Readings_Reading = readings.Reading;
                    BLL_Readings_Data.Readings_QuantityTotalNegative = (readings.QuantityTotalNegative);
                    BLL_Readings_Data.Readings_Date = DateTime.Now;
            
                   // BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
                    //	BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
                    //	BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
                    //	BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
                    //	BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
                    //	BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
                    //	BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
                    //	BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
                    //	BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
                    //	BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
                    //	BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
                    //	BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];


                    BLL_CreditBalance_Data.ReturnMeterAction = meterActions.Actions;
                    BLL_CreditBalance_Data.CreditBalance_RemainCredit = creditBalance.RemainCredit;
                    BLL_CreditBalance_Data.CreditBalance_OverdraftCredit = creditBalance.OverdraftCredit;
                    BLL_CreditBalance_Data.CreditBalance_ConsumedCredit = creditBalance.ConsumedCredit;
                    BLL_CreditBalance_Data.CreditBalance_CumulativeCharges = creditBalance.CumulativeCharges;
                    BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
                    BLL_CreditBalance_Data.CreditBalance_Date = DateTime.Now;
                   // BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
                    //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];



                    BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
                    BLL_MeterState_Data.MeterState_Malfun1_Day = meterState.MalFuns[0].Day;
                    BLL_MeterState_Data.MeterState_Malfun1_Month = meterState.MalFuns[0].Month;

                    BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
                    BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
                    BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

                    BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
                    BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
                    BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

                    BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
                    BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
                    BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

                    BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
                    BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
                    BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

                    //((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



                    BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
                    BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
                    BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
                    BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);
                    BLL_MeterState_Data.MeterState_Date = DateTime.Now;
                    BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;

                    //save data of creadibanlance
                    //save data of readings 
                    //save data of meterstate
                    //edit schulermeterlist edit status and readingtime 
                    DAL_ClientInfo DAL_ClientInfo_Object = new DAL_ClientInfo();
                    DAL_MeterState DAL_MeterState_Object = new DAL_MeterState();
                    DAL_CreditBalance DAL_CreditBalance_OBject = new DAL_CreditBalance();
                    DAL_Readings DAL_Readings_OBject = new DAL_Readings();

                    BLL_ClientInfo_Data = DAL_ClientInfo_Object.GetDataBySubscriberID(BLL_ClientInfo_Data.ClientInfo_SubscriberID);
                    BLL_MeterState_Data.MeterState_ClientInfoID = BLL_ClientInfo_Data.ClientInfo_ID;
                    BLL_CreditBalance_Data.CreditBalance_ClientInfoID = BLL_ClientInfo_Data.ClientInfo_ID;
                    BLL_Readings_Data.Readings_ClientInfoID = BLL_ClientInfo_Data.ClientInfo_ID;

                    DAL_MeterState_Object.Insert(BLL_MeterState_Data);
                    DAL_Readings_OBject.InsertReading(BLL_Readings_Data);
                    DAL_CreditBalance_OBject.Insert(BLL_CreditBalance_Data);

                    Data.SchedularMeter_TimeLastReading = DateTime.Now;

                    DAL_SchedulerMeter_Data.Update(Data);
                    //Edit 
                    Status = true;
                }

                else if (PayloadArray[Index_Inst+1] == 0x00)
                {
                    //ok but not required data
                }
                else
                { //nack data there are error in meter 


                }




            }
        
            //insert this dataArray in table of schduler meter  and return true if inserted setup 
            return Status;

        }


       public  bool Parse_Orders(byte[] Payload,BLL_Orders Data,bool IsError=false,string ErrorMessage=" ")
        {

            bool Status = false;
            Data.Orders_Status = 1;
            if (IsError)
            {
                Data.Orders_ErrorMessege = ErrorMessage;
            }
            else
            {
           
                Data.Orders_Payloadarray(Payload);
            }


             Status= DAL_Order_Obj.Update(Data);
            return Status;
        }
        public bool Send_MeterCharge() //send meter charge data to meter  //chargebasic_info  deduction chargedate_info
        {   //send charge to meter 
            //01
            bool Status = false;
            Meter meter2 = new Meter();

            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());





            ChargeDateInfo chargeDateInfo = new ChargeDateInfo();
            chargeDateInfo.ChargeDate = DateModel.Parse(BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeDate.ToString());
            chargeDateInfo.EnableValvePeriod = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_EnabledValvePeriod;



            string actionsValue = string.Concat(new string[]
          {
           "0" ,    //   CheckBox_setCategoryType.Checked ? "1" : "0",
           "0",     //   CheckBox_setPriceSchedule.Checked ? "1" : "0",
           "1",     //   CheckBox_setMaxOverdraftCredit.Checked ? "1" : "0",
           "1",     //   CheckBox_setCharge.Checked ? "1" : "0",
           "1",     //   CheckBox_setDeduction.Checked ? "1" : "0",
           "0",     //   CheckBox_setOffTimes.Checked ? "1" : "0",
           "1"      //   CheckBox_setChargeDateInfo.Checked ? "1" : "0",
          });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };
            CreditInfo creditInfo = new CreditInfo();
            creditInfo.ChargeAmount = BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount;
            creditInfo.ChargeNo = (uint)BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo;
            creditInfo.CutoffWarningLimit = (byte)BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit;
            creditInfo.MaxOverdraftCredit = (byte)BLL_ChargeBasicInf_Data.ChargeBasicInf_MaxOverdraftCredit;


            Deductions deductions = new Deductions();
            deductions.AppDate = DateModel.Parse(BLL_Deductions_Data.Deductions_AppDate.ToString());
            deductions.Month = (byte)BLL_Deductions_Data.Deductions_Month;
            deductions.MonthFees = BLL_Deductions_Data.Deductions_MonthFees;
            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {



                                                      clientCardInfo,
                                                      meter2.MeterInfo,
                                                      deductions,
                                                      creditInfo,
                                                      systemActions,
                                                      chargeDateInfo

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Write_Inst, Clinet_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Index_Command] == Clinet_Command)
                    {

                        //data setup true  parse payload of data 
                        APDUData apdudata2 = APDUData.ParseTLV(PayloadArray.Skip(2).Take(PayloadArray.Length - 3).ToArray<byte>());
                        Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
                        systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);


                        Status = true;
                    }

                    else if (PayloadArray[Index_Inst] == ACKResponse_Inst)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;
        }
        public bool Read_MeterData()
        {  //read direfct meter 
           //01
            bool Status = false;




            Meter meter2 = new Meter();
            //	WaterCompany waterCompanyById = this._TestToolRepository.GetWaterCompanyById(Convert.ToInt32(this.comboBoxClientCardCompany.SelectedValue));
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo
                                       });


            byte[] dataArray = GenerateFrame(payloadData, Read_Inst, Clinet_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);

            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Index_Command] == Clinet_Command)
                    {

                        //data setup true  parse payload of data 
                        //data setup true  parse payload of data 

                        APDUData apdudata2 = APDUData.ParseTLV(PayloadArray.Skip(2).Take(PayloadArray.Length - 3).ToArray<byte>());


                        Tlv tlv23 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
                        clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);


                        Tlv tlv4 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
                        Deductions deductions = Deductions.Deserialize((tlv4 != null) ? tlv4.Value : null);


                        Tlv tlv6 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
                        CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

                        Tlv tlv7 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
                        MeterActions meterActions = MeterActions.Deserialize((tlv7 != null) ? tlv7.Value : null);

                        Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
                        SystemActions systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

                        Tlv tlv9 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
                        MeterInfo meterInfo = MeterInfo.Deserialize((tlv9 != null) ? tlv9.Value : null);

                        Tlv tlv10 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
                        CreditBalance creditBalance = CreditBalance.Deserialize((tlv10 != null) ? tlv10.Value : null);

                        Tlv tlv11 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
                        Readings readings = Readings.Deserialize((tlv11 != null) ? tlv11.Value : null);

                        Tlv tlv12 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
                        MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);

                        Tlv tlv13 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 195);
                        ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlv13 != null) ? tlv13.Value : null);


                        BLL_Deductions_Data.Deductions_Month = (deductions.Month);
                        BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
                        BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);
                        //Clinet info
                        BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
                        //BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
                        BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
                        BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
                        BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

                        BLL_ClientInfo_Data.ClientInfo_SubscriberID = (int)clientCardInfo.ClientId;
                        //MeterInfo
                        BLL_MeterIssues_Data.Meter_MeterNum = (int)meterInfo.MeterId;
                        BLL_MeterIssues_Data.Meter_Origin = (int)(meterInfo.MeterOrigin + 2000);
                        BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
                        BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
                        BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;

                        //charge basci info 
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = creditInfo.CutoffWarningLimit;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;
                        BLL_Readings_Data.Readings_Reading = readings.Reading;
                        BLL_Readings_Data.Readings_QuantityTotalNegative = (readings.QuantityTotalNegative);

                        BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
                        //	BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
                        //	BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
                        //	BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
                        //	BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
                        //	BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
                        //	BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
                        //	BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
                        //	BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
                        //	BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
                        //	BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
                        //	BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];


                        BLL_CreditBalance_Data.ReturnMeterAction = meterActions.Actions;
                        BLL_CreditBalance_Data.CreditBalance_RemainCredit = creditBalance.RemainCredit;
                        BLL_CreditBalance_Data.CreditBalance_OverdraftCredit = creditBalance.OverdraftCredit;
                        BLL_CreditBalance_Data.CreditBalance_ConsumedCredit = creditBalance.ConsumedCredit;
                        BLL_CreditBalance_Data.CreditBalance_CumulativeCharges = creditBalance.CumulativeCharges;
                        BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];



                        BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
                        BLL_MeterState_Data.MeterState_Malfun1_Day = meterState.MalFuns[0].Day;
                        BLL_MeterState_Data.MeterState_Malfun1_Month = meterState.MalFuns[0].Month;

                        BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
                        BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
                        BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

                        BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
                        BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
                        BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

                        BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
                        BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
                        BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

                        BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
                        BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
                        BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

                        //((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



                        BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
                        BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
                        BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
                        BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);

                        BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;
                        Status = true;
                    }

                    else if (PayloadArray[Index_Inst] == 0x00)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            //insert this dataArray in table of schduler meter  and return true if inserted setup 
            return Status;
        }

        public bool SendALLMeter_Priceshduler()
        { //send meter priceshduler to all mters


            bool Status = false;
            string actionsValue = string.Concat(new string[]
            {
              "0" ,    //   CheckBox_setCategoryType.Checked ? "1" : "0",
              "1",     //   CheckBox_setPriceSchedule.Checked ? "1" : "0",
              "0",     //   CheckBox_setMaxOverdraftCredit.Checked ? "1" : "0",
              "0",     //   CheckBox_setCharge.Checked ? "1" : "0",
              "0",     //   CheckBox_setDeduction.Checked ? "1" : "0",
              "0",     //   CheckBox_setOffTimes.Checked ? "1" : "0",
              "0"      //   CheckBox_setChargeDateInfo.Checked ? "1" : "0",
             });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };



            PriceSchedule priceSchedule = new PriceSchedule();
            priceSchedule.AppDate = DateModel.Parse(BLL_PriceScheduler_Data.PriceSchedule_APPDate.ToString());

            priceSchedule.MonthFee1 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee1;
            priceSchedule.MonthFee2 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee2;

            priceSchedule.MonthFeesOptions = (byte)BLL_PriceScheduler_Data.PriceSchedule_MonthFeeOption;
            priceSchedule.NoOfUnitsInCalc = ((byte)(BLL_PriceScheduler_Data.PriceSchedule_NoOFUintsCalc));
            priceSchedule.PerMeterFees = (ushort)BLL_PriceScheduler_Data.PriceSchedule_PerMeterFee;

            priceSchedule.Pricing = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Pricing;

            priceSchedule.SewagePrice = (ushort)BLL_PriceScheduler_Data.PriceSchedule_SWGPrice;
            priceSchedule.SewagePercentage = (byte)BLL_PriceScheduler_Data.PriceSchedule_SWGPercent;
            try
            {
                priceSchedule.Levels = new PriceScheduleLevel[]
                {
                            new PriceScheduleLevel
                            {
                                StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Fee
                            },
                            new PriceScheduleLevel
                            {
                                StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Fee
                            }
                    };
            }
            catch
            {
                MessageBox.Show("Enter a valid PriceScheduleLevel.", "Error");

            }

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, null, null, null, keytosign, new ModelBase[]
                                       {




                                            systemActions,
                                            priceSchedule

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Write_Inst, Maint_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = ALLMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[0] == ACKResponse_Inst && PayloadArray[1] == Maint_Command)
                    {




                        Status = true;
                    }

                    else if (PayloadArray[0] == 0x00)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;



        }

        public bool SendMeter_Priceshduler()
        {//send priceshduler to one meter 

            //03
            bool Status = false;
            string actionsValue = string.Concat(new string[]
            {
              "0" ,    //   CheckBox_setCategoryType.Checked ? "1" : "0",
              "1",     //   CheckBox_setPriceSchedule.Checked ? "1" : "0",
              "0",     //   CheckBox_setMaxOverdraftCredit.Checked ? "1" : "0",
              "0",     //   CheckBox_setCharge.Checked ? "1" : "0",
              "0",     //   CheckBox_setDeduction.Checked ? "1" : "0",
              "0",     //   CheckBox_setOffTimes.Checked ? "1" : "0",
              "0"      //   CheckBox_setChargeDateInfo.Checked ? "1" : "0",
             });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };



            PriceSchedule priceSchedule = new PriceSchedule();
            priceSchedule.AppDate = DateModel.Parse(BLL_PriceScheduler_Data.PriceSchedule_APPDate.ToString());

            priceSchedule.MonthFee1 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee1;
            priceSchedule.MonthFee2 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee2;

            priceSchedule.MonthFeesOptions = (byte)BLL_PriceScheduler_Data.PriceSchedule_MonthFeeOption;
            priceSchedule.NoOfUnitsInCalc = ((byte)(BLL_PriceScheduler_Data.PriceSchedule_NoOFUintsCalc));
            priceSchedule.PerMeterFees = (ushort)BLL_PriceScheduler_Data.PriceSchedule_PerMeterFee;

            priceSchedule.Pricing = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Pricing;

            priceSchedule.SewagePrice = (ushort)BLL_PriceScheduler_Data.PriceSchedule_SWGPrice;
            priceSchedule.SewagePercentage = (byte)BLL_PriceScheduler_Data.PriceSchedule_SWGPercent;
            try
            {
                priceSchedule.Levels = new PriceScheduleLevel[]
                {
                            new PriceScheduleLevel
                            {
                                StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Fee
                            },
                            new PriceScheduleLevel
                            {
                                StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Fee
                            },
                            new PriceScheduleLevel
                            {
                                    StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Fee
                            },
                            new PriceScheduleLevel
                            {
                                        StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_StepMax,
                                Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Price,
                                Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Fee
                            }
                    };
            }
            catch
            {
                MessageBox.Show("Enter a valid PriceScheduleLevel.", "Error");

            }

            Meter meter2 = new Meter();
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo,
                                            systemActions,
                                            priceSchedule

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Write_Inst, Clinet_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Read_Inst] == Clinet_Command)
                    {




                        Status = true;
                    }

                    else if (PayloadArray[Index_Inst] == ACKResponse_Inst)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;
        }
        public bool GetMeter_Priceshduler()
        { //get direct priceshduler from one meter 
          //03
            bool Status = false;
            string actionsValue = string.Concat(new string[]
                                 {
                         "1",    //checkBoxRetrievalPriceSched.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalDeductions.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalOffTimes.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalClientId.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalThisChargeInfo.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalReadingsQty.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalCreditBalance.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalMState.Checked ? "1" : "0"
                                 });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };
            Meter meter2 = new Meter();
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo,
                                       systemActions

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Read_Inst, Retrival_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Index_Command] == Retrival_Command)
                    {




                        APDUData apdudata2 = APDUData.ParseTLV(PayloadArray.Skip(2).Take(PayloadArray.Length - 3).ToArray<byte>());


                        Tlv tlv23 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
                        clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);


                        Tlv tlv4 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
                        Deductions deductions = Deductions.Deserialize((tlv4 != null) ? tlv4.Value : null);


                        Tlv tlv6 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
                        CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

                        Tlv tlv7 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
                        MeterActions meterActions = MeterActions.Deserialize((tlv7 != null) ? tlv7.Value : null);

                        Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
                        systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

                        Tlv tlv9 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
                        MeterInfo meterInfo = MeterInfo.Deserialize((tlv9 != null) ? tlv9.Value : null);

                        Tlv tlv10 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
                        CreditBalance creditBalance = CreditBalance.Deserialize((tlv10 != null) ? tlv10.Value : null);

                        Tlv tlv11 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
                        Readings readings = Readings.Deserialize((tlv11 != null) ? tlv11.Value : null);

                        Tlv tlv12 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
                        MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);

                        Tlv tlv13 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 195);
                        ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlv13 != null) ? tlv13.Value : null);


                        BLL_Deductions_Data.Deductions_Month = (deductions.Month);
                        BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
                        BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);
                        //Clinet info
                        BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
                        //BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
                        BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
                        BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
                        BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

                        BLL_ClientInfo_Data.ClientInfo_SubscriberID = (int)clientCardInfo.ClientId;
                        //MeterInfo
                        BLL_MeterIssues_Data.Meter_MeterNum = (int)meterInfo.MeterId;
                        BLL_MeterIssues_Data.Meter_Origin = (int)(meterInfo.MeterOrigin + 2000);
                        BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
                        BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
                        BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;

                        //charge basci info 
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = creditInfo.CutoffWarningLimit;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;
                        BLL_Readings_Data.Readings_Reading = readings.Reading;
                        BLL_Readings_Data.Readings_QuantityTotalNegative = (readings.QuantityTotalNegative);

                        BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
                        //	BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
                        //	BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
                        //	BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
                        //	BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
                        //	BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
                        //	BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
                        //	BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
                        //	BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
                        //	BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
                        //	BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
                        //	BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];


                        BLL_CreditBalance_Data.ReturnMeterAction = meterActions.Actions;
                        BLL_CreditBalance_Data.CreditBalance_RemainCredit = creditBalance.RemainCredit;
                        BLL_CreditBalance_Data.CreditBalance_OverdraftCredit = creditBalance.OverdraftCredit;
                        BLL_CreditBalance_Data.CreditBalance_ConsumedCredit = creditBalance.ConsumedCredit;
                        BLL_CreditBalance_Data.CreditBalance_CumulativeCharges = creditBalance.CumulativeCharges;
                        BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
                        //	BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];



                        BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
                        BLL_MeterState_Data.MeterState_Malfun1_Day = meterState.MalFuns[0].Day;
                        BLL_MeterState_Data.MeterState_Malfun1_Month = meterState.MalFuns[0].Month;

                        BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
                        BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
                        BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

                        BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
                        BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
                        BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

                        BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
                        BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
                        BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

                        BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
                        BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
                        BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

                        //((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



                        BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
                        BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
                        BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
                        BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);

                        BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;
                        Status = true;
                    }

                    else if (PayloadArray[Index_Inst] == ACKResponse_Inst)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;
        }
        public bool SendALLMeter_Offtimes()
        { //send offtimes table for all meter
          //02
            bool Status = false;
            string actionsValue = string.Concat(new string[]
            {
           "0" ,    //   CheckBox_setCategoryType.Checked ? "1" : "0",
           "0",     //   CheckBox_setPriceSchedule.Checked ? "1" : "0",
           "0",     //   CheckBox_setMaxOverdraftCredit.Checked ? "1" : "0",
           "0",     //   CheckBox_setCharge.Checked ? "1" : "0",
           "0",     //   CheckBox_setDeduction.Checked ? "1" : "0",
           "1",     //   CheckBox_setOffTimes.Checked ? "1" : "0",
           "0"      //   CheckBox_setChargeDateInfo.Checked ? "1" : "0",
});

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };
            OffTimes offTimes = new OffTimes();
            offTimes.CutoffTime = (byte)BLL_Offtimes_Obj.OFFTime_CutOffTime;
            offTimes.GracePeriod = (byte)BLL_Offtimes_Obj.OFFTime_GracePeriod;
            offTimes.WorkStart = (byte)BLL_Offtimes_Obj.OFFTime_WorkStart;
            offTimes.WorkEnd = (byte)BLL_Offtimes_Obj.OFFTime_WorkEnd;

            offTimes.WorkingDays = (byte)BLL_Offtimes_Obj.OFFTime_WorkingDays;
            List<Holiday> list = new List<Holiday>();

            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Day
            });


            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Day
            });

            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Day
            });

            offTimes.Holidays = list.ToArray();

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, null, null, null, keytosign, new ModelBase[]
                                       {




                                            systemActions,
                                            offTimes

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Write_Inst, Maint_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = ALLMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[0] == ACKResponse_Inst && PayloadArray[1] == Maint_Command)
                    {




                        Status = true;
                    }

                    else if (PayloadArray[0] == 0x00)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;


        }

        public bool SendMeter_Offtimes()
        { //send offtimes table to one meter 
            bool Status = false;
            string actionsValue = string.Concat(new string[]
            {
           "0" ,    //   CheckBox_setCategoryType.Checked ? "1" : "0",
           "0",     //   CheckBox_setPriceSchedule.Checked ? "1" : "0",
           "0",     //   CheckBox_setMaxOverdraftCredit.Checked ? "1" : "0",
           "0",     //   CheckBox_setCharge.Checked ? "1" : "0",
           "0",     //   CheckBox_setDeduction.Checked ? "1" : "0",
           "1",     //   CheckBox_setOffTimes.Checked ? "1" : "0",
           "0"      //   CheckBox_setChargeDateInfo.Checked ? "1" : "0",
});

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };
            OffTimes offTimes = new OffTimes();
            offTimes.CutoffTime = (byte)BLL_Offtimes_Obj.OFFTime_CutOffTime;
            offTimes.GracePeriod = (byte)BLL_Offtimes_Obj.OFFTime_GracePeriod;
            offTimes.WorkStart = (byte)BLL_Offtimes_Obj.OFFTime_WorkStart;
            offTimes.WorkEnd = (byte)BLL_Offtimes_Obj.OFFTime_WorkEnd;

            offTimes.WorkingDays = (byte)BLL_Offtimes_Obj.OFFTime_WorkingDays;
            List<Holiday> list = new List<Holiday>();

            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Day
            });


            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Day
            });
            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Day
            });

            list.Add(new Holiday
            {
                Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Month,
                Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Day
            });

            offTimes.Holidays = list.ToArray();
            Meter meter2 = new Meter();
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo,
                                            systemActions,
                                            offTimes

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Write_Inst, Maint_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[0] == ACKResponse_Inst && PayloadArray[1] == Maint_Command)
                    {




                        Status = true;
                    }

                    else if (PayloadArray[0] == 0x00)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;

            //02
        }
        public bool GetMeter_Offtimes()
        { //get offtimes table from one meter 
          //02
            bool Status = false;
            string actionsValue = string.Concat(new string[]
                      {
                         "0",    //checkBoxRetrievalPriceSched.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalDeductions.Checked ? "1" : "0",
                         "1",   //checkBoxRetrievalOffTimes.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalClientId.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalThisChargeInfo.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalReadingsQty.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalCreditBalance.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalMState.Checked ? "1" : "0"
                      });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };

            Meter meter2 = new Meter();
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo,
                                     systemActions

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Read_Inst, Retrival_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Index_Command] == Retrival_Command)
                    {




                        Status = true;
                    }

                    else if (PayloadArray[Index_Inst] == ACKResponse_Inst)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;
        }

        public bool Get_FullReadings()
        { //get full readings from one meter 
          //04

            bool Status = false;
            string actionsValue = string.Concat(new string[]
                         {
                         "0",    //checkBoxRetrievalPriceSched.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalDeductions.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalOffTimes.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalClientId.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalThisChargeInfo.Checked ? "1" : "0",
                         "1",    //checkBoxRetrievalReadingsQty.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalCreditBalance.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalMState.Checked ? "1" : "0"
                         });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };

            Meter meter2 = new Meter();
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo,
                                     systemActions

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Read_Inst, Retrival_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Index_Command] == Retrival_Command)
                    {

                        APDUData apdudata2 = APDUData.ParseTLV(PayloadArray.Skip(2).Take(PayloadArray.Length - 3).ToArray<byte>());


                        Tlv tlv23 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
                        clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);


                        Tlv tlv4 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
                        Deductions deductions = Deductions.Deserialize((tlv4 != null) ? tlv4.Value : null);


                        Tlv tlv6 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
                        CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

                        Tlv tlv7 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
                        MeterActions meterActions = MeterActions.Deserialize((tlv7 != null) ? tlv7.Value : null);

                        Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
                        systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

                        Tlv tlv9 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
                        MeterInfo meterInfo = MeterInfo.Deserialize((tlv9 != null) ? tlv9.Value : null);

                        Tlv tlv10 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
                        CreditBalance creditBalance = CreditBalance.Deserialize((tlv10 != null) ? tlv10.Value : null);

                        Tlv tlv11 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
                        Readings readings = Readings.Deserialize((tlv11 != null) ? tlv11.Value : null);

                        Tlv tlv12 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
                        MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);

                        Tlv tlv13 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 195);
                        ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlv13 != null) ? tlv13.Value : null);


                        BLL_Deductions_Data.Deductions_Month = (deductions.Month);
                        BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
                        BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);
                        //Clinet info
                        BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
                        //BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
                        BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
                        BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
                        BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

                        BLL_ClientInfo_Data.ClientInfo_SubscriberID = (int)clientCardInfo.ClientId;
                        //MeterInfo
                        BLL_MeterIssues_Data.Meter_MeterNum = (int)meterInfo.MeterId;
                        BLL_MeterIssues_Data.Meter_Origin = (int)(meterInfo.MeterOrigin + 2000);
                        BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
                        BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
                        BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;

                        //charge basci info 
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = creditInfo.CutoffWarningLimit;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;
                        BLL_Readings_Data.Readings_Reading = readings.Reading;
                        BLL_Readings_Data.Readings_QuantityTotalNegative = (readings.QuantityTotalNegative);

                        BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
                        BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
                        BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
                        BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
                        BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
                        BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
                        BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
                        BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
                        BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
                        BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
                        BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
                        BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];


                        BLL_CreditBalance_Data.ReturnMeterAction = meterActions.Actions;
                        BLL_CreditBalance_Data.CreditBalance_RemainCredit = creditBalance.RemainCredit;
                        BLL_CreditBalance_Data.CreditBalance_OverdraftCredit = creditBalance.OverdraftCredit;
                        BLL_CreditBalance_Data.CreditBalance_ConsumedCredit = creditBalance.ConsumedCredit;
                        BLL_CreditBalance_Data.CreditBalance_CumulativeCharges = creditBalance.CumulativeCharges;
                        BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];



                        BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
                        BLL_MeterState_Data.MeterState_Malfun1_Day = meterState.MalFuns[0].Day;
                        BLL_MeterState_Data.MeterState_Malfun1_Month = meterState.MalFuns[0].Month;

                        BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
                        BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
                        BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

                        BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
                        BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
                        BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

                        BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
                        BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
                        BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

                        BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
                        BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
                        BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

                        //((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



                        BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
                        BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
                        BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
                        BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);

                        BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;
                        Status = true;



                    }

                    else if (PayloadArray[Index_Inst] == ACKResponse_Inst)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;
        }
        public bool Get_FullCreaditbalnce()
        { //get full readings from one meter 
          //04

            bool Status = false;
            string actionsValue = string.Concat(new string[]
                        {
                         "0",    //checkBoxRetrievalPriceSched.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalDeductions.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalOffTimes.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalClientId.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalThisChargeInfo.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalReadingsQty.Checked ? "1" : "0",
                         "1",   //checkBoxRetrievalCreditBalance.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalMState.Checked ? "1" : "0"
                        });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };

            Meter meter2 = new Meter();
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo,
                                     systemActions

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Read_Inst, Retrival_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Index_Command] == Retrival_Command)
                    {

                        APDUData apdudata2 = APDUData.ParseTLV(PayloadArray.Skip(2).Take(PayloadArray.Length - 3).ToArray<byte>());


                        Tlv tlv23 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
                        clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);


                        Tlv tlv4 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
                        Deductions deductions = Deductions.Deserialize((tlv4 != null) ? tlv4.Value : null);


                        Tlv tlv6 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
                        CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

                        Tlv tlv7 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
                        MeterActions meterActions = MeterActions.Deserialize((tlv7 != null) ? tlv7.Value : null);

                        Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
                        systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

                        Tlv tlv9 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
                        MeterInfo meterInfo = MeterInfo.Deserialize((tlv9 != null) ? tlv9.Value : null);

                        Tlv tlv10 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
                        CreditBalance creditBalance = CreditBalance.Deserialize((tlv10 != null) ? tlv10.Value : null);

                        Tlv tlv11 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
                        Readings readings = Readings.Deserialize((tlv11 != null) ? tlv11.Value : null);

                        Tlv tlv12 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
                        MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);

                        Tlv tlv13 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 195);
                        ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlv13 != null) ? tlv13.Value : null);


                        BLL_Deductions_Data.Deductions_Month = (deductions.Month);
                        BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
                        BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);
                        //Clinet info
                        BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
                        //BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
                        BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
                        BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
                        BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

                        BLL_ClientInfo_Data.ClientInfo_SubscriberID = (int)clientCardInfo.ClientId;
                        //MeterInfo
                        BLL_MeterIssues_Data.Meter_MeterNum = (int)meterInfo.MeterId;
                        BLL_MeterIssues_Data.Meter_Origin = (int)(meterInfo.MeterOrigin + 2000);
                        BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
                        BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
                        BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;

                        //charge basci info 
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = creditInfo.CutoffWarningLimit;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;
                        BLL_Readings_Data.Readings_Reading = readings.Reading;
                        BLL_Readings_Data.Readings_QuantityTotalNegative = (readings.QuantityTotalNegative);

                        BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
                        BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
                        BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
                        BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
                        BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
                        BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
                        BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
                        BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
                        BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
                        BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
                        BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
                        BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];


                        BLL_CreditBalance_Data.ReturnMeterAction = meterActions.Actions;
                        BLL_CreditBalance_Data.CreditBalance_RemainCredit = creditBalance.RemainCredit;
                        BLL_CreditBalance_Data.CreditBalance_OverdraftCredit = creditBalance.OverdraftCredit;
                        BLL_CreditBalance_Data.CreditBalance_ConsumedCredit = creditBalance.ConsumedCredit;
                        BLL_CreditBalance_Data.CreditBalance_CumulativeCharges = creditBalance.CumulativeCharges;
                        BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];



                        BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
                        BLL_MeterState_Data.MeterState_Malfun1_Day = meterState.MalFuns[0].Day;
                        BLL_MeterState_Data.MeterState_Malfun1_Month = meterState.MalFuns[0].Month;

                        BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
                        BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
                        BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

                        BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
                        BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
                        BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

                        BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
                        BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
                        BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

                        BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
                        BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
                        BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

                        //((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



                        BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
                        BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
                        BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
                        BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);

                        BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;
                        Status = true;



                    }

                    else if (PayloadArray[Index_Inst] == ACKResponse_Inst)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;
        }
        public bool Get_MeterState()
        { //get full readings from one meter 
          //04

            bool Status = false;
            string actionsValue = string.Concat(new string[]
                        {
                         "0",    //checkBoxRetrievalPriceSched.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalDeductions.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalOffTimes.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalClientId.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalThisChargeInfo.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalReadingsQty.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalCreditBalance.Checked ? "1" : "0",
                         "1",   //checkBoxRetrievalMState.Checked ? "1" : "0"
                        });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };

            Meter meter2 = new Meter();
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo,
                                     systemActions

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Read_Inst, Retrival_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Index_Command] == Retrival_Command)
                    {

                        APDUData apdudata2 = APDUData.ParseTLV(PayloadArray.Skip(2).Take(PayloadArray.Length - 3).ToArray<byte>());


                        Tlv tlv23 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
                        clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);


                        Tlv tlv4 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
                        Deductions deductions = Deductions.Deserialize((tlv4 != null) ? tlv4.Value : null);


                        Tlv tlv6 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
                        CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

                        Tlv tlv7 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
                        MeterActions meterActions = MeterActions.Deserialize((tlv7 != null) ? tlv7.Value : null);

                        Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
                        systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

                        Tlv tlv9 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
                        MeterInfo meterInfo = MeterInfo.Deserialize((tlv9 != null) ? tlv9.Value : null);

                        Tlv tlv10 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
                        CreditBalance creditBalance = CreditBalance.Deserialize((tlv10 != null) ? tlv10.Value : null);

                        Tlv tlv11 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
                        Readings readings = Readings.Deserialize((tlv11 != null) ? tlv11.Value : null);

                        Tlv tlv12 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
                        MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);

                        Tlv tlv13 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 195);
                        ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlv13 != null) ? tlv13.Value : null);


                        BLL_Deductions_Data.Deductions_Month = (deductions.Month);
                        BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
                        BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);
                        //Clinet info
                        BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
                        //BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
                        BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
                        BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
                        BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

                        BLL_ClientInfo_Data.ClientInfo_SubscriberID = (int)clientCardInfo.ClientId;
                        //MeterInfo
                        BLL_MeterIssues_Data.Meter_MeterNum = (int)meterInfo.MeterId;
                        BLL_MeterIssues_Data.Meter_Origin = (int)(meterInfo.MeterOrigin + 2000);
                        BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
                        BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
                        BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;

                        //charge basci info 
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = creditInfo.CutoffWarningLimit;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;
                        BLL_Readings_Data.Readings_Reading = readings.Reading;
                        BLL_Readings_Data.Readings_QuantityTotalNegative = (readings.QuantityTotalNegative);

                        BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
                        BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
                        BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
                        BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
                        BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
                        BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
                        BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
                        BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
                        BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
                        BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
                        BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
                        BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];


                        BLL_CreditBalance_Data.ReturnMeterAction = meterActions.Actions;
                        BLL_CreditBalance_Data.CreditBalance_RemainCredit = creditBalance.RemainCredit;
                        BLL_CreditBalance_Data.CreditBalance_OverdraftCredit = creditBalance.OverdraftCredit;
                        BLL_CreditBalance_Data.CreditBalance_ConsumedCredit = creditBalance.ConsumedCredit;
                        BLL_CreditBalance_Data.CreditBalance_CumulativeCharges = creditBalance.CumulativeCharges;
                        BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];



                        BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
                        BLL_MeterState_Data.MeterState_Malfun1_Day = meterState.MalFuns[0].Day;
                        BLL_MeterState_Data.MeterState_Malfun1_Month = meterState.MalFuns[0].Month;

                        BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
                        BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
                        BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

                        BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
                        BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
                        BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

                        BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
                        BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
                        BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

                        BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
                        BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
                        BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

                        //((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



                        BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
                        BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
                        BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
                        BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);

                        BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;
                        Status = true;



                    }

                    else if (PayloadArray[Index_Inst] == ACKResponse_Inst)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;
        }

        public bool Get_Lastcharge()
        { //get full readings from one meter 
          //04

            bool Status = false;
            string actionsValue = string.Concat(new string[]
                        {
                         "0",    //checkBoxRetrievalPriceSched.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalDeductions.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalOffTimes.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalClientId.Checked ? "1" : "0",
                         "1",   //checkBoxRetrievalThisChargeInfo.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalReadingsQty.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalCreditBalance.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalMState.Checked ? "1" : "0"
                        });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };

            Meter meter2 = new Meter();
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo,
                                     systemActions

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Read_Inst, Retrival_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Index_Command] == Retrival_Command)
                    {

                        APDUData apdudata2 = APDUData.ParseTLV(PayloadArray.Skip(2).Take(PayloadArray.Length - 3).ToArray<byte>());


                        Tlv tlv23 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
                        clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);


                        Tlv tlv4 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
                        Deductions deductions = Deductions.Deserialize((tlv4 != null) ? tlv4.Value : null);


                        Tlv tlv6 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
                        CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

                        Tlv tlv7 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
                        MeterActions meterActions = MeterActions.Deserialize((tlv7 != null) ? tlv7.Value : null);

                        Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
                        systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

                        Tlv tlv9 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
                        MeterInfo meterInfo = MeterInfo.Deserialize((tlv9 != null) ? tlv9.Value : null);

                        Tlv tlv10 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
                        CreditBalance creditBalance = CreditBalance.Deserialize((tlv10 != null) ? tlv10.Value : null);

                        Tlv tlv11 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
                        Readings readings = Readings.Deserialize((tlv11 != null) ? tlv11.Value : null);

                        Tlv tlv12 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
                        MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);

                        Tlv tlv13 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 195);
                        ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlv13 != null) ? tlv13.Value : null);


                        BLL_Deductions_Data.Deductions_Month = (deductions.Month);
                        BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
                        BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);
                        //Clinet info
                        BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
                        //BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
                        BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
                        BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
                        BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

                        BLL_ClientInfo_Data.ClientInfo_SubscriberID = (int)clientCardInfo.ClientId;
                        //MeterInfo
                        BLL_MeterIssues_Data.Meter_MeterNum = (int)meterInfo.MeterId;
                        BLL_MeterIssues_Data.Meter_Origin = (int)(meterInfo.MeterOrigin + 2000);
                        BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
                        BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
                        BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;

                        //charge basci info 
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = creditInfo.CutoffWarningLimit;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;
                        BLL_Readings_Data.Readings_Reading = readings.Reading;
                        BLL_Readings_Data.Readings_QuantityTotalNegative = (readings.QuantityTotalNegative);

                        BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
                        BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
                        BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
                        BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
                        BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
                        BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
                        BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
                        BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
                        BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
                        BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
                        BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
                        BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];


                        BLL_CreditBalance_Data.ReturnMeterAction = meterActions.Actions;
                        BLL_CreditBalance_Data.CreditBalance_RemainCredit = creditBalance.RemainCredit;
                        BLL_CreditBalance_Data.CreditBalance_OverdraftCredit = creditBalance.OverdraftCredit;
                        BLL_CreditBalance_Data.CreditBalance_ConsumedCredit = creditBalance.ConsumedCredit;
                        BLL_CreditBalance_Data.CreditBalance_CumulativeCharges = creditBalance.CumulativeCharges;
                        BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];



                        BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
                        BLL_MeterState_Data.MeterState_Malfun1_Day = meterState.MalFuns[0].Day;
                        BLL_MeterState_Data.MeterState_Malfun1_Month = meterState.MalFuns[0].Month;

                        BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
                        BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
                        BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

                        BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
                        BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
                        BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

                        BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
                        BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
                        BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

                        BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
                        BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
                        BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

                        //((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



                        BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
                        BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
                        BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
                        BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);

                        BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;
                        Status = true;



                    }

                    else if (PayloadArray[Index_Inst] == ACKResponse_Inst)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;
        }
        public bool Get_Deduction()
        { //get full readings from one meter 
          //04

            bool Status = false;
            string actionsValue = string.Concat(new string[]
                        {
                         "0",    //checkBoxRetrievalPriceSched.Checked ? "1" : "0",
                         "1",    //checkBoxRetrievalDeductions.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalOffTimes.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalClientId.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalThisChargeInfo.Checked ? "1" : "0",
                         "0",    //checkBoxRetrievalReadingsQty.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalCreditBalance.Checked ? "1" : "0",
                         "0",   //checkBoxRetrievalMState.Checked ? "1" : "0"
                        });

            BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = Convert.ToByte(actionsValue, 2);


            //charge basic info //chargedateinfo  //deduction
            SystemActions systemActions = new SystemActions
            {
                Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
            };

            Meter meter2 = new Meter();
            meter2.MeterInfo = new MeterInfo();
            meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
            byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterMan = bytes[0];
            }
            meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
            meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

            bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
            if (bytes.Length > 0)
            {
                meter2.MeterInfo.MeterModel = bytes[0];
            }



            ClientCardInfo clientCardInfo = new ClientCardInfo();
            clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
            clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
            clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
            clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
            clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

            byte[] keytosign = BLL_SmartConfiguration_Data.SmartConfigurations_SecurityEnable == 0 ? null : GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW);
            byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(BLL_ClientInfo_Data.ClientInfo_SubscriberID), BitConverter.GetBytes(meter2.MeterInfo.MeterId), null, keytosign, new ModelBase[]
                                       {
                                     clientCardInfo,
                                     meter2.MeterInfo,
                                     systemActions

                                       });

            byte[] dataArray = GenerateFrame(payloadData, Read_Inst, Retrival_Command); //read data from client 
            BLL_Orders_Data.Orders_Payloadarray(dataArray);
            BLL_Orders_Data.Orders_TimeOut = 600;
            BLL_Orders_Data.Orders_Status = 0;
            BLL_Orders_Data.Orders_IssueDate = DateTime.Now;
            BLL_Orders_Data.Orders_CommandType = OnlyMeter_CommandType;
            BLL_Orders_Data.Orders_RetransmitNo = 2;
            BLL_Orders_Data.Orders_SchedularID = BLL_SchedulerMeter_Data.SchedularMeter_ID;

            //insert data in order table 
            int orderID = DAL_Order_Obj.Insert(BLL_Orders_Data);



            //insert this dataArray in table of order 
            Thread.Sleep(1000); // Wait for 1 second before checking again

            //read last order inserted and see status if status true
            BLL_Orders_Data = DAL_Order_Obj.Select(orderID);

            if (BLL_Orders_Data.Orders_Status == 1)
            {
                byte[] PayloadArray = BLL_Orders_Data.Orders_Payloadarray();

                Status = CheckFrame(PayloadArray);
                if (Status)
                {
                    if (PayloadArray[Index_Inst] == ACKResponse_Inst && PayloadArray[Index_Command] == Retrival_Command)
                    {

                        APDUData apdudata2 = APDUData.ParseTLV(PayloadArray.Skip(2).Take(PayloadArray.Length - 3).ToArray<byte>());


                        Tlv tlv23 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
                        clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);


                        Tlv tlv4 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
                        Deductions deductions = Deductions.Deserialize((tlv4 != null) ? tlv4.Value : null);


                        Tlv tlv6 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
                        CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

                        Tlv tlv7 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
                        MeterActions meterActions = MeterActions.Deserialize((tlv7 != null) ? tlv7.Value : null);

                        Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
                        systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

                        Tlv tlv9 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
                        MeterInfo meterInfo = MeterInfo.Deserialize((tlv9 != null) ? tlv9.Value : null);

                        Tlv tlv10 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
                        CreditBalance creditBalance = CreditBalance.Deserialize((tlv10 != null) ? tlv10.Value : null);

                        Tlv tlv11 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
                        Readings readings = Readings.Deserialize((tlv11 != null) ? tlv11.Value : null);

                        Tlv tlv12 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
                        MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);

                        Tlv tlv13 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 195);
                        ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlv13 != null) ? tlv13.Value : null);


                        BLL_Deductions_Data.Deductions_Month = (deductions.Month);
                        BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
                        BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);
                        //Clinet info
                        BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
                        //BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
                        BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
                        BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
                        BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

                        BLL_ClientInfo_Data.ClientInfo_SubscriberID = (int)clientCardInfo.ClientId;
                        //MeterInfo
                        BLL_MeterIssues_Data.Meter_MeterNum = (int)meterInfo.MeterId;
                        BLL_MeterIssues_Data.Meter_Origin = (int)(meterInfo.MeterOrigin + 2000);
                        BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
                        BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
                        BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;

                        //charge basci info 
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = creditInfo.CutoffWarningLimit;
                        BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;
                        BLL_Readings_Data.Readings_Reading = readings.Reading;
                        BLL_Readings_Data.Readings_QuantityTotalNegative = (readings.QuantityTotalNegative);

                        BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
                        BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
                        BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
                        BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
                        BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
                        BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
                        BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
                        BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
                        BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
                        BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
                        BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
                        BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];


                        BLL_CreditBalance_Data.ReturnMeterAction = meterActions.Actions;
                        BLL_CreditBalance_Data.CreditBalance_RemainCredit = creditBalance.RemainCredit;
                        BLL_CreditBalance_Data.CreditBalance_OverdraftCredit = creditBalance.OverdraftCredit;
                        BLL_CreditBalance_Data.CreditBalance_ConsumedCredit = creditBalance.ConsumedCredit;
                        BLL_CreditBalance_Data.CreditBalance_CumulativeCharges = creditBalance.CumulativeCharges;
                        BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
                        BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];



                        BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
                        BLL_MeterState_Data.MeterState_Malfun1_Day = meterState.MalFuns[0].Day;
                        BLL_MeterState_Data.MeterState_Malfun1_Month = meterState.MalFuns[0].Month;

                        BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
                        BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
                        BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

                        BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
                        BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
                        BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

                        BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
                        BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
                        BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

                        BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
                        BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
                        BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

                        //((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



                        BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
                        BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
                        BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
                        BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);

                        BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;
                        Status = true;



                    }

                    else if (PayloadArray[Index_Inst] == ACKResponse_Inst)
                    {
                        //ok but not required data
                    }
                    else
                    { //nack data there are error in meter 


                    }




                }
            }
            return Status;
        }

        //----------------------------------------------------------frames
        //todo to check
        private byte[] GenerateFrame(byte[] arrayPayload, byte inst, byte command)
        {
            byte[] array = new byte[3];
            array[0] = (byte)(arrayPayload.Length+2);
            array[1] = inst;
            array[2] = command;

            byte checksumbyte = CalculateChecksum((byte[])array.Concat(arrayPayload));

            byte[] arraychecksum = new byte[1];
            arraychecksum[0] = checksumbyte;

            return array.Concat(arrayPayload).Concat(arraychecksum).ToArray();
        }

        public byte[] GenerateFrame( )
        {
            byte[] array = new byte[3];
            array[0] = (byte)2;
            array[1] = 0xA0;
            array[2] = 0X3D;

            byte checksumbyte = CalculateChecksum(array);

            byte[] arraychecksum = new byte[1];
            arraychecksum[0] = checksumbyte;

            return array.Concat(arraychecksum).ToArray();
        }

        private byte CalculateChecksum(byte[] data)
        {
            byte checksum = 0;
            foreach (byte b in data)
            {
                checksum += b;
            }
            return (byte)(checksum & 0xFF);
        }
        private byte CalculateChecksum(byte[] data, int startIndex, int endIndex)
        {
            byte checksum = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                checksum += data[i];
            }
            return (byte)(checksum & 0xFF);
        }

        public bool CheckFrame(byte[] frame)
        {
            if (frame == null || frame.Length == 0)
            {
                return false;
            }

            byte calcCheckSum = CalculateChecksum(frame, 0, frame.Length - 1);
            byte receivedCheckSum = frame[frame.Length - 1];

            return receivedCheckSum == calcCheckSum;
        }

    }
}
