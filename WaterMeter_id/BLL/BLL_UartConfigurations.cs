using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public  class BLL_UartConfigurations
    {
        public string BLL_ComPort { get; set; } = " ";
        public int BLL_BaudRate { get; set; }=115200;
        public string BLL_Parity { get; set; } = "None";
        public int BLL_DataBits { get; set; } = 8;
        public int  BLL_StopBits { get; set; } = 1;

    }
}
