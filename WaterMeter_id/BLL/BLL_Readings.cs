using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public  class BLL_Readings
    {
        public int Readings_ID { get; set; }
        public double Readings_ClientInfoID { get; set; }
        public DateTime Readings_Date { get; set; }
        public string Readings_ReadingBy { get; set; }
        public double Readings_MonthConsumption1 { get; set; }
        public double Readings_MonthConsumption2 { get; set; }
        public double Readings_MonthConsumption3 { get; set; }
        public double Readings_MonthConsumption4 { get; set; }
        public double Readings_MonthConsumption5 { get; set; }
        public double Readings_MonthConsumption6 { get; set; }
        public double Readings_MonthConsumption7 { get; set; }
        public double Readings_MonthConsumption8 { get; set; }
        public double Readings_MonthConsumption9 { get; set; }
        public double Readings_MonthConsumption10 { get; set; }
        public double Readings_MonthConsumption11 { get; set; }
        public double Readings_MonthConsumption12 { get; set; }
        public double Readings_QuantityTotalNegative { get; set; }
        public double Readings_CurrentMonthConsumption { get; set; }
        public double Readings_Reading { get; set; }
    }
}
