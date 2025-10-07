using Org.BouncyCastle.Bcpg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;
using static WaterMeter_id.SEL_CollectorConfig;

namespace WaterMeter_id
{
    public class SEL_CollectorConfig
    {

        public event Action<string> CallBack_ReveiveData;
        public event Action<string> CallBack_TransimitData;
        public event Action<int,int,int ,int> CallBack_UpdateData;
        // Event to handle re-initialize the system
        public event Action <int> CallBack_label;
        public JsonSettingsManager SettingsManagerObj = new JsonSettingsManager();
        public UartActions UartActionsObj = new UartActions();
        public BLL_SmartConfiguration BLL_SmartConfiguration_Data = new BLL_SmartConfiguration();
        BLL_Getway BLL_Getway_Data = new BLL_Getway();
        DAL_Getway DAL_Getway_Object=   new DAL_Getway();
        DAL_Order DAL_Order_Object = new DAL_Order();
        DAL_SchedulerMeter DAL_SchedulerMeter_Object = new DAL_SchedulerMeter();
        SmartTLV SmartTLV_Object = new SmartTLV();

        public BLL_JsonSettings BLL_JsonSettings_Data = new BLL_JsonSettings();
        SEL_SmartConfiguration SEL_SmartConfigurationObj = new SEL_SmartConfiguration();
        SEL_Aggregation SEL_AggregationObj= new SEL_Aggregation();
        bool Recieveflage = false;
        int LengthCounter = 0;
        string ErrorStateString = "";
        bool TimeoutOtNot = false;
        private bool threadRunning = false;


        #region Packet Identefiers
        public enum PacketID_Field : UInt32
        {
            PacketID_Send = 0x00010000, // Packet ID for sending
            PacketID_SendToAgg = 0x000A0000,
            PacketID_Receive1 = 0x00020000, // Packet ID for first type of receiving
            PacketID_Receive2 = 0x00030000, // Packet ID for second type of receiving
            PacketID_GetMeterNum = 0x00070000, // Packet ID for getting meter number
            PacketID_GetDataFromMeter = 0x00050000, // Packet ID for getting data from meter
        }

        PacketID_Field PacketID_field = PacketID_Field.PacketID_Send;
        const int PacketID_Index = 0;
        const int PacketID_Length = 4;

        int Aggregation_Field = 0;
        const int Aggregation_Index = 4;
        const int Aggregation_Length = 4;
        public enum CMD_field : UInt16
        {
           SendToMeter = 0x0055,
           SendToAggragtor = 0x000a,

        }
        public enum ErrorStatus : UInt16
        {
            No_ResponseBrodcast = 0x1A,
            NO_ERROR = 0x1F, // No error

            METER_NOT_FOUND = 0xE0, // Meter not found error
            CMD_NOT_FOUND = 0xE1, // Command not found error
            CHECKSUM_ERROR = 0xE2, // Checksum error
            WRONG_INPUT = 0xE3, // Wrong input error
            INCORRECT_PACKETID = 0xE4, // Incorrect packet ID error
            INCOMPATIBLE_CMD = 0xE5, // Incompatible command error
            DATA_NOT_SENT = 0xE6, // Data not sent error
            EMPTY_PAYLOAD = 0xE7, // Empty payload error
        }

        const int CMD_Index = 8;
        const int CMD_Length = 2;

        int MeterNum_Field = 0;
        const int MeterNum_Index = 10;
        const int MeterNum_Length = 4;

        UInt32 MessageID_Field = 0;
        const int MessageID_Index = 14;
        const int MessageID_Length = 4;

        const byte MessageRepeat_Field = 0x00;
        const int MessageRepeat_Index = 18;

        const byte MeterType_Field = 0x0A;
        const int MeterType_Index = 19;

        const long SpareFields = 0;
        const int SpareFields_Index = 20;
        const int SpareFields_Length = 6;

        UInt32 Sequencer_Field = 0xFF000000;
        const int Sequencer_Index = 26;
        const int Sequencer_Length = 4;

        
        const byte StartPacket = 0xAA;
        const int StartPacket_Index = 30;

        const byte EndPacket = 0xBB;
        const int EndPacket_Length = 1;


        const byte PingCollector_CommandType = 0x01;// ping colletor 
        const byte OneMeter_CommandType = 0x02; //send to one meter 
        const byte OneAgg_CommandType = 0x03; //send to all meter in aggaragtor 
        const byte ALLMeter_CommandType = 0x04; //send data to all network in this getway 
        #endregion
        public SEL_CollectorConfig()
        {
            BLL_SmartConfiguration_Data = SEL_SmartConfigurationObj.GetSmartConfiguration();
        }

        #region Callback Functions
        protected virtual void OnReciveData(string message)
        {
            CallBack_ReveiveData?.Invoke(message);
        }

        protected virtual void OnTransmitData(string message)
        {
            CallBack_TransimitData?.Invoke(message);
        }

        protected virtual void OnUpdateData(int freq, int remain,int TimeoutData,int frequency)
        {
            CallBack_UpdateData?.Invoke(freq, remain, TimeoutData, frequency);
        }

        protected virtual void Label(int conn)
        {
            CallBack_label?.Invoke(conn);
        }
        #endregion

