using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq.Expressions;


namespace WaterMeter_id
{
    public class UartActions
    {
        public string DataSent;
        public string DataReceived;
        public int ReceivedOrNot = 0;

        public SerialPort MySerialPort = new SerialPort();

        // Event to handle re-initialize the system
        public event Action<int> CallBack_label;
        protected virtual void Label(int conn)
        {
            CallBack_label?.Invoke(conn);
        }

        public void UARTSendData(string data, ref bool PortExistOrNot, ref bool PortIsOpenOrNot, ref string DislayedText)
        {

            string[] Ports = CheckPorts(ref PortExistOrNot);
            try
            {
                if (PortExistOrNot)
                {

                    PortExistOrNot = true;
                    ReceivedOrNot = 0;



                    if (!OpenConnection())
                    {
                        PortIsOpenOrNot = true;

                        DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: " + data;

                        MySerialPort.Write(data);
                        //to color the added Text

                    }
                    else
                    {
                        PortIsOpenOrNot = false;
                        DislayedText = " at " + DateTime.Now.ToString() + " :: UART Port Busy for other Running Task";
                        DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";
                        Label(1);
                    }

                }
                else
                {

                    PortExistOrNot = false;
                    // Handle case where no COM ports are available
                    MessageBox.Show("No COM ports available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        #region How to use UARTSend
        /*
        
            string DislayedText= DesUARTSendData( data, ref  PortExistOrNot, ref  PortIsOpenOrNot)
            
                if(PortExistOrNot)
                {
                    UartRichBox.Enabled = true;
                    AppendColoredText(UartRichBox, DislayedText, Color.Green);

                    if (PortIsOpenOrNot)
                    {
                        //to color the added Text
                        AppendColoredText(UartRichBox, DislayedText, Color.Green);

                    }
                    else
                    {
                        AppendColoredText(UartRichBox, DislayedText, Color.Red);

                    }

                 }
                else
                {
                    UartRichBox.Enabled = false;
                    PortComboBox.Text = " Select Port ";
                }
        */
        #endregion

        public void SetUartConfigurations(string PortComboBox, string BaudRateComboBox, string StopBitsComboBox, string DataBitsComboBox, string ParityComboBox)
        {
            try { 
            MySerialPort.PortName = PortComboBox;

            MySerialPort.BaudRate = int.Parse(BaudRateComboBox); // Convert string to int
            MySerialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBitsComboBox); // Convert string to StopBits enum
            MySerialPort.DataBits = int.Parse(DataBitsComboBox); // Convert string to int
            MySerialPort.Parity = (Parity)Enum.Parse(typeof(Parity), ParityComboBox); // Convert string to Parity enum
            }
          catch (Exception ex)
            {
                // Handle any other exceptions that might occur
            //    MessageBox.Show("Error: " + ex.Message);
            }
         }


        public string[] CheckPorts(ref bool PortExistOrNot)
         {
            string[] Ports = SerialPort.GetPortNames();

            try
            {
                if (Ports.Length > 0)
                {
                    ReceivedOrNot = 0;
                    PortExistOrNot=true;
                    
                }
                else 
                {
                    PortExistOrNot=false;
                    // Handle case where no COM ports are available
                   // MessageBox.Show("No COM ports available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Ports;

        }

        public bool CloseConnection()
        {
            bool Status = false;

            try
            {
                if (MySerialPort.IsOpen)
                {
                    MySerialPort.Close();

                    Status = true;

                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            return Status;
        }

        public bool OpenConnection()
        {
            bool Status = false;

            try
            {



                if (MySerialPort.IsOpen)
                {

                    Status = true;
                }
                else if (!MySerialPort.IsOpen)
                {
                    MySerialPort.Open();

                    Status = true;

                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur
                //MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Status;

        }

       public void UARTReceiveData(ref string DislayedText)
        {
            string ReceivedString = MySerialPort.ReadExisting();
            string RText="";
            if (ReceivedOrNot == 0)
            {
                RText = Environment.NewLine + " at " + DateTime.Now.ToString() + " :: Recieved : ";
                ReceivedOrNot = 1;
            }
            RText += ReceivedString;

            //to color the added Text
            if (ReceivedString != null)
            {
                if (ReceivedString.Contains("\r") || ReceivedString.Contains("\n"))
                {
                    DislayedText = Environment.NewLine;

                }
                else
                {
                    DislayedText = RText;
                }
            }
        }

        public bool Send_Data(byte [] buffer,int length)
        {
            bool status = false;
            if (MySerialPort.IsOpen)
            {
                MySerialPort.DiscardInBuffer();
                MySerialPort.Write(buffer, 0, length);
                status = true;
            }

            return status;

        }
        
       public byte [] Read_Data()
        {
            try
            {
                if (MySerialPort.IsOpen)
                {
                    int Bsize = MySerialPort.BytesToRead; //read ReadBufferSize of data in the receiving buffer.  
                    if (Bsize > 254) Bsize = 254;
                    if (Bsize < 254) Bsize = 0;
                    byte[] Buffer = new byte[Bsize];
                    if (Bsize != 0)
                    {
                        MySerialPort.Read(Buffer, 0, Bsize);
                       // MySerialPort.DiscardInBuffer();

                        return Buffer;

                    }
                }

                /*
                if (MySerialPort.IsOpen)
                {
                    string data = MySerialPort.ReadExisting();
                    // Reset the serial port buffer
                    MySerialPort.DiscardInBuffer();
                    if (data != "")
                    {

                        return Encoding.ASCII.GetBytes(data);
                    }
                }*/
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                return null;

        }
    }
}
