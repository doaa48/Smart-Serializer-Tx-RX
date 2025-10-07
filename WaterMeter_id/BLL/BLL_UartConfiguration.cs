using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_UartConfiguration
    {
        public string ComPort { get; set; } = "";
        public int BaudRate { get; set; } = 115200;

        public string ParityCheck { get; set; } = "None";

        public int DateBits { get; set; } = 8;
        public int StopBites { get; set; } = 1;
    }
}