        #region Automatic Part
        public void Mainthread() //main function that using to send and receive data from data base 
        {
            int frquencynum_Now = 0;
            int remainNum_Meter = 0;
            DateTime TimeNow = DateTime.Now;
            DateTime TimeStartQuery;
            DateTime TimeEndQuery;

            bool isUARTConnectionOpen = false;
            // Convert array to string and save in member Orders_Payload

            while (true)
            {
              
              while (threadRunning)
              {

                  TimeNow =DateTime.Now;

                    BLL_SmartConfiguration_Data = SEL_SmartConfigurationObj.GetSmartConfiguration();
                    if (BLL_SmartConfiguration_Data != null)
                    {

                        // SetSerialConfig(BLL_JsonSettings_Data.ComPort, BLL_JsonSettings_Data.UART_BaudRate.ToString(), BLL_JsonSettings_Data.UART_StopBits.ToString(), BLL_JsonSettings_Data.UART_DataBits.ToString(), BLL_JsonSettings_Data.UART_Parity)
                        if (OpenSerialConnection())
                        {

                            if (isUARTConnectionOpen != true)
                            {
                                string DislayedText = Environment.NewLine + " at " + DateTime.Now.ToString() + " :: " + "COM Port Openned Succesfully";
                                DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";
                                Label(0);
                                OnTransmitData(DislayedText);
                            }
                            isUARTConnectionOpen = true;


                            //to color the added Text



                        }
                        else
                        {
                            if (isUARTConnectionOpen != false)
                            {
                                string DislayedText = " at " + DateTime.Now.ToString() + " :: UART Port Busy for other Running Task";
                                DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";
                                Label(1);
                                OnTransmitData(DislayedText);
                            }
                            isUARTConnectionOpen = false;

                        }

                        if (isUARTConnectionOpen == true)
                        {
                            BLL_JsonSettings_Data.Frequency = BLL_SmartConfiguration_Data.SmartConfigurations_Frequency;
                            BLL_JsonSettings_Data.Timeout = BLL_SmartConfiguration_Data.SmartConfigurations_TimeOut;

                            //BLL_JsonSettings_Data.GatewayNumber=1;
                            // BLL_SmartConfiguration_Data;
                            if (BLL_SmartConfiguration_Data.SmartConfigurations_Frequency == 1)
                            {
                                TimeStartQuery = DateTime.Today; // Start of the current day at midnight.
                                TimeEndQuery = DateTime.Today.AddDays(1).AddMinutes(-30); //befor last day by 30 minute 
                            }
                            else if (BLL_SmartConfiguration_Data.SmartConfigurations_Frequency > 1)
                            {
                                int differentHours = 24 / BLL_SmartConfiguration_Data.SmartConfigurations_Frequency;
                                frquencynum_Now = TimeNow.Hour / differentHours; // Consider if this reassignment is necessary.
                                TimeStartQuery = DateTime.Today.AddHours(differentHours * frquencynum_Now);
                                TimeEndQuery = DateTime.Today.AddHours((differentHours * (frquencynum_Now + 1))).AddMinutes(-30);
                            }
                            else
                            {     //0
                                break; //dont make anything 

                            }
                            List<BLL_SchedulerMeter> SchulerMeterList = DAL_SchedulerMeter_Object.Getschduler(BLL_JsonSettings_Data.GatewayNumber, TimeStartQuery, "RF");
                            remainNum_Meter = SchulerMeterList.Count;
                            OnUpdateData(frquencynum_Now, remainNum_Meter, BLL_JsonSettings_Data.Timeout, BLL_JsonSettings_Data.Frequency);
                            if (SchulerMeterList.Count > 0)
                            {  // There must be data set up
                                foreach (var data in SchulerMeterList)
                                {

                                 
                                    if (ProcessMeterSchulerData(data))
                                    {
                                        remainNum_Meter--;
                                        OnUpdateData(frquencynum_Now, remainNum_Meter, BLL_JsonSettings_Data.Timeout, BLL_JsonSettings_Data.Frequency);
                                        // Process successful
                                    }
                                    else
                                    {
                                        // Process failed
                                    }

                                    List<BLL_Orders> OrderList1 = DAL_Order_Object.GetOrdersFromGatwayNum(BLL_JsonSettings_Data.GatewayNumber);

                                    foreach (var orderData in OrderList1)
                                    {
                                        if (ProcessMeterOrder(orderData))
                                        { //data process sucessfully 
                                        }


                                    }



                                }

                            }

                            List<BLL_Orders> OrderList = DAL_Order_Object.GetOrdersFromGatwayNum(BLL_JsonSettings_Data.GatewayNumber);

                            foreach (var orderData in OrderList)
                            {
                                if (ProcessMeterOrder(orderData))
                                { //data process sucessfull
                                }


                            }

                        }
                    }
              }
                Thread.Sleep(10); // Simulate some work
                                  //
                                  //
              }

            }




