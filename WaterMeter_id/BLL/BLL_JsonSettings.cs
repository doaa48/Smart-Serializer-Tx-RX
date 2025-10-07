using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_JsonSettings
    {
            public string ComPort { get; set; }
            public int UART_BaudRate { get; set; }
            public string UART_Parity { get; set; }
            public int UART_DataBits { get; set; }
            public int UART_StopBits { get; set; }
            public int GatewayNumber { get; set; }
            public int Timeout { get; set; }
            public int Frequency { get; set; }
            public string TxLog { get; set; }
            public string RxLog { get; set; }

    }
}
