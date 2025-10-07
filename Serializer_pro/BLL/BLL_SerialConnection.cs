using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;


namespace WindowsFormsApp1
{
    public class BLL_SerialConnection
    {
        public string SerialConnection_PortName { get; set; }
        public int SerialConnection_BaudRate { get; set; }
        public int SerialConnection_DataBits { get; set; }
        public StopBits SerialConnection_StopBits { get; set; }
        public Parity SerialConnection_Parity { get; set; }

    }
}
