using System;

namespace WaterMeter_id
{
    public  class BLL_RetrivalCard
    {
        public int RetrivalCard_ID { get; set; }
        public int RetrivalCard_CardID { get; set; }
        public DateTime RetrivalCard_IssueDate { get; set; }
        public int RetrivalCard_RequiredData { get; set; }
        public int RetrivalCard_TimeEffective { get; set; }
        public int RetrivalCard_StartConsumerID { get; set; }
        public int RetrivalCard_EndConsumerID { get; set; }
        public ushort CountList { get; set; } = 0;
    }
}
