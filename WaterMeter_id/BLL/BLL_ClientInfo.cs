using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
     public class BLL_ClientInfo
    {
          public int          ClientInfo_ID                  { get; set; }
          public int          ClientInfo_ClientID            { get; set; }
          public DateTime     ClientInfo_IssueDate           { get; set; }
          public int          ClientInfo_MeterID             { get; set; }
          public int          ClientInfo_CardID              { get; set; }
          public int          ClientInfo_PriceScheduleID     { get; set; }
          public int          ClientInfo_OFFTimeID           { get; set; }
          public int          ClientInfo_Activity            { get; set; }
          public int          ClientInfo_SwGServices         { get; set; }
          public int          ClientInfo_NumOFUnit           { get; set; }
          public int          ClientInfo_UnityTypeID         { get; set; }
          public int          ClientInfo_Category            { get; set; }
          public string       ClientInfo_Address             { get; set; }="";
           public string      ClientInfo_Desc                { get; set; } = "";
          public int          ClientInfo_SubscriberID          { get; set; }
           public string CertWaterCompToMeterSubscriber      { get; set; } = "";
    }
}
