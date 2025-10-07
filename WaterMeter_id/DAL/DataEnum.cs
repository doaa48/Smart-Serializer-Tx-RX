using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class DataEnum
    {

        public enum ELastUpdateBY
        {
            
            System=1,
            Meter  

        }
        public enum EAPDUInst
        {
         ReadInstruction = 0xB0,
         WriteInstruction= 0xD6,
          EraseInstruction = 0x0E

        }
        public enum EAPDU_Command
        {
            NoBatch = 0x00,
            WaitBatch = 0x01,
            LastBatch = 0xff

        }
        public enum EAPDU_P1Param
        {
            NoBatch = 0x00,
            WaitBatch = 0x01,
            LastBatch = 0xff

        }
        public enum EAPDU_P2Param
        {
            NoBatch = 0x00,
            WaitBatch = 0x01,
            LastBatch = 0xff

        }
        public enum EAPDUResponse
        {
            Success_Response = 0x9000,
            Success_AvailBatch = 0x5100,
            Unsupported_Instruction = 0x6D00,
            Unsupported_Parameter = 0x919E,
            Invalid_Certificate = 0x9D19,
            Invalid_Signature = 0x9D1A,
            Authentication_Failed = 0x9804
            
        }
        public enum EMeter_Dimention
        {
            inch3_4 = 0x01,
            inch1 ,
            inch1_5

        }
        public enum ECharge_Mode
        { 
        PrePaid=1,
        PostPaid
        }
        public enum ECard_Mode
        {
            NewCard_Mode = 0x00,
            ActiveCard_Mode = 0x01,
            Initialized_Mode = 0x02,
            ReplacementCard_Mode = 0x03,
            InactiveCard_Mode = 0x05,
            RegistrationInfoSent_Mode = 0x06,
            MeterInfoSent_Mode = 0x07,
            MeterIssued_Mode = 0x08
        }
      
        public enum ECatogry {
           HABITAT=1,
           COMMERCIAL,
           GOVERNMENT,
           KHADAMY,
           TOURIST,
           INDUSTRIAL,
           OTHER,
           CLUB
        }
        public enum EActivity
        {
            bit0  = 0,
            bit1,
            bit2,
            bit3,
            bit4,
            bit5,
            bit6,
            bit7,
            bit8,
            bit9,
            bit10,
            bit11
           
        }
        public enum ECard_Function
        {
            Customer_Card = 0x00,
            Configuration_Card = 0x01,
            Retrieval_Card = 0x02,
            Maintenance_Card = 0x03,
            MeterRegistration_Card = 0x04,
            MeterInitialization_Card = 0x05,
            MeterIssuing_Card = 0x06,
            Initialization_Card = 0x07
        }
  


           


    }
}