        private bool ProcessMeterSchulerData(BLL_SchedulerMeter Data)
        {
            // Initialize status indicator
            bool Status = false;

            try
            {
                // Initialize array to store payload data
                byte[] arraypayload1 = new byte[31];

                // Retrieve payload data from BLL_SchedulerMeter object
                byte[] arraypayload2 = Data.SchedularMeter_Payloadarray();

                //CheckSum Part 
                byte[] arraypayloadCheckSum = new byte[arraypayload2.Length + 2];
                arraypayloadCheckSum[0] = 0xAA;
                Array.Copy(arraypayload2, 0, arraypayloadCheckSum, 1, arraypayload2.Length);
                arraypayloadCheckSum[arraypayloadCheckSum.Length - 1] = 0xBB;

                byte PayloadcheckSumResult = CalculateChecksum(arraypayloadCheckSum);
                PacketID_Field PacketID_field = PacketID_Field.PacketID_Send;

                byte[] packetIDBytes = BitConverter.GetBytes((UInt32)PacketID_field).Reverse().ToArray();
                Array.Copy(packetIDBytes, 0, arraypayload1, PacketID_Index, PacketID_Length);


                Aggregation_Field = Data.AggregationNum;
                byte[] Aggregation_FieldBytes = BitConverter.GetBytes((int)Aggregation_Field).Reverse().ToArray();
                Array.Copy(Aggregation_FieldBytes, 0, arraypayload1, Aggregation_Index, Aggregation_Length);

                CMD_field CMD_Field = CMD_field.SendToMeter;
                byte[] CMD_FieldBytes = BitConverter.GetBytes((UInt16)CMD_Field).Reverse().ToArray();
                Array.Copy(CMD_FieldBytes, 0, arraypayload1, CMD_Index, CMD_Length);


                MeterNum_Field = Data.MeterNum;
                byte[] MeterNum_FieldBytes = BitConverter.GetBytes((int)MeterNum_Field).Reverse().ToArray();
                Array.Copy(MeterNum_FieldBytes, 0, arraypayload1, MeterNum_Index, MeterNum_Length);


                arraypayload1[MeterType_Index] = MeterType_Field;

                MessageID_Field++;
                byte[] MessageID_FieldBytes = BitConverter.GetBytes((UInt32)MessageID_Field).Reverse().ToArray();
                Array.Copy(MessageID_FieldBytes, 0, arraypayload1, MessageID_Index, MessageID_Length);


                arraypayload1[MessageRepeat_Index] = MessageRepeat_Field;


                Sequencer_Field = (UInt32)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                byte[] Sequencer_FieldBytes = BitConverter.GetBytes((UInt32)Sequencer_Field).Reverse().ToArray();
                Sequencer_FieldBytes[0] = 0xff;
                Array.Copy(Sequencer_FieldBytes, 0, arraypayload1, Sequencer_Index, Sequencer_Length);


                byte[] SpareFieldsBytes = BitConverter.GetBytes((long)SpareFields).Reverse().ToArray();
                Array.Copy(SpareFieldsBytes, 0, arraypayload1, SpareFields_Index, SpareFields_Length);


                arraypayload1[StartPacket_Index] = StartPacket;

                byte[] arraypayload = new byte[arraypayload1.Length + arraypayload2.Length + 1]; // Increase the length by 1 to accommodate EndPacket


                Array.Copy(arraypayload1, 0, arraypayload, 0, arraypayload1.Length);
                Array.Copy(arraypayload2, 0, arraypayload, arraypayload1.Length, arraypayload2.Length);

                arraypayload[arraypayload.Length - 1] = EndPacket;

                byte[] arraypayloadFinal = new byte[254];
                byte[] arraypayloadZeros = new byte[254 - arraypayload.Length];

                Array.Copy(arraypayload, 0, arraypayloadFinal, 0, arraypayload.Length);
                arraypayloadFinal[arraypayload.Length] = PayloadcheckSumResult;
                Array.Copy(arraypayloadZeros, 0, arraypayloadFinal, arraypayload.Length + 1, arraypayloadZeros.Length - 1);


                // Concatenate arraypayload1 and arraypayload2

                bool falge = UartActionsObj.Send_Data(arraypayloadFinal, arraypayloadFinal.Length);
                string DataTransmit = "";
                if (falge == false)
                {
                    DataTransmit = DateTime.Now.ToString() + " Fail Serial Close:: " + ConvertToString(arraypayloadFinal) + Environment.NewLine;
                    OnTransmitData(DataTransmit);
                    return false;
                }
                // Convert array to string and save in member Orders_Payload
                DataTransmit = DateTime.Now.ToString() + " :: " + ConvertToString(arraypayloadFinal) + Environment.NewLine;
                OnTransmitData(DataTransmit);

                // Get the time at which sending started
                DateTime startTime = DateTime.Now;

                //todo to ask
                // int waitingTime = BLL_SmartConfiguration_Data.SmartConfigurations_TimeOut * 1000;
                // Thread.Sleep(5000); // Simulate some work


                string DataReceive = "";
                byte[] arrayRead = null;
                byte[] PacketID_Receive1ExpectedPattern = BitConverter.GetBytes((UInt32)PacketID_Field.PacketID_Receive1).Reverse().ToArray();
                byte[] PacketID_Receive2ExpectedPattern = BitConverter.GetBytes((UInt32)PacketID_Field.PacketID_Receive2).Reverse().ToArray();
                // Set timeout duration to 2 seconds
                TimeSpan timeoutDuration = TimeSpan.FromSeconds(BLL_SmartConfiguration_Data.SmartConfigurations_TimeOut);//TimeSpan.FromSeconds(7);

                while (DateTime.Now - startTime < timeoutDuration)
                {
                    arrayRead = UartActionsObj.Read_Data();

                    if (arrayRead != null)
                    {
                        DataReceive = DateTime.Now.ToString() + " ::" + ConvertToString(arrayRead) + Environment.NewLine;

                        if (arrayRead.Length >= 254 && (UInt32)PacketID_Field.PacketID_Receive2 == GetFourBytes(arrayRead, PacketID_Index) && (byte)ErrorStatus.NO_ERROR == arrayRead[9])
                        {
                            OnReciveData(DataReceive);
                            Recieveflage = true;
                            break;
                        }
                        else if ((byte)ErrorStatus.METER_NOT_FOUND == arrayRead[9])
                        {
                            DataReceive += "\n Meter is not found:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.WRONG_INPUT == arrayRead[9])
                        {
                            DataReceive += "\n Wrong Input:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.INCORRECT_PACKETID == arrayRead[9])
                        {
                            DataReceive += "\nIcorrect packet ID:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.INCOMPATIBLE_CMD == arrayRead[9])
                        {
                            DataReceive += "\n Incorrect CMD:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.DATA_NOT_SENT == arrayRead[9])
                        {
                            DataReceive += "\n Data is not sent:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.EMPTY_PAYLOAD == arrayRead[9])
                        {
                            DataReceive += "\nEmpty Payload:\n";
                            OnReciveData(DataReceive);
                        }
                        else
                        {
                            DataReceive += "\n other Packet \n";
                            OnReciveData(DataReceive);
                            Thread.Sleep(100); // Adjust sleep duration as needed

                        }
                    }
                    else
                    {
                        // Sleep for a short duration before checking again
                        Thread.Sleep(100); // Adjust sleep duration as needed
                    }

                }
                if (Recieveflage == true)
                {

                    bool flage = GetFourBytes(arrayRead, Aggregation_Index) == Aggregation_Field
                        && GetFourBytes(arrayRead, MeterNum_Index) == MeterNum_Field;

                    //&& (arrayRead[] == StartPacket) && (arrayRead[] == EndPacket);


                    if (flage == false)
                    {

                        UInt32 extractedNumber = GetFourBytes(arrayRead, Aggregation_Index);
                        if (extractedNumber != Aggregation_Field)
                        {
                            DataReceive = "Aggregation Number error:\n";

                            OnReciveData(DataReceive);
                            return false;
                        }

                        extractedNumber = GetFourBytes(arrayRead, MeterNum_Index);
                        if (extractedNumber != MeterNum_Field)
                        {
                            DataReceive = "Meter Number error:\n";

                            OnReciveData(DataReceive);
                            return false;
                        }
                    }
                    byte[] dataToParse = new byte[arrayRead[31] + 2]; // Exclude the last byte and the first 31 bytes
                    Array.Copy(arrayRead, 31, dataToParse, 0, arrayRead[31] + 2);
                    if (SmartTLV_Object.ParseClient_MeterData(dataToParse, Data)) //Payload only
                    {
                        DataReceive = "data Parsed and inserted well :\n";

                        OnReciveData(DataReceive);
                        Status = true;
                    }
                    else
                    {
                        DataReceive = "data fail to pass:\n";

                        OnReciveData(DataReceive);
                    }
                }
                else
                {
                    DataReceive = "No Data Recieved:\n";

                    OnReciveData(DataReceive);
                }
            }
            catch (Exception ex)
            {
                Status = false;
                // Handle exception
            }
            return Status;

        }


        private bool ProcessMeterFunction(BLL_Orders Data, int Aggregator_Num, int Aggregator_NumLength)
        {
            // Increment the length counter
            LengthCounter++;

            // Initialize status indicator
            bool Status = false;

            try
            {
                // Initialize array to store payload data
                byte[] arraypayload1 = new byte[31];

                // Retrieve payload data from BLL_Orders object
                byte[] arraypayload2 = Data.Orders_Payloadarray();

                //CheckSum Part 
                byte[] arraypayloadCheckSum = new byte[arraypayload2.Length + 2];
                arraypayloadCheckSum[0] = 0xAA;
                Array.Copy(arraypayload2, 0, arraypayloadCheckSum, 1, arraypayload2.Length);
                arraypayloadCheckSum[arraypayloadCheckSum.Length - 1] = 0xBB;
                byte PayloadcheckSumResult = CalculateChecksum(arraypayloadCheckSum);

                if (Data.Orders_CommandType != 1)
                {
                    PacketID_Field PacketID_field = PacketID_Field.PacketID_SendToAgg;
                    byte[] packetIDBytes = BitConverter.GetBytes((UInt32)PacketID_field).Reverse().ToArray();
                    Array.Copy(packetIDBytes, 0, arraypayload1, PacketID_Index, PacketID_Length);


                    Aggregation_Field = Aggregator_Num;
                    byte[] Aggregation_FieldBytes = BitConverter.GetBytes((int)Aggregation_Field).Reverse().ToArray();
                    Array.Copy(Aggregation_FieldBytes, 0, arraypayload1, Aggregation_Index, Aggregation_Length);

                    CMD_field CMD_Field = CMD_field.SendToMeter;
                    byte[] CMD_FieldBytes = BitConverter.GetBytes((UInt16)CMD_Field).Reverse().ToArray();
                    Array.Copy(CMD_FieldBytes, 0, arraypayload1, CMD_Index, CMD_Length);


                    MeterNum_Field = 0;
                    byte[] MeterNum_FieldBytes = BitConverter.GetBytes((int)MeterNum_Field).Reverse().ToArray();
                    Array.Copy(MeterNum_FieldBytes, 0, arraypayload1, MeterNum_Index, MeterNum_Length);

                }

                else
                {
                    PacketID_Field PacketID_field = PacketID_Field.PacketID_Send;

                    Aggregation_Field = Data.Aggergation_Number;

                    byte[] packetIDBytes = BitConverter.GetBytes((UInt32)PacketID_field).Reverse().ToArray();
                    Array.Copy(packetIDBytes, 0, arraypayload1, PacketID_Index, PacketID_Length);



                    byte[] Aggregation_FieldBytes = BitConverter.GetBytes((int)Aggregation_Field).Reverse().ToArray();
                    Array.Copy(Aggregation_FieldBytes, 0, arraypayload1, Aggregation_Index, Aggregation_Length);

                    CMD_field CMD_Field = CMD_field.SendToMeter;
                    byte[] CMD_FieldBytes = BitConverter.GetBytes((UInt16)CMD_Field).Reverse().ToArray();
                    Array.Copy(CMD_FieldBytes, 0, arraypayload1, CMD_Index, CMD_Length);


                    MeterNum_Field = Data.Meter_Number;
                    byte[] MeterNum_FieldBytes = BitConverter.GetBytes((int)MeterNum_Field).Reverse().ToArray();
                    Array.Copy(MeterNum_FieldBytes, 0, arraypayload1, MeterNum_Index, MeterNum_Length);

                }



                arraypayload1[MeterType_Index] = MeterType_Field;

                MessageID_Field++;
                byte[] MessageID_FieldBytes = BitConverter.GetBytes((UInt32)MessageID_Field).Reverse().ToArray();
                Array.Copy(MessageID_FieldBytes, 0, arraypayload1, MessageID_Index, MessageID_Length);


                arraypayload1[MessageRepeat_Index] = MessageRepeat_Field;


                Sequencer_Field = (UInt32)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                byte[] Sequencer_FieldBytes = BitConverter.GetBytes((UInt32)Sequencer_Field).Reverse().ToArray();
                Sequencer_FieldBytes[0] = 0xff;
                Array.Copy(Sequencer_FieldBytes, 0, arraypayload1, Sequencer_Index, Sequencer_Length);


                byte[] SpareFieldsBytes = BitConverter.GetBytes((long)SpareFields).Reverse().ToArray();
                Array.Copy(SpareFieldsBytes, 0, arraypayload1, SpareFields_Index, SpareFields_Length);


                arraypayload1[StartPacket_Index] = StartPacket;

                byte[] arraypayload = new byte[arraypayload1.Length + arraypayload2.Length + 1]; // Increase the length by 1 to accommodate EndPacket


                Array.Copy(arraypayload1, 0, arraypayload, 0, arraypayload1.Length);
                Array.Copy(arraypayload2, 0, arraypayload, arraypayload1.Length, arraypayload2.Length);

                arraypayload[arraypayload.Length - 1] = EndPacket;

                byte[] arraypayloadFinal = new byte[254];
                byte[] arraypayloadZeros = new byte[254 - arraypayload.Length];

                Array.Copy(arraypayload, 0, arraypayloadFinal, 0, arraypayload.Length);
                arraypayloadFinal[arraypayload.Length] = PayloadcheckSumResult;
                Array.Copy(arraypayloadZeros, 0, arraypayloadFinal, arraypayload.Length + 1, arraypayloadZeros.Length - 1);

                // Send data over UART
                bool falge = UartActionsObj.Send_Data(arraypayloadFinal, arraypayloadFinal.Length);
                string DataTransmit = "";
                if (falge == false)
                {
                    DataTransmit = DateTime.Now.ToString() + " Fail Serial Close:: " + ConvertToString(arraypayloadFinal) + Environment.NewLine;
                    OnTransmitData(DataTransmit);
                    return false;
                }
                // Convert array to string and save in member Orders_Payload
                DataTransmit = DateTime.Now.ToString() + " Order :: " + ConvertToString(arraypayloadFinal) + Environment.NewLine;
                OnTransmitData(DataTransmit);

                // Get the time at which sending started
                DateTime startTime = DateTime.Now;

                //  int waitingTime = Data.Orders_TimeOut* 1000;
                //  Thread.Sleep(waitingTime); // Simulate some work

                string DataReceive = "";
                byte[] arrayRead = null;
                byte[] PacketID_Receive1ExpectedPattern = BitConverter.GetBytes((UInt32)PacketID_Field.PacketID_Receive1).Reverse().ToArray();
                byte[] PacketID_Receive2ExpectedPattern = BitConverter.GetBytes((UInt32)PacketID_Field.PacketID_Receive2).Reverse().ToArray();
                // Set timeout duration to 2 seconds
                TimeSpan timeoutDuration = TimeSpan.FromSeconds(BLL_SmartConfiguration_Data.SmartConfigurations_TimeOut);//TimeSpan.FromSeconds(7);


                while (DateTime.Now - startTime < timeoutDuration)
                {
                    arrayRead = UartActionsObj.Read_Data();

                    if (arrayRead != null)
                    {
                        DataReceive = DateTime.Now.ToString() + " ::" + ConvertToString(arrayRead) + Environment.NewLine;
                        //Mostaf



                        if (arrayRead.Length >= 254 && (UInt32)PacketID_Field.PacketID_Receive2 == GetFourBytes(arrayRead, PacketID_Index) && ((byte)ErrorStatus.NO_ERROR == arrayRead[9] || (byte)ErrorStatus.No_ResponseBrodcast == arrayRead[9]))
                        {
                            Recieveflage = true;
                            OnReciveData(DataReceive);

                            break;
                        }
                        else if ((byte)ErrorStatus.METER_NOT_FOUND == arrayRead[9])
                        {
                            DataReceive += "\n Meter is not found:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.WRONG_INPUT == arrayRead[9])
                        {
                            DataReceive += "\n Wrong Input:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.INCORRECT_PACKETID == arrayRead[9])
                        {
                            DataReceive += "\nIcorrect packet ID:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.INCOMPATIBLE_CMD == arrayRead[9])
                        {
                            DataReceive += "\n Incorrect CMD:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.DATA_NOT_SENT == arrayRead[9])
                        {
                            DataReceive += "\n Data is not sent:\n";
                            OnReciveData(DataReceive);
                        }
                        else if ((byte)ErrorStatus.EMPTY_PAYLOAD == arrayRead[9])
                        {
                            DataReceive += "\nEmpty Payload:\n";
                            OnReciveData(DataReceive);
                        }
                        else
                        {
                            DataReceive += "\n other Packet \n";
                            OnReciveData(DataReceive);


                        }
                    }
                    else
                    {
                        // Sleep for a short duration before checking again
                        Thread.Sleep(100); // Adjust sleep duration as needed
                    }

                }
                if (Recieveflage == true)
                {
                    bool flage = false;

                    if (Data.Orders_CommandType != 1)
                    {
                        flage = GetFourBytes(arrayRead, Aggregation_Index) == Aggregation_Field;


                    }
                    else
                    {
                        flage = GetFourBytes(arrayRead, Aggregation_Index) == Aggregation_Field
                                                    && GetFourBytes(arrayRead, MeterNum_Index) == MeterNum_Field;

                    }




                    //  && (arrayRead[StartPacket_Index] == StartPacket)        && (arrayRead[arrayRead.Length - 1] == EndPacket);


                    if (flage == false)
                    {

                        UInt32 extractedNumber = GetFourBytes(arrayRead, Aggregation_Index);
                        if (extractedNumber != Aggregation_Field)
                        {
                            DataReceive = "Aggregation Number error:\n";

                            OnReciveData(DataReceive);
                            return false;
                        }

                        extractedNumber = GetFourBytes(arrayRead, MeterNum_Index);
                        if (extractedNumber != MeterNum_Field)
                        {
                            DataReceive = "Meter Number error:\n";

                            OnReciveData(DataReceive);
                            return false;
                        }
                    }


                    byte[] dataToParse = new byte[arrayRead[31] + 2]; // Exclude the last byte and the first 31 bytes
                    Array.Copy(arrayRead, 31, dataToParse, 0, arrayRead[31] + 2);

                    if (LengthCounter == Aggregator_NumLength)
                    {
                        LengthCounter = 0;

                        if (SmartTLV_Object.Parse_Orders(dataToParse, Data)) //Payload only
                        {
                            DataReceive = "data Parsed and inserted well :\n";

                            OnReciveData(DataReceive);
                            Status = true;
                        }
                        else
                        {
                            DataReceive = "data fail to pass:\n";

                            OnReciveData(DataReceive);
                        }
                        TimeoutOtNot = false;
                        ErrorStateString = " ";

                    }

                }
                else
                {
                    ErrorStateString += "Timeout in Aggregator Number " + Aggregator_Num;
                    TimeoutOtNot = true;
                    DataReceive = "No Data Recieved:\n";

                    OnReciveData(DataReceive);

                    if (LengthCounter == Aggregator_NumLength)
                    {
                        LengthCounter = 0;

                        if (arrayRead == null)
                        {
                            if (SmartTLV_Object.Parse_Orders(null, Data, TimeoutOtNot, ErrorStateString)) //Payload only
                            {
                                DataReceive = "data Parsed and inserted well :\n";

                                OnReciveData(DataReceive);
                                Status = true;
                            }
                            else
                            {
                                DataReceive = "data fail to pass:\n";

                                OnReciveData(DataReceive);
                            }
                        }
                        else if (arrayRead.Length >= 32)
                        {
                            byte[] dataToParse = new byte[arrayRead[31] + 2]; // Exclude the last byte and the first 31 bytes
                            Array.Copy(arrayRead, 31, dataToParse, 0, arrayRead[31] + 2);

                            if (SmartTLV_Object.Parse_Orders(dataToParse, Data, TimeoutOtNot, ErrorStateString)) //Payload only
                            {
                                DataReceive = "data Parsed and inserted well :\n";

                                OnReciveData(DataReceive);
                                Status = true;
                            }
                            else
                            {
                                DataReceive = "data fail to pass:\n";

                                OnReciveData(DataReceive);
                            }
                        }
                        TimeoutOtNot = false;
                        ErrorStateString = " ";

                    }

                }
            }
            catch (Exception ex)
            {
                Status = false;
                // Handle exception
            }
            return Status;
        }


        private bool ProcessMeterOrder(BLL_Orders Data)
        {
            // Initialize status indicator
            bool status = false;
            try
            {
                if (Data.Orders_CommandType != 1) //order type  send data to specific meter 
                {

                    // Get all aggregation numbers associated with the gateway
                    int[] aggregationNumbers = SEL_AggregationObj.GetAllAggregationNumbersFromGetwayData(BLL_JsonSettings_Data.GatewayNumber);
                    LengthCounter = 0;
                    // Iterate through each aggregation number and process the meter function
                    foreach (int aggregationNumber in aggregationNumbers)
                    {

                        status = ProcessMeterFunction(Data, aggregationNumber, aggregationNumbers.Length);
                    }
                }
                else
                {
                    // For order type 1, process meter function with single aggregation number
                    LengthCounter = 0;
                    status = ProcessMeterFunction(Data, Data.Aggergation_Number, 1);
                }
            }
            catch (Exception ex)
            {
                status = false;
                // Handle exception
            }
            return status;

        }

        public void Mainthread_Start()
        {
            try
            {
                // Set threadRunning flag to true to start the main thread
                threadRunning = true;
            }
            catch (Exception ex)
            {

            }
        }

        public void Mainthread_Stop()
        {
            try
            {
                // Set threadRunning flag to false to stop the main thread
                threadRunning = false;
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        #region Manual Part

        public string MeterNumberVar = "0";
        public string MeterAggregationNumberVar = "0";

        public byte[] ManualConcatinatedData(string MeterNumParam, string AggregationNumParam, ref string SendString)
        {
            // Initialize byte arrays
            byte[] arraypayload1 = new byte[31];
            byte[] arraypayload2 = SmartTLV_Object.GenerateFrame();

            // CheckSum Part 
            byte[] arraypayloadCheckSum = new byte[arraypayload2.Length + 2];
            arraypayloadCheckSum[0] = 0xAA;
            Array.Copy(arraypayload2, 0, arraypayloadCheckSum, 1, arraypayload2.Length);
            arraypayloadCheckSum[arraypayloadCheckSum.Length - 1] = 0xBB;
            byte PayloadcheckSumResult = CalculateChecksum(arraypayloadCheckSum);

            // Convert PacketID_field to byte array and copy it to arraypayload1
            byte[] packetIDBytes = BitConverter.GetBytes((UInt32)PacketID_field).Reverse().ToArray();
            Array.Copy(packetIDBytes, 0, arraypayload1, PacketID_Index, PacketID_Length);

            // Parse AggregationNumParam to int and copy it to Aggregation_Field
            Aggregation_Field = int.Parse(AggregationNumParam);
            byte[] Aggregation_FieldBytes = BitConverter.GetBytes((int)Aggregation_Field).Reverse().ToArray();
            Array.Copy(Aggregation_FieldBytes, 0, arraypayload1, Aggregation_Index, Aggregation_Length);

            // Set CMD_Field and copy it to arraypayload1
            CMD_field CMD_Field = CMD_field.SendToMeter;
            Byte[] CMD_FieldBytes = BitConverter.GetBytes((UInt16)CMD_Field).Reverse().ToArray();
            Array.Copy(CMD_FieldBytes, 0, arraypayload1, CMD_Index, CMD_Length);

            // Parse MeterNumParam to int and copy it to MeterNum_Field
            MeterNum_Field = int.Parse(MeterNumParam);
            byte[] MeterNum_FieldBytes = BitConverter.GetBytes((int)MeterNum_Field).Reverse().ToArray();
            Array.Copy(MeterNum_FieldBytes, 0, arraypayload1, MeterNum_Index, MeterNum_Length);

            // Copy MeterType_Field to arraypayload1
            arraypayload1[MeterType_Index] = MeterType_Field;

            // Reset MessageID_Field and copy it to arraypayload1
            MessageID_Field = 0;
            byte[] MessageID_FieldBytes = BitConverter.GetBytes((UInt32)MessageID_Field).Reverse().ToArray();
            Array.Copy(MessageID_FieldBytes, 0, arraypayload1, MessageID_Index, MessageID_Length);

            // Copy MessageRepeat_Field to arraypayload1
            arraypayload1[MessageRepeat_Index] = MessageRepeat_Field;

            // Set Sequencer_Field and copy it to arraypayload1
            Sequencer_Field = (UInt32)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            byte[] Sequencer_FieldBytes = BitConverter.GetBytes((UInt32)Sequencer_Field).Reverse().ToArray();
            Sequencer_FieldBytes[0] = 0xff;
            Array.Copy(Sequencer_FieldBytes, 0, arraypayload1, Sequencer_Index, Sequencer_Length);

            // Copy SpareFields to arraypayload1
            byte[] SpareFieldsBytes = BitConverter.GetBytes((long)SpareFields).Reverse().ToArray();
            Array.Copy(SpareFieldsBytes, 0, arraypayload1, SpareFields_Index, SpareFields_Length);

            // Set StartPacket and copy it to arraypayload1
            arraypayload1[StartPacket_Index] = StartPacket;

            // Create final array by concatenating arraypayload1, arraypayload2, and checksum
            byte[] arraypayload = new byte[arraypayload1.Length + arraypayload2.Length + 1]; // Increase the length by 1 to accommodate EndPacket
            Array.Copy(arraypayload1, 0, arraypayload, 0, arraypayload1.Length);
            Array.Copy(arraypayload2, 0, arraypayload, arraypayload1.Length, arraypayload2.Length);
            arraypayload[arraypayload.Length - 1] = EndPacket;

            // Create arraypayloadFinal by adding checksum and padding zeros
            byte[] arraypayloadFinal = new byte[254];
            byte[] arraypayloadZeros = new byte[254 - arraypayload.Length];
            Array.Copy(arraypayload, 0, arraypayloadFinal, 0, arraypayload.Length);
            arraypayloadFinal[arraypayload.Length] = PayloadcheckSumResult;
            Array.Copy(arraypayloadZeros, 0, arraypayloadFinal, arraypayload.Length + 1, arraypayloadZeros.Length - 1);

            // Convert arraypayloadFinal to string and assign it to SendString
            SendString = ConvertToString(arraypayloadFinal);

            return arraypayloadFinal;
        }

        public bool ManualSendRecieveData(string MeterNumParam, string AggregationNumParam, string SendString)
        {
            // Initialize variables
            bool Status = false;
            string RefSendString = null;

            // Call ManualConcatinatedData function to generate payload data
            ManualConcatinatedData(MeterNumberVar, MeterAggregationNumberVar, ref RefSendString);

            // Convert SendString to byte array
            byte[] arraypayloadFinal = ConvertToByteArray(SendString);

            // Set Sequencer_Field and update arraypayloadFinal
            Sequencer_Field = (UInt32)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            byte[] Sequencer_FieldBytes = BitConverter.GetBytes((UInt32)Sequencer_Field).Reverse().ToArray();
            Sequencer_FieldBytes[0] = 0xff;
            Array.Copy(Sequencer_FieldBytes, 0, arraypayloadFinal, Sequencer_Index, Sequencer_Length);

            // Send data over UART
            bool falge = UartActionsObj.Send_Data(arraypayloadFinal, arraypayloadFinal.Length);

            string DataTransmit = "";
            if (falge == false)
            {
                // Handle transmission failure
                DataTransmit = DateTime.Now.ToString() + " Fail Serial Close:: " + ConvertToString(arraypayloadFinal) + Environment.NewLine;
                OnTransmitData(DataTransmit);
                return false;
            }
            // Convert array to string and save in member Orders_Payload
            DataTransmit = DateTime.Now.ToString() + " :: " + ConvertToString(arraypayloadFinal) + Environment.NewLine;
            OnTransmitData(DataTransmit);

            // Get the time at which sending started
            DateTime startTime = DateTime.Now;

            //todo to ask
            // int waitingTime = BLL_SmartConfiguration_Data.SmartConfigurations_TimeOut * 1000;
            //Thread.Sleep(5000); // Simulate some work


            string DataReceive = "";
            byte[] arrayRead = new byte[254];

            byte[] PacketID_Receive1ExpectedPattern = BitConverter.GetBytes((UInt32)PacketID_Field.PacketID_Receive1).Reverse().ToArray();
            byte[] PacketID_Receive2ExpectedPattern = BitConverter.GetBytes((UInt32)PacketID_Field.PacketID_Receive2).Reverse().ToArray();
            // Set timeout duration to 2 seconds
            TimeSpan timeoutDuration = TimeSpan.FromSeconds(6);

            while (DateTime.Now - startTime < timeoutDuration)
            {
                Thread.Sleep(10); // Adjust sleep duration as needed
                arrayRead = UartActionsObj.Read_Data();

                if (arrayRead != null)
                {

                    if (arrayRead.Length >= 254 &&
                     (UInt32)PacketID_Field.PacketID_Receive2 == GetFourBytes(arrayRead, PacketID_Index)
                     && (byte)ErrorStatus.NO_ERROR == arrayRead[CMD_Index])
                    {
                        Recieveflage = true;
                        break;
                    }
                    else if ((byte)ErrorStatus.METER_NOT_FOUND == arrayRead[CMD_Index])
                    {
                        DataReceive = "Meter is not found:\n";
                        OnReciveData(DataReceive);
                    }
                    else if ((byte)ErrorStatus.WRONG_INPUT == arrayRead[CMD_Index])
                    {
                        DataReceive = "Wrong Input:\n";
                        OnReciveData(DataReceive);
                    }
                    else if ((byte)ErrorStatus.INCORRECT_PACKETID == arrayRead[CMD_Index])
                    {
                        DataReceive = "Icorrect packet ID:\n";
                        OnReciveData(DataReceive);
                    }
                    else if ((byte)ErrorStatus.INCOMPATIBLE_CMD == arrayRead[CMD_Index])
                    {
                        DataReceive = "Incorrect CMD:\n";
                        OnReciveData(DataReceive);
                    }
                    else if ((byte)ErrorStatus.DATA_NOT_SENT == arrayRead[CMD_Index])
                    {
                        DataReceive = "Data is not sent:\n";
                        OnReciveData(DataReceive);
                    }
                    else if ((byte)ErrorStatus.EMPTY_PAYLOAD == arrayRead[CMD_Index])
                    {
                        DataReceive = "Empty Payload:\n";
                        OnReciveData(DataReceive);
                    }
                    else
                    {
                        DataReceive = DateTime.Now.ToString() + " ::" + ConvertToString(arrayRead) + Environment.NewLine;
                        OnReciveData(DataReceive);
                    }
                }
                else
                {
                    // Sleep for a short duration before checking again

                }
            }
            if (Recieveflage == true)
            {

                bool flage = GetFourBytes(arrayRead, Aggregation_Index) == Aggregation_Field
                    && GetFourBytes(arrayRead, MeterNum_Index) == MeterNum_Field
                    && (arrayRead[StartPacket_Index] == StartPacket) && (arrayRead[arrayRead.Length - 1] == EndPacket);


                if (flage == false)
                {
                    UInt32 extractedNumber = GetFourBytes(arrayRead, Aggregation_Index);

                    if (extractedNumber != Aggregation_Field)
                    {
                        DataReceive = "Aggregation Number error:\n";

                        OnReciveData(DataReceive);
                        return false;
                    }

                    extractedNumber = GetFourBytes(arrayRead, MeterNum_Index);
                    if (extractedNumber != MeterNum_Field)
                    {
                        DataReceive = "Meter Number error:\n";

                        OnReciveData(DataReceive);
                        return false;
                    }
                }

            }
            else
            {
                DataReceive = "No Sucssessful Data Recieved:\n";

                OnReciveData(DataReceive);
            }
            return Status;
        }

        #endregion

        #region Generic Functions
        public static byte[] ConvertToByteArray(string sendString)
        {
            // Split the string into individual byte strings
            string[] byteStrings = sendString.Split(',');

            // Create a byte array to hold the converted bytes
            byte[] byteArray = new byte[byteStrings.Length];

            // Convert each byte string to byte and store it in the byte array
            for (int i = 0; i < byteStrings.Length; i++)
            {
                // Remove the "0x" prefix and parse the byte value
                byte byteValue = byte.Parse(byteStrings[i].Substring(2), System.Globalization.NumberStyles.HexNumber);
                byteArray[i] = byteValue;
            }

            return byteArray;
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

        UInt32 GetFourBytes(byte[] arrayRead, int Index)
        {
            if (arrayRead == null)
            {
                return 0;
            }
            byte[] extractedBytes = new byte[4];
            Array.Copy(arrayRead, Index, extractedBytes, 0, 4);
            // Reverse the bytes
            Array.Reverse(extractedBytes);
            return BitConverter.ToUInt32(extractedBytes, 0);
        }

        private bool CheckBytes(byte[] byteArray, byte[] expectedPattern, int index, int Length)
        {
            int counter = 0;


            // Check if the first 4 bytes of byteArray match the expected pattern
            for (int i = index; i < Length; i++)
            {
                if (byteArray[i] != expectedPattern[counter++])
                {
                    return false;
                }
            }

            return true;
        }

        private string ConvertToString(byte[] byteArray)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteArray)
            {
                sb.AppendFormat("0x{0:X2},", b);
            }
            return sb.ToString().TrimEnd(',');
        }

        #endregion

        #region Json Part


        public void LoadFromGUIToJson(string PortComboBox, string BaudRateComboBox, string DataBitsComboBox, string StopBitsComboBox, string GetwaySelectionComboBox, string ParityComboBox, string TxFileTextBox, string RxFileTextBox)
        {
            BLL_JsonSettings ASettings = new BLL_JsonSettings();

            ASettings.ComPort = PortComboBox;
            ASettings.UART_BaudRate = int.Parse(BaudRateComboBox);
            ASettings.UART_Parity = ParityComboBox;
            ASettings.UART_DataBits = int.Parse(DataBitsComboBox);
            ASettings.UART_StopBits = int.Parse(StopBitsComboBox);
            ASettings.GatewayNumber = int.Parse(GetwaySelectionComboBox);
            ASettings.TxLog = TxFileTextBox;
            ASettings.RxLog = RxFileTextBox;

            SettingsManagerObj.SaveSettings(ASettings);

        }

        public BLL_JsonSettings GetJsonData()
        {
            return SettingsManagerObj.LoadSettings();
        }
        public void  SaveJsonData(BLL_JsonSettings JsonData)
        {

            SettingsManagerObj.SaveSettings(JsonData);
        }
  
        #endregion

        #region Log Files Creations and Writing On them
        public string txfilePath;
        public string rxfilePath;
        public bool TxFileCreation()
        {
            bool FuncStatus = true;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txfilePath = saveFileDialog.FileName;

                }

                if (string.IsNullOrEmpty(txfilePath))
                {
                    MessageBox.Show("Please select a file path.");
                    FuncStatus = false;
                    return FuncStatus;
                }

                try
                {
                    using (StreamWriter TXwriter = File.CreateText(txfilePath))
                    {
                        TXwriter.WriteLine("This is a Transmitted Data in the text file.");

                    }

                    MessageBox.Show("Text file created successfully.");
                }
                catch (Exception ex)
                {
                    FuncStatus = false;

                    MessageBox.Show($"An error occurred: {ex.Message}");
                }

            }
            return FuncStatus;

        }
        public bool RxFileCreation()
        {
            bool FuncStatus = true;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    rxfilePath = saveFileDialog.FileName;

                }

                if (string.IsNullOrEmpty(rxfilePath))
                {
                    MessageBox.Show("Please select a file path.");
                    FuncStatus = false;
                    return FuncStatus;
                }

                try
                {
                    using (StreamWriter RXwriter = File.CreateText(rxfilePath))
                    {
                        RXwriter.WriteLine("This is a Received Data in the text file.");

                    }

                    MessageBox.Show("Text file created successfully.");
                }
                catch (Exception ex)
                {
                    FuncStatus = false;

                    MessageBox.Show($"An error occurred: {ex.Message}");
                }

            }

            return FuncStatus;
        }
       
        public void WriteOnTxLogFile(string Data)
        {
            if(!string.IsNullOrEmpty(txfilePath))
            { 
                using (StreamWriter TXwriter = File.AppendText(txfilePath))
                {
                    // Write to the file
                    TXwriter.Write(Data);                    // Add more writing operations if needed
                }
            }
          
        }
        public void WriteOnRxLogFile(string Data)
        {
            if (!string.IsNullOrEmpty(rxfilePath))
            {
                using (StreamWriter RXwriter = File.AppendText(rxfilePath))
                {
                    RXwriter.Write(Data);
                }
            }
        }


        #endregion
        
        #region Serial Port Functions

        public void SetSerialConfig(string PortComboBox, string BaudRateComboBox, string StopBitsComboBox, string DataBitsComboBox, string ParityComboBox)
        {
             UartActionsObj.SetUartConfigurations(PortComboBox, BaudRateComboBox, StopBitsComboBox, DataBitsComboBox, ParityComboBox);
        }

        public bool CloseSerialConnection()
        {
            return UartActionsObj.CloseConnection();
        }

        public bool OpenSerialConnection()
        {
            return UartActionsObj.OpenConnection();
        }
        
        public string[] CheckSerialPorts(ref bool  PortExistOrNot)
        {
            return UartActionsObj.CheckPorts(ref PortExistOrNot);
        }
      
        public void SendSerialData(string data, ref bool PortExistOrNot, ref bool PortIsOpenOrNot, ref string DislayedText)
        {
            UartActionsObj.UARTSendData( data, ref  PortExistOrNot, ref  PortIsOpenOrNot, ref  DislayedText);
        }

        public void ReceiveSerialData( ref string DislayedText)
        {
            UartActionsObj.UARTReceiveData(ref DislayedText);
        }
        
        public int GetVarReceivedOrNot()
        {
            return UartActionsObj.ReceivedOrNot;
        }

        public void SetVarReceivedOrNot(int Val)
        {
             UartActionsObj.ReceivedOrNot=Val;
        }
       
        #endregion
    }
}
